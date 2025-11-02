using FluentAssertions;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using Ticket = MdkLegal.HelpDesk.Support.Domain.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenCreatingATicket
{
    #region Requirements

    [Theory]
    [ClassData(typeof(NullOrWhitespace))]
    public void ThenTitleIsRequired(string invalidTitle)
    {
        var createTicket = new CreateTicket(invalidTitle);
        var repository = new TicketRepository(new Context());
        var handler = new CreateTicketHandler(repository);

        var error = handler.Handle(createTicket).Error;

        error.Should().Be(Ticket.TitleRequired());
    }

    #endregion
}
