using System;

namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Thrown by <see cref="ApiClientBase"/> for transport-level failures that callers
    /// choose not to handle via the <see cref="ApiResult{T}"/> return value (e.g. when
    /// using the throwing convenience methods). Not thrown anywhere today — no code
    /// path performs real HTTP calls yet.
    /// </summary>
    public class ApiException : Exception
    {
        public int? StatusCode { get; }
        public ApiError ApiError { get; }

        public ApiException(ApiError apiError, int? statusCode = null)
            : base(apiError.Message)
        {
            ApiError = apiError;
            StatusCode = statusCode;
        }

        public ApiException(ApiError apiError, Exception innerException, int? statusCode = null)
            : base(apiError.Message, innerException)
        {
            ApiError = apiError;
            StatusCode = statusCode;
        }
    }
}
