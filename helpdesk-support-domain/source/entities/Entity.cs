namespace MdkLegal.HelpDesk.Support.Domain;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; }
}
