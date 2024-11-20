using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Products.Queries;
using ProductManager.Application.Users.Query;
using ProductManager.Authorization;
using ProductManager.Models.Dtos;
using ProductManager.Models.User;
using ProductManager.Repositories;

namespace ProductManager.Pages.Accounts.Admin
{
    public class ListModel(
        GetUsersHandler handler, 
        PermissionsAccessor permissionsAccessor,
        CancellationToken cancellationToken = default) : PageModel
    {
        [BindProperty]
        public string? OrderBy { get; set; }
        
        [BindProperty]
        public string? SearchString { get; set; }
        
        public bool IsHavePermission { get; set; } = false;
        
        public IReadOnlyList<UserDto> Users { get; set; }
        
        
        public async Task<IActionResult> OnGet()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("users.get");
            
            if (IsHavePermission == false)
                return Page();
            
            var query = new GetUsersQuery(null);

            Users = await handler.Handle(query, cancellationToken);
            
            return Page();
        }
        
        public async Task<IActionResult> OnPost()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("users.get");
            
            if (IsHavePermission == false)
                return Page();
            
            var query = new GetUsersQuery(OrderBy, SearchString);

            Users = await handler.Handle(query, cancellationToken);

            return Page();
        }
    }
}
