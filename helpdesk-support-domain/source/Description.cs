using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public class Description(string value) : TinyType<string>(value)
{
    public static Result<Description> From(string title) =>
        string.IsNullOrWhiteSpace(title)
            ? Ticket.DescriptionRequired()
            : new Description(title);

    public static bool From(string candidateDescription, out Description description)
    {
        description = null;
        var result = From(candidateDescription);

        if (result.IsFailure)
            return false;

        description = result.Value!;

        return true;
    }
}
