using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Domain;
using Ticket = MdkLegal.HelpDesk.Support.Domain.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenCreatingATicket : TicketSpec, IAsyncLifetime
{
    #region Setup

    private Read.Ticket? _ticket;

    #endregion

    #region Implementation

    public Task DisposeAsync()
    {
        if (_ticket is null)
            return Task.CompletedTask;

        DeleteTicket(_ticket.Id);

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

        var error = CreateTicketHandler.Handle(createTicket).Error;

        error.Should().Be(Ticket.DescriptionRequired());
    }

    [Theory]
    [ClassData(typeof(ValidCreateTicketCommands))]
    public void ThenTicketIsExpected(CreateTicket command)
    {
        _ticket = CreateTicketHandler.Handle(command)
            .Then(FindTicket).Value!;

        using var scope = new AssertionScope();

        _ticket.Title.Should().Be(command.Title);
        _ticket.Description.Should().Be(command.Description);
        _ticket.IsClosed.Should().BeFalse();
        _ticket.IsInProgress.Should().BeFalse();
        _ticket.IsOpen.Should().BeTrue();
        _ticket.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        _ticket.UpdatedAt.Should().BeNull();
        _ticket.UserId.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(NullOrWhitespace))]
    public void ThenTitleIsRequired(string invalidTitle)
    {
        var createTicket = new CreateTicket("Description", invalidTitle);

        var error = CreateTicketHandler.Handle(createTicket).Error;

        error.Should().Be(Ticket.TitleRequired());
    }

    #endregion
}
