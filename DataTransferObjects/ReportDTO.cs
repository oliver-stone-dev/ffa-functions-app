namespace ffa_functions_app.DataTransferObjects;

public class ReportDTO
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public string? AccountId { get; set; }
    public int TerminalId { get; set; }
    public DateTime TimeStamp { get; set; }
}
