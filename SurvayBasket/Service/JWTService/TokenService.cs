using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SurvayBasket.Models;
using SurvayBasket.Service.JWTService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurvayBasket.Service.JWTSericves
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateToken(ApplicationUser user)
        {
            Claim[] claims =
                [
                  new(JwtRegisteredClaimNames.Sub,user.Id),
                  new(JwtRegisteredClaimNames.Email,user.Email!),
                  new(JwtRegisteredClaimNames.Name,user.FristName),
                  new(JwtRegisteredClaimNames.FamilyName,user.LastName),
                  new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                ];

            var SummetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"]!));

            var signinCredintials = new SigningCredentials(SummetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"]!)),
                signingCredentials: signinCredintials

                );


            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
