using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace ffa_functions_app.Functions.AccountFunction;

public class GetAuthnAccountFunction
{
    private readonly ILogger<GetAuthnAccountFunction> _logger;
    private readonly UserManager<Account> _userManager;
    private readonly ITokenAuthenticationService _tokenAuthService;

    public GetAuthnAccountFunction(
        ILogger<GetAuthnAccountFunction> logger, 
        UserManager<Account> userManager,
        ITokenAuthenticationService tokenAuthService)
    {
        _logger = logger;
        _userManager = userManager;
        _tokenAuthService = tokenAuthService;
    }

    [Function("GetAuthnAccountFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get" ,Route = "account" )] HttpRequest req)
    {
        _logger.LogInformation("Return authenticated account details");

        var token = req.Headers["Authorization"];
        if (string.IsNullOrEmpty(token)) return new UnauthorizedResult();

        var claimsPrinciple = _tokenAuthService.ValidateToken(token!);
        if (claimsPrinciple == null) return new BadRequestResult();

        if (_tokenAuthService.HasTokenExpired(claimsPrinciple)) return new UnauthorizedResult();

        var claimsAccountId = _tokenAuthService.GetAccountId(claimsPrinciple);

        var user = await _userManager.FindByIdAsync(claimsAccountId);

        if (user == null)
        {
            return new NotFoundResult();
        }

        var dto = user.ToAccountDTO();

        return new OkObjectResult(dto);
    }
}
