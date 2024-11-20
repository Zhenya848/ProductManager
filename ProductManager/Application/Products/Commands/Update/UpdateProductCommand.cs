namespace ProductManager.Application.Products.Commands.Update;

public record UpdateProductCommand(
    Guid productId,
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    float Price,
    DateTime ExpirationDate);