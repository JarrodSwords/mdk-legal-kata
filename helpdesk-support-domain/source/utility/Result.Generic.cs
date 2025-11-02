namespace MdkLegal.HelpDesk.Support.Domain;

public class Result<T> : Result
{
    public Result(Error error) : base(error)
    {
    }

    public Result(T value)
    {
        Value = value;
    }

    public T? Value { get; }

    public Result<T> Catch(Func<Error, Result<T>> next) => IsFailure ? next(Error!) : this;
    public Result Then(Func<T, Result> next) => IsSuccess ? next(Value!) : this;
    public Result<T> Then(Func<T, Result<T>> next) => IsSuccess ? next(Value!) : this;
    public Result<TOut> Then<TOut>(Func<T, Result<TOut>> next) => IsSuccess ? next(Value!) : Error!;

    public Result<T> Then(Action<T> next)
    {
        if (IsSuccess)
            next(Value!);

        return this;
    }

    public static implicit operator Result<T>(Error error) => new(error);
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator T(Result<T> source) => source.Value;
}
