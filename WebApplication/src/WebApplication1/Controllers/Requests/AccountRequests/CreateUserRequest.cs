namespace WebApplication1.Controllers.Requests.AccountRequests;

public record CreateUserRequest(
    string Username,
    string Email,
    string FullName,
    string Password);