namespace model;

public static class AccountConstants
{
    public const string StatusActive = "Active";
    public const string StatusDisabled = "Disabled";

    public static readonly string[] ValidStatuses = { StatusActive, StatusDisabled };

    public const int PinLength = 5;
    public const decimal MinimumBalance = 0m;
}
