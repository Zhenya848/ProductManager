using ProductManager.Models;
using ProductManager.Models.Dtos;
using ProductManager.Models.User;

namespace ProductManager.Repositories;

public interface IReadDbContext
{
    public IQueryable<ProductDto> Products { get; }
}