namespace Ambev.DeveloperEvaluation.Application.Common;

/// <summary>
/// Configuration for the pagination of list endpoints, bound from the "Pagination"
/// section of the application settings.
/// </summary>
public class PaginationSettings
{
    /// <summary>
    /// The name of the configuration section that holds these settings.
    /// </summary>
    public const string SectionName = "Pagination";

    /// <summary>
    /// The page size used when a request does not specify one.
    /// </summary>
    public int DefaultPageSize { get; set; } = 10;

    /// <summary>
    /// The maximum page size a request is allowed to ask for.
    /// </summary>
    public int MaxPageSize { get; set; } = 50;
}
