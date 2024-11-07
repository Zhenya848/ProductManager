namespace WebApplication1.Models.User.Accounts;

public class ModeratorAccount
{
    public const string MODERATOR = "Moderator";
    
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    private ModeratorAccount()
    {
        
    }
    
    public static ModeratorAccount CreateModerator(User user)
    {
        return new ModeratorAccount()
        {
            Id = Guid.NewGuid(),

            User = user
        };
    }
}