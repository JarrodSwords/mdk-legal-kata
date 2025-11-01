using MdkLegal.HelpDesk.Support.Domain;

namespace MdkLegal.HelpDesk.Support.Services;

public record CreateTicket : Command
{
    public CreateTicket(string title, Guid? id) : base(id)
    {
        Title = title;
    }

    public string Title { get; }
}

public class CreateTicketHandler(ITicketRepository Repository)
    : ICommandHandler<CreateTicket, Result<Guid>>
{
    public Result<Guid> Handle(CreateTicket command)
    {
        var ticket = Ticket.From(command.Title);

        return Repository.Create(ticket);
    }
}
