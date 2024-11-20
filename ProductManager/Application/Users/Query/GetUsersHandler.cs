using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductManager.Abstractions;
using ProductManager.Application.Products.Queries;
using ProductManager.Data;
using ProductManager.Models;
using ProductManager.Models.Dtos;
using ProductManager.Models.User;
using ProductManager.Repositories;

namespace ProductManager.Application.Users.Query;

public class GetUsersHandler : IQueryHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetUsersHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<IReadOnlyList<UserDto>> Handle(
        GetUsersQuery query, 
        CancellationToken cancellationToken = default)
    {
        var usersQuery = _appDbContext.Users
            .Include(r => r.Roles).AsQueryable();

        Expression<Func<User, object>> selector = query.OrderBy?.ToLower() switch
        {
            "имя" => user => user.NameOfUser ?? "",
            "полное имя" => user => user.FullName,
            "почта" => user => user.Email ?? "",
            "роль" => user => user.Roles.FirstOrDefault().Name ?? "",
            _ => user => user.Id
        };
        
        usersQuery = usersQuery.OrderBy(selector);

        if (string.IsNullOrWhiteSpace(query.SearchString) == false)
            usersQuery = usersQuery.Where(u => u.NameOfUser.Contains(query.SearchString));

        var result = await usersQuery.ToListAsync(cancellationToken);

        var usersDto = new List<UserDto>();
        
        foreach (var user in result)
        {
            var id = user.Id;
            var userName = user.NameOfUser;
            var fullName = user.FullName;
            var email = user.Email;
            var roleNames = string.Empty;

            if (user.Roles.Count > 0)
            {
                for (int i = 0; i < user.Roles.Count - 1; i++)
                    roleNames += $"{user.Roles[i].Name}, ";
            
                roleNames += $"{user.Roles[^1].Name}";
            }
            
            usersDto.Add(new UserDto(id, userName, fullName, email, roleNames));
        }
        
        return usersDto;
    }
}