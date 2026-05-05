using model;

namespace tests;

public class AccountConstantsTests
{
    [Fact]
    public void Constants_ShouldExposeExpectedValues()
    {
        Assert.Equal("Active", AccountConstants.StatusActive);
        Assert.Equal("Disabled", AccountConstants.StatusDisabled);
        Assert.Equal(5, AccountConstants.PinLength);
        Assert.Equal(0m, AccountConstants.MinimumBalance);
    }

    [Fact]
    public void ValidStatuses_ShouldContainExpectedStatuses()
    {
        Assert.Contains(AccountConstants.StatusActive, AccountConstants.ValidStatuses);
        Assert.Contains(AccountConstants.StatusDisabled, AccountConstants.ValidStatuses);
        Assert.Equal(2, AccountConstants.ValidStatuses.Length);
    }
}
