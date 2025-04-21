using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using CORE.Entities;


namespace APPLICATION.Services
{
    public class LoginService(ICommonRepository _commonRepository, IConfiguration configuration) : ILogin
    {
        private readonly string _jwtSecretKey = configuration["JwtSecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "JwtSecretKey is not configured.");

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _commonRepository.GetFirstOrDefaultAsync<User>(filter: u => u.Username == username);
            if (user == null || password != user.Password)
            {
                return new LoginResponse(false, "Login unsuccessful");
            }
            var token = GenerateJwtToken(username);
            return new LoginResponse(true, "Login successful", token);
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                   new Claim(JwtRegisteredClaimNames.Sub, username),
                   new Claim(ClaimTypes.Name, username),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: "your-app",
                audience: "your-app",
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
