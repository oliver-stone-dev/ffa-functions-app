using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReviewFunctions;

public class ReviewByIdFunction
{
    private readonly ILogger<ReviewByIdFunction> _logger;
    private readonly IReviewService _reviewService;

    public ReviewByIdFunction(ILogger<ReviewByIdFunction> logger, IReviewService reviewService)
    {
        _logger = logger;
        _reviewService = reviewService;
    }

    [Function("ReviewByIdFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews/{id}")] 
    HttpRequest req,
    int id)
    {
        _logger.LogInformation($"Request review for Id {id}");

        var data = _reviewService.GetById(id);

        if (data == null)
        {
            return new NotFoundResult();
        }

        //get data transfer object
        var dto = data.ToReviewDTO();

        return new OkObjectResult(dto);
    }
}
