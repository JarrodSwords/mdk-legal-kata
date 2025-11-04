using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Services;

public record RegisterUser(string Name, string Email) : Command;

public class RegisterUserHandler(IUserRepository repository)
    : ICommandHandler<RegisterUser, Result<Guid>>
{
    public Result<Guid> Handle(RegisterUser command)
    {
        var user = new User(command.Name, command.Email);

        repository.Create(user);

        return user.Id;
    }
}
