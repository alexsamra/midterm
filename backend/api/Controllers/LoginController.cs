using Microsoft.AspNetCore.Mvc;
using model;

namespace api.Controllers;

/// Authentication endpoint

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAccountService _accountService;

    public LoginController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var (validLogin, loginError) = AccountValidator.ValidateLogin(request.Login);
        if (!validLogin)
            return BadRequest(new { message = loginError });

        var (validPin, pinError) = AccountValidator.ValidatePin(request.Pin);
        if (!validPin)
            return BadRequest(new { message = pinError });

        var user = _accountService.ValidateLogin(request.Login, request.Pin);
        if (user == null)
            return Unauthorized(new { message = "Invalid login or pin." });

        return Ok(user);
    }
}

/// Login DTO

public class LoginRequest
{
    public string Login { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
}
