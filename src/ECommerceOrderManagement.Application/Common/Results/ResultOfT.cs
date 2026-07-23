namespace ECommerceOrderManagement.Application.Common.Results;

public sealed class Result<TValue> : Result, IResult<Result<TValue>>
{
    private readonly TValue? _value;

    private Result(TValue value) : base(true, Error.None)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)

    {
        _value = default;
    }

    public TValue Value =>
            IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failed result cannot be accessed.");


    public static Result<TValue> Success(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Result<TValue>(value);
    }

    public static new Result<TValue> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result<TValue>(error);
    }
}