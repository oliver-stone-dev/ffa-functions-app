using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AccountFunction;

public class AccountLoginFunction
{
    private readonly ILogger<AccountLoginFunction> _logger;
    private readonly UserManager<Account> _userManager;
    private readonly ITokenProviderService _tokenProviderService;

    public AccountLoginFunction(
        ILogger<AccountLoginFunction> logger, 
        UserManager<Account> userManager,
        ITokenProviderService tokenProviderService)
    {
        _logger = logger;
        _userManager = userManager;
        _tokenProviderService = tokenProviderService;
    }

    [Function("AccountLoginFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/login")] 
    HttpRequest req,
    [FromBody] string email,
    [FromBody] string password)
    {
        var token = string.Empty;
        _logger.LogInformation("Request login");

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return new NotFoundResult();
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (passwordValid == false)
            {
                return new UnauthorizedResult();
            }

            token = _tokenProviderService.Create(user);
        }
        catch (Exception e)
        {
            _logger.LogInformation($"Excepion {e.Message}");
        }
        
        return new OkObjectResult(token);
    }
}
