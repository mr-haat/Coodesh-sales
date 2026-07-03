using Ambev.DeveloperEvaluation.Domain.Services;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest that defines the validation rules for sale updates.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleRequestValidator with defined validation rules.
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        RuleFor(sale => sale.SaleNumber).NotEmpty().MaximumLength(20);
        RuleFor(sale => sale.SaleDate).NotEmpty();
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(sale => sale.BranchId).NotEmpty();
        RuleFor(sale => sale.BranchName).NotEmpty().MaximumLength(100);
        RuleFor(sale => sale.Items).NotEmpty().WithMessage("A sale must contain at least one item.");
        RuleForEach(sale => sale.Items).SetValidator(new UpdateSaleItemRequestValidator());
    }
}

/// <summary>
/// Validator for a single item of an UpdateSaleRequest.
/// </summary>
public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleItemRequestValidator with defined validation rules.
    /// </summary>
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty().MaximumLength(100);
        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(SaleDiscountPolicy.MaxItemsPerProduct)
            .WithMessage($"It is not possible to sell more than {SaleDiscountPolicy.MaxItemsPerProduct} identical items.");
        RuleFor(item => item.UnitPrice).GreaterThan(0);
    }
}
