using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AirportFunctions;

public class AirportStatsByIdFunction
{
    private readonly ILogger<AirportStatsByIdFunction> _logger;
    private readonly IAirportService _airportService;
    private readonly IReviewService _reviewService;

    public AirportStatsByIdFunction(ILogger<AirportStatsByIdFunction> logger,
                            IAirportService airportService, IReviewService reviewService)
    {
        _logger = logger;
        _airportService = airportService;
        _reviewService = reviewService;
    }

    [Function("AirportStatsByIdFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "airports/{id}/stats")] HttpRequest req, int id)
    {
        _logger.LogInformation($"Requesting Airport stats for {id}");

        var totalReviews = _reviewService.GetAirportReviewCount(id);
        var averageRating = _reviewService.GetAirportRatingAvg(id);

        var statsDTO = new AirportStatsDTO
        {
            AverageRating = averageRating,
            TotalReviews = totalReviews
        };

        if (statsDTO == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(statsDTO);
    }
}
