using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class TicketRepository(Context context) : ITicketRepository
{
    private Ticket? FindDbTicket(Guid id) => context.Ticket.SingleOrDefault(x => x.Id == id);

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

    public Result<Domain.Ticket> Find(Guid id)
    {
        var ticket = FindDbTicket(id);

        if (ticket is null)
            return NotFound();

        return (Domain.Ticket) ticket;
    }

    public Result Update(Domain.Ticket ticket)
    {
        try
        {
            var dbTicket = FindDbTicket(ticket.Id);

            dbTicket.Description = ticket.Description;
            dbTicket.IsClosed = ticket.Status == TicketStatus.Closed;
            dbTicket.IsInProgress = ticket.Status == TicketStatus.InProgress;
            dbTicket.IsOpen = ticket.Status == TicketStatus.Open;
            dbTicket.Title = ticket.Title;
            dbTicket.UpdatedAt = ticket.UpdatedAt;

            context.Update(dbTicket);
            context.SaveChanges();

            return Success();
        }
        catch (Exception e)
        {
            return UpdateFailed();
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

    public static Error NotFound() =>
        new(
            "ticket-not-found",
            $"Could not find {nameof(Ticket)}."
        );

    public static Error UpdateFailed() =>
        new(
            "update-ticket-failed",
            $"Could not commit {nameof(Ticket)} to storage."
        );
}
