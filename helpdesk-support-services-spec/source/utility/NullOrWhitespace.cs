using System.Collections;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public class NullOrWhitespace : IEnumerable<object[]>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [null];
        yield return [""];
        yield return [" "];
    }
}
