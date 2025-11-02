using FluentAssertions;
using FluentAssertions.Execution;
using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using Ticket = MdkLegal.HelpDesk.Support.Domain.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenCreatingATicket
{
    #region Setup

    private readonly CreateTicketHandler _createTicketHandler;
    private readonly FindTicketHandler _findTicketHandler;
    private readonly ITicketRepository _repository;

    public WhenCreatingATicket()
    {
        _repository = new TicketRepository(new Context());
        _createTicketHandler = new(_repository);
    }

    #endregion

    #region Implementation

    public static IEnumerable<object[]> CreateTicketCommands()
    {
        yield return [new CreateTicket("Laptop battery request")];
        yield return [new CreateTicket("Need new security token")];
    }

    #endregion

    #region Requirements

    [Theory]
    [MemberData(nameof(CreateTicketCommands))]
    public void ThenTicketIsExpected(CreateTicket command)
    {
        var ticket = _createTicketHandler.Handle(command)
            .Then(ticketId => _findTicketHandler.Handle(new(ticketId))).Value;

        using var scope = new AssertionScope();

        ticket.Title.Should().Be(command.Title);
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
