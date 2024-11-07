using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Abstractions;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;
using WebApplication1.Repositories.Accounts;

namespace WebApplication1.Application.Users.Commands.Delete;

public class DeleteUserHandler : ICommandHandler<string, Result<Guid, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountRepository _accountRepository;

    public DeleteUserHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        string userEmail, 
        CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.FindByEmailAsync(userEmail);

        if (userExist == null)
            return (ErrorList)Errors.User.NotFound($"User with email {userEmail} does not exist.");
        
        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {ParticipantAccount.PARTICIPANT} does not exist");

        switch (role.Name)
        {
            case ParticipantAccount.PARTICIPANT:
                var participantAccountExist = await _accountRepository
                    .GetParticipant(userExist.Id, cancellationToken);
                
                if (participantAccountExist.IsFailure)
                    return (ErrorList)participantAccountExist.Error;
                
                _accountRepository.DeleteParticipant(participantAccountExist.Value);
                
                break;
            
            case ModeratorAccount.MODERATOR:
                var moderatorAccountExist = await _accountRepository
                    .GetModerator(userExist.Id, cancellationToken);
                
                if (moderatorAccountExist.IsFailure)
                    return (ErrorList)moderatorAccountExist.Error;
                
                _accountRepository.DeleteModerator(moderatorAccountExist.Value);
                
                break;
        }
        
        var result = await _userManager.DeleteAsync(userExist);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return userExist.Id;
    }
}