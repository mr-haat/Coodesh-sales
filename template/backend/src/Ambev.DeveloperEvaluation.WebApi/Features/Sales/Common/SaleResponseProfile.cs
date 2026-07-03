using Ambev.DeveloperEvaluation.Application.Sales.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

/// <summary>
/// Profile for mapping application sale results to API responses.
/// </summary>
public class SaleResponseProfile : Profile
{
    /// <summary>
    /// Initializes the mappings shared by the sale endpoints.
    /// </summary>
    public SaleResponseProfile()
    {
        CreateMap<SaleResult, SaleResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
    }
}
