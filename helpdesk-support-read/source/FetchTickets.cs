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

            var tickets = connection.Query<Ticket, User, Ticket>(
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

    public class Ticket
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

    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
