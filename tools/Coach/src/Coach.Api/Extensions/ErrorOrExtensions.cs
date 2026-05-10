using ErrorOr;

namespace Coach.Api.Extensions;

/// <summary>
/// Extension methods for converting ErrorOr results to HTTP Results.
/// </summary>
public static class ErrorOrExtensions
{
    public static IResult ToProblemDetails(this List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem(
                title: "An error occurred",
                statusCode: StatusCodes.Status500InternalServerError);
        }

        // If all errors are validation errors, return validation problem
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var validationErrors = errors.ToDictionary(
                e => e.Code,
                e => new[] { e.Description });

            return Results.ValidationProblem(validationErrors);
        }

        // Map first error to appropriate status code
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            statusCode: statusCode,
            title: firstError.Code,
            detail: firstError.Description,
            extensions: errors.Count > 1
                ? new Dictionary<string, object?> { ["errors"] = errors.Select(e => e.Description) }
                : null);
    }

    public static IResult ToCreatedResult<T>(this ErrorOr<T> result, Func<T, string> location, Func<T, object> mapper)
    {
        return result.Match(x => Results.Created(location(x), mapper(x)), x=> x.ToProblemDetails());
    }

    public static IResult ToOkResult<T>(this ErrorOr<T> result, Func<T, object> mapper)
    {
        return result.Match(x => Results.Ok(mapper(x)), x => x.ToProblemDetails());
    }

    public static IResult ToOkResult<T>(this ErrorOr<T> result)
        where T : class
    {
        return result.Match(Results.Ok, x => x.ToProblemDetails());
    }

    public static IResult ToNoContentResult(this ErrorOr<Success> result)
    {
        return result.Match(_ => Results.NoContent(), x => x.ToProblemDetails());
    }

    public static async Task<IResult> ToCreatedAsync<T>(
        this Task<ErrorOr<T>> task,
        Func<T, string> location,
        Func<T, object> mapper)
    {
        return (await task).ToCreatedResult(location, mapper);
    }

    public static async Task<IResult> ToOkAsync<T>(this Task<ErrorOr<T>> task, Func<T, object> mapper)
    {
        return (await task).ToOkResult(mapper);
    }

    public static async Task<IResult> ToOkAsync<T>(this Task<ErrorOr<T>> task)
        where T : class
    {
        return (await task).ToOkResult();
    }

    public static async Task<IResult> ToNoContentAsync(this Task<ErrorOr<Success>> task)
    {
        return (await task).ToNoContentResult();
    }
}
