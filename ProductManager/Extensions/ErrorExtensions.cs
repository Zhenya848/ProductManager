using Microsoft.AspNetCore.Mvc;
using ProductManager.Models;
using ProductManager.Models.Shared;

namespace ProductManager.Extensions;

public static class ErrorExtensions
{
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
    
    public static string GetResponse(this ErrorList errors)
    {
        var distinctErrorMessages = errors.Select(e => e.Message).Distinct().ToList();
        var error = "";

        for (int i = 0; i < distinctErrorMessages.Count - 1; i++)
            error += distinctErrorMessages[i] + ", ";
        
        error += distinctErrorMessages[^1];
        
        var statusCode = GetStatusCode(errors);
                
        return $"Status code {statusCode}: {error}";
    }
}