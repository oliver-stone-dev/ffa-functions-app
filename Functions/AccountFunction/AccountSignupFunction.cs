using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AccountFunction
{
    public class AccountSignupFunction
    {
        private readonly ILogger<AccountSignupFunction> _logger;

        public AccountSignupFunction(ILogger<AccountSignupFunction> logger)
        {
            _logger = logger;
        }

        [Function("AccountSignupFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
