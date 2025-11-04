using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public record CreateTicket(
    string Description,
    string Title
) : Command;

public partial class Ticket
{
    private Ticket(Description description, Title title)
    {
        Description = description;
        Title = title;
        Status = TicketStatus.Open;
        CreatedAt = DateTime.Now;
    }

    public static Result<Ticket> From(CreateTicket command)
    {
        if (!Description.From(command.Description, out var description))
            return DescriptionRequired();

        if (!Title.From(command.Title, out var title))
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
