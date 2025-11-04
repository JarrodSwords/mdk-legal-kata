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
