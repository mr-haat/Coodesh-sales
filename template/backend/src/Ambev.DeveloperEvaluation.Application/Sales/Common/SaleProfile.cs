using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

/// <summary>
/// Profile for mapping Sale entities to their application result models.
/// </summary>
public class SaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings shared by the sale operations.
    /// </summary>
    public SaleProfile()
    {
        CreateMap<Sale, SaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
    }
}
