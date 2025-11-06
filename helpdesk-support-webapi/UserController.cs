using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.Services;
using MdkLegal.Kernel;
using Microsoft.AspNetCore.Mvc;
using Ticket = MdkLegal.HelpDesk.Support.Read.Ticket;
using User = MdkLegal.HelpDesk.Support.Read.User;

namespace MdkLegal.HelpDesk.Support.WebApi;

[Route("api/users")]
[ApiController]
public class UserController(
    IQueryHandler<FetchUsers, Result<IEnumerable<User>>> fetchUsersHandler,
    ICommandHandler<RegisterUser, Result<Guid>> registerUserHandler
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(Ticket), Status200OK)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult Fetch() =>
        fetchUsersHandler.Handle(new())
            .Then<IActionResult>(users => Ok(users)).Value!;

    [HttpPost("seed")]
    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(typeof(Error), Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public IActionResult Seed()
    {
        registerUserHandler.Handle(new("John Doe", "john.doe@gmail.com"));
        registerUserHandler.Handle(new("Jane Doe", "jane.doe@gmail.com"));

        return Ok();
    }
}
