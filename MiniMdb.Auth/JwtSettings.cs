using Microsoft.IdentityModel.Tokens;
using System;

namespace MiniMdb.Auth
{
    public class JwtSettings
    {
        /// <summary>
        /// Set the timespan the token will be valid for (default is 120 min)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);

        public TimeSpan RenewedBefore { get; set; } = TimeSpan.FromMinutes(20);

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
