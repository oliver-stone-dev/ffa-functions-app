using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.AirportFunctions;

public class AirportTerminalsByIdFunction
{
    private readonly ILogger<AirportTerminalsByIdFunction> _logger;
    private readonly IAirportService _airportService;

    public AirportTerminalsByIdFunction(ILogger<AirportTerminalsByIdFunction> logger, IAirportService airportService)
    {
        _logger = logger;
        _airportService = airportService;
    }

    [Function("AirportTerminalsByIdFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "airport/{id}/terminals")] 
    HttpRequest req,
    int id)
    {
        _logger.LogInformation($"Request Airport Terminals for id {id}");

        var terminalId = req.Query["terminalId"];

        var data = _airportService.GetTerminalsByAirportId(id);

        if (data == null)
        {
            return new NotFoundResult();
        }

        //Filter terminal id
        var filtered = data.Where(t => (terminalId == 0) || (terminalId == t.Id)).ToList();

        //convert to data transfer object
        var dto = filtered.Select(t => t.ToTerminalDTO());

        return new OkObjectResult(dto);
    }
}
