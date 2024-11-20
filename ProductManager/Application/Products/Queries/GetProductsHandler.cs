using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProductManager.Abstractions;
using ProductManager.Extensions;
using ProductManager.Models;
using ProductManager.Models.Dtos;
using ProductManager.Models.Shared;
using ProductManager.Repositories;

namespace ProductManager.Application.Products.Queries;

public class GetProductsHandler : IQueryHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IReadDbContext _readDbContext;

    public GetProductsHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<IReadOnlyList<ProductDto>> Handle(
        GetProductsQuery query, 
        CancellationToken cancellationToken = default)
    {
        var productsQuery = _readDbContext.Products;

        Expression<Func<ProductDto, object>> selector = query.OrderBy?.ToLower() switch
        {
            "название" => product => product.ProductName,
            "тип" => product => product.Type,
            "описание" => product => product.Description,
            "характеристики" => product => product.FunctionalRequirements,
            "цена" => product => product.Price,
            "срок годности" => product => product.ExpirationDate,
            _ => product => product.ProductName
        };

        productsQuery = productsQuery.OrderBy(selector);
        
        if (string.IsNullOrWhiteSpace(query.SearchString) == false)
            productsQuery = productsQuery.Where(p => p.ProductName.Contains(query.SearchString));

        var result = await productsQuery.ToListAsync(cancellationToken);

        return result;
    }
}