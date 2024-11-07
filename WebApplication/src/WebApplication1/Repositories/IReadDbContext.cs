using WebApplication1.Models;

namespace WebApplication1.Repositories;

public interface IReadDbContext
{
    public IQueryable<Product> Products { get; }
}