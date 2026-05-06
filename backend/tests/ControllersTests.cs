using System.Text.Json;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace tests;

public class ControllersTests
{
    [Fact]
    public void AccountController_Withdraw_ShouldReturnBadRequest_OnFailure()
    {
        var service = new FakeAccountService { WithdrawResult = (false, "bad") };
        var controller = new AccountController(service);

        var result = controller.Withdraw(new WithdrawRequest { UserId = 1, Amount = 10 });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("bad", Serialize(bad.Value));
    }

    [Fact]
    public void AccountController_Withdraw_ShouldReturnOk_OnSuccess()
    {
        var service = new FakeAccountService { WithdrawResult = (true, null) };
        var controller = new AccountController(service);

        var result = controller.Withdraw(new WithdrawRequest { UserId = 1, Amount = 10 });

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void AccountController_Deposit_ShouldReturnBadRequest_OnFailure()
    {
        var service = new FakeAccountService { DepositResult = (false, "bad") };
        var controller = new AccountController(service);

        var result = controller.Deposit(new DepositRequest { UserId = 1, Amount = 10 });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("bad", Serialize(bad.Value));
    }

    [Fact]
    public void AccountController_Deposit_ShouldReturnOk_OnSuccess()
    {
        var service = new FakeAccountService { DepositResult = (true, null) };
        var controller = new AccountController(service);

        var result = controller.Deposit(new DepositRequest { UserId = 1, Amount = 10 });

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void AccountController_Create_ShouldReturnConflict_OnFailure()
    {
        var service = new FakeAccountService { CreateAccountResult = (false, null, "duplicate") };
        var controller = new AccountController(service);

        var result = controller.CreateAccount(new CreateAccountRequest
        {
            Login = "alice",
            Pin = "12345",
            HolderName = "Alice",
            Balance = 10,
            Status = AccountConstants.StatusActive
        });

        var conflict = Assert.IsType<ConflictObjectResult>(result);
        Assert.Contains("duplicate", Serialize(conflict.Value));
    }

    [Fact]
    public void AccountController_Create_ShouldReturnOkWithId_OnSuccess()
    {
        var service = new FakeAccountService { CreateAccountResult = (true, 99, null) };
        var controller = new AccountController(service);

        var result = controller.CreateAccount(new CreateAccountRequest
        {
            Login = "alice",
            Pin = "12345",
            HolderName = "Alice",
            Balance = 10,
            Status = AccountConstants.StatusActive
        });

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("99", Serialize(ok.Value));
    }

    [Fact]
    public void AccountController_GetUser_ShouldReturnNotFound_WhenMissing()
    {
        var service = new FakeAccountService { GetAccountByIdResult = null };
        var controller = new AccountController(service);

        var result = controller.GetUser(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void AccountController_GetUser_ShouldReturnOk_WhenFound()
    {
        var service = new FakeAccountService
        {
            GetAccountByIdResult = new User { Id = 1, Login = "alice", Pin = "12345" }
        };
        var controller = new AccountController(service);

        var result = controller.GetUser(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        var user = Assert.IsType<User>(ok.Value);
        Assert.Equal("alice", user.Login);
    }

    [Fact]
    public void AccountController_Delete_ShouldReturnNotFound_OnFailure()
    {
        var service = new FakeAccountService { DeleteAccountResult = (false, "missing") };
        var controller = new AccountController(service);

        var result = controller.DeleteUser(1);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("missing", Serialize(notFound.Value));
    }

    [Fact]
    public void AccountController_Delete_ShouldReturnOk_OnSuccess()
    {
        var service = new FakeAccountService { DeleteAccountResult = (true, null) };
        var controller = new AccountController(service);

        var result = controller.DeleteUser(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AccountController_Update_ShouldReturnBadRequest_OnFailure()
    {
        var service = new FakeAccountService { UpdateAccountResult = (false, "bad") };
        var controller = new AccountController(service);

        var result = controller.UpdateAccount(new UpdateAccountRequest
        {
            Id = 1,
            Login = "alice",
            Pin = "12345",
            HolderName = "Alice",
            Status = AccountConstants.StatusActive
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("bad", Serialize(bad.Value));
    }

    [Fact]
    public void AccountController_Update_ShouldReturnOk_OnSuccess()
    {
        var service = new FakeAccountService { UpdateAccountResult = (true, null) };
        var controller = new AccountController(service);

        var result = controller.UpdateAccount(new UpdateAccountRequest
        {
            Id = 1,
            Login = "alice",
            Pin = "12345",
            HolderName = "Alice",
            Status = AccountConstants.StatusActive
        });

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void LoginController_Login_ShouldReturnBadRequest_ForInvalidLogin()
    {
        var controller = new LoginController(new FakeAccountService());

        var result = controller.Login(new LoginRequest { Login = "", Pin = "12345" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void LoginController_Login_ShouldReturnBadRequest_ForInvalidPin()
    {
        var controller = new LoginController(new FakeAccountService());

        var result = controller.Login(new LoginRequest { Login = "alice", Pin = "12" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void LoginController_Login_ShouldReturnUnauthorized_WhenCredentialsInvalid()
    {
        var service = new FakeAccountService { ValidateLoginResult = null };
        var controller = new LoginController(service);

        var result = controller.Login(new LoginRequest { Login = "alice", Pin = "12345" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public void LoginController_Login_ShouldReturnOk_WhenCredentialsValid()
    {
        var service = new FakeAccountService
        {
            ValidateLoginResult = new User { Id = 1, Login = "alice", Pin = "12345" }
        };
        var controller = new LoginController(service);

        var result = controller.Login(new LoginRequest { Login = "alice", Pin = "12345" });

        var ok = Assert.IsType<OkObjectResult>(result);
        var user = Assert.IsType<User>(ok.Value);
        Assert.Equal(1, user.Id);
    }

    private static string Serialize(object? value)
    {
        return JsonSerializer.Serialize(value);
    }
}
