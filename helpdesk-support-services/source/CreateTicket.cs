using MdkLegal.HelpDesk.Support.Domain;

namespace MdkLegal.HelpDesk.Support.Services;

public record CreateTicket : Command
{
    public CreateTicket(string title, Guid? id = null) : base(id)
    {
        Title = title;
    }

    public string Title { get; }
}

/// <summary>
///     Creates a <see cref="Ticket" />.
/// </summary>
/// <param name="Repository"></param>
/// <remarks>
///     I won't chain methods in this handler to show the longer, less-functional approach.
///     While I prefer chaining, some teams don't. I'm demonstrating that this is an option.
/// </remarks>
public class CreateTicketHandler(ITicketRepository Repository)
    : ICommandHandler<CreateTicket, Result<Guid>>
{
    public Result<Guid> Handle(CreateTicket command)
    {
        var initializeTicketResult = Ticket.From(command.Title);

        if (initializeTicketResult.IsFailure)
            return initializeTicketResult.Error!;

        var ticket = initializeTicketResult.Value;

        var storeTicketResult = Repository.Create(ticket!);

        if (storeTicketResult.IsFailure)
            return storeTicketResult.Error!;

        return ticket!.Id;
    }
}
