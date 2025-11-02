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

    public Guid? AssignedUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public bool IsClosed { get; set; }
    public bool IsInProgress { get; set; }
    public bool IsOpen { get; set; }
    public string Title { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static implicit operator Ticket(Domain.Ticket source) => new(source);
}
