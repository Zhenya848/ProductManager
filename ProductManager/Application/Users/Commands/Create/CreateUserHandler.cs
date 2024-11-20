using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Models.User;
using ProductManager.Models.User.Accounts;
using ProductManager.Repositories.Accounts;

namespace ProductManager.Application.Users.Commands.Create;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager,
        IAccountRepository accountRepository,
        ILogger<CreateUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email))
            return (ErrorList)Errors.General.ValueIsRequired(command.Email);
        
        if (string.IsNullOrWhiteSpace(command.FullName))
            return (ErrorList)Errors.General.ValueIsInvalid("full name");
        
        if (string.IsNullOrWhiteSpace(command.NameOfUser))
            return (ErrorList)Errors.General.ValueIsInvalid("User name)");

        var emails = _userManager.Users.Select(e => e.Email);
        
        if (emails.Contains(command.Email))
            return (ErrorList)Errors.User.AlreadyExist();
        
        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {ParticipantAccount.PARTICIPANT} does not exist");

        var user = User.CreateParticipant(command.NameOfUser, command.Email, command.FullName, role);
        var participantAccount = ParticipantAccount.CreateParticipant(user);

        _accountRepository.CreateParticipant(participantAccount);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return Result.Success<ErrorList>();
    }
}