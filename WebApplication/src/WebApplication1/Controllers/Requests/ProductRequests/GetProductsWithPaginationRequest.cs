namespace WebApplication1.Controllers.Requests;

public record GetProductsWithPaginationRequest(
    int Page,
    int PageSize,
    string? OrderBy = null);