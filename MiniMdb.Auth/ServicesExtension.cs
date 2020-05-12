using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MiniMdb.Auth
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, string secretKey, JwtSettings jwtSettings)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            jwtSettings.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            services.AddSingleton(jwtSettings);

            services.AddSingleton<JwtFactory>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = jwtSettings.SigningCredentials.Key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

            return services;
        }
    }
}
