namespace WebApplication1.Controllers.Requests.AccountRequests;

public record UpdateUserRequest(
    string Username,
    string FullName,
    string? RoleName);