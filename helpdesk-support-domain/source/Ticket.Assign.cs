namespace MdkLegal.HelpDesk.Support.Domain;

public partial class Ticket
{
    public Ticket Assign(Guid userId)
    {
        AssignedUserId = userId;
        return this;
    }
}
