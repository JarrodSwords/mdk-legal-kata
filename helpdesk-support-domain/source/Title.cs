using MdkLegal.Kernel;

namespace MdkLegal.HelpDesk.Support.Domain;

public class Title(string value) : TinyType<string>(value)
{
    public static Result<Title> From(string title) =>
        string.IsNullOrWhiteSpace(title)
            ? Ticket.TitleRequired()
            : new Title(title);

    public static bool From(string candidateTitle, out Title title)
    {
        title = null;
        var result = From(candidateTitle);

        if (result.IsFailure)
            return false;

        title = result.Value!;

        return true;
    }
}
