namespace MdkLegal.HelpDesk.Support.Domain;

public partial class Ticket : Entity
{
    private Ticket(Guid id, string description, string title) : base(id)
    {
        Description = description;
        Title = title;
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

    public Guid? AssignedUserId { get; }
    public DateTime CreatedAt { get; }
    public string Description { get; }
    public TicketStatus Status { get; }
    public string Title { get; }
    public DateTime? UpdatedAt { get; }
}
