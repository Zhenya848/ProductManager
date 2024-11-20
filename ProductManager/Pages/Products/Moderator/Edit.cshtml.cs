using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Products.Commands.Delete;
using ProductManager.Application.Products.Commands.Update;
using ProductManager.Authorization;
using ProductManager.Extensions;
using ProductManager.Models;

namespace ProductManager.Pages.Products.Moderator
{
    public class EditModel(
        UpdateProductHandler updateHandler, 
        DeleteProductHandler deleteHandler,
        PermissionsAccessor permissionsAccessor,
        CancellationToken cancellationToken = default) : PageModel
    {
        [BindProperty]
        public Product Product { get; set; }

        public bool IsHavePermission { get; set; } = false;
        public string ErrorMessage { get; set; } = String.Empty;

        public async Task OnGet()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("products.update");
        }
        
        public async Task<IActionResult> OnPostUpdate(Guid id)
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("products.update");
            
            if (IsHavePermission == false)
                return RedirectToPage("/Index");
            
            var command = new UpdateProductCommand(
                id,
                Product.ProductName,
                Product.Type,
                Product.Description,
                Product.FunctionalRequirements,
                Product.Price,
                Product.ExpirationDate);

            var result = await updateHandler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }
            
            return RedirectToPage("/Products/List");
        }
        
        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("products.delete");
            
            if (IsHavePermission == false)
                return RedirectToPage("/Index");
            
            var result = await deleteHandler.Handle(id, cancellationToken);

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }
            
            return RedirectToPage("/Products/List");
        }
    }
}
