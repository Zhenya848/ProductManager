using Microsoft.Build.Framework;

namespace ProductManager.Models;

public class Product
{
    public Guid Id { get; set; }
    
    public string ProductName { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string FunctionalRequirements { get; set; }
    public float Price { get; set; }
    public DateTime ExpirationDate { get; set;}

    public Product()
    {
        
    }
    
    public Product(
        Guid id, 
        string productName, 
        string type, 
        string description, 
        string functionalRequirements,
        float price,
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
        float price,
        DateTime expirationDate)
    {
        ProductName = string.IsNullOrWhiteSpace(productName) ? ProductName : productName;
        Type = string.IsNullOrWhiteSpace(type) ? Type : type;
        Description = string.IsNullOrWhiteSpace(description) ? Description : description;
        FunctionalRequirements = string.IsNullOrWhiteSpace(functionalRequirements) 
            ? FunctionalRequirements : functionalRequirements;
        Price = price > 0 ? price : Price;
        ExpirationDate = expirationDate.Year < DateTime.UtcNow.Year ? ExpirationDate : expirationDate;
    }
}