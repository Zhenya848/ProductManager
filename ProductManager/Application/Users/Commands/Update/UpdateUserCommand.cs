namespace ProductManager.Application.Users.Commands.Update;

public record UpdateUserCommand(
    Guid UserId,
    string Email,
    string Username,
    string FullName);