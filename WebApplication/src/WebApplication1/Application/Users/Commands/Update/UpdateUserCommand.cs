namespace WebApplication1.Application.Users.Commands.Update;

public record UpdateUserCommand(
    string Email,
    string Username,
    string FullName,
    string? RoleName);