using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Domain;
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
    [ClassData(typeof(NullOrWhitespace))]
    public void ThenDescriptionIsRequired(string invalidDescription)
    {
        var createTicket = new CreateTicket(invalidDescription, "Title");

        var error = _createTicketHandler.Handle(createTicket).Error;

        error.Should().Be(Ticket.DescriptionRequired());
    }

    [Theory]
    [ClassData(typeof(ValidCreateTicketCommands))]
    public void ThenTicketIsExpected(CreateTicket command)
    {
        _ticket = _createTicketHandler.Handle(command)
            .Then(ticketId => _findTicketHandler.Handle(new(ticketId))).Value!;

        using var scope = new AssertionScope();

        _ticket.Title.Should().Be(command.Title);
        _ticket.Description.Should().Be(command.Description);
        _ticket.IsClosed.Should().BeFalse();
        _ticket.IsInProgress.Should().BeFalse();
        _ticket.IsOpen.Should().BeTrue();
        _ticket.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        _ticket.UpdatedAt.Should().BeNull();
        _ticket.AssignedUserId.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(NullOrWhitespace))]
    public void ThenTitleIsRequired(string invalidTitle)
    {
        var createTicket = new CreateTicket("Description", invalidTitle);

        var error = _createTicketHandler.Handle(createTicket).Error;

        error.Should().Be(Ticket.TitleRequired());
    }

    #endregion
}
