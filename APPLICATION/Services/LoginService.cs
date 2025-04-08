using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


namespace APPLICATION.Services
{
    public class LoginService : ILogin
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecretKey;

        public LoginService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecretKey = configuration["JwtSecretKey"];
        }


        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || password != user.Password)
            {
                return new LoginResponse(false, "Login unsuccessful");
                
            }
            var token = GenerateJwtToken(username);
            return new LoginResponse(true, "Login successful", token);
        }
        private string GenerateJwtToken(string username)
        {
            // Create claims for the JWT token (in your example, it contains the username)
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username), // Sub is a standard claim for the subject (user)
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Jti is a unique identifier for the token
        };

            // Define the security key and algorithm used to sign the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the token expiration (e.g., 2 hours)
            var expires = DateTime.Now.AddHours(2);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: "your-app", // optional
                audience: "your-app", // optional
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
