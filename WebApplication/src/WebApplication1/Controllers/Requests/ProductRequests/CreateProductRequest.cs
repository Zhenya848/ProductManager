namespace WebApplication1.Controllers.Requests;

public record CreateProductRequest(
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    decimal Price,
    DateTime ExpirationDate);