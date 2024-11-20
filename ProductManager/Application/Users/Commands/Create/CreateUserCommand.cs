namespace ProductManager.Application.Users.Commands.Create;

public record CreateUserCommand(
    string NameOfUser,
    string Email,
    string FullName,
    string Password);