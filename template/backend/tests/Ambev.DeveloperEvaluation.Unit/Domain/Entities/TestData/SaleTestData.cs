using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides Sale and SaleItem test data using the Bogus library, keeping the
/// generation rules in a single place so tests stay consistent.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .RuleFor(i => i.Id, f => f.Random.Guid())
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 1000));

    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(s => s.SaleNumber, f => f.Random.Replace("SALE-#####"))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.CustomerName, f => f.Person.FullName)
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName());

    /// <summary>
    /// Generates a valid sale with the requested number of items already added.
    /// </summary>
    /// <param name="itemCount">The number of items to add to the sale.</param>
    /// <returns>A sale populated with valid, randomized data.</returns>
    public static Sale GenerateValidSale(int itemCount = 3)
    {
        var sale = SaleFaker.Generate();
        foreach (var item in SaleItemFaker.Generate(itemCount))
            sale.AddItem(item);

        return sale;
    }

    /// <summary>
    /// Generates a single item with a controlled quantity and unit price.
    /// </summary>
    /// <param name="quantity">The quantity of the item.</param>
    /// <param name="unitPrice">The unit price of the item.</param>
    /// <returns>A sale item with the given quantity and price.</returns>
    public static SaleItem GenerateItem(int quantity, decimal unitPrice = 100m)
    {
        return new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Sample product",
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}
