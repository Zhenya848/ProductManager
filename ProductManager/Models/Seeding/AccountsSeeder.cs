using Microsoft.Extensions.Options;
using ProductManager.Models.Options;

namespace ProductManager.Models.Seeding;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
{
    public async Task SeedAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        
        var service = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();
        var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>().Value;
        
        await service.SeedAsync(adminOptions);
    }
}