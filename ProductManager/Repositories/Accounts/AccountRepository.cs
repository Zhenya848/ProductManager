using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Models.Shared;
using ProductManager.Models.User.Accounts;

namespace ProductManager.Repositories.Accounts;

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

    public async Task<UnitResult<Error>> DeleteParticipant(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var participant = await _appDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);
        
        if (participant == null)
            return Errors.User.NotFound();

        var result = _appDbContext.ParticipantAccounts
            .Remove(participant);

        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteModerator(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var moderator = await _appDbContext.ModeratorAccounts
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);
        
        if (moderator == null)
            return Errors.User.NotFound();

        var result = _appDbContext.ModeratorAccounts
            .Remove(moderator);
        
        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteAdmin(Guid userId, CancellationToken cancellationToken = default)
    {
        var admin = await _appDbContext.AdminAccounts
            .FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);
        
        if (admin == null)
            return Errors.User.NotFound();

        var result = _appDbContext.AdminAccounts
            .Remove(admin);
        
        return Result.Success<Error>();
    }

    public async Task SaveChanges()
    {
        await _appDbContext.SaveChangesAsync();
    }
}