using FluentAssertions;
using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.WebApi;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenAssigningATicketToAUser : IAsyncLifetime
{
    #region Setup

    private readonly AssignTicketHandler _assignTicketHandler;
    private readonly CreateTicketHandler _createTicketHandler;
    private readonly DeleteTicketHandler _deleteTicketHandler;
    private readonly FindTicketHandler _findTicketHandler;
    private Ticket? _ticket;
    private Guid _ticketId;

    public WhenAssigningATicketToAUser()
    {
        var context = new Context(DbOptionsFactory.DbContextOptions);
        var repository = new TicketRepository(context);
        _createTicketHandler = new(repository);
        _deleteTicketHandler = new(repository);
        var provider = new ConnectionStringProvider(DbOptionsFactory.Configuration);
        _findTicketHandler = new(provider);
        _assignTicketHandler = new AssignTicketHandler(repository);
    }

    #endregion

    #region Implementation

    public Task DisposeAsync()
    {
        _deleteTicketHandler.Handle(new(_ticketId));

        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        var command = new ValidCreateTicketCommands().First()[0] as CreateTicket;
        _ticketId = _createTicketHandler.Handle(command!).Value;

        return Task.CompletedTask;
    }

    #endregion

    #region Requirements

    [Fact]
    public void ThenAssignedUserIdIsSet()
    {
        var command = new AssignTicket(_ticketId, Guid.NewGuid());

        _ticket = _assignTicketHandler.Handle(command)
            .Then(() => _findTicketHandler.Handle(new(_ticketId))).Value!;

        _ticket.AssignedUserId.Should().Be(command.UserId);
    }

    #endregion
}
