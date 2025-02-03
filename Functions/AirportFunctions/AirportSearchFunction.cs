using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AirportFunctions;

public class AirportSearchFunction
{
    private readonly ILogger<AirportSearchFunction> _logger;
    private readonly IAirportService _airportService;

    public AirportSearchFunction(ILogger<AirportSearchFunction> logger, IAirportService airportService)
    {
        _logger = logger;
        _airportService = airportService;
    }

    [Function("AirportSearchFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route ="airport")] HttpRequest req)
    {
        _logger.LogInformation("Search for airport request.");

        var searchText = req.Query["search"];
        if (string.IsNullOrEmpty(searchText))
        {
            return new NotFoundResult();
        }

        var data = _airportService.SearchForAirport(searchText!);
        if (data == null)
        {
            return new NotFoundResult();
        }

        //convert to data transfer object
        var dto = data.Select(a => a.ToAirportDTO());

        return new OkObjectResult(dto);
    }
}
