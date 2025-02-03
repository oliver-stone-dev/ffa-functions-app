namespace ffa_functions_app.DataTransferObjects;

public class ReviewDTO
{
    public int Id { get; set; }
    public int TerminalId { get; set; }
    public string? AccountId { get; set; }
    public bool Recommended { get; set; }
    public DateTime DateTime { get; set; }
    public string? Comment { get; set; }
}
