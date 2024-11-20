namespace ProductManager.Application.Users.Commands.UpdateRole;

public record UpdateRoleCommand(Guid UserId, string RoleName);