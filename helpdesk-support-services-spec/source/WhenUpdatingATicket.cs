using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Domain;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenUpdatingATicket : TicketSpec, IAsyncLifetime
{
    #region Setup

    private readonly UpdateTicketHandler _updateTicketHandler;
    private Ticket _originalTicket;
    private Ticket? _ticket;
    private Guid _ticketId;

    public WhenUpdatingATicket()
    {
        _updateTicketHandler = new UpdateTicketHandler(TicketRepository);
    }

    #endregion

    #region Implementation

    public Task DisposeAsync()
    {
        DeleteTicket(_ticketId);

        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        var command = new ValidCreateTicketCommands().First()[0] as CreateTicket;

        _ticketId = CreateTicket(command!);
        _originalTicket = FindTicket(_ticketId).Value!;

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
            TicketStatus.Closed,
            "New Title"
        );

        _ticket = _updateTicketHandler.Handle(command)
            .Then(() => FindTicket(_ticketId)).Value!;

        using var scope = new AssertionScope();

        _ticket.Title.Should().Be(command.Title);
        _ticket.Description.Should().Be(command.Description);
        _ticket.IsClosed.Should().BeTrue();
        _ticket.IsInProgress.Should().BeFalse();
        _ticket.IsOpen.Should().BeFalse();
        _ticket.CreatedAt.Should().Be(_originalTicket.CreatedAt);
        _ticket.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        _ticket.UpdatedAt.Should().BeAfter(_ticket.CreatedAt);
        _ticket.UserId.Should().BeNull();
    }

    #endregion
}
