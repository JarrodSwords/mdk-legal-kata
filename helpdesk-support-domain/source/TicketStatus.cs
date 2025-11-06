namespace MdkLegal.HelpDesk.Support.Domain;

public enum TicketStatus
{
    Open = 0,
    InProgress = 1,
    Closed = 2
}

public class TicketStatusFactory
{
    public static TicketStatus From(string status) =>
        status switch
        {
            "Open" => TicketStatus.Open,
            "In Progress" => TicketStatus.InProgress,
            "Closed" => TicketStatus.Closed,
            _ => TicketStatus.Open
        };
}
