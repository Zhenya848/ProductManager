using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Users.Commands.UpdateRole;
using ProductManager.Authorization;
using ProductManager.Extensions;
using ProductManager.Models.User;
using ProductManager.Models.User.Accounts;

namespace ProductManager.Pages.Accounts.Admin
{
    public class EditRoleModel(
        PermissionsAccessor permissionsAccessor,
        UpdateRoleHandler handler,
        CancellationToken cancellationToken = default) : PageModel
    {
        public bool IsHavePermission { get; set; } = false;
        public string ErrorMessage { get; set; } = String.Empty;
        
        public async Task OnGet()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("user.update");
        }
        
        public async Task<IActionResult> OnPostParticipant(Guid Id)
        {
            return await ChangeRole(Id, ParticipantAccount.PARTICIPANT);
        }
        
        public async Task<IActionResult> OnPostModerator(Guid Id)
        {
            return await ChangeRole(Id, ModeratorAccount.MODERATOR);
        }
        
        public async Task<IActionResult> OnPostAdmin(Guid Id)
        {
            return await ChangeRole(Id, AdminAccount.ADMIN);
        }

        private async Task<IActionResult> ChangeRole(Guid Id, string roleName)
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("user.update");

            if (IsHavePermission == false)
                return Page();

            var command = new UpdateRoleCommand(Id, roleName);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }
            
            return Redirect("~/Accounts/Admin/List");
        }
    }
}
