namespace ProductManager.Models.Dtos;

public record ProductDto(
    Guid Id,
    string ProductName,
    string Type,
    string Description,
    string FunctionalRequirements,
    float Price,
    DateTime ExpirationDate);