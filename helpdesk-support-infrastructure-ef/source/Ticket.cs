using static MdkLegal.HelpDesk.Support.Domain.TicketStatus;

namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class Ticket : Entity
{
    public Ticket()
    {
    }

    public Ticket(Domain.Ticket source)
    {
        var (id, assignedUserId, createdAt, description, status, title, updatedAt) = source;

        Id = id;
        AssignedUserId = assignedUserId;
        CreatedAt = createdAt;
        Description = description;
        IsClosed = status == Closed;
        IsInProgress = status == InProgress;
        IsOpen = status == Open;
        Title = title;
        UpdatedAt = updatedAt;
    }

    public void Deconstruct(
        out Guid id,
        out Guid? assignedUserId,
        out DateTime createdAt,
        out string description,
        out bool isClosed,
        out bool isInProgress,
        out bool isOpen,
        out string title,
        out DateTime? updatedAt
    )
    {
        id = Id;
        assignedUserId = AssignedUserId;
        createdAt = CreatedAt;
        description = Description;
        isClosed = IsClosed;
        isInProgress = IsInProgress;
        isOpen = IsOpen;
        title = Title;
        updatedAt = UpdatedAt;
    }

    public Guid? AssignedUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public bool IsClosed { get; set; }
    public bool IsInProgress { get; set; }
    public bool IsOpen { get; set; }
    public string Title { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static implicit operator Ticket(Domain.Ticket source) => new(source);

    public static implicit operator Domain.Ticket(Ticket source)
    {
        var (id, assignedUserId, createdAt, description, isClosed, isInProgress, isOpen, title, updatedAt) = source;

        var status = Open;

        if (isClosed)
            status = Closed;
        else if (isInProgress)
            status = InProgress;

        return new(id, assignedUserId, createdAt, description, status, title, updatedAt);
    }
}
