using System;

namespace NexAsset.Web.Models.Foundation
{
    /// <summary>
    /// Minimal shape for a create response — every foundation create endpoint returns at least
    /// an <c>Id</c> (plus a name/code we don't need here, which JSON deserialization ignores).
    /// </summary>
    public sealed record CreatedResponse(Guid Id);
}
