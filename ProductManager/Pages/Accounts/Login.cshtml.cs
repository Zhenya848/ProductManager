using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Users.Commands.Login;
using ProductManager.Extensions;
using ProductManager.Models.Options;
using ProductManager.Models.Shared;
using ProductManager.Models.User;

namespace ProductManager.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly LoginUserHandler _handler;

        public LoginModel(
            LoginUserHandler handler)
        {
            _handler = handler;
        }
        
        [BindProperty]
        public User User { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        
        
        public async Task<IActionResult> OnPost(CancellationToken cancellationToken = default)
        {
            var command = new LoginUserCommand(User.Email, Request.Form["Password"]);
            var result = await _handler.Handle(command, cancellationToken);
        
            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }
            
            HttpContext.Response.Cookies.Append(Constants.USER_INFO, result.Value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Redirect("~/Products/List");
        }
    }
}
