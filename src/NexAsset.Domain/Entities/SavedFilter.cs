using NexAsset.Domain.Common;

namespace NexAsset.Domain.Entities;

public class SavedFilter : BaseEntity
{
    public string FilterName { get; set; } = default!;              // MaxLength(100)
    public string EntityType { get; set; } = default!;              // MaxLength(100) e.g. "Project", "Risk"
    public string? SearchKeyword { get; set; }                      // MaxLength(200)
    public string FilterCriteriaJson { get; set; } = default!;      // JSON object of filter fields

    public Guid UserId { get; set; }                                 // ApplicationUser.Id
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;
}
