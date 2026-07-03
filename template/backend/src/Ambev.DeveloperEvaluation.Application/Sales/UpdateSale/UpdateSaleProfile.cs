using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping the UpdateSale command items to sale item entities.
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for the UpdateSale operation.
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleItemCommand, SaleItem>();
    }
}
