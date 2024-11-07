namespace WebApplication1.Models.User.Accounts;

public class AdminAccount
{
    public const string ADMIN = "Admin";
    
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    private AdminAccount()
    {
        
    }
    
    public static AdminAccount CreateAdmin(User user)
    {
        return new AdminAccount()
        {
            Id = Guid.NewGuid(),
            User = user
        };
    }
}