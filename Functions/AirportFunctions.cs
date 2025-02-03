using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ffa_functions_app.Services;

namespace ffa_functions_app.Functions
{
    public class AirportFunctions
    {
        private readonly ILogger<AirportFunctions> _logger;
        private readonly IAirportService _airportService;

        public AirportFunctions(ILogger<AirportFunctions> logger, IAirportService airportService)
        {
            _logger = logger;
            _airportService = airportService;
        }

        [Function("GetAirportById")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "airport/{id}")] HttpRequest req, int id)
        {
            _logger.LogInformation($"Requesting Airport data for {id}");

            var data = _airportService.GetAirportById(id);

            if (data == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(data);
        }
    }
}
