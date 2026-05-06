using Microsoft.AspNetCore.Mvc;
using model;

namespace api.Controllers;

/// <summary>
/// Controller providing account management endpoints for the banking API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    /// <param name="accountService">The account service dependency.</param>
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Withdraws the specified amount from a user account.
    /// </summary>
    /// <param name="request">The withdrawal request containing user ID and amount.</param>
    /// <returns>OK if withdrawal is successful; otherwise, BadRequest with error details.</returns>
    [HttpPost("withdraw")]
    public IActionResult Withdraw([FromBody] WithdrawRequest request)
    {
        var (success, error) = _accountService.Withdraw(request.UserId, request.Amount);
        if (!success)
            return BadRequest(new { message = error });

        return Ok();
    }

    /// <summary>
    /// Deposits the specified amount into a user account.
    /// </summary>
    /// <param name="request">The deposit request containing user ID and amount.</param>
    /// <returns>OK if deposit is successful; otherwise, BadRequest with error details.</returns>
    [HttpPost("deposit")]
    public IActionResult Deposit([FromBody] DepositRequest request)
    {
        var (success, error) = _accountService.Deposit(request.UserId, request.Amount);
        if (!success)
            return BadRequest(new { message = error });

        return Ok();
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="request">The account creation request with required details.</param>
    /// <returns>OK with the new account ID if successful; otherwise, Conflict with error details.</returns>
    [HttpPost("create")]
    public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
    {
        var (success, accountId, error) = _accountService.CreateAccount(request.Login, request.Pin, request.HolderName, request.Balance, request.Status);
        if (!success)
            return Conflict(new { message = error });

        return Ok(new { id = accountId });
    }

    /// <summary>
    /// Retrieves account information for the specified user ID.
    /// </summary>
    /// <param name="id">The user account ID.</param>
    /// <returns>OK with account details if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _accountService.GetAccountById(id);
        if (user == null)
            return NotFound(new { message = "Account not found." });

        return Ok(user);
    }

    /// <summary>
    /// Deletes the user account with the specified ID.
    /// </summary>
    /// <param name="id">The user account ID to delete.</param>
    /// <returns>OK with success message if deletion is successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var (success, error) = _accountService.DeleteAccount(id);
        if (!success)
            return NotFound(new { message = error });

        return Ok(new { message = "Account deleted successfully." });
    }

    /// <summary>
    /// Updates the information for an existing user account.
    /// </summary>
    /// <param name="request">The account update request with new details.</param>
    /// <returns>OK with success message if update is successful; otherwise, BadRequest with error details.</returns>
    [HttpPost("update")]
    public IActionResult UpdateAccount([FromBody] UpdateAccountRequest request)
    {
        var (success, error) = _accountService.UpdateAccount(request.Id, request.Login, request.Pin, request.HolderName, request.Status);
        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Account updated successfully." });
    }
}

/// <summary>
/// Request DTO for withdrawal operations.
/// </summary>
public class WithdrawRequest
{
    /// <summary>
    /// Gets or sets the user ID for the withdrawal.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the amount to withdraw.
    /// </summary>
    public decimal Amount { get; set; }
}

/// <summary>
/// Request DTO for deposit operations.
/// </summary>
public class DepositRequest
{
    /// <summary>
    /// Gets or sets the user ID for the deposit.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the amount to deposit.
    /// </summary>
    public decimal Amount { get; set; }
}

/// <summary>
/// Request DTO for creating a new user account.
/// </summary>
public class CreateAccountRequest
{
    /// <summary>
    /// Gets or sets the login username for the new account.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PIN for the new account.
    /// </summary>
    public string Pin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account holder's name.
    /// </summary>
    public string HolderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the initial account balance.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the account status.
    /// </summary>
    public string Status { get; set; } = AccountConstants.StatusActive;
}

/// <summary>
/// Request DTO for updating an existing user account.
/// </summary>
public class UpdateAccountRequest
{
    /// <summary>
    /// Gets or sets the account ID to update.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new login username.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new PIN.
    /// </summary>
    public string Pin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new account holder's name.
    /// </summary>
    public string HolderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new account status.
    /// </summary>
    public string Status { get; set; } = AccountConstants.StatusActive;
}
