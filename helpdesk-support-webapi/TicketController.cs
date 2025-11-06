using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.Services;
using MdkLegal.Kernel;
using Microsoft.AspNetCore.Mvc;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.WebApi;

[Route("api/tickets")]
[ApiController]
public class TicketController(
    ICommandHandler<AssignUser, Result> assignUserHandler,
    ICommandHandler<CreateTicket, Result<Guid>> createTicketHandler,
    ICommandHandler<Services.UpdateTicket, Result> updateTicketHandler,
    IQueryHandler<FetchTickets, Result<IEnumerable<FetchTicketsHandler.Ticket>>> fetchTicketsHandler,
    IQueryHandler<FindTicket, Result<Ticket>> findTicketHandler
) : ControllerBase
{
    [HttpPut("{id:guid}/assign-user")]
    [ProducesResponseType(typeof(Ticket), Status200OK)]
    [ProducesResponseType(typeof(Error), Status404NotFound)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult AssignUser(Guid id, AssignUser command) =>
        assignUserHandler.Handle(command)
            .Then<IActionResult>(() => Ok()).Value!;

    [HttpPost]
    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult Create(CreateTicket args) =>
        createTicketHandler.Handle(args)
            .Then<IActionResult>(
                id => CreatedAtAction(
                    nameof(Find),
                    new { Id = id },
                    id
                )
            ).Value!;

    [HttpGet]
    [ProducesResponseType(typeof(Ticket), Status200OK)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult Fetch() =>
        fetchTicketsHandler.Handle(new())
            .Then<IActionResult>(tickets => Ok(tickets)).Value!;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Ticket), Status200OK)]
    [ProducesResponseType(typeof(Error), Status404NotFound)]
    [ProducesResponseType(typeof(Error), Status409Conflict)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult Find(Guid id) =>
        findTicketHandler.Handle(new(id))
            .Then<IActionResult>(ticket => Ok(ticket)).Value!;

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Ticket), Status200OK)]
    [ProducesResponseType(typeof(Error), Status404NotFound)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult Update(Guid id, UpdateTicket command) =>
        updateTicketHandler.Handle(command)
            .Then<IActionResult>(() => Ok()).Value!;
}

public record UpdateTicket(
    Guid TicketId,
    string Description,
    string Status,
    string Title
)
{
    public static implicit operator Services.UpdateTicket(UpdateTicket source) =>
        new(
            source.TicketId,
            source.Description,
            TicketStatusFactory.From(source.Status),
            source.Title
        );
}
