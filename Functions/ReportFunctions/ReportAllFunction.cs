using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReportFunctions;

public class ReportAllFunction
{
    private readonly ILogger<ReportAllFunction> _logger;
    private readonly IReportService _reportService;

    public ReportAllFunction(ILogger<ReportAllFunction> logger, IReportService reportService)
    {
        _logger = logger;
        _reportService = reportService;
    }

    [Function("ReportAllFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "report")] HttpRequest req)
    {
        _logger.LogInformation("Request all reports");

        var terminalId = Convert.ToInt32(req.Query["terminalId"]);
        var accountId = req.Query["accountId"];
        var typeId = Convert.ToInt32(req.Query["type"]);

        if (terminalId == 0)
        {
            return new NotFoundResult();
        }

        var accountReports = _reportService.GetAllTerminalReports(terminalId);

        var report = accountReports.Where(r => (String.IsNullOrEmpty(accountId)) || r.AccountId == accountId)
                                   .Where(r => (typeId == 0) || r.TypeId == typeId)
                                   .ToList();

        if (report == null)
        {
            return new NotFoundResult();
        }

        var reportDTOs = report.Select(r => r.ToReportDTO()).ToList();

        return new OkObjectResult(reportDTOs);
    }
}
