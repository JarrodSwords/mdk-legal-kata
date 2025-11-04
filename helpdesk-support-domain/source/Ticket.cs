using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public partial class Ticket : Entity
{
    public void Deconstruct(
        out Guid id,
        out Guid? assignedUserId,
        out DateTime createdAt,
        out string description,
        out TicketStatus status,
        out string title,
        out DateTime? updatedAt
    )
    {
        id = Id;
        assignedUserId = AssignedUserId;
        createdAt = CreatedAt;
        description = Description;
        status = Status;
        title = Title;
        updatedAt = UpdatedAt;
    }

    public Guid? AssignedUserId { get; }
    public DateTime CreatedAt { get; set; }
    public Description Description { get; set; }
    public TicketStatus Status { get; set; }
    public Title Title { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Description(string value) : TinyType<string>(value)
{
    public static Result<Description> From(string title) =>
        string.IsNullOrWhiteSpace(title)
            ? Ticket.DescriptionRequired()
            : new Description(title);

    public static bool From(string candidateDescription, out Description description)
    {
        description = null;
        var result = From(candidateDescription);

        if (result.IsFailure)
            return false;

        description = result.Value!;

        return true;
    }
}

public class Title(string value) : TinyType<string>(value)
{
    public static Result<Title> From(string title) =>
        string.IsNullOrWhiteSpace(title)
            ? Ticket.TitleRequired()
            : new Title(title);

    public static bool From(string candidateTitle, out Title title)
    {
        title = null;
        var result = From(candidateTitle);

        if (result.IsFailure)
            return false;

        title = result.Value!;

        return true;
    }
}
