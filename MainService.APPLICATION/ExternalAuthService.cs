// ============================================================================
// External Auth Service - Plug and Play Single File Solution
// ============================================================================
// This file contains everything needed for external JWT validation:
// - DTOs for auth service communication
// - Auth service client interface and implementation
// - JWT validation middleware
// - Extension methods for easy setup
//
// Usage in Program.cs:
//   1. Add: builder.Services.AddExternalAuthService(builder.Configuration);
//   2. Add: app.UseExternalAuthValidation();
// ============================================================================

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;

namespace ExternalAuth;

// ============================================================================
// DTOs
// ============================================================================

public class AuthorizationResponse
{
    public bool IsAuthorized { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Roles { get; set; }
    public Dictionary<string, string>? Claims { get; set; }
}

public class AuthorizationRequest
{
    public string Token { get; set; } = string.Empty;
    public string Service  { get; set; } = string.Empty;
    public string Controller { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? Method { get; set; }
}

public class AuthResponse
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public UserInfo? User { get; set; }
}

public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// ============================================================================
// Interface
// ============================================================================

public interface IExternalAuthServiceClient
{
    Task<AuthorizationResponse?> AuthorizeAsync(string token, string controller, string action);
    Task<AuthResponse?> LoginAsync(string username, string password);
    Task<AuthorizationResponse?> ValidateTokenAsync(string token, string controller, string action);
}

// ============================================================================
// Service Client Implementation
// ============================================================================

public class ExternalAuthServiceClient : IExternalAuthServiceClient
{
    private readonly HttpClient _httpClient;

    public ExternalAuthServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthorizationResponse?> AuthorizeAsync(string token, string controller, string action)
    {
        try
        {
            var request = new AuthorizationRequest
            {
                Token = token,
                Controller = controller,
                Action = action
            };

            var response = await _httpClient.PostAsJsonAsync("/Auth/authorize", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthorizationResponse>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return await response.Content.ReadFromJsonAsync<AuthorizationResponse>();
            }
            
            return new AuthorizationResponse
            {
                IsAuthorized = false,
                Message = $"Auth service returned status: {response.StatusCode}"
            };
        }
        catch (HttpRequestException)
        {
            return new AuthorizationResponse
            {
                IsAuthorized = false,
                Message = "Unable to reach auth service"
            };
        }
        catch (Exception)
        {
            return new AuthorizationResponse
            {
                IsAuthorized = false,
                Message = "An error occurred during authorization"
            };
        }
    }

    public async Task<AuthResponse?> LoginAsync(string username, string password)
    {
        try
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("/Auth/login", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            
            return new AuthResponse
            {
                Success = false,
                Message = $"Login failed: {response.StatusCode}",
                Token = string.Empty
            };
        }
        catch (HttpRequestException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Unable to reach auth service",
                Token = string.Empty
            };
        }
        catch (Exception)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during login",
                Token = string.Empty
            };
        }
    }

    public async Task<AuthorizationResponse?> ValidateTokenAsync(string token, string controller, string action)
    {
        try
        {
            var request = new AuthorizationRequest
            {
                Token = token,
                Service = "MainService",
                Controller = controller,
                Action = action
            };

            var response = await _httpClient.PostAsJsonAsync("/Auth/authorize", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthorizationResponse>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return await response.Content.ReadFromJsonAsync<AuthorizationResponse>();
            }
            
            return new AuthorizationResponse
            {
                IsAuthorized = false,
                Message = $"Token validation failed: {response.StatusCode}"
            };
        }
        catch (HttpRequestException)
        {
            return new AuthorizationResponse
            {
                IsAuthorized = false,
                Message = "Unable to reach auth service"
            };
        }
        catch (Exception)
        {
            return new AuthorizationResponse
            {
                IsAuthorized = false,
                Message = "An error occurred during token validation"
            };
        }
    }
}

// ============================================================================
// Middleware
// ============================================================================

public class ExternalAuthValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExternalAuthServiceClient _authServiceClient;
    private readonly ILogger<ExternalAuthValidationMiddleware> _logger;

    public ExternalAuthValidationMiddleware(
        RequestDelegate next,
        IExternalAuthServiceClient authServiceClient,
        ILogger<ExternalAuthValidationMiddleware> logger)
    {
        _next = next;
        _authServiceClient = authServiceClient;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldSkipValidation(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var token = ExtractToken(context);
        
        if (!string.IsNullOrEmpty(token))
        {
            var (controller, action) = ExtractControllerAndAction(context);
            var validationResult = await _authServiceClient.ValidateTokenAsync(token, controller, action);
            
            if (validationResult != null && validationResult.IsAuthorized)
            {
                List<Claim> claims;
                
                if (validationResult.Claims != null && validationResult.Claims.Count > 0)
                {
                    claims = BuildClaimsFromResponse(validationResult.Claims);
                }
                else
                {
                    claims = DecodeClaimsFromJwtToken(token);
                }
                
                if (validationResult.Roles != null && validationResult.Roles.Count > 0)
                {
                    foreach (var role in validationResult.Roles)
                    {
                        if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }
                
                if (claims.Count > 0)
                {
                    var identity = new ClaimsIdentity(claims, "ExternalAuth");
                    context.User = new ClaimsPrincipal(identity);
                }
            }
        }

        await _next(context);
    }

    private List<Claim> BuildClaimsFromResponse(Dictionary<string, string> claimsDict)
    {
        var claims = new List<Claim>();
        
        foreach (var claim in claimsDict)
        {
            if (claim.Key == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" || 
                claim.Key == ClaimTypes.Role)
            {
                var roleValue = claim.Value;
                if (roleValue.StartsWith("[") && roleValue.EndsWith("]"))
                {
                    try
                    {
                        var roles = JsonSerializer.Deserialize<string[]>(roleValue);
                        if (roles != null)
                        {
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
                            }
                        }
                    }
                    catch
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleValue));
                    }
                }
                else
                {
                    var roles = roleValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Trim().Trim('"').Trim('[').Trim(']')));
                    }
                }
            }
            else if (claim.Key == "permission")
            {
                var permissionValue = claim.Value;
                if (permissionValue.StartsWith("[") && permissionValue.EndsWith("]"))
                {
                    try
                    {
                        var permissions = JsonSerializer.Deserialize<string[]>(permissionValue);
                        if (permissions != null)
                        {
                            foreach (var permission in permissions)
                            {
                                claims.Add(new Claim("permission", permission.Trim()));
                            }
                        }
                    }
                    catch
                    {
                        claims.Add(new Claim("permission", permissionValue));
                    }
                }
                else
                {
                    var permissions = permissionValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var permission in permissions)
                    {
                        claims.Add(new Claim("permission", permission.Trim().Trim('"').Trim('[').Trim(']')));
                    }
                }
            }
            else
            {
                claims.Add(new Claim(claim.Key, claim.Value));
            }
        }

        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
            var usernameClaim = claims.FirstOrDefault(c => c.Type == "username" || c.Type == JwtRegisteredClaimNames.Sub);
            if (usernameClaim != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, usernameClaim.Value));
            }
        }

        return claims;
    }

    private List<Claim> DecodeClaimsFromJwtToken(string token)
    {
        var claims = new List<Claim>();
        
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return claims;
            }
            
            foreach (var claim in jwtToken.Claims)
            {
                if (claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" || 
                    claim.Type == ClaimTypes.Role)
                {
                    var roleValue = claim.Value;
                    if (roleValue.StartsWith("[") && roleValue.EndsWith("]"))
                    {
                        try
                        {
                            var roles = JsonSerializer.Deserialize<string[]>(roleValue);
                            if (roles != null)
                            {
                                foreach (var role in roles)
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
                                }
                            }
                        }
                        catch
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roleValue));
                        }
                    }
                    else
                    {
                        var roles = roleValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.Trim().Trim('"').Trim('[').Trim(']')));
                        }
                    }
                }
                else if (claim.Type == "permission")
                {
                    var permissionValue = claim.Value;
                    if (permissionValue.StartsWith("[") && permissionValue.EndsWith("]"))
                    {
                        try
                        {
                            var permissions = JsonSerializer.Deserialize<string[]>(permissionValue);
                            if (permissions != null)
                            {
                                foreach (var permission in permissions)
                                {
                                    claims.Add(new Claim("permission", permission.Trim()));
                                }
                            }
                        }
                        catch
                        {
                            claims.Add(new Claim("permission", permissionValue));
                        }
                    }
                    else
                    {
                        var permissions = permissionValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var permission in permissions)
                        {
                            claims.Add(new Claim("permission", permission.Trim().Trim('"').Trim('[').Trim(']')));
                        }
                    }
                }
                else
                {
                    claims.Add(claim);
                }
            }
            
            if (!claims.Any(c => c.Type == ClaimTypes.Name))
            {
                var usernameClaim = claims.FirstOrDefault(c => c.Type == "username" || c.Type == JwtRegisteredClaimNames.Sub);
                if (usernameClaim != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, usernameClaim.Value));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to decode JWT token");
        }
        
        return claims;
    }

    private string? ExtractToken(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(authHeader))
        {
            return null;
        }

        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }

        return authHeader.Trim();
    }

    private (string controller, string action) ExtractControllerAndAction(HttpContext context)
    {
        var routeData = context.GetRouteData();
        
        var controller = routeData?.Values["controller"]?.ToString();
        var action = routeData?.Values["action"]?.ToString();

        if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action))
        {
            var path = context.Request.Path.Value?.TrimStart('/');
            if (!string.IsNullOrEmpty(path))
            {
                var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length >= 2)
                {
                    controller = segments[0];
                    action = segments[1];
                }
                else if (segments.Length == 1)
                {
                    controller = segments[0];
                    action = context.Request.Method;
                }
            }
        }

        if (!string.IsNullOrEmpty(controller))
        {
            controller = controller.Replace("Controller", "", StringComparison.OrdinalIgnoreCase);
            controller = $"{char.ToUpper(controller[0])}{controller.Substring(1)}";
        }

        if (!string.IsNullOrEmpty(action))
        {
            action = $"{char.ToUpper(action[0])}{action.Substring(1)}";
        }

        return (controller ?? string.Empty, action ?? string.Empty);
    }

    private bool ShouldSkipValidation(PathString path)
    {
        var skipPaths = new[]
        {
            "/health",
            "/swagger",
            "/api/health",
            "/favicon.ico",
            "/login",
            "/Login",
        };

        return skipPaths.Any(skipPath => path.StartsWithSegments(skipPath, StringComparison.OrdinalIgnoreCase));
    }
}

// ============================================================================
// Extension Methods for Easy Setup
// ============================================================================

public static class ExternalAuthExtensions
{
    /// <summary>
    /// Adds external auth service to the dependency injection container.
    /// Configure in appsettings.json:
    ///   "AuthService:BaseUrl": "http://your-auth-service-url"
    ///   "useAuth": true
    /// </summary>
    public static IServiceCollection AddExternalAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        var useAuth = configuration.GetValue<bool>("useAuth", false);
        
        if (useAuth)
        {
            services.AddHttpClient<IExternalAuthServiceClient, ExternalAuthServiceClient>(client =>
            {
                var authServiceUrl = configuration["AuthService:BaseUrl"] 
                    ?? throw new InvalidOperationException("AuthService:BaseUrl is not configured");
                client.BaseAddress = new Uri(authServiceUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            
            services.AddAuthentication();
        }
        
        return services;
    }

    /// <summary>
    /// Adds external auth validation middleware to the request pipeline.
    /// Must be called after UseRouting() and before UseAuthentication()/UseAuthorization()
    /// </summary>
    public static IApplicationBuilder UseExternalAuthValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExternalAuthValidationMiddleware>();
    }
}

