using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ffa_functions_app.DataTransferObjects;

namespace ffa_functions_app.Functions.ReportFunctions;

public class ReportAddFunction
{
    private readonly ILogger<ReportAddFunction> _logger;
    private readonly IReportService _reportService;

    public ReportAddFunction(ILogger<ReportAddFunction> logger, IReportService reportService)
    {
        _logger = logger;
        _reportService = reportService;
    }

    [Function("ReportAddFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.User, "post", Route = "report")] 
        HttpRequest req, 
        [FromBody] ReportDTO report)
    {
        var data = report.ToReport();

        _logger.LogInformation("Adding report.");

        _reportService.AddReport(data);

        return new CreatedResult();
    }
}
