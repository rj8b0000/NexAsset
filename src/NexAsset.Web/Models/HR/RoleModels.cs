using System;

namespace NexAsset.Web.Models.HR
{
    // Wire contracts for /api/roles. A backend role is an ASP.NET Identity role: it has only
    // Id + Name (no description/status/dates/user-count — see the mismatch notes in the report).

    public sealed record RoleItem(Guid Id, string Name);

    /// <summary>Create/edit form model. The only editable field the backend accepts is Name.</summary>
    public sealed class RoleFormModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = "";

        public static RoleFormModel FromItem(RoleItem r) => new() { Id = r.Id, Name = r.Name };
    }
}
