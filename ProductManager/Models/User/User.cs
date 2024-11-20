using Microsoft.AspNetCore.Identity;
using ProductManager.Models.User.Accounts;

namespace ProductManager.Models.User;

public class User : IdentityUser<Guid>
{
    public string NameOfUser { get; set; }
    public string FullName { get; set; }

    private List<Role> _roles = [];
    
    public IReadOnlyList<Role> Roles => _roles;

    public User()
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
            UserName = Guid.NewGuid().ToString(),
            
            FullName = fullName,
            Email = email,
            
            NameOfUser = user,
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
        NameOfUser = string.IsNullOrWhiteSpace(username) ? NameOfUser : username;
        FullName = string.IsNullOrWhiteSpace(fullName) ? FullName : fullName;
        Email = string.IsNullOrWhiteSpace(email) ? Email : email;
    }
}