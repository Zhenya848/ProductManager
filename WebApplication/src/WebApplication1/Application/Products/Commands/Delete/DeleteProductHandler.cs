using CSharpFunctionalExtensions;
using WebApplication1.Abstractions;
using WebApplication1.Models.Shared;
using WebApplication1.Repositories.Products;

namespace WebApplication1.Application.Products.Commands.Delete;

public class DeleteProductHandler : ICommandHandler<Guid, Result<Guid, Error>>
{
    private readonly IProductsRepository _productsRepository;

    public DeleteProductHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(Guid priductId, CancellationToken cancellationToken = default)
    {
        var productExist = await _productsRepository
            .GetById(priductId, cancellationToken);
        
        if (productExist.IsFailure)
            return productExist.Error;

        var result = await _productsRepository.Delete(productExist.Value, cancellationToken);

        return result;
    }
}