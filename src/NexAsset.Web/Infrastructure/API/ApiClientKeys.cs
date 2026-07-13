namespace NexAsset.Web.Infrastructure.API
{
    /// <summary>
    /// Keys used to resolve the keyed <see cref="System.Net.Http.HttpClient"/> registered
    /// for the future NexAsset.API client, via <c>[FromKeyedServices(ApiClientKeys.NexAssetApi)]</c>.
    /// A dedicated key (rather than the default unkeyed HttpClient) avoids silently
    /// overriding the HttpClient the WASM host already registers for static assets.
    /// </summary>
    public static class ApiClientKeys
    {
        public const string NexAssetApi = "NexAssetApi";
    }
}
