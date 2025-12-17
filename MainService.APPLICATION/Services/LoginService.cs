using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MainService.APPLICATION.DTOs;
using MainService.APPLICATION.Interfaces;
using MainService.CORE.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using MainService.CORE.Entities;
using ExternalAuth;


namespace MainService.APPLICATION.Services
{
    public class LoginService(ICommonRepository _commonRepository, IConfiguration configuration, IActivityLogService _logService, IExternalAuthServiceClient authServiceClient) : ILogin
    {
        private readonly string _jwtSecretKey = configuration["JwtSecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "JwtSecretKey is not configured.");
        private readonly bool _useAuth = bool.TryParse(configuration["useAuth"], out var useAuthValue) && useAuthValue;

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            // If useAuth is true, forward to Auth API
            if (_useAuth)
            {
                var authResponse = await authServiceClient.LoginAsync(username, password);
                if (authResponse != null && authResponse.Success)
                {
                    _logService.ChangeLog(username);
                    _logService.ChangeLog(authResponse.Token);
                    await _logService.FlushAsync("Login Successful using Auth API");
                    return new LoginResponse(true, "Login Successful", authResponse.Token);
                }
                else
                {
                    return new LoginResponse(false, authResponse?.Message ?? "Login unsuccessful");
                }
            }

            // Original logic when useAuth is false
            var user = await _commonRepository.GetFirstOrDefaultAsync<User>(filter: u => u.Username == username);
            if (user == null || password != user.Password)
            {
                return new LoginResponse(false, "Login unsuccessful");
            }
            var token = GenerateJwtToken(username);
            _logService.ChangeLog(username);
            _logService.ChangeLog(token);
            await _logService.FlushAsync("Login Successful");
            return new LoginResponse(true, "Login successful", token);
        }

        public async Task<LoginResponse> AuthenticateMobileAsync(string username, string password)
        {
            // If useAuth is true, forward to Auth API
            if (_useAuth)
            {
                var authResponse = await authServiceClient.LoginAsync(username, password);
                if (authResponse != null && authResponse.Success)
                {
                    _logService.ChangeLog(username);
                    _logService.ChangeLog(authResponse.Token);
                    await _logService.FlushAsync("Mobile Login Successful");
                    return new LoginResponse(true, authResponse.Message, authResponse.Token);
                }
                else
                {
                    return new LoginResponse(false, authResponse?.Message ?? "Login unsuccessful");
                }
            }

            // Original logic when useAuth is false
            var user = await _commonRepository.GetFirstOrDefaultAsync<User>(filter: u => u.Username == username);
            if (user == null || password != user.Password)
            {
                return new LoginResponse(false, "Login unsuccessful");
            }
            var token = GenerateJwtTokenMobile(username);
            _logService.ChangeLog(username);
            _logService.ChangeLog(token);
            await _logService.FlushAsync("Mobile Login Successful");
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

        private string GenerateJwtTokenMobile(string username)
        {
            var claims = new[]
            {
                   new Claim(JwtRegisteredClaimNames.Sub, username),
                   new Claim(ClaimTypes.Name, username),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(7); // 1 week expiration for mobile

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
