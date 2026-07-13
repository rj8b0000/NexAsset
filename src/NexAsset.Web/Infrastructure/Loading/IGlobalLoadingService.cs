using System;

namespace NexAsset.Web.Infrastructure.Loading
{
    /// <summary>
    /// Tracks in-flight operations so multiple concurrent callers can each signal "I'm loading"
    /// without stomping on each other (a plain bool would go false as soon as the first of two
    /// simultaneous calls finished). Service-only this phase — no new global loading indicator UI
    /// is introduced; existing per-page <c>LoadingState</c> components are untouched.
    /// </summary>
    public interface IGlobalLoadingService
    {
        bool IsLoading { get; }
        event Action? OnChange;

        /// <summary>Call Dispose on the returned token when the operation completes.</summary>
        IDisposable BeginOperation();
    }
}
