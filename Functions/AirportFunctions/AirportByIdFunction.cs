using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AirportFunctions;

public class AirportByIdFunction
{
    private readonly ILogger<AirportByIdFunction> _logger;
    private readonly IAirportService _airportService;

    public AirportByIdFunction(ILogger<AirportByIdFunction> logger,
                            IAirportService airportService)
    {
        _logger = logger;
        _airportService = airportService;
    }

    [Function("AirportByIdFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "airports/{id}")] HttpRequest req, int id)
    {
        _logger.LogInformation($"Requesting Airport data for {id}");

        var data = _airportService.GetAirportById(id);

        if (data == null)
        {
            return new NotFoundResult();
        }

        var dto = data.ToAirportDTO();

        return new OkObjectResult(dto);
    }
}
