using Ambev.DeveloperEvaluation.Domain.Services;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

/// <summary>
/// Contains unit tests for the SaleDiscountPolicy discount tiers.
/// </summary>
public class SaleDiscountPolicyTests
{
    /// <summary>
    /// Tests that the discount rate returned by the policy matches the quantity tier,
    /// covering the boundaries of every tier.
    /// </summary>
    [Theory(DisplayName = "The discount rate matches the quantity tier")]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Given_Quantity_When_GettingRate_Then_ReturnsTierRate(int quantity, int expectedPercentage)
    {
        // Act
        var rate = SaleDiscountPolicy.GetRateFor(quantity);

        // Assert
        Assert.Equal(expectedPercentage / 100m, rate);
    }
}
