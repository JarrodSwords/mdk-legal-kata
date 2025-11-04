using MdkLegal.HelpDesk.Support.Domain;
using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class UserRepository(Context context) : IUserRepository
{
    public Result Delete(Guid id)
    {
        var user = context.User.Find(id);

        context.User.Remove(user);
        context.SaveChanges();

        return Success();
    }

    public Result Create(Domain.User user)
    {
        context.User.Add(user);
        context.SaveChanges();

        return Success();
    }
}
