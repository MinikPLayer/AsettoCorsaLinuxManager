namespace AcUtils.Utils;

public class ResultException : Exception
{
    public ResultException(string message = "")
        : base(message)
    {
    }
}

public struct Result<T>
{
    private T? Value;
    public bool Good { get; private set; }
    public string Message { get; private set; }

    public T Unwrap()
    {
        return Expect("Failed to unwrap Result");
    }

    public T Expect(string message)
    {
        if (Good && Value != null)
            return Value;

        throw new ResultException(message);
    }

    public static Result<T> Ok(T value)
    {
        Result<T> r = new Result<T>();
        r.Value = value;
        r.Good = true;
        return r;
    }

    public static Result<T> Err(string message = "")
    {
        Result<T> r = new Result<T>();
        r.Message = message;
        r.Good = false;
        return r;
    }
}