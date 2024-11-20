using CSharpFunctionalExtensions;
using ProductManager.Models.Shared;
using ProductManager.Models.User.Accounts;

namespace ProductManager.Repositories.Accounts;

public interface IAccountRepository
{
    public Guid CreateParticipant(
        ParticipantAccount participantAccount);
    
    public Guid CreateModerator(
        ModeratorAccount moderatorAccount);
    
    public Guid CreateAdmin(
        AdminAccount adminAccount);
    
    public Task<UnitResult<Error>> DeleteParticipant(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    public Task<UnitResult<Error>> DeleteModerator(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    public Task<UnitResult<Error>> DeleteAdmin(
        Guid userId,
        CancellationToken cancellationToken = default);

    public Task SaveChanges();
}