using CSharpFunctionalExtensions;
using WebApplication1.Models.Shared;

namespace WebApplication1.Models;

public class Product
{
    public Guid Id { get; private set; }
    
    public string ProductName { get; private set; }
    public string Type { get; private set;}
    public string Description { get; private set;}
    public string FunctionalRequirements { get; private set;}
    
    public decimal Price { get; private set;}
    public DateTime ExpirationDate { get; private set;}
    
    public Product(
        Guid id, 
        string productName, 
        string type, 
        string description, 
        string functionalRequirements,
        decimal price,
        DateTime expirationDate)
    {
        Id = id;
        ProductName = productName;
        Type = type;
        Description = description;
        FunctionalRequirements = functionalRequirements;
        Price = price;
        ExpirationDate = expirationDate;
    }
    
    public void UpdateInfo(
        string productName,
        string type,
        string description,
        string functionalRequirements,
        decimal price,
        DateTime expirationDate)
    {
        ProductName = productName;
        Type = type;
        Description = description;
        FunctionalRequirements = functionalRequirements;
        Price = price;
        ExpirationDate = expirationDate;
    }
}