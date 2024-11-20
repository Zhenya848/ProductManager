using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Users.Commands.Delete;
using ProductManager.Application.Users.Commands.Update;
using ProductManager.Authorization;
using ProductManager.Extensions;
using ProductManager.Models.User;

namespace ProductManager.Pages.Accounts.Admin
{
    public class EditModel(
        UpdateUserHandler updateHandler, 
        DeleteUserHandler deleteHandler,
        PermissionsAccessor permissionsAccessor,
        CancellationToken cancellationToken = default) : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        public bool IsHavePermission { get; set; } = false;
        public string ErrorMessage { get; set; } = String.Empty;
            

        public async Task OnGet()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("user.update");
        }
        
        public async Task<IActionResult> OnPostUpdate(Guid Id)
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("user.update");
            
            if (IsHavePermission == false)
                return RedirectToPage("/Index");
            
            var command = new UpdateUserCommand(
                Id,
                User.Email, 
                User.UserName, 
                User.FullName);
        
            var result = await updateHandler.Handle(command, cancellationToken);
        
            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }

            return RedirectToPage("/Accounts/Admin/List");
        }
        
        public async Task<IActionResult> OnPostDelete(Guid Id)
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("user.delete");
            
            if (IsHavePermission == false)
                return RedirectToPage("/Index");
            
            var result = await deleteHandler.Handle(Id, cancellationToken);
        
            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }

            return RedirectToPage("/Accounts/Admin/List");
        }
    }
}
