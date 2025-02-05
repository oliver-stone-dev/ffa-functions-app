using ffa_functions_app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AccountFunction;

public class AccountExistsFunction
{
    private readonly ILogger<AccountExistsFunction> _logger;
    private readonly UserManager<Account> _userManager;

    public AccountExistsFunction(ILogger<AccountExistsFunction> logger, UserManager<Account> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [Function("AccountExistsFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/exists")] HttpRequest req)
    {
        var email = req.Query["email"];
        _logger.LogInformation("Confirm account exists");
        return new OkObjectResult(await _userManager.FindByEmailAsync(email!) != null);
    }
}
