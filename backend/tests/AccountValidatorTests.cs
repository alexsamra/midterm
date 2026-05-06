using Model;

namespace tests;

public class AccountValidatorTests
{
    [Fact]
    public void ValidateLogin_ShouldFail_WhenLoginIsNullOrWhitespace()
    {
        Assert.False(AccountValidator.ValidateLogin(null).IsValid);
        Assert.False(AccountValidator.ValidateLogin(string.Empty).IsValid);
        Assert.False(AccountValidator.ValidateLogin("   ").IsValid);
    }

    [Fact]
    public void ValidateLogin_ShouldPass_WhenLoginIsValid()
    {
        var result = AccountValidator.ValidateLogin("alice");
        Assert.True(result.IsValid);
        Assert.Null(result.Error);
    }

    [Fact]
    public void ValidatePin_ShouldFail_WhenPinIsInvalid()
    {
        Assert.False(AccountValidator.ValidatePin(null).IsValid);
        Assert.False(AccountValidator.ValidatePin("1234").IsValid);
        Assert.False(AccountValidator.ValidatePin("123456").IsValid);
        Assert.False(AccountValidator.ValidatePin("12a45").IsValid);
    }

    [Fact]
    public void ValidatePin_ShouldPass_WhenPinHasExpectedFormat()
    {
        var result = AccountValidator.ValidatePin("12345");
        Assert.True(result.IsValid);
        Assert.Null(result.Error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidateAmount_ShouldFail_ForNonPositive(decimal amount)
    {
        var result = AccountValidator.ValidateAmount(amount);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void ValidateAmount_ShouldPass_ForPositive()
    {
        var result = AccountValidator.ValidateAmount(10);
        Assert.True(result.IsValid);
        Assert.Null(result.Error);
    }

    [Fact]
    public void ValidateBalance_ShouldFail_WhenNegative()
    {
        var result = AccountValidator.ValidateBalance(-0.01m);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateBalance_ShouldPass_WhenZeroOrPositive()
    {
        Assert.True(AccountValidator.ValidateBalance(0).IsValid);
        Assert.True(AccountValidator.ValidateBalance(100).IsValid);
    }

    [Fact]
    public void ValidateStatus_ShouldFail_ForNullWhitespaceOrUnknown()
    {
        Assert.False(AccountValidator.ValidateStatus(null).IsValid);
        Assert.False(AccountValidator.ValidateStatus(string.Empty).IsValid);
        Assert.False(AccountValidator.ValidateStatus("Paused").IsValid);
    }

    [Fact]
    public void ValidateStatus_ShouldPass_ForKnownStatuses()
    {
        Assert.True(AccountValidator.ValidateStatus(AccountConstants.StatusActive).IsValid);
        Assert.True(AccountValidator.ValidateStatus(AccountConstants.StatusDisabled).IsValid);
    }

    [Fact]
    public void ValidateHolderName_ShouldFail_ForNullOrWhitespace()
    {
        Assert.False(AccountValidator.ValidateHolderName(null).IsValid);
        Assert.False(AccountValidator.ValidateHolderName(string.Empty).IsValid);
        Assert.False(AccountValidator.ValidateHolderName("  ").IsValid);
    }

    [Fact]
    public void ValidateHolderName_ShouldPass_ForValue()
    {
        var result = AccountValidator.ValidateHolderName("Alice Smith");
        Assert.True(result.IsValid);
        Assert.Null(result.Error);
    }
}
