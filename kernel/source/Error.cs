namespace MdkLegal.Kernel;

public class Error(string code, string description) : ValueObject
{
    #region Equality

    /// <remarks><see cref="Error" />s disregard <see cref="description" /> when checking for equality.</remarks>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return code;
    }

    #endregion
}
