using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Models.User;
using ProductManager.Models.User.Accounts;
using ProductManager.Repositories.Accounts;

namespace ProductManager.Application.Users.Commands.Update;

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
        var userExist = await _userManager.Users
            .FirstOrDefaultAsync(i => i.Id == command.UserId, cancellationToken);

        if (userExist == null)
            return (ErrorList)Errors.User.NotFound();

        var emails = _userManager.Users.Select(e => e.Email);

        if (emails.Contains(command.Email))
            return (ErrorList)Errors.User.AlreadyExist();

        userExist.UpdateUser(command.Username, command.FullName, command.Email);
        
        await _userManager.UpdateAsync(userExist);
        await _accountRepository.SaveChanges();

        return userExist.Id;
    }
}