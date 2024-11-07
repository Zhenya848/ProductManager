namespace WebApplication1.Application.Users.Commands.Create;

public record CreateUserCommand(
    string Username,
    string Email,
    string FullName,
    string Password);