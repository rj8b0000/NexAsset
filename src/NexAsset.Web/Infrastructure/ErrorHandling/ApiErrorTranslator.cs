using System;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;

namespace NexAsset.Web.Infrastructure.ErrorHandling
{
    /// <summary>
    /// Single place to turn a raw exception or <see cref="ApiError"/> into copy that's
    /// safe to show a user. Consolidates what today is ad-hoc, duplicated try/catch
    /// messaging scattered across pages.
    /// </summary>
    public static class ApiErrorTranslator
    {
        public static string ToFriendlyMessage(ApiError error) => error.Message;

        public static string ToFriendlyMessage(Exception exception) => exception switch
        {
            ApiException apiEx => apiEx.ApiError.Message,
            TaskCanceledException => "The request took too long and was cancelled. Please try again.",
            _ => "Something unexpected happened. Please try again."
        };
    }
}
