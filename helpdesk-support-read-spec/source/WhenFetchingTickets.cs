using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Services;
using MdkLegal.HelpDesk.Support.Services.Spec;
using MdkLegal.HelpDesk.Support.WebApi;

namespace MdkLegal.HelpDesk.Support.Read.Spec;

public class WhenFetchingTickets : IAsyncLifetime
{
    #region Setup

    private readonly CreateTicketHandler _createTicketHandler;
    private readonly DeleteTicketHandler _deleteTicketHandler;
    private readonly FetchTicketsHandler _fetchTicketsHandler;
    private readonly List<Guid> _ticketIds = new();

    public WhenFetchingTickets()
    {
        var context = new Context(DbOptionsFactory.DbContextOptions);
        var repository = new TicketRepository(context);
        _createTicketHandler = new(repository);
        _deleteTicketHandler = new(repository);
        var provider = new ConnectionStringProvider(DbOptionsFactory.Configuration);
        _fetchTicketsHandler = new FetchTicketsHandler(provider);
    }

    #endregion

    #region Implementation

    public Task DisposeAsync()
    {
        foreach (var ticketId in _ticketIds)
            _deleteTicketHandler.Handle(new(ticketId));

        return Task.CompletedTask;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    #endregion

    #region Requirements

    [Theory]
    [ClassData(typeof(ValidCreateTicketCommands))]
    public void ThenTicketsAreExpected(CreateTicket[] commands)
    {
        foreach (var command in commands)
        {
            var ticketId = _createTicketHandler.Handle(command).Value;
            _ticketIds.Add(ticketId);
        }

        var tickets = _fetchTicketsHandler.Handle(new()).Value;

        using var scope = new AssertionScope();

        tickets.Should().HaveCount(2);
        tickets.Should().OnlyContain(x => _ticketIds.Contains(x.Id));
    }

    #endregion
}
