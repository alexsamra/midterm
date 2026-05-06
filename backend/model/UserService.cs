// <copyright file="UserService.cs" company="Midterm">
// Copyright (c) Midterm. All rights reserved.
// </copyright>

namespace Model;

using Dal;

/// <summary>
/// Service implementation for account management operations.
/// Implements business logic validation and delegates data access to the repository.
/// </summary>
public class UserService : IAccountService
{
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository dependency for data access.</param>
    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    /// <summary>
    /// Validates user login credentials.
    /// </summary>
    /// <param name="login">The user's login username.</param>
    /// <param name="pin">The user's PIN.</param>
    /// <returns>The authenticated user if credentials are valid; otherwise, null.</returns>
    public User? ValidateLogin(string login, string pin)
    {
        var result = this.userRepository.ValidateLogin(login, pin);
        if (result == null)
        {
            return null;
        }

        var r = result.Value;
        return new User
        {
            Id = r.Id,
            Login = r.Login,
            Pin = r.Pin,
            HoldersName = r.HoldersName,
            Balance = r.Balance,
            IsAdmin = r.IsAdmin,
            Status = r.Status,
        };
    }

    /// <summary>
    /// Withdraws the specified amount from a user account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    public (bool Success, string? Error) Withdraw(int userId, decimal amount)
    {
        var (isValid, error) = AccountValidator.ValidateAmount(amount);
        if (!isValid)
        {
            return (false, error);
        }

        var success = this.userRepository.Withdraw(userId, amount);
        if (!success)
        {
            return (false, "Withdrawal failed. Check balance and account.");
        }

        return (true, null);
    }

    /// <summary>
    /// Deposits the specified amount into a user account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to deposit.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    public (bool Success, string? Error) Deposit(int userId, decimal amount)
    {
        var (isValid, error) = AccountValidator.ValidateAmount(amount);
        if (!isValid)
        {
            return (false, error);
        }

        var success = this.userRepository.Deposit(userId, amount);
        if (!success)
        {
            return (false, "Deposit failed.");
        }

        return (true, null);
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="login">The login username for the new account.</param>
    /// <param name="pin">The PIN for the new account.</param>
    /// <param name="holdersName">The name of the account holder.</param>
    /// <param name="balance">The initial account balance.</param>
    /// <param name="status">The status of the account.</param>
    /// <returns>A tuple containing success status, the new account ID, and any error message.</returns>
    public (bool Success, int? AccountId, string? Error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status)
    {
        var (validLogin, loginError) = AccountValidator.ValidateLogin(login);
        if (!validLogin)
        {
            return (false, null, loginError);
        }

        var (validPin, pinError) = AccountValidator.ValidatePin(pin);
        if (!validPin)
        {
            return (false, null, pinError);
        }

        var (validHolder, holderError) = AccountValidator.ValidateHolderName(holdersName);
        if (!validHolder)
        {
            return (false, null, holderError);
        }

        var (validBalance, balanceError) = AccountValidator.ValidateBalance(balance);
        if (!validBalance)
        {
            return (false, null, balanceError);
        }

        var (validStatus, statusError) = AccountValidator.ValidateStatus(status);
        if (!validStatus)
        {
            return (false, null, statusError);
        }

        if (this.userRepository.LoginExists(login))
        {
            return (false, null, "Login already exists.");
        }

        var id = this.userRepository.CreateUser(login, pin, holdersName, balance, status);
        return id > 0 ? (true, id, null) : (false, null, "Failed to create account.");
    }

    /// <summary>
    /// Retrieves account information for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The user account information if found; otherwise, null.</returns>
    public User? GetAccountById(int userId)
    {
        var result = this.userRepository.GetUserById(userId);
        if (result == null)
        {
            return null;
        }

        var r = result.Value;
        return new User
        {
            Id = r.Id,
            Login = r.Login,
            Pin = r.Pin,
            HoldersName = r.HoldersName,
            Balance = r.Balance,
            IsAdmin = r.IsAdmin,
            Status = r.Status,
        };
    }

    /// <summary>
    /// Deletes the specified user account.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    public (bool Success, string? Error) DeleteAccount(int userId)
    {
        var success = this.userRepository.DeleteUser(userId);
        if (!success)
        {
            return (false, "Account not found.");
        }

        return (true, null);
    }

    /// <summary>
    /// Updates the information for an existing user account.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="login">The new login username.</param>
    /// <param name="pin">The new PIN.</param>
    /// <param name="holdersName">The new holder name.</param>
    /// <param name="status">The new account status.</param>
    /// <returns>A tuple indicating success and any error message.</returns>
    public (bool Success, string? Error) UpdateAccount(int userId, string login, string pin, string holdersName, string status)
    {
        var (validLogin, loginError) = AccountValidator.ValidateLogin(login);
        if (!validLogin)
        {
            return (false, loginError);
        }

        var (validPin, pinError) = AccountValidator.ValidatePin(pin);
        if (!validPin)
        {
            return (false, pinError);
        }

        var (validHolder, holderError) = AccountValidator.ValidateHolderName(holdersName);
        if (!validHolder)
        {
            return (false, holderError);
        }

        var (validStatus, statusError) = AccountValidator.ValidateStatus(status);
        if (!validStatus)
        {
            return (false, statusError);
        }

        var existing = this.userRepository.GetUserById(userId);
        if (existing == null)
        {
            return (false, "Account not found.");
        }

        if (existing.Value.Login != login && this.userRepository.LoginExists(login))
        {
            return (false, "Login already in use.");
        }

        var updated = this.userRepository.UpdateUser(userId, login, pin, holdersName, status);
        return updated ? (true, null) : (false, "Update failed.");
    }
}
