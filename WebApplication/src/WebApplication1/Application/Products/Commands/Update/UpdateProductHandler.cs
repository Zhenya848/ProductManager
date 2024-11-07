using CSharpFunctionalExtensions;
using WebApplication1.Abstractions;
using WebApplication1.Models;
using WebApplication1.Models.Shared;
using WebApplication1.Repositories.Products;

namespace WebApplication1.Application.Products.Commands.Update;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result<Guid, Error>>
{
    private readonly IProductsRepository _productsRepository;

    public UpdateProductHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.ProductName))
            return Errors.General.ValueIsInvalid("product name");
        
        if (string.IsNullOrEmpty(command.Type))
            return Errors.General.ValueIsInvalid("type");
        
        if (string.IsNullOrEmpty(command.Description))
            return Errors.General.ValueIsInvalid("description");
        
        if (string.IsNullOrEmpty(command.FunctionalRequirements))
            return Errors.General.ValueIsInvalid("functional requirements");
        
        if (command.Price <= 0)
            return Error.Validation("price.is.invalid", "Price must be greater than 0");
        
        var productExist = await _productsRepository
            .GetById(command.productId, cancellationToken);
        
        if (productExist.IsFailure)
            return productExist.Error;
        
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