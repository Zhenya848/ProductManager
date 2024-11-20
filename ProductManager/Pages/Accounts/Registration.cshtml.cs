using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Application.Users.Commands.Create;
using ProductManager.Application.Users.Commands.Login;
using ProductManager.Extensions;
using ProductManager.Models.Shared;
using ProductManager.Models.User;

namespace ProductManager.Pages.Accounts
{
    public class RegistrationModel(
        CreateUserHandler registerHandler, 
        LoginUserHandler loginHandler,
        CancellationToken cancellationToken = default) : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public async Task<IActionResult> OnPost()
        {
            var command = new CreateUserCommand(
                User.NameOfUser, 
                User.Email, User.FullName, 
                Request.Form["Password"]);
        
            var result = await registerHandler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }
            
            var loginCommand = new LoginUserCommand(User.Email, Request.Form["Password"]);
            var loginResult = await loginHandler.Handle(loginCommand, cancellationToken);

            if (loginResult.IsFailure)
            {
                ErrorMessage = result.Error.GetResponse();
                return Page();
            }
            
            HttpContext.Response.Cookies.Append(Constants.USER_INFO, loginResult.Value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            
            return Redirect("~/Products/List");
        }
    }
}
