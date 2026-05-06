// <copyright file="IUserRepository.cs" company="Midterm">
// Copyright (c) Midterm. All rights reserved.
// </copyright>

namespace Dal;

/// <summary>
/// Data access interface for user repository operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Validates user login credentials against the database.
    /// </summary>
    /// <param name="login">The user's login username.</param>
    /// <param name="pin">The user's PIN.</param>
    /// <returns>A tuple containing user information if credentials are valid; otherwise, null.</returns>
    (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? ValidateLogin(string login, string pin);

    /// <summary>
    /// Withdraws the specified amount from a user account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <returns>True if the withdrawal was successful; otherwise, false.</returns>
    bool Withdraw(int userId, decimal amount);

    /// <summary>
    /// Deposits the specified amount into a user account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to deposit.</param>
    /// <returns>True if the deposit was successful; otherwise, false.</returns>
    bool Deposit(int userId, decimal amount);

    /// <summary>
    /// Checks if a login username already exists in the database.
    /// </summary>
    /// <param name="login">The login username to check.</param>
    /// <returns>True if the login exists; otherwise, false.</returns>
    bool LoginExists(string login);

    /// <summary>
    /// Creates a new user account in the database.
    /// </summary>
    /// <param name="login">The login username for the new account.</param>
    /// <param name="pin">The PIN for the new account.</param>
    /// <param name="holdersName">The name of the account holder.</param>
    /// <param name="balance">The initial account balance.</param>
    /// <param name="status">The status of the account (default: "Active").</param>
    /// <returns>The ID of the newly created user.</returns>
    int CreateUser(string login, string pin, string holdersName, decimal balance, string status = "Active");

    /// <summary>
    /// Retrieves user information from the database by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A tuple containing user information if found; otherwise, null.</returns>
    (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? GetUserById(int userId);

    /// <summary>
    /// Deletes a user account from the database.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    bool DeleteUser(int userId);

    /// <summary>
    /// Updates user account information in the database.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="login">The new login username.</param>
    /// <param name="pin">The new PIN.</param>
    /// <param name="holdersName">The new holder name.</param>
    /// <param name="status">The new account status.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    bool UpdateUser(int userId, string login, string pin, string holdersName, string status);
}
