using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;

namespace ffa_functions_app.Services;
public interface IReportService
{
    Report GetById(int id);

    List<Report> GetAllTerminalReports(int terminalId);

    List<ReportAlertDTO> GetReportAlertsForTerminal(int terminaldId);

    void AddReport(Report report);
}
