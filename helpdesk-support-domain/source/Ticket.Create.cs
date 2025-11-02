using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public record CreateTicket(
    string Description,
    string Title
) : Command;

public partial class Ticket
{
    private Ticket(string description, string title)
    {
        Description = description;
        Title = title;
        Status = TicketStatus.Open;
        CreatedAt = DateTime.Now;
    }

    public static Result<Ticket> From(CreateTicket command)
    {
        var (description, title) = command;

        if (string.IsNullOrWhiteSpace(description))
            return DescriptionRequired();

        if (string.IsNullOrWhiteSpace(title))
            return TitleRequired();

        return new Ticket(description, title);
    }

    public static Error DescriptionRequired() =>
        new(
            "ticket-description-required",
            $"A {nameof(Ticket)} must have a {nameof(Description)}."
        );

    public static Error TitleRequired() =>
        new(
            "ticket-title-required",
            $"A {nameof(Ticket)} must have a {nameof(Title)}."
        );
}
