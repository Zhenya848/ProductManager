using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.Users.Commands.Create;
using WebApplication1.Application.Users.Commands.Delete;
using WebApplication1.Application.Users.Commands.Login;
using WebApplication1.Application.Users.Commands.Update;
using WebApplication1.Authorization;
using WebApplication1.Controllers.Requests.AccountRequests;
using WebApplication1.Extensions;

namespace WebApplication1.Controllers;

public class AccountsController : ApplicationController
{
    [HttpPost("register")]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        [FromServices] CreateUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(request.Username, request.Email, request.FullName, request.Password);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
    }

    [Permission("user.delete")]
    [HttpDelete("user/{userEmail}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string userEmail,
        [FromServices] DeleteUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(userEmail, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission("user.update")]
    [HttpPut("user/{userEmail}")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserRequest request,
        [FromRoute] string userEmail,
        [FromServices] UpdateUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserCommand(
            userEmail, 
            request.Username, 
            request.FullName, 
            request.RoleName);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}