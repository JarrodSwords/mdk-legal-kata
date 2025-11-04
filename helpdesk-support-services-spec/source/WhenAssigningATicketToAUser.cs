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

    private readonly AssignUserHandler _assignUserHandler;
    private readonly CreateTicketHandler _createTicketHandler;
    private readonly DeleteTicketHandler _deleteTicketHandler;
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly FindTicketHandler _findTicketHandler;
    private readonly RegisterUserHandler _registerUserHandler;
    private Ticket? _ticket;
    private Guid _ticketId;
    private Guid _userId;

    public WhenAssigningATicketToAUser()
    {
        var context = new Context(DbOptionsFactory.DbContextOptions);
        var ticketRepository = new TicketRepository(context);
        var userRepository = new UserRepository(context);
        _createTicketHandler = new(ticketRepository);
        _deleteTicketHandler = new(ticketRepository);
        _deleteUserHandler = new(userRepository);
        _registerUserHandler = new(userRepository);
        var provider = new ConnectionStringProvider(DbOptionsFactory.Configuration);
        _findTicketHandler = new(provider);
        _assignUserHandler = new AssignUserHandler(ticketRepository);
    }

    #endregion

    #region Implementation

    public Task DisposeAsync()
    {
        _deleteTicketHandler.Handle(new(_ticketId));
        _deleteUserHandler.Handle(new(_userId));

        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        var command = new ValidCreateTicketCommands().First()[0] as CreateTicket;
        _ticketId = _createTicketHandler.Handle(command!).Value;

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
            .Then(() => _findTicketHandler.Handle(new(_ticketId))).Value!;

        _ticket.UserId.Should().Be(command.UserId);
    }

    #endregion
}
