using MainService.INFRASTRUCTURE;
using MainService.APPLICATION.Services;
using MainService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ExternalAuth;

var builder = WebApplication.CreateBuilder(args);

// Add service information
var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "MainService";
var serviceVersion = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "1.0.0";

Console.WriteLine($"Starting {serviceName} v{serviceVersion}");

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    // Example: ignore reference loops
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    // Example: ignore null values
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MainService API",
        Version = "v1"
    });
    
    // Add JWT Bearer authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
// Register services for DI
builder.Services.AddApplicationServices();

// Check useAuth setting
var useAuth = builder.Configuration.GetValue<bool>("useAuth", false);

// Add external auth service (replaces manual HTTP client setup when useAuth is true)
// This registers IExternalAuthServiceClient which is used by both middleware and LoginService
builder.Services.AddExternalAuthService(builder.Configuration);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://stage-myfinance.onrender.com", "https://myfinance.chanuthperera.com", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Allow credentials for authenticated requests
    });
});
builder.Services.AddHttpContextAccessor();

if (!useAuth)
{
    // Use generic JWT validation
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecretKey"] ?? throw new InvalidOperationException("JwtSecretKey is not configured")))
        };
    });
}
else
{
    // Use external auth service - middleware will validate tokens and set up user identity
    // No authentication scheme needed since middleware handles it
    builder.Services.AddAuthentication();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
// Apply CORS policy globally (before Authorization)
app.UseCors("AllowFrontend");

// HTTPS redirection is handled by Cloudflare, so we don't need this
// app.UseHttpsRedirection();

// Add external auth validation middleware (before authentication/authorization)
if (useAuth)
{
    app.UseExternalAuthValidation();
}

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();