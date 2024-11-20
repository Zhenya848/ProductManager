using CSharpFunctionalExtensions;
using ProductManager.Abstractions;
using ProductManager.Models;
using ProductManager.Models.Shared;
using ProductManager.Repositories.Products;

namespace ProductManager.Application.Products.Commands.Create;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result<Guid, ErrorList>>
{
    private readonly IProductsRepository _productsRepository;

    public CreateProductHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.ProductName))
            return (ErrorList)Errors.General.ValueIsInvalid("product name");
        
        if (string.IsNullOrEmpty(command.Type))
            return (ErrorList)Errors.General.ValueIsInvalid("type");
        
        if (string.IsNullOrEmpty(command.Description))
            return (ErrorList)Errors.General.ValueIsInvalid("description");
        
        if (string.IsNullOrEmpty(command.FunctionalRequirements))
            return (ErrorList)Errors.General.ValueIsInvalid("functional requirements");
        
        if (command.Price <= 0)
            return (ErrorList)Error.Validation("price.is.invalid", "Price must be greater than 0");
        
        var product = new Product(
            Guid.NewGuid(), 
            command.ProductName, 
            command.Type,
            command.Description,
            command.FunctionalRequirements,
            command.Price,
            command.ExpirationDate);

        await _productsRepository.Create(product, cancellationToken);

        return product.Id;
    }
}