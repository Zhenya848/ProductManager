namespace ProductManager.Application.Users.Query;

public record GetUsersQuery(
    string? OrderBy = null,
    string? SearchString = null);