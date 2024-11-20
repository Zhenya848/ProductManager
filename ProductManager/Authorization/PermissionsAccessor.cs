using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Models.Shared;

namespace ProductManager.Authorization;

public class PermissionsAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionsAccessor(
        IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory scopeFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _scopeFactory = scopeFactory;
    }

    public async Task<bool> IsHavePermission(string permissionName)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        var userInfo = httpContext.Request.Cookies.ContainsKey(Constants.USER_INFO) 
            ? httpContext.Request.Cookies[Constants.USER_INFO] 
            : null;
        
        if (userInfo == null)
            return false;
        
        Guid userId = Guid.Empty;
        
        if (Guid.TryParse(userInfo, out userId) == false)
            return false;
        
        var scope = _scopeFactory.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
        var permissions = await appDbContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        return permissions.Contains(permissionName);
    }
}