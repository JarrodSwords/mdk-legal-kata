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

    public static Result<Ticket> From(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return TitleRequired();

        return new Ticket(title);
    }

    public string Title { get; }

    public static Error TitleRequired() =>
        new(
            "ticket-title-required",
            $"A {nameof(Ticket)} must have a {nameof(Title)}."
        );
}
