using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

public record AssignTicket(Guid TicketId, Guid UserId) : Command;

public class AssignTicketHandler(ITicketRepository repository)
    : ICommandHandler<AssignTicket, Result>
{
    public Result Handle(AssignTicket command) =>
        repository.Find(command.TicketId)
            .Then(
                ticket =>
                {
                    ticket.Assign(command.UserId);

                    return repository.Update(ticket);
                }
            );
}
