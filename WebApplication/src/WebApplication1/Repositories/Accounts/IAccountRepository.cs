using CSharpFunctionalExtensions;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;

namespace WebApplication1.Repositories.Accounts;

public interface IAccountRepository
{
    public Guid CreateParticipant(
        ParticipantAccount participantAccount);
    
    public Guid CreateModerator(
        ModeratorAccount moderatorAccount);
    
    public Guid CreateAdmin(
        AdminAccount adminAccount);
    
    public Guid DeleteParticipant(
        ParticipantAccount participantAccount);
    
    public Guid DeleteModerator(
        ModeratorAccount moderatorAccount);
    
    public Task<Result<ParticipantAccount, Error>> GetParticipant(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    public Task<Result<ModeratorAccount, Error>> GetModerator(
        Guid userId,
        CancellationToken cancellationToken = default);
}