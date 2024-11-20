namespace ProductManager.Application.Products.Queries;

public record GetProductsQuery(
    string? OrderBy = null,
    string? SearchString = null);