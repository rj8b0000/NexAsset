namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Uniform success/failure envelope for future typed HTTP calls, mirroring the
    /// Result pattern already used on the backend (NexAsset.Application). Nothing in
    /// the app produces or consumes this yet — it exists so the next phase can adopt
    /// it without inventing a new shape mid-migration.
    /// </summary>
    public class ApiResult<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public ApiError? Error { get; }

        private ApiResult(bool isSuccess, T? data, ApiError? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static ApiResult<T> Success(T data) => new(true, data, null);

        public static ApiResult<T> Failure(ApiError error) => new(false, default, error);
    }

    /// <summary>
    /// Non-generic counterpart for calls that return no payload (e.g. DELETE).
    /// </summary>
    public class ApiResult
    {
        public bool IsSuccess { get; }
        public ApiError? Error { get; }

        private ApiResult(bool isSuccess, ApiError? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static ApiResult Success() => new(true, null);

        public static ApiResult Failure(ApiError error) => new(false, error);
    }
}
