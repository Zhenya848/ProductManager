using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Abstractions;
using WebApplication1.Models;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User;
using WebApplication1.Models.User.Accounts;
using WebApplication1.Repositories.Accounts;

namespace WebApplication1.Application.Users.Commands.Create;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountRepository _accountRepository;

    public CreateUserHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountRepository = accountRepository;
    }

    public async Task<UnitResult<ErrorList>> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FullName))
            return (ErrorList)Errors.General.ValueIsInvalid("full name");
        
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist != null)
            return (ErrorList)Errors.User.AlreadyExist();
        
        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {ParticipantAccount.PARTICIPANT} does not exist");

        var user = User.CreateParticipant(command.Username, command.Email, command.FullName, role);
        var participantAccount = ParticipantAccount.CreateParticipant(user);

        _accountRepository.CreateParticipant(participantAccount);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return Result.Success<ErrorList>();
    }
}