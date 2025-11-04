using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.WebApi;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenUpdatingATicket : IAsyncLifetime
{
    #region Setup

    private readonly CreateTicketHandler _createTicketHandler;
    private readonly DeleteTicketHandler _deleteTicketHandler;
    private readonly FindTicketHandler _findTicketHandler;
    private readonly UpdateTicketHandler _updateTicketHandler;
    private Ticket _originalTicket;
    private Ticket? _ticket;
    private Guid _ticketId;

    public WhenUpdatingATicket()
    {
        var context = new Context(DbOptionsFactory.DbContextOptions);
        var repository = new TicketRepository(context);
        _createTicketHandler = new(repository);
        _deleteTicketHandler = new(repository);
        var provider = new ConnectionStringProvider(DbOptionsFactory.Configuration);
        _findTicketHandler = new(provider);
        _updateTicketHandler = new UpdateTicketHandler(repository);
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

        _originalTicket = _findTicketHandler.Handle(new(_ticketId)).Value!;

        return Task.CompletedTask;
    }

    #endregion

    #region Requirements

    [Fact]
    public void ThenTicketIsUpdated()
    {
        var command = new UpdateTicket(
            _ticketId,
            "New Description",
            true,
            false,
            false,
            "New Title"
        );

        _ticket = _updateTicketHandler.Handle(command)
            .Then(() => _findTicketHandler.Handle(new(_ticketId))).Value!;

        using var scope = new AssertionScope();

        _ticket.Title.Should().Be(command.Title);
        _ticket.Description.Should().Be(command.Description);
        _ticket.IsClosed.Should().BeTrue();
        _ticket.IsInProgress.Should().BeFalse();
        _ticket.IsOpen.Should().BeFalse();
        _ticket.CreatedAt.Should().Be(_originalTicket.CreatedAt);
        _ticket.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        _ticket.UpdatedAt.Should().BeAfter(_ticket.CreatedAt);
        _ticket.AssignedUserId.Should().BeNull();
    }

    #endregion
}
