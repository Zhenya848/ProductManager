using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManager.Abstractions;
using ProductManager.Models.Shared;
using ProductManager.Models.User;
using ProductManager.Models.User.Accounts;
using ProductManager.Repositories.Accounts;

namespace ProductManager.Application.Users.Commands.Delete;

public class DeleteUserHandler : ICommandHandler<Guid, Result<Guid, ErrorList>>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.Users
            .FirstOrDefaultAsync(i => i.Id == userId, cancellationToken);

        if (userExist == null)
            return (ErrorList)Errors.User.NotFound();
        
        var result = await _userManager.DeleteAsync(userExist);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return userExist.Id;
    }
}