using ProductManager.Abstractions;
using ProductManager.Application;
using ProductManager.Application.Products.Commands.Create;
using ProductManager.Authorization;
using ProductManager.Data;
using ProductManager.Models.Options;
using ProductManager.Models.Seeding;
using ProductManager.Models.User;
using ProductManager.Repositories;
using ProductManager.Repositories.Accounts;
using ProductManager.Repositories.Products;

namespace ProductManager;

public static class Injects
{
    public static IServiceCollection AddInjects(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<PermissionsAccessor>();
        
        services.AddIdentification();

        services.AddOptions(config);
        
        services.AddDatabase();

        services.AddSeeding();

        services.AddAuthentication();
        
        services.AddRepositories();
        
        services.AddCommandsAndQueries();
        
        return services;
    }

    private static void AddIdentification(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AdminOptions>(
            config.GetSection(AdminOptions.ADMIN));
        
        services.AddOptions<AdminOptions>();
    }

    private static void AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
    }

    private static void AddSeeding(this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
    }

    private static void AddCommandsAndQueries(this IServiceCollection services)
    {
        var assembly = typeof(Injects).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
    }
}