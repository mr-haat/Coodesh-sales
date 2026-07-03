using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity, focused on the discount rules.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that applying the discount rules sets the item discount and total.
    /// </summary>
    [Theory(DisplayName = "The item discount and total reflect the applied rate")]
    [InlineData(3, 0)]
    [InlineData(5, 10)]
    [InlineData(15, 20)]
    public void Given_Quantity_When_ApplyingDiscountRules_Then_DiscountAndTotalAreCalculated(int quantity, int expectedPercentage)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Sample product",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.ApplyDiscountRules();

        // Assert
        var grossAmount = quantity * 100m;
        var expectedDiscount = grossAmount * expectedPercentage / 100m;
        Assert.Equal(expectedDiscount, item.Discount);
        Assert.Equal(grossAmount - expectedDiscount, item.Total);
    }

    /// <summary>
    /// Tests that a cancelled item is flagged as cancelled.
    /// </summary>
    [Fact(DisplayName = "A cancelled item is flagged as cancelled")]
    public void Given_Item_When_Cancelled_Then_IsCancelledIsTrue()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Sample product",
            Quantity = 5,
            UnitPrice = 10m
        };

        // Act
        item.Cancel();

        // Assert
        Assert.True(item.IsCancelled);
    }
}
