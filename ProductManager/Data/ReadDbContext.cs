using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManager.Models;
using ProductManager.Models.Dtos;
using ProductManager.Models.Shared;
using ProductManager.Models.User;
using ProductManager.Models.User.Accounts;
using ProductManager.Repositories;
using Constants = ProductManager.Models.Shared.Constants;

namespace ProductManager.Data;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<ProductDto> Products => Set<ProductDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ProductDto>().ToTable("products");
        
        modelBuilder.Entity<ProductDto>().HasKey(p => p.Id);

        modelBuilder.Entity<ProductDto>()
            .Property(t => t.ProductName)
            .IsRequired();

        modelBuilder.Entity<ProductDto>()
            .Property(t => t.Type);

        modelBuilder.Entity<ProductDto>()
            .Property(t => t.Description);

        modelBuilder.Entity<ProductDto>()
            .Property(t => t.Price).HasColumnType("REAL")
            .IsRequired();

        modelBuilder.Entity<ProductDto>()
            .Property(t => t.ExpirationDate).HasColumnType("DATE");

        modelBuilder.Entity<ProductDto>()
            .Property(t => t.FunctionalRequirements)
            .IsRequired();
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}