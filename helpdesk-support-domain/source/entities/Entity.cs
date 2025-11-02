namespace MdkLegal.HelpDesk.Support.Domain;

public abstract class Entity(Guid id)
{
    protected Entity() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}
