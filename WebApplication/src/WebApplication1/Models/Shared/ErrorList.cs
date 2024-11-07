using System.Collections;

namespace WebApplication1.Models.Shared;

public class ErrorList : IEnumerable<Error>
{
    public readonly List<Error> Value;

    private ErrorList(IEnumerable<Error> errors)
    {
        Value = [.. errors];
    }

    public IEnumerator<Error> GetEnumerator()
    {
        return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator ErrorList(List<Error> errors)
        => new ErrorList(errors);

    public static implicit operator ErrorList(Error errors)
        => new ErrorList([errors]);
}