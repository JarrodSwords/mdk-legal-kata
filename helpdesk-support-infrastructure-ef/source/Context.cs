namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class Context : DbContext
{
    public DbSet<Ticket> Ticket { get; set; }
}
