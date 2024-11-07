using Microsoft.Extensions.Options;
using WebApplication1.Models.Options;

namespace WebApplication1.Models.Seeding;

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