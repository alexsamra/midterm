using model;

namespace tests;

public class AccountValidatorTests
{
    [Fact]
    public void ValidateLogin_ShouldFail_WhenLoginIsNullOrWhitespace()
    {
        Assert.False(AccountValidator.ValidateLogin(null).isValid);
        Assert.False(AccountValidator.ValidateLogin(string.Empty).isValid);
        Assert.False(AccountValidator.ValidateLogin("   ").isValid);
    }

    [Fact]
    public void ValidateLogin_ShouldPass_WhenLoginIsValid()
    {
        var result = AccountValidator.ValidateLogin("alice");
        Assert.True(result.isValid);
        Assert.Null(result.error);
    }

    [Fact]
    public void ValidatePin_ShouldFail_WhenPinIsInvalid()
    {
        Assert.False(AccountValidator.ValidatePin(null).isValid);
        Assert.False(AccountValidator.ValidatePin("1234").isValid);
        Assert.False(AccountValidator.ValidatePin("123456").isValid);
        Assert.False(AccountValidator.ValidatePin("12a45").isValid);
    }

    [Fact]
    public void ValidatePin_ShouldPass_WhenPinHasExpectedFormat()
    {
        var result = AccountValidator.ValidatePin("12345");
        Assert.True(result.isValid);
        Assert.Null(result.error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidateAmount_ShouldFail_ForNonPositive(decimal amount)
    {
        var result = AccountValidator.ValidateAmount(amount);
        Assert.False(result.isValid);
        Assert.NotNull(result.error);
    }

    [Fact]
    public void ValidateAmount_ShouldPass_ForPositive()
    {
        var result = AccountValidator.ValidateAmount(10);
        Assert.True(result.isValid);
        Assert.Null(result.error);
    }

    [Fact]
    public void ValidateBalance_ShouldFail_WhenNegative()
    {
        var result = AccountValidator.ValidateBalance(-0.01m);
        Assert.False(result.isValid);
    }

    [Fact]
    public void ValidateBalance_ShouldPass_WhenZeroOrPositive()
    {
        Assert.True(AccountValidator.ValidateBalance(0).isValid);
        Assert.True(AccountValidator.ValidateBalance(100).isValid);
    }

    [Fact]
    public void ValidateStatus_ShouldFail_ForNullWhitespaceOrUnknown()
    {
        Assert.False(AccountValidator.ValidateStatus(null).isValid);
        Assert.False(AccountValidator.ValidateStatus(string.Empty).isValid);
        Assert.False(AccountValidator.ValidateStatus("Paused").isValid);
    }

    [Fact]
    public void ValidateStatus_ShouldPass_ForKnownStatuses()
    {
        Assert.True(AccountValidator.ValidateStatus(AccountConstants.StatusActive).isValid);
        Assert.True(AccountValidator.ValidateStatus(AccountConstants.StatusDisabled).isValid);
    }

    [Fact]
    public void ValidateHolderName_ShouldFail_ForNullOrWhitespace()
    {
        Assert.False(AccountValidator.ValidateHolderName(null).isValid);
        Assert.False(AccountValidator.ValidateHolderName(string.Empty).isValid);
        Assert.False(AccountValidator.ValidateHolderName("  ").isValid);
    }

    [Fact]
    public void ValidateHolderName_ShouldPass_ForValue()
    {
        var result = AccountValidator.ValidateHolderName("Alice Smith");
        Assert.True(result.isValid);
        Assert.Null(result.error);
    }
}
