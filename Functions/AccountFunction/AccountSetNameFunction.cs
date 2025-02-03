using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AccountFunction
{
    public class AccountSetNameFunction
    {
        private readonly ILogger<AccountSetNameFunction> _logger;
        private readonly UserManager<Account> _userManager;

        public AccountSetNameFunction(ILogger<AccountSetNameFunction> logger, UserManager<Account> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Function("AccountSetNameFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.User, "post", Route = "account/displayname")] 
        HttpRequest req,
        [FromBody] AccountRegisterDTO accountRegister)
        {
            _logger.LogInformation("Set account display name.");

            if (string.IsNullOrEmpty(accountRegister.DisplayName))
            {
                return new BadRequestResult();
            }

            var user = await _userManager.FindByEmailAsync(accountRegister.Email!);

            if (user == null)
            {
                return new NotFoundResult();
            }

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
