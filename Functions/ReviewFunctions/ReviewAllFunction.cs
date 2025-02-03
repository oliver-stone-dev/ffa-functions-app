using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReviewFunctions;

public class ReviewAllFunction
{
    private readonly ILogger<ReviewAllFunction> _logger;
    private readonly IReviewService _reviewService;
    public ReviewAllFunction(ILogger<ReviewAllFunction> logger, IReviewService reviewService)
    {
        _logger = logger;
        _reviewService = reviewService;
    }

    [Function("ReviewAllFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews")] HttpRequest req)
    {
        var airportId = Convert.ToInt32(req.Query["airportId"]);
        var terminalId = Convert.ToInt32(req.Query["terminalId"]);
        var offset = Convert.ToInt32(req.Query["offset"]);
        var sort = req.Query["sort"];
        var results = Convert.ToInt32(req.Query["results"]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var data = _reviewService.GetFilteredReviews(airportId, terminalId, offset, results);
        if (data == null)
        {
            return new NotFoundResult();
        }

        var dto = data.Select(r => r.ToReviewDTO()).ToList();

        return new OkObjectResult(dto);
    }
}
