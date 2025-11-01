namespace MdkLegal.HelpDesk.Support.Domain;

public class Ticket : Entity
{
    private Ticket(Guid id, string title) : base(id)
    {
        Title = title;
    }

    private Ticket(string title)
    {
    }

    public string Title { get; }

    public Result<Ticket> From(string title) => new Ticket(title);

    public static Error TitleRequired() =>
        new(
            "ticket-title-required",
            $"A {nameof(Ticket)} must have a {nameof(Title)}."
        );
}
