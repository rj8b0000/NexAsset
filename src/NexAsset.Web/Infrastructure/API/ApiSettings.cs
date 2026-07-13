namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Strongly-typed binding target for the "Api" section of appsettings.json.
    /// Not consumed by any live HTTP calls yet — this phase only prepares the
    /// infrastructure so a future phase can wire real NexAsset.API endpoints
    /// without touching call sites again.
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Root URL of the NexAsset.API backend, e.g. "https://localhost:5001/api/".
        /// Placeholder value only — no service currently reads this at runtime.
        /// </summary>
        public string BaseUrl { get; set; } = "https://localhost:5001/api/";

        /// <summary>
        /// Default request timeout applied to the typed HttpClient below.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
}
