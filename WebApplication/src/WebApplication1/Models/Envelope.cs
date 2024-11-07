using WebApplication1.Models.Shared;

namespace WebApplication1.Models;

public class Envelope
{
    public object? Result { get; }
    public ErrorList? ResponseErrors { get; }

    public DateTime? Time {  get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        ResponseErrors = errors;
        Time = DateTime.Now;
    }

    public static Envelope Ok(object? result) =>
        new Envelope (result, null);

    public static Envelope Error(ErrorList errors) =>
        new Envelope (null, errors);
}