// <copyright file="LoginController.cs" company="Midterm">
// Copyright (c) Midterm. All rights reserved.
// </copyright>

namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dal;
using Model;

/// <summary>
/// Authentication controller for login operations.
/// </summary>
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
        {
            return BadRequest(new { message = loginError });
        }

        var (validPin, pinError) = AccountValidator.ValidatePin(request.Pin);
        if (!validPin)
        {
            return BadRequest(new { message = pinError });
        }

        var user = this._accountService.ValidateLogin(request.Login, request.Pin);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid login or pin." });
        }

        return Ok(user);
    }
}

/// <summary>
/// Request DTO for login operations.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the user's login username.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's PIN.
    /// </summary>
    public string Pin { get; set; } = string.Empty;
}
