using System.Collections;
using MdkLegal.HelpDesk.Support.Domain;

namespace MdkLegal.HelpDesk.Support.Read.Spec;

public class ValidCreateTicketCommands : IEnumerable<object[]>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new List<CreateTicket>
            {
                new(
                    "Current battery life is too short.",
                    "Laptop battery request"
                ),
                new(
                    "John lost his security token in St. Louis last week.",
                    "Need new security token"
                )
            }
        ];
    }
}
