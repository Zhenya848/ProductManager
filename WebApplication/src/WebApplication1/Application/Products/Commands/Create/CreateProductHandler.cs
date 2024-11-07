using CSharpFunctionalExtensions;
using WebApplication1.Abstractions;
using WebApplication1.Models;
using WebApplication1.Models.Shared;
using WebApplication1.Repositories.Products;

namespace WebApplication1.Application.Products.Commands.Create;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result<Guid, Error>>
{
    private readonly IProductsRepository _productsRepository;

    public CreateProductHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken = default)
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