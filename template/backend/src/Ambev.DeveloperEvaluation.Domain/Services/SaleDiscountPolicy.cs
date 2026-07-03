namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Represents a discount tier: the discount rate that applies once the quantity
/// of identical items reaches the given minimum.
/// </summary>
/// <param name="MinimumQuantity">The minimum quantity required for the tier to apply.</param>
/// <param name="Rate">The discount rate of the tier, expressed as a fraction.</param>
public sealed record DiscountTier(int MinimumQuantity, decimal Rate);

/// <summary>
/// Centralizes the quantity-based discount rules for sale items, keeping the
/// thresholds, rates and quantity limit in a single place.
/// </summary>
public static class SaleDiscountPolicy
{
    /// <summary>
    /// The maximum quantity of identical items allowed in a single sale item.
    /// </summary>
    public const int MaxItemsPerProduct = 20;

    /// <summary>
    /// The discount tiers, ordered from the highest minimum quantity to the lowest
    /// so the first tier the quantity reaches is the one that applies.
    /// </summary>
    private static readonly IReadOnlyList<DiscountTier> Tiers = new[]
    {
        new DiscountTier(MinimumQuantity: 10, Rate: 0.20m),
        new DiscountTier(MinimumQuantity: 4, Rate: 0.10m)
    };

    /// <summary>
    /// Gets the discount rate that applies to the given quantity of identical items.
    /// </summary>
    /// <param name="quantity">The quantity of identical items.</param>
    /// <returns>The discount rate as a fraction, or zero when no tier applies.</returns>
    public static decimal GetRateFor(int quantity)
    {
        return Tiers.FirstOrDefault(tier => quantity >= tier.MinimumQuantity)?.Rate ?? 0m;
    }
}
