namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<Ticket> Ticket { get; set; }
}
