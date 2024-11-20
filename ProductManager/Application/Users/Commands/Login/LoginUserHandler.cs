using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Models.User;

namespace ProductManager.Application.Users.Commands.Login;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<string, ErrorList>>
{
    private readonly UserManager<User> _userManager;

    public LoginUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<string, ErrorList>> Handle(
        LoginUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email))
            return (ErrorList)Errors.General.ValueIsRequired(command.Email);

        var user = await _userManager.Users.Where(e => e.Email == command.Email)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
            return (ErrorList)Errors.User.NotFound(command.Email);
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);

        if (passwordConfirmed == false)
            return (ErrorList)Errors.User.WrongCredentials();

        var result = user.Id.ToString();

        return result;
    }
}