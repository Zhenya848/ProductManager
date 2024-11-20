using CSharpFunctionalExtensions;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Repositories.Products;

namespace ProductManager.Application.Products.Commands.Update;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result<Guid, ErrorList>>
{
    private readonly IProductsRepository _productsRepository;

    public UpdateProductHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        var productExist = await _productsRepository
            .GetById(command.productId, cancellationToken);
        
        if (productExist.IsFailure)
            return (ErrorList)productExist.Error;
        
        productExist.Value.UpdateInfo(
            command.ProductName, 
            command.Type,
            command.Description,
            command.FunctionalRequirements,
            command.Price,
            command.ExpirationDate);

        var result = await _productsRepository.Save(productExist.Value, cancellationToken);

        return result;
    }
}