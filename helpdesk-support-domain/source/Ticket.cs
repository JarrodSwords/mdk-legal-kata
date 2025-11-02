using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public class Ticket : Entity
{
    private Ticket(Guid id, string description, string title) : base(id)
    {
        Description = description;
        Title = title;
    }

    private Ticket(string description, string title)
    {
        Description = description;
        Title = title;
        Status = TicketStatus.Open;
        CreatedAt = DateTime.Now;
    }

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

    public static Result<Ticket> From(string description, string title)
    {
        if (string.IsNullOrWhiteSpace(description))
            return DescriptionRequired();

        if (string.IsNullOrWhiteSpace(title))
            return TitleRequired();

        return new Ticket(description, title);
    }

    public Guid? AssignedUserId { get; }
    public DateTime CreatedAt { get; }
    public string Description { get; }
    public TicketStatus Status { get; }
    public string Title { get; }
    public DateTime? UpdatedAt { get; }

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
