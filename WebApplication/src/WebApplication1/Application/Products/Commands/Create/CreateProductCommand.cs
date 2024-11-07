namespace WebApplication1.Application.Products.Commands.Create;

public record CreateProductCommand(
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    decimal Price,
    DateTime ExpirationDate);