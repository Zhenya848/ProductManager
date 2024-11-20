using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Models.User;
using ProductManager.Models.User.Accounts;
using ProductManager.Repositories.Accounts;

namespace ProductManager.Application.Users.Commands.UpdateRole;

public class UpdateRoleHandler : ICommandHandler<UpdateRoleCommand, Result<Guid, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountRepository _accountRepository;

    public UpdateRoleHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRoleCommand command, 
        CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.Users.Include(r => r.Roles)
            .FirstOrDefaultAsync(i => i.Id == command.UserId, cancellationToken);

        if (userExist == null)
            return (ErrorList)Errors.User.NotFound();
        
        if (string.IsNullOrWhiteSpace(command.RoleName))
            return (ErrorList)Errors.General.ValueIsRequired("Role name");
        
        var newRole = await _roleManager.FindByNameAsync(command.RoleName);
        
        if (newRole == null)
            return (ErrorList)Errors.General.ValueIsInvalid("Role name");

        if (userExist.Roles.Count > 0)
        {
            var allRoles = userExist.Roles.Select(n => n.Name);
            await _userManager.RemoveFromRolesAsync(userExist, allRoles);
        }
        
        await _userManager.AddToRoleAsync(userExist, newRole.Name);
        
        await _accountRepository.DeleteParticipant(userExist.Id, cancellationToken);
        await _accountRepository.DeleteModerator(userExist.Id, cancellationToken);
        await _accountRepository.DeleteAdmin(userExist.Id, cancellationToken);

        switch (newRole.Name)
        {
            case ParticipantAccount.PARTICIPANT:
                var participantAccount = ParticipantAccount.CreateParticipant(userExist);
                _accountRepository.CreateParticipant(participantAccount);
                
                break;
            case ModeratorAccount.MODERATOR:
            {
                var moderatorAccount = ModeratorAccount.CreateModerator(userExist);
                _accountRepository.CreateModerator(moderatorAccount);
                    
                break;
            }
            case AdminAccount.ADMIN:
            {
                var adminAccount = AdminAccount.CreateAdmin(userExist);
                _accountRepository.CreateAdmin(adminAccount);
                    
                break;
            }
        }

        await _accountRepository.SaveChanges();
        
        return userExist.Id;
    }
}