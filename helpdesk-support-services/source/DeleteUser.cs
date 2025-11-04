using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

public record DeleteTicket(Guid TicketId) : Command;

public class DeleteTicketHandler(ITicketRepository repository)
    : ICommandHandler<DeleteTicket, Result>
{
    public Result Handle(DeleteTicket command) => repository.Delete(command.TicketId);
}
