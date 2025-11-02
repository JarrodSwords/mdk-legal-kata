namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class Ticket : Entity
{
    public Ticket()
    {
    }

    public Ticket(Guid id, string title) : base(id)
    {
        Title = title;
    }

    public Ticket(Domain.Ticket source) : this(
        source.Id,
        source.Title
    )
    {
    }

    public string Title { get; set; }

    public static implicit operator Ticket(Domain.Ticket source) => new(source);
}
