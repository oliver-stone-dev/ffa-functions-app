using ffa_functions_app.DataTransferObjects;
using ffa_functions_app.Models;

namespace ffa_functions_app;

public static class MappingExtensions
{
    public static AirportDTO ToAirportDTO(this Airport airport)
    {
        return new AirportDTO
        {
            Id = airport.Id,
            Name = airport.Name,
            Code = airport.Code,
            Website = airport.Website,
            Country = airport.Country
        };
    }

    public static Airport ToAirport(this AirportDTO airportDTO)
    {
        return new Airport
        {
            Id = airportDTO.Id,
            Name = airportDTO.Name,
            Code = airportDTO.Code,
            Website = airportDTO.Website,
            Country = airportDTO.Country
        };
    }

    public static TerminalDTO ToTerminalDTO(this Terminal terminal)
    {
        return new TerminalDTO
        {
            Id = terminal.Id,
            Name = terminal.Name,
            AirportId = terminal.AirportId
        };
    }

    public static Terminal ToTerminal(this TerminalDTO terminalDTO)
    {
        return new Terminal
        {
            Id = terminalDTO.Id,
            Name = terminalDTO.Name,
            AirportId = terminalDTO.AirportId
        };
    }

    public static ReportDTO ToReportDTO(this Report report)
    {
        return new ReportDTO
        {
            Id = report.Id,
            TypeId = report.TypeId,
            AccountId = report.AccountId,
            TerminalId = report.TerminalId,
            TimeStamp = report.TimeStamp
        };
    }

    public static Report ToReport(this ReportDTO reportDTO)
    {
        return new Report
        {
            Id = reportDTO.Id,
            TypeId = reportDTO.TypeId,
            AccountId = reportDTO.AccountId,
            TerminalId = reportDTO.TerminalId,
            TimeStamp = reportDTO.TimeStamp
        };
    }

    public static ReviewDTO ToReviewDTO(this Review review)
    {
        return new ReviewDTO
        {
            Id = review.Id,
            AccountId = review.AccountId,
            Comment = review.Comment,
            DateTime = review.DateTime,
            Recommended = review.Recommended,
            TerminalId = review.TerminalId
        };
    }

    public static Review ToReview(this ReviewDTO reviewDTO)
    {
        return new Review
        {
            Id = reviewDTO.Id,
            AccountId = reviewDTO.AccountId,
            Comment = reviewDTO.Comment,
            DateTime = reviewDTO.DateTime,
            Recommended = reviewDTO.Recommended,
            TerminalId = reviewDTO.TerminalId
        };
    }

    public static AccountDTO ToAccountDTO(this Account account)
    {
        return new AccountDTO
        {
            Id = account.Id,
            Email = account.Email,
            DisplayName = account.DisplayName
        };
    }

    public static Account ToAccount(this AccountDTO accountDTO)
    {
        return new Account
        {
            Id = accountDTO.Id!,
            Email = accountDTO.Email,
            DisplayName = accountDTO.DisplayName
        };
    }
}
