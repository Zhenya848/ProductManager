using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using WebApplication1.Models;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;
using Constants = WebApplication1.Models.Shared.Constants;

namespace WebApplication1.Data;

public class AppDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Permission> Permissions => Set<Permission>();
    
    public DbSet<Product> Products => Set<Product>();
    
    public DbSet<AdminAccount > AdminAccounts => Set<AdminAccount>();
    public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
    public DbSet<ModeratorAccount> ModeratorAccounts => Set<ModeratorAccount>();
    
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
        
        modelBuilder.Entity<User>().ToTable("users");
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
        modelBuilder.Entity<User>().Property(f => f.FullName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        modelBuilder.Entity<AdminAccount>().ToTable("admin_accounts");
        modelBuilder.Entity<AdminAccount>().HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(i => i.UserId);
        
        modelBuilder.Entity<ParticipantAccount>().ToTable("participant_accounts");
        modelBuilder.Entity<ParticipantAccount>().HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<ParticipantAccount>(i => i.UserId);
        
        modelBuilder.Entity<ModeratorAccount>().ToTable("moderators_accounts");
        modelBuilder.Entity<ModeratorAccount>().HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<ModeratorAccount>(i => i.UserId);
        
        modelBuilder.Entity<Role>().ToTable("roles");
        
        modelBuilder.Entity<Permission>().ToTable("permissions");
        modelBuilder.Entity<Permission>().HasIndex(c => c.Code).IsUnique();
        modelBuilder.Entity<Permission>().Property(d => d.Description).HasMaxLength(200);
        
        modelBuilder.Entity<RolePermission>().ToTable("role_permissions");
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);
        
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        
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