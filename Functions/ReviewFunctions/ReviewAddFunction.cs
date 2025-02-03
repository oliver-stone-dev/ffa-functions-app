using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReviewFunctions;

public class ReviewAddFunction
{
    private readonly ILogger<ReviewAddFunction> _logger;
    private readonly IReviewService _reviewService;

    public ReviewAddFunction(ILogger<ReviewAddFunction> logger, IReviewService reviewService)
    {
        _logger = logger;
        _reviewService = reviewService;
    }

    [Function("ReviewAddFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.User, "post", Route = "reviews")] 
    HttpRequest req,
    [FromBody] ReviewDTO review)
    {
        _logger.LogInformation("Add new review");

        var data = review.ToReview();

        _reviewService.AddReview(data);

        return new CreatedResult();
    }
}
