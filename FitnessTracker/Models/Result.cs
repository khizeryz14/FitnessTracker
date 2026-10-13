public enum ErrorKind { None, Validation, Conflict }

public class Result<T>
{
    public bool Success { get; private init; }
    public T? Value { get; private init; }
    public string? Error { get; private init; }
    public ErrorKind Kind { get; private init; }

    public static Result<T> Ok(T value) => new() { Success = true, Value = value };
    public static Result<T> Fail(ErrorKind kind, string error) => new() { Kind = kind, Error = error };
}