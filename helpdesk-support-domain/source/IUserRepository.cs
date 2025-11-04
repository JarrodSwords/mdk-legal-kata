using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public interface IUserRepository
{
    Result Create(User user);
    Result Delete(Guid id);
}
