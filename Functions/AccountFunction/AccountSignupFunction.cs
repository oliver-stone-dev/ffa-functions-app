using ffa_functions_app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AccountFunction
{
    public class AccountSignupFunction
    {
        private readonly ILogger<AccountSignupFunction> _logger;
        private readonly UserManager<Account> _userManager;

        public AccountSignupFunction(ILogger<AccountSignupFunction> logger, UserManager<Account> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Function("AccountSignupFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
            HttpRequest req,
            [FromBody] string email,
            [FromBody] string password)
        {
            _logger.LogInformation("Register new account");

            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return new BadRequestResult();
                }

                var result = await CreateAccount(email, password);
                return result;
            }

            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return new BadRequestResult();
        }

        private async Task<IActionResult> CreateAccount(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new ConflictResult();
            }

            var account = new Account { Email = email, UserName = email };

            var accountResult = await _userManager.CreateAsync(account);
            if (accountResult.Succeeded == false)
            {
                return new BadRequestResult();
            }

            var passwordResult = await _userManager.AddPasswordAsync(account, password);
            if (passwordResult.Succeeded == false)
            {
                return new BadRequestResult();
            }

            return new CreatedResult();
        }
    }
}
