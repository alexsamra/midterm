namespace model;

/// <summary>
/// Validator for account-related operations and data.
/// </summary>
public static class AccountValidator
{
    /// <summary>
    /// Validates a login username.
    /// </summary>
    /// <param name="login">The login username to validate.</param>
    /// <returns>A tuple indicating validity and any error message.</returns>
    public static (bool isValid, string? error) ValidateLogin(string? login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return (false, "Login cannot be empty.");
        return (true, null);
    }

    /// <summary>
    /// Validates a PIN.
    /// </summary>
    /// <param name="pin">The PIN to validate.</param>
    /// <returns>A tuple indicating validity and any error message.</returns>
    public static (bool isValid, string? error) ValidatePin(string? pin)
    {
        if (pin == null || pin.Length != AccountConstants.PinLength || !pin.All(char.IsDigit))
            return (false, $"Pin must be exactly {AccountConstants.PinLength} digits.");
        return (true, null);
    }

    /// <summary>
    /// Validates a transaction amount.
    /// </summary>
    /// <param name="amount">The amount to validate.</param>
    /// <returns>A tuple indicating validity and any error message.</returns>
    public static (bool isValid, string? error) ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            return (false, "Amount must be greater than zero.");
        return (true, null);
    }

    /// <summary>
    /// Validates an account balance.
    /// </summary>
    /// <param name="balance">The balance to validate.</param>
    /// <returns>A tuple indicating validity and any error message.</returns>
    public static (bool isValid, string? error) ValidateBalance(decimal balance)
    {
        if (balance < AccountConstants.MinimumBalance)
            return (false, $"Balance cannot be negative.");
        return (true, null);
    }

    /// <summary>
    /// Validates an account status.
    /// </summary>
    /// <param name="status">The status to validate.</param>
    /// <returns>A tuple indicating validity and any error message.</returns>
    public static (bool isValid, string? error) ValidateStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status) || !AccountConstants.ValidStatuses.Contains(status))
            return (false, $"Status must be one of: {string.Join(", ", AccountConstants.ValidStatuses)}");
        return (true, null);
    }

    /// <summary>
    /// Validates an account holder name.
    /// </summary>
    /// <param name="name">The holder name to validate.</param>
    /// <returns>A tuple indicating validity and any error message.</returns>
    public static (bool isValid, string? error) ValidateHolderName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Holder name cannot be empty.");
        return (true, null);
    }
}
