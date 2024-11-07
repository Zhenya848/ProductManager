using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Shared;

namespace WebApplication1.Authorization
{
    public class PermissionRequirementHandler
        : AuthorizationHandler<PermissionAttribute>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionRequirementHandler(
            IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }
        
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionAttribute permission)
        {
            var userIdString = context.User.Claims
                .FirstOrDefault(c => c.Type == CustomClaims.Sub)
                ?.Value;
            Guid userId = Guid.Empty;
            
            if (userIdString is null || Guid.TryParse(userIdString, out userId) == false)
                return;
            
            var scope = _scopeFactory.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            var permissions = await appDbContext.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();
            
            if (permissions.Contains(permission.Code))
                 context.Succeed(permission);
        }
    }
}
