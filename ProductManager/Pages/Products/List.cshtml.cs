using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Products.Queries;
using ProductManager.Authorization;
using ProductManager.Models;
using ProductManager.Models.Dtos;

namespace ProductManager.Pages.Products
{
    public class ListModel(
        GetProductsHandler handler, 
        PermissionsAccessor permissionsAccessor,
        CancellationToken cancellationToken = default) : PageModel
    {
        [BindProperty]
        public string? OrderBy { get; set; }
        
        [BindProperty]
        public string? SearchString { get; set; }
        
        public bool IsHavePermissions { get; set; } = false;
        
        public IReadOnlyList<ProductDto> Products { get; set; }
        
        public async Task OnGet()
        {
            IsHavePermissions = await permissionsAccessor.IsHavePermission("products.get");
            
            var query = new GetProductsQuery(null);

            Products = await handler.Handle(query, cancellationToken);
        }
        
        public async Task OnPost()
        {
            IsHavePermissions = await permissionsAccessor.IsHavePermission("products.get");
            
            var query = new GetProductsQuery(OrderBy, SearchString);

            Products = await handler.Handle(query, cancellationToken);
        }
    }
}
