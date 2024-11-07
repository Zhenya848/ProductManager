using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Abstractions;
using WebApplication1.Application;
using WebApplication1.Application.Products.Commands.Create;
using WebApplication1.Authorization;
using WebApplication1.Data;
using WebApplication1.Models.Options;
using WebApplication1.Models.Seeding;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Token;
using WebApplication1.Repositories;
using WebApplication1.Repositories.Accounts;
using WebApplication1.Repositories.Products;

namespace WebApplication1;

public static class Injects
{
    public static IServiceCollection AddInjects(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.Configure<AdminOptions>(
            config.GetSection(AdminOptions.ADMIN));
        
        services.Configure<JwtOptions>(
            config.GetSection(JwtOptions.JWT));
        
        services.AddOptions<AdminOptions>();
        services.AddOptions<JwtOptions>();
        
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        
        services.AddScoped<AppDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();

        services.AddScoped<CreateProductHandler>();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtOptions = config.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                             ?? throw new ApplicationException("Missing JWT configuration");

            options.TokenValidationParameters = TokenValidationParametersFactory
                .CreateWithLifeTime(jwtOptions);
        });
        
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        
        var assembly = typeof(Injects).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
        
        return services;
    }
}