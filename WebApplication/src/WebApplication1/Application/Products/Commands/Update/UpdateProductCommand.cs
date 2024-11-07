namespace WebApplication1.Application.Products.Commands.Update;

public record UpdateProductCommand(
    Guid productId,
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    decimal Price,
    DateTime ExpirationDate);