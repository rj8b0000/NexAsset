using NexAsset.Domain.Common;
using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Entities;

public class ServiceTicket : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
    public string TicketNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public string? Resolution { get; set; }
    public string? Comments { get; set; }
}
