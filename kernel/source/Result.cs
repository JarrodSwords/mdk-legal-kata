namespace MdkLegal.Kernel;

/// <summary>
///     Encapsulates a success or failure.
/// </summary>
/// <remarks>
///     Leveraged for railway programming.
/// </remarks>
public class Result
{
    public Result(Error? error = null)
    {
        Error = error;
    }

    public Error? Error { get; }
    public bool IsFailure => !IsSuccess;
    public bool IsSuccess => Error is null;

    public Result Catch(Func<Error, Result> next) => IsFailure ? next(Error!) : this;
    public Result Then(Func<Result> next) => IsSuccess ? next() : this;
    public Result<T> Then<T>(Func<Result<T>> next) => IsSuccess ? next() : Error!;

    public Result Then(Action next)
    {
        if (IsSuccess)
            next();

        return this;
    }

    public static Result Failure(Error error) => new(error);
    public static Result<T> Failure<T>(Error error) => new(error);

    public static Result Success() => new();
    public static Result<T> Success<T>(T value) => new(value);

    public static implicit operator Error(Result source) => source.Error;
    public static implicit operator Result(Error error) => new(error);
}
