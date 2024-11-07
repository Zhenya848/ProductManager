using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Abstractions;
using WebApplication1.Models.Shared;
using WebApplication1.Models.User;

namespace WebApplication1.Application.Users.Commands.Login;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<string, Error>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserHandler(
        UserManager<User> userManager, 
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<string, Error>> Handle(
        LoginUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        
        if (user == null)
            return Errors.User.NotFound(command.Email);
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);

        if (passwordConfirmed == false)
            return Errors.User.WrongCredentials();

        var accessToken = _tokenProvider.GenerateAccessToken(user);

        return accessToken;
    }
}