using MdkLegal.HelpDesk.Support.Services;
using Microsoft.Data.SqlClient;

namespace MdkLegal.HelpDesk.Support.Read;

/// <param name="Id">The <see cref="Ticket" />'s <see cref="Ticket.Id" />.</param>
public record FindTicket(Guid Id) : Query;

public class FindTicketHandler(IConnectionStringProvider provider)
    : IQueryHandler<FindTicket, Result<Ticket>>
{
    private const string Query =
        """
        SELECT Id
             , UserId
             , CreatedAt
             , Description
             , IsClosed
             , IsInProgress
             , IsOpen
             , Title
             , UpdatedAt
          FROM Ticket
         WHERE Id = @Id
        """;

    public Result<Ticket> Handle(FindTicket query)
    {
        using var connection = new SqlConnection(provider.GetConnectionString());

        try
        {
            connection.Open();

            var ticket = connection.QuerySingleOrDefault<Ticket>(Query, query);

            if (ticket is null)
                return TicketNotFound();

            return ticket;
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

    public static Error TicketNotFound() =>
        new(
            "ticket-not-found",
            $"{nameof(Ticket)} not found."
        );
}

public class Ticket
{
    public Guid Id { get; init; }
    public Guid? UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Description { get; init; }
    public bool IsClosed { get; init; }
    public bool IsInProgress { get; init; }
    public bool IsOpen { get; init; }
    public string Title { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
