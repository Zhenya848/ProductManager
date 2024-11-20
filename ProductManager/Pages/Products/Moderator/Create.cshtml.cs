using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Products.Commands.Create;
using ProductManager.Authorization;
using ProductManager.Extensions;
using ProductManager.Models;
using ProductManager.Models.Shared;

namespace ProductManager.Pages.Products.Moderator
{
    public class CreateModel(
        CreateProductHandler handler, 
        PermissionsAccessor permissionsAccessor,
        CancellationToken cancellationToken = default) : PageModel
    {
        [BindProperty]
        public Product Product { get; set; }

        public bool IsHavePermission { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;


        public async Task OnGet()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("products.create");
        }
        
        public async Task<IActionResult> OnPost()
        {
            IsHavePermission = await permissionsAccessor.IsHavePermission("products.create");
            
            if (IsHavePermission == false)
                return RedirectToPage("/Index");
            
            var command = new CreateProductCommand(
                Product.ProductName,
                Product.Type,
                Product.Description,
                Product.FunctionalRequirements,
                Product.Price,
                Product.ExpirationDate);
            
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }

            return RedirectToPage("/Products/List");
        }
    }
}
