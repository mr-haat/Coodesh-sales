using Ambev.DeveloperEvaluation.Domain.Services;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand that defines the validation rules for sale updates.
/// </summary>
public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleCommandValidator with defined validation rules.
    /// </summary>
    public UpdateSaleCommandValidator()
    {
        RuleFor(sale => sale.Id).NotEmpty();
        RuleFor(sale => sale.SaleNumber).NotEmpty().MaximumLength(20);
        RuleFor(sale => sale.SaleDate).NotEmpty();
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(sale => sale.BranchId).NotEmpty();
        RuleFor(sale => sale.BranchName).NotEmpty().MaximumLength(100);
        RuleFor(sale => sale.Items).NotEmpty().WithMessage("A sale must contain at least one item.");
        RuleForEach(sale => sale.Items).SetValidator(new UpdateSaleItemCommandValidator());
    }
}

/// <summary>
/// Validator for a single item of an UpdateSaleCommand.
/// </summary>
public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleItemCommandValidator with defined validation rules.
    /// </summary>
    public UpdateSaleItemCommandValidator()
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
