using Microsoft.AspNetCore.Identity;
namespace ffa_functions_app.Models;

public class Account : IdentityUser
{
    public string? DisplayName { get; set; }
    public ICollection<Report> Reports { get; } = new List<Report>();
    public ICollection<Review> Reviews { get; } = new List<Review>();
}
