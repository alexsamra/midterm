namespace model;

/// <summary>
/// Constants for account validation and configuration.
/// </summary>
public static class AccountConstants
{
    /// <summary>
    /// The status value indicating an active account.
    /// </summary>
    public const string StatusActive = "Active";

    /// <summary>
    /// The status value indicating a disabled account.
    /// </summary>
    public const string StatusDisabled = "Disabled";

    /// <summary>
    /// Array of valid account status values.
    /// </summary>
    public static readonly string[] ValidStatuses = { StatusActive, StatusDisabled };

    /// <summary>
    /// The required length for a PIN.
    /// </summary>
    public const int PinLength = 5;

    /// <summary>
    /// The minimum allowed balance for an account.
    /// </summary>
    public const decimal MinimumBalance = 0m;
}
