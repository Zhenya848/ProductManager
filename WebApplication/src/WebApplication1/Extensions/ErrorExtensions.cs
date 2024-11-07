using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Shared;

namespace WebApplication1.Extensions;

public static class ErrorExtensions
{
    public static ActionResult ToResponse(this Error error)
    {
        Envelope envelope = Envelope.Error(error);

        return new ObjectResult(envelope) { StatusCode = GetStatusCode(error) };
    }

    public static ActionResult ToResponse(this ErrorList errors)
    {
        if (errors.Any() == false)
            return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };

        Envelope envelope = Envelope.Error(errors);

        return new ObjectResult(envelope) { StatusCode = GetStatusCode(errors) };
    }
    
    private static int GetStatusCode(ErrorList errors)
    {
        var distinctErrorTypes = errors.Select(e => e.ErrorType).Distinct().ToList();
        var statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : distinctErrorTypes.First() switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException()
            };

        return statusCode;
    }
}