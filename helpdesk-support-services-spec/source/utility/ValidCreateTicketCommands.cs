using System.Collections;
using MdkLegal.HelpDesk.Support.Domain;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class ValidCreateTicketCommands : IEnumerable<object[]>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new CreateTicket(
                "Current battery life is too short.",
                "Laptop battery request"
            )
        ];

        yield return
        [
            new CreateTicket(
                "John lost his security token in St. Louis last week.",
                "Need new security token"
            )
        ];
    }
}
