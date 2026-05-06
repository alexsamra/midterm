namespace model;

/// <summary>
/// Service interface for account management operations.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Validates user login credentials.
    /// </summary>
    /// <param name="login">The user's login username.</param>
    /// <param name="pin">The user's PIN.</param>
    /// <returns>The authenticated user if credentials are valid; otherwise, null.</returns>
    User? ValidateLogin(string login, string pin);

    /// <summary>
    /// Withdraws the specified amount from a user account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    (bool success, string? error) Withdraw(int userId, decimal amount);

    /// <summary>
    /// Deposits the specified amount into a user account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to deposit.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    (bool success, string? error) Deposit(int userId, decimal amount);

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="login">The login username for the new account.</param>
    /// <param name="pin">The PIN for the new account.</param>
    /// <param name="holdersName">The name of the account holder.</param>
    /// <param name="balance">The initial account balance.</param>
    /// <param name="status">The status of the account.</param>
    /// <returns>A tuple containing success status, the new account ID, and any error message.</returns>
    (bool success, int? accountId, string? error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status);

    /// <summary>
    /// Retrieves account information for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The user account information if found; otherwise, null.</returns>
    User? GetAccountById(int userId);

    /// <summary>
    /// Deletes the specified user account.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    (bool success, string? error) DeleteAccount(int userId);

    /// <summary>
    /// Updates the information for an existing user account.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="login">The new login username.</param>
    /// <param name="pin">The new PIN.</param>
    /// <param name="holdersName">The new holder name.</param>
    /// <param name="status">The new account status.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    (bool success, string? error) UpdateAccount(int userId, string login, string pin, string holdersName, string status);
}
