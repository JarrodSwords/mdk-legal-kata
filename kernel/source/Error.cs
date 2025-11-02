namespace MdkLegal.Kernel;

public class Error(string Code, string Description) : ValueObject
{
    #region Equality

    /// <remarks><see cref="Error" />s disregard <see cref="Description" /> when checking for equality.</remarks>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    #endregion
}
