using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniMdb.Auth
{
    public class JwtFactory
    {
        private static readonly DateTimeOffset D1970 = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        private readonly JwtSettings _settings;
        //private readonly string _issuedAt;

        public TimeSpan TokenValidFor => _settings.ValidFor;

        public TimeSpan RenewedBefore => _settings.RenewedBefore;

        public JwtFactory(JwtSettings settings)
        {
            ThrowIfInvalidOptions(settings);
            _settings = settings;
            //_issuedAt = ToUnixEpochDate(_settings.IssuedAt).ToString();
        }

        public string Generate(string email, params string[] roles)
        {
            return Generate(email, _settings.ValidFor, roles);
        }

        public string Generate(string email, TimeSpan validFor, params string[] roles)
        {
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, GenerateJti()),
            };
            foreach(var role in roles)
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create the JWT security token and encode it.
            var jwt = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(jwtClaims),
                Expires = DateTime.UtcNow.Add(validFor),
                SigningCredentials = _settings.SigningCredentials
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(jwt);
            return handler.WriteToken(token);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
            => (long) Math.Round((date.ToUniversalTime() - D1970).TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtSettings options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtSettings.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtSettings.SigningCredentials));
        }

        private static string GenerateJti() => Guid.NewGuid().ToString();
    }
}
