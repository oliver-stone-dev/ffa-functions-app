using ffa_functions_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ffa_functions_app.DataTransferObjects;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;
using Microsoft.AspNetCore.Identity;

namespace ffa_functions_app.Functions.ReportFunctions;

public class ReportAddFunction
{
    private readonly ILogger<ReportAddFunction> _logger;
    private readonly IReportService _reportService;
    private readonly ITokenAuthenticationService _tokenAuthService;

    public ReportAddFunction(
        ILogger<ReportAddFunction> logger, 
        IReportService reportService,
        ITokenAuthenticationService tokenAuthService)
    {
        _logger = logger;
        _reportService = reportService;
        _tokenAuthService = tokenAuthService;
    }

    [Function("ReportAddFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "report")] 
        HttpRequest req, 
        [FromBody] ReportDTO report)
    {
        _logger.LogInformation("Adding report.");

        var token = req.Headers["Authorization"];
        if (string.IsNullOrEmpty(token)) return new UnauthorizedResult();
                _logger.LogInformation("Adding report.");
        var claimsPrinciple = _tokenAuthService.ValidateToken(token!);
        if (claimsPrinciple == null) return new BadRequestResult();

        if (_tokenAuthService.HasTokenExpired(claimsPrinciple)) return new UnauthorizedResult();

        var claimsAccountId = _tokenAuthService.GetAccountId(claimsPrinciple);

        if (report.AccountId != claimsAccountId) return new UnauthorizedResult();

        var data = report.ToReport();

        _reportService.AddReport(data);

        return new CreatedResult();
    }
}
