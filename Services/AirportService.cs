using ffa_functions_app.Models;
using Microsoft.EntityFrameworkCore;

namespace ffa_functions_app.Services;

public class AirportService : IAirportService
{
    private readonly AppDbContext _database;

    public AirportService(AppDbContext database)
    {
        _database = database;
    }

    public Airport GetAirportById(int airportId)
    {
        var data = _database.Airports.Where(a => a.Id == airportId)
                                     //.Include(a => a.Terminals)
                                     .FirstOrDefault()!;
        return data;
    }
    public List<Terminal> GetTerminalsByAirportId(int airportId)
    {
        var data = _database.Terminals.Where(t => t.AirportId == airportId).ToList();
        return data;
    }

    public List<Airport> SearchForAirport(string text)
    {
        var lookupText = $"\"{text}*\"";

        FormattableString query =
            $@"SELECT TOP 20 * FROM dbo.Airports 
            WHERE CONTAINS (Name,{lookupText}) 
            OR CONTAINS (Code,{lookupText}) 
            OR CONTAINS (Country,{lookupText})";

        var data = _database.Airports.FromSql(query).Where(a => a.Terminals.Count() > 0).ToList();

        return data;
    }
}
