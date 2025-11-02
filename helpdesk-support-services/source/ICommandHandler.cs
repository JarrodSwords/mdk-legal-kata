namespace MdkLegal.HelpDesk.Support.Services;

public interface ICommandHandler<in T, out TResult> where T : Command
{
    TResult Handle(T command);
}
