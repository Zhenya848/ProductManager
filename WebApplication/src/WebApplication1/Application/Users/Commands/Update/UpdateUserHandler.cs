using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Abstractions;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;
using WebApplication1.Repositories.Accounts;

namespace WebApplication1.Application.Users.Commands.Update;

public class UpdateUserHandler : ICommandHandler<UpdateUserCommand, Result<Guid, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountRepository _accountRepository;

    public UpdateUserHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FullName))
            return (ErrorList)Errors.General.ValueIsInvalid("full name");
        
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist == null)
            return (ErrorList)Errors.User.NotFound();
        
        userExist.UpdateUser(command.Username, command.FullName, command.Email);

        if (string.IsNullOrWhiteSpace(command.RoleName) == false)
        {
            var role = await _roleManager.FindByNameAsync(command.RoleName.ToLower());
            
            if (role == null)
                return (ErrorList)Error
                    .NotFound("role.not.found", $"Role with name {command.RoleName} was not found");

            await _userManager.AddToRoleAsync(userExist, role.Name!);

            switch (role.Name)
            {
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
        }
        
        await _userManager.UpdateAsync(userExist);

        return userExist.Id;
    }
}