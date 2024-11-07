using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using WebApplication1.Models;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;
using WebApplication1.Repositories;
using Constants = WebApplication1.Models.Shared.Constants;

namespace WebApplication1.Data;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<Product> Products => Set<Product>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>().ToTable("products");
        
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        
        modelBuilder.Entity<Product>()
            .Property(t => t.ProductName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        modelBuilder.Entity<Product>()
            .Property(t => t.Type);

        modelBuilder.Entity<Product>()
            .Property(t => t.Description);

        modelBuilder.Entity<Product>()
            .Property(t => t.Price).HasColumnType("REAL")
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(t => t.ExpirationDate).HasColumnType("DATE");

        modelBuilder.Entity<Product>()
            .Property(t => t.FunctionalRequirements)
            .IsRequired();
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}