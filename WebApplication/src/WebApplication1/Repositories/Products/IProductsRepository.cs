using CSharpFunctionalExtensions;
using WebApplication1.Models;
using WebApplication1.Models.Shared;

namespace WebApplication1.Repositories.Products;

public interface IProductsRepository
{
    public Task<Guid> Create(Product product, CancellationToken cancellationToken = default);
    
    public Task<Guid> Save(Product product, CancellationToken cancellationToken = default);
    
    public Task<Result<Product, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

    public Task<Guid> Delete(Product product, CancellationToken cancellationToken = default);
}