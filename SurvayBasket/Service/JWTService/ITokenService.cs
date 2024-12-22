using Microsoft.AspNetCore.Identity;
using SurvayBasket.Models;

namespace SurvayBasket.Service.JWTService
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUser user, IList<string> Roles,CancellationToken cancellationToken);
    }
}
