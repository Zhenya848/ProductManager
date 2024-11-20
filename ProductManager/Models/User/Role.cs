using Microsoft.AspNetCore.Identity;

namespace ProductManager.Models.User;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}