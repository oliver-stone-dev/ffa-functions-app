using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;


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
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] 
    HttpRequest req,
    [FromBody] LoginDTO loginDTO)
    {
        var token = string.Empty;
        _logger.LogInformation("Request login");

        if (loginDTO == null)
        {
            return new BadRequestResult();
        }

        if (string.IsNullOrEmpty(loginDTO.Email))
        {
            return new NotFoundResult();
        }

        if (string.IsNullOrEmpty(loginDTO.Password))
        {
            return new NotFoundResult();
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
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

        var tokenDto = new TokenDTO
        {
            TokenType = "Bearer",
            AccessToken = token
        };
        
        return new OkObjectResult(tokenDto);
    }
}
