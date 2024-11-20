namespace ProductManager.Models.User.Accounts;

public class ParticipantAccount
{
    public const string PARTICIPANT = "Пользователь";
    
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    private ParticipantAccount()
    {
        
    }
    
    public static ParticipantAccount CreateParticipant(User user)
    {
        return new ParticipantAccount()
        {
            Id = Guid.NewGuid(),

            User = user
        };
    }
}