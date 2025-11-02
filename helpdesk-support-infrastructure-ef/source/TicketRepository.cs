using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class TicketRepository(Context context) : ITicketRepository
{
    public Result Create(Domain.Ticket ticket)
    {
        try
        {
            if (context.Ticket.Any(x => x.Id == ticket.Id))
                return AlreadyExists();

            context.Ticket.Add(ticket);
            context.SaveChanges();

            return Success();
        }
        catch (Exception ex)
        {
            // handle specific database/connection-related issues and return a custom error.
            return CreateFailed();
        }
    }

    public Result Delete(Guid id)
    {
        var ticket = context.Ticket.Find(id);

        if (ticket is null)
            return NotFound();

        context.Ticket.Remove(ticket);
        context.SaveChanges();

        return Success();
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

    public static Error NotFound() =>
        new(
            "ticket-not-found",
            $"Could not find {nameof(Ticket)}."
        );
}
