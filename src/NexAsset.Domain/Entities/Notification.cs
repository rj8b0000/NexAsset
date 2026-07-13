using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationType NotificationType { get; set; } = NotificationType.Info;
    public bool IsRead { get; set; }
    public DateTime? ReadAtUtc { get; set; }
}
