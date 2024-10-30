using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoAPI.Models;

namespace TodoAPI.Auth
{
    public class TokenService{
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user){
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Add GUID for UserId
                
            };

            #pragma warning disable CS8604 // Possible null reference argument.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            #pragma warning restore CS8604 // Possible null reference argument.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpirationInMinutes"]));

            var token = new JwtSecurityToken(
                _config["JwtSettings:Issuer"],
                _config["JwtSettings:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}