using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

public record AssignUser(Guid TicketId, Guid UserId) : Command;

public class AssignUserHandler(ITicketRepository repository)
    : ICommandHandler<AssignUser, Result>
{
    public Result Handle(AssignUser command) =>
        repository.Find(command.TicketId)
            .Then(
                ticket =>
                {
                    ticket.Assign(command.UserId);

                    return repository.Update(ticket);
                }
            );
}
