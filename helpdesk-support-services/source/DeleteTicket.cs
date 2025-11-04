using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

public record DeleteUser(Guid UserId) : Command;

public class DeleteUserHandler(IUserRepository repository)
    : ICommandHandler<DeleteUser, Result>
{
    public Result Handle(DeleteUser command) => repository.Delete(command.UserId);
}
