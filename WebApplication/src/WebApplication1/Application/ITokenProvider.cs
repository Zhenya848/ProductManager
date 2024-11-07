using WebApplication1.Models.User;

namespace WebApplication1.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}