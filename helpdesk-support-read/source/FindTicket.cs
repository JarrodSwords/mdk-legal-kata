using Microsoft.Data.SqlClient;

namespace MdkLegal.HelpDesk.Support.Read;

/// <param name="Id">The <see cref="Ticket" />'s <see cref="Ticket.Id" />.</param>
public record FindTicket(Guid Id) : Query;

public class FindTicketHandler : IQueryHandler<FindTicket, Result<Ticket>>
{
    private const string Query =
        """
        SELECT Id
             , Title
          FROM Ticket
         WHERE Id = @Id                    
        """;

    public Result<Ticket> Handle(FindTicket query)
    {
        using var connection = new SqlConnection();

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
    public string Title { get; init; }
}
