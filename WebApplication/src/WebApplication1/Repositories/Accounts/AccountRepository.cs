using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;

namespace WebApplication1.Repositories.Accounts;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _appDbContext;

    public AccountRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Guid CreateParticipant(
        ParticipantAccount participantAccount)
    {
        var addResult = _appDbContext.ParticipantAccounts
            .Add(participantAccount);
        
        return participantAccount.Id;
    }
    
    public Guid CreateModerator(
        ModeratorAccount moderatorAccount)
    {
        var addResult = _appDbContext.ModeratorAccounts
            .Add(moderatorAccount);
        
        return moderatorAccount.Id;
    }
    
    public Guid CreateAdmin(
        AdminAccount adminAccount)
    {
        var addResult = _appDbContext.AdminAccounts
            .Add(adminAccount);

        return adminAccount.Id;
    }

    public Guid DeleteParticipant(
        ParticipantAccount participantAccount)
    {
        var removeResult = _appDbContext.ParticipantAccounts
            .Remove(participantAccount);
        
        return participantAccount.Id;
    }

    public Guid DeleteModerator(
        ModeratorAccount moderatorAccount)
    {
        var removeResult = _appDbContext.ModeratorAccounts
            .Remove(moderatorAccount);
        
        return moderatorAccount.Id;
    }

    public async Task<Result<ParticipantAccount, Error>> GetParticipant(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var participant = await _appDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        
        if (participant == null)
            return Errors.User.NotFound();

        return participant;
    }

    public async Task<Result<ModeratorAccount, Error>> GetModerator(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var moderator = await _appDbContext.ModeratorAccounts
            .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken);
        
        if (moderator == null)
            return Errors.User.NotFound();

        return moderator;
    }
}