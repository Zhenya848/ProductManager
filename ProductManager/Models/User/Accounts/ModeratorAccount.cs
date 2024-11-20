namespace ProductManager.Models.User.Accounts;

public class ModeratorAccount
{
    public const string MODERATOR = "Модератор";
    
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