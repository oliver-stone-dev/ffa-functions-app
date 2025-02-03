using System.ComponentModel.DataAnnotations;

namespace ffa_functions_app.Models;
public class Terminal
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public int AirportId { get; set; }

    public ICollection<Review> Reviews { get; } = new List<Review>();
    public ICollection<Report> Reports { get; } = new List<Report>();

    public Airport? Airport { get; set; }
}
