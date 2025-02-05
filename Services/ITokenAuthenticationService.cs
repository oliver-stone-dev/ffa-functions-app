using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ffa_functions_app.Services;

public interface ITokenAuthenticationService
{
    public bool HasTokenExpired(ClaimsPrincipal principle);
    public string GetAccountId(ClaimsPrincipal principle);

    public ClaimsPrincipal ValidateToken(string token);
}
