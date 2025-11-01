using MdkLegal.HelpDesk.Support.Domain;

namespace MdkLegal.HelpDesk.Support.Services;

public record CreateTicket(Guid Id, string Title) : Command(Id);

public class CreateTicketHandler(ITicketRepository Repository)
    : ICommandHandler<CreateTicket, Result<Guid>>
{
    public Result<Guid> Handle(CreateTicket command)
    {
        var ticket = Ticket.From(command);

        return Repository.Create(ticket);
    }
}
