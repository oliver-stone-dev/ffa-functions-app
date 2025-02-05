using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ffa_functions_app.Services;

internal class TokenAuthenticationService : ITokenAuthenticationService
{
    readonly ILogger<TokenAuthenticationService> _logger;

    public TokenAuthenticationService(ILogger<TokenAuthenticationService> logger) => _logger = logger;

    public ClaimsPrincipal ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwt = token.Replace("Bearer ", string.Empty);
            if (handler.CanReadToken(jwt) == false)
            {
                throw new SecurityTokenMalformedException("Token not in valid format");
            }

            var keyString = Environment.GetEnvironmentVariable("JwtPrivateKey");
            if (string.IsNullOrEmpty(keyString)) return null!;

            var keyEncoded = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            if (keyEncoded == null) return null!;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = keyEncoded,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            return handler.ValidateToken(jwt, validationParameters, out _);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
        }

        return null!;
    }

    public bool HasTokenExpired(ClaimsPrincipal principle)
    {
        if (principle == null) return true;

        var claim = principle.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

        if (claim != null)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claim)).UtcDateTime;

            if (dateTime >= DateTime.UtcNow) return false;
        }

        return true;
    }

    public string GetAccountId(ClaimsPrincipal principle)
    {
        if (principle == null) return string.Empty;

        var claim = principle.FindFirst("Id");
        if (claim == null) return string.Empty;

        return claim.Value;
    }
}
