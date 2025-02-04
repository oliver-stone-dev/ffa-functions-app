using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ffa_functions_app.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ffa_functions_app.Services;

public class TokenProviderService : ITokenProviderService
{
    private readonly ILogger<TokenProviderService> _logger;

    public TokenProviderService(ILogger<TokenProviderService> logger) => _logger = logger;

    public string Create(Account account)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            var keyString = Environment.GetEnvironmentVariable("JwtPrivateKey");
            if (string.IsNullOrEmpty(keyString)) return string.Empty;

            var keyEncoded = Encoding.UTF8.GetBytes(keyString);

            var credentials = new SigningCredentials(
                           new SymmetricSecurityKey(keyEncoded),
                           SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = GenerateClaims(account)
            };

            var jwtToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(jwtToken);

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        return string.Empty;
    }

    private static ClaimsIdentity GenerateClaims(Account account)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("Id", account.Id));
        ci.AddClaim(new Claim("UserName", account.UserName!));
        ci.AddClaim(new Claim("Email", account.Email!));

        return ci;
    }
}
