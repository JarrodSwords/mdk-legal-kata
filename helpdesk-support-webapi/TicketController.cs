using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.Services;
using MdkLegal.Kernel;
using Microsoft.AspNetCore.Mvc;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;

namespace MdkLegal.HelpDesk.Support.WebApi;

[Route("api/[controller]")]
[ApiController]
public class TicketController(
    CreateTicketHandler createTicketHandler,
    FetchTicketsHandler fetchTicketsHandler,
    AssignUserHandler assignUserHandler,
    FindTicketHandler findTicketHandler
) : ControllerBase
{
    [HttpPut("{id:guid}/assign-user")]
    [ProducesResponseType(typeof(Ticket), Status200OK)]
    [ProducesResponseType(typeof(Error), Status404NotFound)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult AssignUser(Guid id, Guid userId) =>
        assignUserHandler.Handle(new(id, userId))
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
}
