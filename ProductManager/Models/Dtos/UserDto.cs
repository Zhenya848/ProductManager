using ProductManager.Models.User;

namespace ProductManager.Models.Dtos;

public record UserDto(Guid Id, string NameOfUser, string FullName, string Email, string? RoleNames = null);