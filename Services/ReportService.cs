using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _database;
    private readonly ILogger _logger;

    public ReportService(AppDbContext database, ILogger<ReportService> logger)
    {
        _database = database;
        _logger = logger;
    }

    public Report GetById(int id)
    {
        var data = _database.Reports.Where(r => r.Id == id).FirstOrDefault()!;
        return data;
    }

    public List<Report> GetAllTerminalReports(int terminalId)
    {
        var data = _database.Reports.Where(r => r.TerminalId == terminalId).ToList();
        return data;
    }
    public List<ReportAlertDTO> GetReportAlertsForTerminal(int terminaldId)
    {
        const int minDays = 7;
        const int minReports = 5;
        var data = new List<ReportAlertDTO>();

        var latest = _database.Reports.Where(r => r.TerminalId == terminaldId && 
                                       EF.Functions.DateDiffDay(r.TimeStamp, DateTime.Now) < minDays)
                                      .ToList();

        foreach(var type in _database.ReportTypes)
        {
            var getReports = latest.Where(r => r.Type != null && r.Type.Id == type.Id).ToList();

            if (getReports.Count() > minReports)
            {
                data.Add
                (
                    new ReportAlertDTO
                    {
                        Text = type.AlertText
                    }
                );
            }
        }

        return data;
    }

    public void AddReport(Report report)
    {
        try 
        {
            _database.Reports.Add(report);
            _database.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}
