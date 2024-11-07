namespace WebApplication1.Application.Products.Queries;

public record GetProductsWithPaginationQuery(
    int Page,
    int PageSize,
    string? OrderBy = null);