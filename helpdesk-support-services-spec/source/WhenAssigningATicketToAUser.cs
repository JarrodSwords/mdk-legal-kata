using FluentAssertions;
using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class WhenAssigningATicketToAUser : TicketSpec, IAsyncLifetime
{
    #region Setup

    private readonly AssignUserHandler _assignUserHandler;
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly RegisterUserHandler _registerUserHandler;
    private Ticket? _ticket;
    private Guid _ticketId;
    private Guid _userId;

    public WhenAssigningATicketToAUser()
    {
        var userRepository = new UserRepository(Context);
        _deleteUserHandler = new(userRepository);
        _registerUserHandler = new(userRepository);
        _assignUserHandler = new AssignUserHandler(TicketRepository);
    }

    #endregion

    #region Implementation

    public Task DisposeAsync()
    {
        DeleteTicket(_ticketId);

        _deleteUserHandler.Handle(new(_userId));

        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        var command = new ValidCreateTicketCommands().First()[0] as CreateTicket;
        _ticketId = CreateTicket(command!);

        _userId = _registerUserHandler.Handle(new("JohnDoe", "john.doe@gmail.com"));

        return Task.CompletedTask;
    }

    #endregion

    #region Requirements

    [Fact]
    public void ThenAssignedUserIdIsSet()
    {
        var command = new AssignUser(_ticketId, _userId);

        _ticket = _assignUserHandler.Handle(command)
            .Then(() => FindTicket(_ticketId));

        _ticket.UserId.Should().Be(command.UserId);
    }

    #endregion
}
