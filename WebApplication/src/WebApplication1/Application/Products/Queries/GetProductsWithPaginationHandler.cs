using System.Linq.Expressions;
using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using WebApplication1.Abstractions;
using WebApplication1.Extensions;
using WebApplication1.Models;
using WebApplication1.Models.Shared;
using WebApplication1.Repositories;

namespace WebApplication1.Application.Products.Queries;

public class GetProductsWithPaginationHandler : IQueryHandler<GetProductsWithPaginationQuery, 
    Result<PagedList<Product>, Error>>
{
    private readonly IReadDbContext _readDbContext;

    public GetProductsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<PagedList<Product>, Error>> Handle(
        GetProductsWithPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        if (query.Page <= 0)
            return Error.Validation("page.is.invalid","Page must be greater than 0");
        
        if (query.PageSize <= 0)
            return Error.Validation("page_size.is.invalid","Page size must be greater than 0");
        
        var productsQuery = _readDbContext.Products;

        Expression<Func<Product, object>> selector = query.OrderBy?.ToLower() switch
        {
            "product_name" => product => product.ProductName,
            "type" => product => product.Type,
            "description" => product => product.Description,
            "functional_requirements" => product => product.FunctionalRequirements,
            "price" => product => product.Price,
            _ => product => product.Id
        };

        productsQuery = productsQuery.OrderBy(selector);
        
        return await productsQuery.GetItemsWithPagination(query.Page, query.PageSize, cancellationToken);
    }
}