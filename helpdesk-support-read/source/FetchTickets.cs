using MdkLegal.HelpDesk.Support.Services;
using Microsoft.Data.SqlClient;

namespace MdkLegal.HelpDesk.Support.Read;

public record FetchTickets : Query;

public class FetchTicketsHandler(IConnectionStringProvider provider)
    : IQueryHandler<FetchTickets, Result<IEnumerable<FetchTicketsHandler.Ticket>>>
{
    private const string Query =
        """
        SELECT T.Id
             , T.UserId
             , T.CreatedAt
             , T.Description
             , T.IsClosed
             , T.IsInProgress
             , T.IsOpen
             , T.Title
             , T.UpdatedAt
             , U.Id
             , U.Email
             , U.Name
          FROM Ticket T
          LEFT JOIN [User] U
            ON U.Id = T.UserId
        """;

    public Result<IEnumerable<Ticket>> Handle(FetchTickets query)
    {
        using var connection = new SqlConnection(provider.GetConnectionString());

        try
        {
            connection.Open();

            var tickets = connection.Query<DbTicket, User, Ticket>(
                Query,
                (t, u) =>
                {
                    t.User = u;
                    return t;
                },
                query,
                splitOn: "Id"
            ).ToList();

            return tickets;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            connection.Close();
        }
    }

    public class DbTicket
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Description { get; init; }
        public bool IsClosed { get; init; }
        public bool IsInProgress { get; init; }
        public bool IsOpen { get; init; }
        public string Title { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public User User { get; set; }
    }

    public class Ticket(DbTicket source)
    {
        public Guid Id { get; } = source.Id;
        public DateTime CreatedAt { get; } = source.CreatedAt;
        public string Description { get; } = source.Description;
        public string Status { get; } = GetStatus(source);
        public string Title { get; } = source.Title;
        public DateTime? UpdatedAt { get; } = source.UpdatedAt;
        public User User { get; } = source.User;

        public static string GetStatus(DbTicket source)
        {
            if (source.IsOpen)
                return "Open";

            if (source.IsInProgress)
                return "In Progress";

            return "Closed";
        }

        public static implicit operator Ticket(DbTicket source) => new(source);
    }
}
