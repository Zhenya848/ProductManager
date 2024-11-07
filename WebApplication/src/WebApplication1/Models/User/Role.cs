using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models.User;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}