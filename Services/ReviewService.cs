using ffa_functions_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ffa_functions_app.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _database;
    private readonly ILogger _logger;

    public ReviewService(AppDbContext database, ILogger<ReviewService> logger)
    {
        _database = database;
        _logger = logger;
    }

    public Review GetById(int id)
    {
        Review data = new();

        try
        {
            data = _database.Reviews.Where(r => r.Id == id).FirstOrDefault()!;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return data!;
    }

    public void AddReview(Review review)
    {
        try 
        {
            _database.Reviews.Add(review);
            _database.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    public List<Review> GetFilteredReviews(
        int airportId,
        int terminalId,
        int offset,
        int maxReturn)
    {
        List<Review> reviews = new();

        try
        {
            reviews = _database.Reviews
            .Where(r => 
                (airportId == 0 || r.Terminal != null && r.Terminal.AirportId == airportId) &&
                (terminalId == 0 || r.TerminalId == terminalId))
            .Include(r => r.Terminal)
            .Skip(offset)
            .Take(maxReturn)
            .ToList();
        }
        catch (Exception e)
        {

        }

        return reviews;
    }

    public int GetAirportReviewCount(int airportId)
    {
        try
        {
            var count = _database.Reviews
                .Where(r => (r.Terminal != null) && (r.Terminal.AirportId == airportId))
                .Include(r => r.Terminal)
                .ToList()
                .Count();

            return count;
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public int GetAccountReviewCount(string accountId)
    {
        try
        {
            var count = _database.Reviews
                .Where(r => (r.Account != null) && (r.Account.Id == accountId))
                .Include(r => r.Account)
                .ToList()
                .Count();

            return count;
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public double GetAirportRatingAvg(int airportId)
    {
        try
        {
            var airportReviews = _database.Reviews
                .Where(r => (r.Terminal != null) && (r.Terminal.AirportId == airportId))
                .Include(r => r.Terminal)
                .ToList();

            var positiveReviews = airportReviews.Where(r => r.Recommended == true).ToList().Count();

            return (double)positiveReviews / (double)airportReviews.Count();
        }
        catch (Exception e)
        {
            return 0;
        }
    }

}
