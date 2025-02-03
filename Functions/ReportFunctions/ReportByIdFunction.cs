using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Functions.ReportFunctions;

public class ReportByIdFunction
{
    private readonly ILogger<ReportByIdFunction> _logger;
    private readonly IReportService _reportService;

    public ReportByIdFunction(ILogger<ReportByIdFunction> logger, IReportService reportService)
    {
        _logger = logger;
        _reportService = reportService;
    }

    [Function("ReportByIdFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "report/{id}")] 
    HttpRequest req,
    int id)
    {
        _logger.LogInformation($"Request report for id {id}");

        var data = _reportService.GetById(id);

        if (data == null)
        {
            return new NotFoundResult();
        }

        var dto = data.ToReportDTO();

        return new OkObjectResult(dto);
    }
}
