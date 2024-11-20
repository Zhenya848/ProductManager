using CSharpFunctionalExtensions;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Repositories.Products;

namespace ProductManager.Application.Products.Commands.Delete;

public class DeleteProductHandler : ICommandHandler<Guid, Result<Guid, ErrorList>>
{
    private readonly IProductsRepository _productsRepository;

    public DeleteProductHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(Guid priductId, CancellationToken cancellationToken = default)
    {
        var productExist = await _productsRepository
            .GetById(priductId, cancellationToken);
        
        if (productExist.IsFailure)
            return (ErrorList)productExist.Error;

        var result = await _productsRepository.Delete(productExist.Value, cancellationToken);

        return result;
    }
}