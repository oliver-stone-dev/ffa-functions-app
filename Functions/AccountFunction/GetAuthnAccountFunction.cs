using ffa_functions_app.Models;
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

    public GetAuthnAccountFunction(ILogger<GetAuthnAccountFunction> logger, UserManager<Account> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [Function("GetAuthnAccountFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.User, "get" ,Route = "account" )] HttpRequest req)
    {
        _logger.LogInformation("Return authenticated account details");
        return new NotFoundResult();

        //var userAccount = await _userManager.GetUserAsync(User);

        //if (userAccount == null)
        //{
        //    return new NotFoundResult();
        //}

        //var dto = userAccount.ToAccountDTO();


        //return new OkObjectResult("Welcome to Azure Functions!");
    }
}
