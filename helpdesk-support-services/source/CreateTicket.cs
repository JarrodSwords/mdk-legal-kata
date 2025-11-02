using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

/// <summary>
///     Creates a <see cref="Ticket" />.
/// </summary>
/// <param name="repository"></param>
/// <remarks>
///     I won't chain methods in this handler to show the longer, less-functional approach.
///     While I prefer chaining, some teams don't. I'm demonstrating that this is an option.
/// </remarks>
public class CreateTicketHandler(ITicketRepository repository)
    : ICommandHandler<CreateTicket, Result<Guid>>
{
    public Result<Guid> Handle(CreateTicket command)
    {
        var initializeTicketResult = Ticket.From(command);

        if (initializeTicketResult.IsFailure)
            return initializeTicketResult.Error!;

        var ticket = initializeTicketResult.Value;

        var storeTicketResult = repository.Create(ticket!);

        if (storeTicketResult.IsFailure)
            return storeTicketResult.Error!;

        return ticket!.Id;
    }
}
