using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ffa_functions_app.Models;

public class Report
{
    
    [Key]
    public int Id { get; set; }

    [Required]
    public int TypeId { get; set; }

    [Required]
    [ForeignKey ("Account")]
    public string? AccountId { get; set; }

    [Required]
    public int TerminalId { get; set; }

    public DateTime TimeStamp { get; set; }
    public Terminal? Terminal { get; set; }
    public Account? Account { get; set; }
    public ReportType? Type { get; set; }
}
