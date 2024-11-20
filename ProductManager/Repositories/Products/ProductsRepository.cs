using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Models;
using ProductManager.Models.Shared;

namespace ProductManager.Repositories.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductsRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<Guid> Create(Product product, CancellationToken cancellationToken = default)
    {
        await _appDbContext.Products.AddAsync(product, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return product.Id;
    }

    public async Task<Guid> Save(Product product, CancellationToken cancellationToken = default)
    {
        _appDbContext.Products.Attach(product);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return product.Id;
    }
    
    public async Task<Guid> Delete(Product product, CancellationToken cancellationToken = default)
    {
        _appDbContext.Products.Remove(product);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return product.Id;
    }
    
    public async Task<Result<Product, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var productExist = await _appDbContext.Products
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        
        if (productExist == null)
            return Errors.General.NotFound(id);

        return productExist;
    }
}