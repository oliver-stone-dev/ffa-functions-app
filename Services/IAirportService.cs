using ffa_functions_app.Models;

namespace ffa_functions_app.Services;

public interface IAirportService
{
    Airport GetAirportById(int airportId);
    List<Airport> SearchForAirport(string text);
    List<Terminal> GetTerminalsByAirportId(int airportId);
}
