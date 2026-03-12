using Microsoft.AspNetCore.Mvc;
using model;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("withdraw")]
    public IActionResult Withdraw([FromBody] WithdrawRequest request)
    {
        if (request.Amount <= 0)
            return BadRequest(new { message = "Amount must be greater than zero" });

        var success = _userService.Withdraw(request.UserId, request.Amount);
        if (!success)
            return BadRequest(new { message = "Error" });

        return Ok();
    }

    [HttpPost("deposit")]
    public IActionResult Deposit([FromBody] DepositRequest request)
    {
        if (request.Amount <= 0)
            return BadRequest(new { message = "Amount must be greater than zero" });

        var success = _userService.Deposit(request.UserId, request.Amount);
        if (!success)
            return BadRequest(new { message = "Error" });

        return Ok();
    }

    [HttpPost("create")]
    public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
            return BadRequest(new { message = "Invalid Login" });

        if (request.Pin.Length != 5 || !request.Pin.All(char.IsDigit))
            return BadRequest(new { message = "Invalid Pin" });

        var (success, accountId, error) = _userService.CreateUser(request.Login, request.Pin, request.HolderName, request.Balance, request.Status);
        if (!success)
            return Conflict(new { message = error });

        return Ok(new { id = accountId });
    }
}

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
    public string Status { get; set; } = "Active";
}
