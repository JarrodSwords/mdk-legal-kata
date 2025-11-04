using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

public record UpdateTicket(
    Guid TicketId,
    string Description,
    bool IsClosed,
    bool IsInProgress,
    bool IsOpen,
    string Title
) : Command;

public class UpdateTicketHandler(ITicketRepository repository)
    : ICommandHandler<UpdateTicket, Result>
{
    public Result Handle(UpdateTicket command) =>
        repository.Find(command.TicketId)
            .Then(
                ticket =>
                {
                    if (!Description.From(command.Description, out var description))
                        return Ticket.DescriptionRequired();

                    ticket.Set(description);

                    if (!Title.From(command.Title, out var title))
                        return Ticket.TitleRequired();

                    ticket.Set(title);

                    if (command.IsClosed)
                        ticket.Close();

                    ticket.UpdatedAt = DateTime.Now;

                    return repository.Update(ticket);
                }
            );
}
