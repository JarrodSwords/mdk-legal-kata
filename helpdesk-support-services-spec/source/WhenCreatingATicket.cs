using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.WebApi;
using Ticket = MdkLegal.HelpDesk.Support.Domain.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenCreatingATicket : IAsyncLifetime
{
    #region Setup

    private readonly CreateTicketHandler _createTicketHandler;
    private readonly DeleteTicketHandler _deleteTicketHandler;
    private readonly FindTicketHandler _findTicketHandler;
    private Read.Ticket? _ticket;

    public WhenCreatingATicket()
    {
        var context = new Context(DbOptionsFactory.DbContextOptions);
        var repository = new TicketRepository(context);
        _createTicketHandler = new(repository);
        _deleteTicketHandler = new(repository);
        var provider = new ConnectionStringProvider(DbOptionsFactory.Configuration);
        _findTicketHandler = new(provider);
    }

    #endregion

    #region Implementation

    public static IEnumerable<object[]> CreateTicketCommands()
    {
        yield return [new CreateTicket("Laptop battery request")];
        yield return [new CreateTicket("Need new security token")];
    }

    public Task DisposeAsync()
    {
        if (_ticket is null)
            return Task.CompletedTask;

        _deleteTicketHandler.Handle(new(_ticket.Id));

        return Task.CompletedTask;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    #endregion

    #region Requirements

    [Theory]
    [MemberData(nameof(CreateTicketCommands))]
    public void ThenTicketIsExpected(CreateTicket command)
    {
        _ticket = _createTicketHandler.Handle(command)
            .Then(ticketId => _findTicketHandler.Handle(new(ticketId))).Value!;

        using var scope = new AssertionScope();

        _ticket.Title.Should().Be(command.Title);
    }

    [Theory]
    [ClassData(typeof(NullOrWhitespace))]
    public void ThenTitleIsRequired(string invalidTitle)
    {
        var createTicket = new CreateTicket(invalidTitle);

        var error = _createTicketHandler.Handle(createTicket).Error;

        error.Should().Be(Ticket.TitleRequired());
    }

    #endregion
}
