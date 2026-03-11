using Microsoft.AspNetCore.Mvc;
using model;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserService _userService;

    public LoginController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Pin))
            return BadRequest(new { message = "Login and pin must be valid inputs" });

        if (request.Pin.Length != 5 || !request.Pin.All(char.IsDigit))
            return BadRequest(new { message = "Pin must be 5 digits long" });

        var user = _userService.ValidateLogin(request.Login, request.Pin);
        if (user == null)
            return Unauthorized(new { message = "Incorrect login or pin" });

        return Ok(user);
    }
}

public class LoginRequest
{
    public string Login { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
}
