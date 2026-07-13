namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Normalized error shape returned by <see cref="ApiResult{T}"/> on failure.
    /// Designed to map cleanly onto the backend's Result/Error pattern
    /// (NexAsset.Domain / NexAsset.Application) once real HTTP calls are wired in.
    /// </summary>
    public class ApiError
    {
        public string Message { get; set; } = "Something went wrong. Please try again.";
        public int? StatusCode { get; set; }
        public string? Code { get; set; }
        public string? Details { get; set; }

        public static ApiError FromException(System.Exception ex) => new()
        {
            Message = "An unexpected error occurred.",
            Details = ex.Message
        };

        public static ApiError FromStatusCode(int statusCode, string? message = null) => new()
        {
            StatusCode = statusCode,
            Message = message ?? DefaultMessageForStatus(statusCode)
        };

        private static string DefaultMessageForStatus(int statusCode) => statusCode switch
        {
            400 => "The request was invalid.",
            401 => "You need to sign in to continue.",
            403 => "You don't have permission to do that.",
            404 => "The requested item could not be found.",
            409 => "This item was changed by someone else. Please refresh and try again.",
            >= 500 => "The server encountered a problem. Please try again shortly.",
            _ => "Something went wrong. Please try again."
        };
    }
}
