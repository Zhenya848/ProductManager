namespace WebApplication1.Controllers.Requests;

public record UpdateProductRequest(
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    decimal Price,
    DateTime ExpirationDate);