using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReviewFunctions;

public class ReviewAuthorByIdFunction
{
    private readonly ILogger<ReviewAuthorByIdFunction> _logger;
    private readonly IReviewService _reviewService;
    private readonly UserManager<Account> _userManager;

    public ReviewAuthorByIdFunction(ILogger<ReviewAuthorByIdFunction> logger, 
        IReviewService reviewService, 
        UserManager<Account> userManager)
    {
        _logger = logger;
        _reviewService = reviewService;
        _userManager = userManager;

    }

    [Function("ReviewAuthorByIdFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews/{id}/details")] 
    HttpRequest req,
    int id)
    {
        var accountId = req.Query["accountId"];

        if (string.IsNullOrEmpty(accountId))
        {
            return new NotFoundResult();
        }

        _logger.LogInformation("Request author detail for review");

        var user = await _userManager.FindByIdAsync(accountId!);
        if (user == null)
        {
            return new NotFoundResult();
        }

        var name = user.DisplayName;
        var reviews = _reviewService.GetAccountReviewCount(accountId!);

        var details = new ReviewDetailsDTO
        {
            Username = name,
            UserReviews = reviews
        };

        return new OkObjectResult(details);
    }
}
