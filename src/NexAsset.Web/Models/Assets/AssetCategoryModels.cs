using System;

namespace NexAsset.Web.Models.Assets
{
    // Wire contracts for /api/asset-categories (mirrors NexAsset.Application; no project reference).

    public sealed record AssetCategoryListItem(Guid Id, Guid OrganizationId, string Code, string Name, bool IsActive);

    public sealed record AssetCategoryDetail(Guid Id, Guid OrganizationId, string Code, string Name, string? Description, bool IsActive);

    /// <summary>Create/edit form model. Required (server-validated): OrganizationId, Code, Name.</summary>
    public sealed class AssetCategoryFormModel
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public static AssetCategoryFormModel FromDetail(AssetCategoryDetail d) => new()
        {
            Id = d.Id, OrganizationId = d.OrganizationId, Code = d.Code, Name = d.Name,
            Description = d.Description, IsActive = d.IsActive
        };
    }
}
