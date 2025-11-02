using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class TicketRepository(Context Context) : ITicketRepository
{
    public Result Create(Domain.Ticket ticket)
    {
        try
        {
            if (Context.Ticket.Any(x => x.Id == ticket.Id))
                return AlreadyExists();

            Context.Ticket.Add(ticket);
            Context.SaveChanges();

            return Success();
        }
        catch (Exception ex)
        {
            // handle specific database/connection-related issues and return a custom error.
            return CreateFailed();
        }
    }

    public static Error AlreadyExists() =>
        new(
            "ticket-already-exists",
            $"Cannot create duplicate {nameof(Ticket)}."
        );

    public static Error CreateFailed() =>
        new(
            "create-ticket-failed",
            $"Could not commit {nameof(Ticket)} to storage."
        );
}
