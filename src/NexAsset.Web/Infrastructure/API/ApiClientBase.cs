using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Base class for typed API clients that talk to NexAsset.API over HTTP. Wraps
    /// <see cref="HttpClient"/> calls in a uniform try/catch → <see cref="ApiResult{T}"/>
    /// pipeline so individual clients (Authentication today; Organization/Employee/Asset/Vendor
    /// later) only declare business-specific endpoints and never repeat transport or
    /// error-handling boilerplate.
    ///
    /// Error bodies from the backend are normalized here: NexAsset.API returns its
    /// <c>Result.Error</c> as a bare JSON string (e.g. <c>"Invalid credentials."</c>), which
    /// is surfaced verbatim as <see cref="ApiError.Message"/> when present, otherwise a
    /// friendly per-status-code default is used.
    /// </summary>
    public abstract class ApiClientBase
    {
        protected readonly HttpClient Http;

        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        protected ApiClientBase(HttpClient httpClient)
        {
            Http = httpClient;
        }

        protected Task<ApiResult<T>> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
            => ExecuteAsync(async () =>
            {
                var response = await Http.GetAsync(requestUri, cancellationToken);
                return await HandleResponseAsync<T>(response, cancellationToken);
            });

        protected Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string requestUri, TRequest payload, CancellationToken cancellationToken = default)
            => ExecuteAsync(async () =>
            {
                var response = await Http.PostAsJsonAsync(requestUri, payload, JsonOptions, cancellationToken);
                return await HandleResponseAsync<TResponse>(response, cancellationToken);
            });

        protected Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string requestUri, TRequest payload, CancellationToken cancellationToken = default)
            => ExecuteAsync(async () =>
            {
                var response = await Http.PutAsJsonAsync(requestUri, payload, JsonOptions, cancellationToken);
                return await HandleResponseAsync<TResponse>(response, cancellationToken);
            });

        /// <summary>POST with no request body and no response payload (e.g. /logout).</summary>
        protected async Task<ApiResult> PostAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await Http.PostAsync(requestUri, content: null, cancellationToken);
                return response.IsSuccessStatusCode
                    ? ApiResult.Success()
                    : ApiResult.Failure(await BuildErrorAsync(response, cancellationToken));
            }
            catch (Exception ex)
            {
                return ApiResult.Failure(ApiError.FromException(ex));
            }
        }

        protected async Task<ApiResult> DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await Http.DeleteAsync(requestUri, cancellationToken);
                return response.IsSuccessStatusCode
                    ? ApiResult.Success()
                    : ApiResult.Failure(await BuildErrorAsync(response, cancellationToken));
            }
            catch (Exception ex)
            {
                return ApiResult.Failure(ApiError.FromException(ex));
            }
        }

        private static async Task<ApiResult<T>> ExecuteAsync<T>(Func<Task<ApiResult<T>>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Failure(ApiError.FromException(ex));
            }
        }

        private static async Task<ApiResult<T>> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                return ApiResult<T>.Failure(await BuildErrorAsync(response, cancellationToken));
            }

            var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
            if (data is null)
            {
                return ApiResult<T>.Failure(new ApiError { Message = "The server returned an empty response." });
            }

            return ApiResult<T>.Success(data);
        }

        /// <summary>
        /// Turns a non-success response into an <see cref="ApiError"/>, preferring the backend's
        /// own error text when it sends one. Never throws — falls back to the status default.
        /// </summary>
        private static async Task<ApiError> BuildErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            string? serverMessage = null;
            try
            {
                var raw = (await response.Content.ReadAsStringAsync(cancellationToken))?.Trim();
                if (!string.IsNullOrEmpty(raw))
                {
                    if (raw.StartsWith('"') && raw.EndsWith('"') && raw.Length >= 2)
                    {
                        // Bare JSON string, e.g. Results.BadRequest("Invalid credentials.")
                        serverMessage = JsonSerializer.Deserialize<string>(raw, JsonOptions);
                    }
                    else if (!raw.StartsWith('{') && !raw.StartsWith('['))
                    {
                        serverMessage = raw;
                    }
                    // A JSON object/array (e.g. ProblemDetails) is left for the status default,
                    // so we never leak a raw serialized envelope into the UI.
                }
            }
            catch
            {
                // Best-effort only — fall through to the friendly per-status default.
            }

            return ApiError.FromStatusCode((int)response.StatusCode, string.IsNullOrWhiteSpace(serverMessage) ? null : serverMessage);
        }
    }
}
