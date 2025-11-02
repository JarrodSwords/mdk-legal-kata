namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public abstract class Entity
{
    protected Entity()
    {
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
