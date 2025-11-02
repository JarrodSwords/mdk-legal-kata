using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public interface ITicketRepository
{
    /// <summary>
    ///     Stores a <see cref="Ticket" />.
    /// </summary>
    /// <param name="ticket">The <see cref="Ticket" /> domain object.</param>
    Result Create(Ticket ticket);

    Result Delete(Guid id);
}
