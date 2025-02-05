using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace ffa_functions_app.Functions.ReviewFunctions;

public class ReviewAddFunction
{
    private readonly ILogger<ReviewAddFunction> _logger;
    private readonly IReviewService _reviewService;
    private readonly ITokenAuthenticationService _tokenAuthService;

    public ReviewAddFunction(
        ILogger<ReviewAddFunction> logger, 
        IReviewService reviewService,
        ITokenAuthenticationService tokenAuthService)
    {
        _logger = logger;
        _reviewService = reviewService;
        _tokenAuthService = tokenAuthService;
    }

    [Function("ReviewAddFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reviews")] 
    HttpRequest req,
    [FromBody] ReviewDTO review)
    {
        _logger.LogInformation("Add new review");

        var token = req.Headers["Authorization"];
        if (string.IsNullOrEmpty(token)) return new UnauthorizedResult();
        _logger.LogInformation("Adding report.");
        var claimsPrinciple = _tokenAuthService.ValidateToken(token!);
        if (claimsPrinciple == null) return new BadRequestResult();

        if (_tokenAuthService.HasTokenExpired(claimsPrinciple)) return new UnauthorizedResult();

        var claimsAccountId = _tokenAuthService.GetAccountId(claimsPrinciple);

        if (claimsAccountId != review.AccountId) return new UnauthorizedResult();

        var data = review.ToReview();

        _reviewService.AddReview(data);

        return new CreatedResult();
    }
}
