using Library.Domain.Enums;
using Library.Infrastructure.Crosscutting.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Library.Infrastructure.Crosscutting
{
    public static class SetupDependencies
    {
        public static void AddAuthorizationDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAuthService, AuthService>();
            services.AddScoped<ILogService, LogService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserRoleEnum.Librarian.ToString(), policy => policy.RequireClaim(ClaimTypes.Role, UserRoleEnum.Librarian.ToString()));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("SuperSecureKey"))),
                };
            });
        }
    }
}
