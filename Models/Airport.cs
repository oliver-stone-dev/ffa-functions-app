using System.ComponentModel.DataAnnotations;

namespace ffa_functions_app.Models;

public class Airport
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [Required]
    public string? Code { get; set; }

    public string? Country { get; set; }

    public string? Website { get; set; }

    public ICollection<Terminal> Terminals { get; } = new List<Terminal>();
}
