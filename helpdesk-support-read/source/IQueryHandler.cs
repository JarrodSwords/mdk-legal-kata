namespace MdkLegal.HelpDesk.Support.Read;

public interface IQueryHandler<in T, out TResult> where T : Query
{
    TResult Handle(T query);
}
