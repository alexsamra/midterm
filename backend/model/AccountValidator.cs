namespace model;

public static class AccountValidator
{
    public static (bool isValid, string? error) ValidateLogin(string? login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return (false, "Login cannot be empty.");
        return (true, null);
    }

    public static (bool isValid, string? error) ValidatePin(string? pin)
    {
        if (pin == null || pin.Length != AccountConstants.PinLength || !pin.All(char.IsDigit))
            return (false, $"Pin must be exactly {AccountConstants.PinLength} digits.");
        return (true, null);
    }

    public static (bool isValid, string? error) ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            return (false, "Amount must be greater than zero.");
        return (true, null);
    }

    public static (bool isValid, string? error) ValidateBalance(decimal balance)
    {
        if (balance < AccountConstants.MinimumBalance)
            return (false, $"Balance cannot be negative.");
        return (true, null);
    }

    public static (bool isValid, string? error) ValidateStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status) || !AccountConstants.ValidStatuses.Contains(status))
            return (false, $"Status must be one of: {string.Join(", ", AccountConstants.ValidStatuses)}");
        return (true, null);
    }

    public static (bool isValid, string? error) ValidateHolderName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Holder name cannot be empty.");
        return (true, null);
    }
}
