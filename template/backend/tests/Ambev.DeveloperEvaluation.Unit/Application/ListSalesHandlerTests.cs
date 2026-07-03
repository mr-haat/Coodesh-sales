using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="ListSalesHandler"/> class, focused on the
/// pagination behavior driven by the configuration.
/// </summary>
public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesHandler _handler;

    /// <summary>
    /// Initializes the test dependencies with a known pagination configuration.
    /// </summary>
    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        var options = Options.Create(new PaginationSettings { DefaultPageSize = 10, MaxPageSize = 50 });
        _handler = new ListSalesHandler(_saleRepository, _mapper, options);

        _mapper.Map<List<SaleResult>>(Arg.Any<object>()).Returns(new List<SaleResult>());
    }

    private void StubList(int totalCount)
    {
        _saleRepository.ListAsync(Arg.Any<SaleListFilter>(), Arg.Any<CancellationToken>())
            .Returns(_ => ((IReadOnlyList<Sale>)new List<Sale>(), totalCount));
    }

    /// <summary>
    /// Tests that a request without a page size uses the configured default.
    /// </summary>
    [Fact(DisplayName = "Given no page size When listing Then uses the default page size")]
    public async Task Handle_NoPageSize_UsesDefault()
    {
        // Given
        StubList(0);

        // When
        await _handler.Handle(new ListSalesCommand { Page = 1, PageSize = 0 }, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<SaleListFilter>(f => f.PageSize == 10),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a request asking for more than the maximum page size is clamped.
    /// </summary>
    [Fact(DisplayName = "Given page size over the maximum When listing Then clamps to the maximum")]
    public async Task Handle_PageSizeOverMaximum_ClampsToMaximum()
    {
        // Given
        StubList(0);

        // When
        await _handler.Handle(new ListSalesCommand { Page = 1, PageSize = 999 }, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<SaleListFilter>(f => f.PageSize == 50),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the total number of pages is calculated from the total count.
    /// </summary>
    [Fact(DisplayName = "Given a total count When listing Then calculates the total pages")]
    public async Task Handle_WithTotalCount_CalculatesTotalPages()
    {
        // Given
        StubList(25);

        // When
        var result = await _handler.Handle(new ListSalesCommand { Page = 1, PageSize = 10 }, CancellationToken.None);

        // Then
        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3);
    }
}
