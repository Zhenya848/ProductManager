using Microsoft.AspNetCore.Identity;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User.Accounts;

namespace WebApplication1.Models.User;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }

    private List<Role> _roles = [];
    
    public IReadOnlyList<Role> Roles => _roles;

    private User()
    {
        
    }
    
    private static User Create(
        string user, 
        string email,
        string fullName,
        Role role)
    {
        return new User
        {
            FullName = fullName,
            Email = email,
            
            UserName = user,
            _roles = [role]
        };
    }

    public static User CreateParticipant(
        string user,
        string email,
        string fullName,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, fullName, role);
    }

    public static User CreateModerator(
        string user,
        string email,
        string fullName,
        Role role)
    {
        if (role.Name != ModeratorAccount.MODERATOR)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, fullName, role);
    }
    
    public static User CreateAdmin(
        string user,
        string email,
        string fullName,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, fullName, role);
    }

    public void UpdateUser(
        string username,
        string fullName,
        string email)
    {
        UserName = username;
        FullName = fullName;
        Email = email;
    }
}