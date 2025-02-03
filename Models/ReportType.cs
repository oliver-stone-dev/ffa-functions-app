using System.ComponentModel.DataAnnotations;

namespace ffa_functions_app.Models;

public class ReportType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? AlertText { get; set; }
    public ICollection<Report> Reports { get; } = new List<Report>();
}
