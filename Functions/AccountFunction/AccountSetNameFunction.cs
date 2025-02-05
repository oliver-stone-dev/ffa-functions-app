using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;


namespace ffa_functions_app.Functions.AccountFunction
{
    public class AccountSetNameFunction
    {
        private readonly ILogger<AccountSetNameFunction> _logger;
        private readonly UserManager<Account> _userManager;
        private readonly ITokenAuthenticationService _tokenAuthService;

        public AccountSetNameFunction(ILogger<AccountSetNameFunction> logger,
            UserManager<Account> userManager,
            ITokenAuthenticationService tokenAuthService)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenAuthService = tokenAuthService;
        }

        [Function("AccountSetNameFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/displayname")] 
        HttpRequest req,
        [FromBody] AccountRegisterDTO accountRegister)
        {
            var token = req.Headers["Authorization"];
            if (string.IsNullOrEmpty(token)) return new UnauthorizedResult();

            if (accountRegister == null) return new BadRequestResult();

            if (string.IsNullOrEmpty(accountRegister.DisplayName)) return new BadRequestResult();

            _logger.LogInformation("Set account display name.");

            var claimsPrinciple = _tokenAuthService.ValidateToken(token!);
            if (claimsPrinciple == null) return new BadRequestResult();


            if (_tokenAuthService.HasTokenExpired(claimsPrinciple)) return new UnauthorizedResult();

            var claimsAccountId = _tokenAuthService.GetAccountId(claimsPrinciple);

            var user = await _userManager.FindByEmailAsync(accountRegister.Email!);
            if (user == null)
            {
                return new NotFoundResult();
            }

            if (claimsAccountId != user.Id) return new UnauthorizedResult();

            if (await _userManager.CheckPasswordAsync(user, accountRegister.Password!) == false)
            {
                return new UnauthorizedResult();
            }

            user.DisplayName = accountRegister.DisplayName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult(result.Errors);
            }
        }
    }
}
