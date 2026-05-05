using Microsoft.AspNetCore.Mvc;
using model;

namespace api.Controllers;

/// Account endpoints

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("withdraw")]
    public IActionResult Withdraw([FromBody] WithdrawRequest request)
    {
        var (success, error) = _accountService.Withdraw(request.UserId, request.Amount);
        if (!success)
            return BadRequest(new { message = error });

        return Ok();
    }

    [HttpPost("deposit")]
    public IActionResult Deposit([FromBody] DepositRequest request)
    {
        var (success, error) = _accountService.Deposit(request.UserId, request.Amount);
        if (!success)
            return BadRequest(new { message = error });

        return Ok();
    }

    [HttpPost("create")]
    public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
    {
        var (success, accountId, error) = _accountService.CreateAccount(request.Login, request.Pin, request.HolderName, request.Balance, request.Status);
        if (!success)
            return Conflict(new { message = error });

        return Ok(new { id = accountId });
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _accountService.GetAccountById(id);
        if (user == null)
            return NotFound(new { message = "Account not found." });

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var (success, error) = _accountService.DeleteAccount(id);
        if (!success)
            return NotFound(new { message = error });

        return Ok(new { message = "Account deleted successfully." });
    }

    [HttpPost("update")]
    public IActionResult UpdateAccount([FromBody] UpdateAccountRequest request)
    {
        var (success, error) = _accountService.UpdateAccount(request.Id, request.Login, request.Pin, request.HolderName, request.Status);
        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Account updated successfully." });
    }
}

///  Account DTOs

public class WithdrawRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
}

public class DepositRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
}

public class CreateAccountRequest
{
    public string Login { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
    public string HolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Status { get; set; } = AccountConstants.StatusActive;
}

public class UpdateAccountRequest
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
    public string HolderName { get; set; } = string.Empty;
    public string Status { get; set; } = AccountConstants.StatusActive;
}
