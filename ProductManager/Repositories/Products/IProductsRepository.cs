using CSharpFunctionalExtensions;
using ProductManager.Models;
using ProductManager.Models.Shared;

namespace ProductManager.Repositories.Products;

public interface IProductsRepository
{
    public Task<Guid> Create(Product product, CancellationToken cancellationToken = default);
    
    public Task<Guid> Save(Product product, CancellationToken cancellationToken = default);
    
    public Task<Result<Product, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

    public Task<Guid> Delete(Product product, CancellationToken cancellationToken = default);
}