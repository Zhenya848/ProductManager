namespace ProductManager.Application.Products.Commands.Create;

public record CreateProductCommand(
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    float Price,
    DateTime ExpirationDate);