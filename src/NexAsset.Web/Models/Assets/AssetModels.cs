using System;

namespace NexAsset.Web.Models.Assets
{
    // Wire contracts for /api/assets. FK ids resolve to names client-side (like the Branch/Employee lists).
    // PurchaseDate/WarrantyExpiry are nullable DateOnly (ISO "yyyy-MM-dd" strings on the wire).

    public sealed record AssetListItem(
        Guid Id, Guid OrganizationId, Guid CategoryId, Guid? BranchId, Guid? DepartmentId, Guid? CurrentEmployeeId,
        string AssetCode, string AssetName, string? SerialNumber, string? Barcode, int AssetStatus);

    public sealed record AssetDetail(
        Guid Id, Guid OrganizationId, Guid CategoryId, Guid? BranchId, Guid? DepartmentId, Guid? CurrentEmployeeId,
        string AssetCode, string AssetName, string? SerialNumber, string? Barcode, string? QrCode,
        string? Brand, string? Model, DateOnly? PurchaseDate, DateOnly? WarrantyExpiry, string? Vendor,
        decimal? PurchaseCost, decimal? CurrentValue, int AssetStatus, string? Location, string? Remarks);

    /// <summary>
    /// Create/edit form model. Required (server-validated): OrganizationId, CategoryId, AssetCode,
    /// AssetName, AssetStatus. Dates carried as "yyyy-MM-dd" strings ("" = null on submit).
    /// </summary>
    public sealed class AssetFormModel
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? CurrentEmployeeId { get; set; }
        public string AssetCode { get; set; } = "";
        public string AssetName { get; set; } = "";
        public string? SerialNumber { get; set; }
        public string? Barcode { get; set; }
        public string? QrCode { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? PurchaseDate { get; set; }
        public string? WarrantyExpiry { get; set; }
        public string? Vendor { get; set; }
        public decimal? PurchaseCost { get; set; }
        public decimal? CurrentValue { get; set; }
        public int AssetStatus { get; set; } = Assets.AssetStatus.Available;
        public string? Location { get; set; }
        public string? Remarks { get; set; }

        public static AssetFormModel FromDetail(AssetDetail d) => new()
        {
            Id = d.Id, OrganizationId = d.OrganizationId, CategoryId = d.CategoryId, BranchId = d.BranchId,
            DepartmentId = d.DepartmentId, CurrentEmployeeId = d.CurrentEmployeeId, AssetCode = d.AssetCode,
            AssetName = d.AssetName, SerialNumber = d.SerialNumber, Barcode = d.Barcode, QrCode = d.QrCode,
            Brand = d.Brand, Model = d.Model,
            PurchaseDate = d.PurchaseDate?.ToString("yyyy-MM-dd"),
            WarrantyExpiry = d.WarrantyExpiry?.ToString("yyyy-MM-dd"),
            Vendor = d.Vendor, PurchaseCost = d.PurchaseCost, CurrentValue = d.CurrentValue,
            AssetStatus = d.AssetStatus, Location = d.Location, Remarks = d.Remarks
        };
    }
}
