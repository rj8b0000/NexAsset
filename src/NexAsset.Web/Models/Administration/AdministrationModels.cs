using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Administration
{
    // Wire contracts for /api/enterprise-operations/{notifications|audit-logs|system-settings}.
    // Named ServerNotification / AuditLogRecord to avoid colliding with the client-side
    // NexAsset.Web.State.NotificationItem (toast chrome) and the retained AuditLogMock.

    /// <summary>Mirrors NexAsset.Domain.Enums.NotificationType.</summary>
    public static class NotificationType
    {
        public const int Info = 1;
        public const int Warning = 2;
        public const int Success = 3;
        public const int Error = 4;

        public static string Label(int value) => value switch
        {
            Info => "Info", Warning => "Warning", Success => "Success", Error => "Error",
            _ => "Unknown"
        };

        public static string BadgeClass(int value) => value switch
        {
            Info => "badge-custom-info",
            Warning => "badge-custom-warning",
            Success => "badge-custom-success",
            Error => "badge-custom-danger",
            _ => "badge-custom-secondary"
        };
    }

    public sealed record ServerNotification(
        Guid Id, Guid? UserId, string Title, string Message, int NotificationType,
        bool IsRead, DateTime? ReadAtUtc);

    public sealed record AuditLogRecord(
        Guid Id, Guid? UserId, string EntityName, Guid? EntityId, string Action,
        string? OldValues, string? NewValues, DateTime TimestampUtc);

    public sealed record SystemSettingRecord(
        Guid Id, Guid? OrganizationId, string Key, string Value, string? Description, bool IsEncrypted);

    /// <summary>Body for POST /system-settings (upsert by key).</summary>
    public sealed class SystemSettingFormModel
    {
        public Guid? OrganizationId { get; set; }
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public string? Description { get; set; }
        public bool IsEncrypted { get; set; }

        public static SystemSettingFormModel FromRecord(SystemSettingRecord s) => new()
        {
            OrganizationId = s.OrganizationId, Key = s.Key, Value = s.Value,
            Description = s.Description, IsEncrypted = s.IsEncrypted
        };
    }
}
