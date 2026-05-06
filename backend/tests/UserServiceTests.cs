using Model;

namespace tests;

public class UserServiceTests
{
    [Fact]
    public void ValidateLogin_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        var repo = new FakeUserRepository { ValidateLoginResult = null };
        var service = new UserService(repo);

        var user = service.ValidateLogin("x", "12345");

        Assert.Null(user);
    }

    [Fact]
    public void ValidateLogin_ShouldMapUser_WhenRepositoryReturnsData()
    {
        var repo = new FakeUserRepository
        {
            ValidateLoginResult = (1, "alice", "12345", "Alice", 250m, true, "Active")
        };
        var service = new UserService(repo);

        var user = service.ValidateLogin("alice", "12345");

        Assert.NotNull(user);
        Assert.Equal(1, user!.Id);
        Assert.Equal("alice", user.Login);
        Assert.Equal("12345", user.Pin);
        Assert.Equal("Alice", user.HoldersName);
        Assert.Equal(250m, user.Balance);
        Assert.True(user.IsAdmin);
        Assert.Equal("Active", user.Status);
    }

    [Fact]
    public void Withdraw_ShouldFail_WhenAmountInvalid()
    {
        var service = new UserService(new FakeUserRepository());

        var result = service.Withdraw(1, 0);

        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void Withdraw_ShouldFail_WhenRepositoryRejects()
    {
        var repo = new FakeUserRepository { WithdrawResult = false };
        var service = new UserService(repo);

        var result = service.Withdraw(1, 10);

        Assert.False(result.Success);
        Assert.Contains("Withdrawal failed", result.Error);
    }

    [Fact]
    public void Withdraw_ShouldSucceed_WhenRepositoryAccepts()
    {
        var repo = new FakeUserRepository { WithdrawResult = true };
        var service = new UserService(repo);

        var result = service.Withdraw(1, 10);

        Assert.True(result.Success);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Deposit_ShouldFail_WhenAmountInvalid()
    {
        var service = new UserService(new FakeUserRepository());

        var result = service.Deposit(1, -1);

        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void Deposit_ShouldFail_WhenRepositoryRejects()
    {
        var repo = new FakeUserRepository { DepositResult = false };
        var service = new UserService(repo);

        var result = service.Deposit(1, 10);

        Assert.False(result.Success);
        Assert.Equal("Deposit failed.", result.Error);
    }

    [Fact]
    public void Deposit_ShouldSucceed_WhenRepositoryAccepts()
    {
        var repo = new FakeUserRepository { DepositResult = true };
        var service = new UserService(repo);

        var result = service.Deposit(1, 10);

        Assert.True(result.Success);
        Assert.Null(result.Error);
    }

    [Fact]
    public void CreateAccount_ShouldFail_ForInvalidInputs()
    {
        var service = new UserService(new FakeUserRepository());

        var badLogin = service.CreateAccount("", "12345", "A", 0, "Active");
        var badPin = service.CreateAccount("alice", "1234", "A", 0, "Active");
        var badHolder = service.CreateAccount("alice", "12345", "", 0, "Active");
        var badBalance = service.CreateAccount("alice", "12345", "A", -1, "Active");
        var badStatus = service.CreateAccount("alice", "12345", "A", 0, "Paused");

        Assert.False(badLogin.Success);
        Assert.False(badPin.Success);
        Assert.False(badHolder.Success);
        Assert.False(badBalance.Success);
        Assert.False(badStatus.Success);
    }

    [Fact]
    public void CreateAccount_ShouldFail_WhenLoginExists()
    {
        var repo = new FakeUserRepository { LoginExistsResult = true };
        var service = new UserService(repo);

        var result = service.CreateAccount("alice", "12345", "Alice", 10, "Active");

        Assert.False(result.Success);
        Assert.Equal("Login already exists.", result.Error);
    }

    [Fact]
    public void CreateAccount_ShouldFail_WhenCreateReturnsNonPositiveId()
    {
        var repo = new FakeUserRepository { LoginExistsResult = false, CreateUserResult = 0 };
        var service = new UserService(repo);

        var result = service.CreateAccount("alice", "12345", "Alice", 10, "Active");

        Assert.False(result.Success);
        Assert.Equal("Failed to create account.", result.Error);
    }

    [Fact]
    public void CreateAccount_ShouldSucceed_WhenCreateReturnsId()
    {
        var repo = new FakeUserRepository { LoginExistsResult = false, CreateUserResult = 44 };
        var service = new UserService(repo);

        var result = service.CreateAccount("alice", "12345", "Alice", 10, "Active");

        Assert.True(result.Success);
        Assert.Equal(44, result.AccountId);
        Assert.Null(result.Error);
    }

    [Fact]
    public void GetAccountById_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        var service = new UserService(new FakeUserRepository());

        var user = service.GetAccountById(1);

        Assert.Null(user);
    }

    [Fact]
    public void GetAccountById_ShouldMapUser_WhenRepositoryReturnsData()
    {
        var repo = new FakeUserRepository
        {
            GetUserByIdResult = (2, "bob", "67890", "Bob", 99m, false, "Disabled")
        };
        var service = new UserService(repo);

        var user = service.GetAccountById(2);

        Assert.NotNull(user);
        Assert.Equal(2, user!.Id);
        Assert.Equal("bob", user.Login);
        Assert.Equal("Disabled", user.Status);
    }

    [Fact]
    public void DeleteAccount_ShouldFail_WhenRepositoryRejects()
    {
        var repo = new FakeUserRepository { DeleteUserResult = false };
        var service = new UserService(repo);

        var result = service.DeleteAccount(1);

        Assert.False(result.Success);
        Assert.Equal("Account not found.", result.Error);
    }

    [Fact]
    public void DeleteAccount_ShouldSucceed_WhenRepositoryAccepts()
    {
        var repo = new FakeUserRepository { DeleteUserResult = true };
        var service = new UserService(repo);

        var result = service.DeleteAccount(1);

        Assert.True(result.Success);
        Assert.Null(result.Error);
    }

    [Fact]
    public void UpdateAccount_ShouldFail_ForInvalidInputs()
    {
        var service = new UserService(new FakeUserRepository());

        Assert.False(service.UpdateAccount(1, "", "12345", "A", "Active").Success);
        Assert.False(service.UpdateAccount(1, "a", "12", "A", "Active").Success);
        Assert.False(service.UpdateAccount(1, "a", "12345", "", "Active").Success);
        Assert.False(service.UpdateAccount(1, "a", "12345", "A", "Paused").Success);
    }

    [Fact]
    public void UpdateAccount_ShouldFail_WhenAccountNotFound()
    {
        var repo = new FakeUserRepository { GetUserByIdResult = null };
        var service = new UserService(repo);

        var result = service.UpdateAccount(1, "alice", "12345", "Alice", "Active");

        Assert.False(result.Success);
        Assert.Equal("Account not found.", result.Error);
    }

    [Fact]
    public void UpdateAccount_ShouldFail_WhenLoginTakenByAnotherAccount()
    {
        var repo = new FakeUserRepository
        {
            GetUserByIdResult = (1, "current", "12345", "Current", 10m, false, "Active"),
            LoginExistsResult = true
        };
        var service = new UserService(repo);

        var result = service.UpdateAccount(1, "other", "12345", "Alice", "Active");

        Assert.False(result.Success);
        Assert.Equal("Login already in use.", result.Error);
    }

    [Fact]
    public void UpdateAccount_ShouldFail_WhenRepositoryUpdateFails()
    {
        var repo = new FakeUserRepository
        {
            GetUserByIdResult = (1, "alice", "12345", "Alice", 10m, false, "Active"),
            UpdateUserResult = false
        };
        var service = new UserService(repo);

        var result = service.UpdateAccount(1, "alice", "12345", "Alice", "Active");

        Assert.False(result.Success);
        Assert.Equal("Update failed.", result.Error);
    }

    [Fact]
    public void UpdateAccount_ShouldSucceed_WhenRepositoryUpdatePasses()
    {
        var repo = new FakeUserRepository
        {
            GetUserByIdResult = (1, "alice", "12345", "Alice", 10m, false, "Active"),
            UpdateUserResult = true
        };
        var service = new UserService(repo);

        var result = service.UpdateAccount(1, "alice", "12345", "Alice", "Active");

        Assert.True(result.Success);
        Assert.Null(result.Error);
    }
}
