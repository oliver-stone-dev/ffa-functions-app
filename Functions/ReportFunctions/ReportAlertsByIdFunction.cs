using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReportFunctions;

public class ReportAlertsByIdFunction
{
    private readonly ILogger<ReportAlertsByIdFunction> _logger;
    private readonly IReportService _reportService;

    public ReportAlertsByIdFunction(ILogger<ReportAlertsByIdFunction> logger, IReportService reportService)
    {
        _logger = logger;
        _reportService = reportService;
    }

    [Function("ReportAlertsByIdFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "report/alerts")] HttpRequest req)
    {
        var terminalId = Convert.ToInt32(req.Query["terminalId"]);

        _logger.LogInformation($"Request report alerts for terminal id {terminalId}");

        var data = _reportService.GetReportAlertsForTerminal(terminalId);

        if (data == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(data);
    }
}
