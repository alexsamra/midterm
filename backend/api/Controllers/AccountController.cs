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
