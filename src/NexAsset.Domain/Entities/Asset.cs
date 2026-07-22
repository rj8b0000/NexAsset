using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class Asset : BaseEntity
{
    public string AssetCode { get; set; } = default!;
    public string AssetName { get; set; } = default!;
    public string? SerialNumber { get; set; }
    public string? Barcode { get; set; }
    public string? QrCode { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? WarrantyExpiry { get; set; }
    public string? Vendor { get; set; }
    public decimal? PurchaseCost { get; set; }
    public decimal? CurrentValue { get; set; }
    public AssetStatus AssetStatus { get; set; } = AssetStatus.Available;
    public string? Location { get; set; }
    public string? Remarks { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid CategoryId { get; set; }
    public AssetCategory Category { get; set; } = default!;

    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid? CurrentEmployeeId { get; set; }
    public Employee? CurrentEmployee { get; set; }

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
}
