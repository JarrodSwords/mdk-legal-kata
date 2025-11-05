using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.WebApi;
using MdkLegal.Kernel;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public abstract class TicketSpec
{
    protected readonly Context Context;
    protected readonly CreateTicketHandler CreateTicketHandler;
    protected readonly ITicketRepository TicketRepository;
    private readonly DeleteTicketHandler _deleteTicketHandler;
    private readonly FindTicketHandler _findTicketHandler;

    protected TicketSpec()
    {
        Context = new Context(DbOptionsFactory.DbContextOptions);
        TicketRepository = new TicketRepository(Context);
        CreateTicketHandler = new(TicketRepository);
        _deleteTicketHandler = new(TicketRepository);

        var connectionStringProvider = new ConnectionStringProvider(DbOptionsFactory.Configuration);
        _findTicketHandler = new(connectionStringProvider);
    }

    protected Guid CreateTicket(CreateTicket command) => CreateTicketHandler.Handle(command).Value;

    protected void DeleteTicket(Guid ticketId)
    {
        _deleteTicketHandler.Handle(new(ticketId));
    }

    protected Result<Ticket> FindTicket(Guid ticketId) => _findTicketHandler.Handle(new(ticketId)).Value!;
}
