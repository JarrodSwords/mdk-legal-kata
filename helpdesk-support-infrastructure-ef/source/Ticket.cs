namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class Ticket : Entity
{
    public Ticket(string title)
    {
        Title = title;
    }

    public Ticket(Domain.Ticket source) : this(source.Title)
    {
    }

    public string Title { get; set; }

    public static implicit operator Ticket(Domain.Ticket source) => new(source);
}
