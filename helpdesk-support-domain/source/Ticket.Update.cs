namespace MdkLegal.HelpDesk.Support.Domain;

public partial class Ticket
{
    /// <summary>
    ///     Recreates a stored <see cref="Ticket" /> in memory.
    /// </summary>
    public Ticket(
        Guid id,
        Guid? assignedUserId,
        DateTime createdAt,
        string description,
        TicketStatus status,
        string title,
        DateTime? updatedAt
    ) : base(id)
    {
        AssignedUserId = assignedUserId;
        CreatedAt = createdAt;
        Description = Description.From(description).Value!;
        Status = status;
        Title = Title.From(title).Value!;
        UpdatedAt = updatedAt;
    }

    public Ticket Close()
    {
        Status = TicketStatus.Closed;
        return this;
    }

    public Ticket Set(Description description)
    {
        Description = description;
        return this;
    }

    public Ticket Set(Title title)
    {
        Title = title;
        return this;
    }
}
