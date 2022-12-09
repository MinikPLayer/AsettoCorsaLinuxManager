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

    public T? Unwrap(Action<string>? failAction = null)
    {
        return Except("Failed to unwrap Result", failAction);
    }

    public T? Except(string message, Action<string>? failAction = null)
    {
        if (Good)
            return Value;

        if (failAction != null)
        {
            failAction(Message);
            return default(T);
        }
        
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