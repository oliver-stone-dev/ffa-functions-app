using ffa_functions_app.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        var data = new List<Airport>();
        var lookupText = $"\'{text}*\'";

        string query = $@"SELECT TOP 20 * FROM dbo.Airports WHERE CONTAINS (Name,{lookupText}) OR CONTAINS (Code,{lookupText}) OR CONTAINS (Country,{lookupText})";

        var connectionString = Environment.GetEnvironmentVariable("AzureSQLConnectionString");

        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using var command = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var airport = new Airport();

                    airport.Id = reader.GetInt32(0);
                    airport.Name = reader.GetString(1);
                    airport.Code = reader.GetString(2);
                    airport.Country = reader.GetString(3);
                    airport.Website = reader.GetString(4);

                    data.Add(airport);
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }

        return data;
    }
}
