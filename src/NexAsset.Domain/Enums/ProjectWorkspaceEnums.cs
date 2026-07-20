namespace NexAsset.Domain.Enums;

public enum ProjectPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum ProjectStatus
{
    Draft = 1,
    Planned = 2,
    Active = 3,
    OnHold = 4,
    Completed = 5,
    Cancelled = 6,
    Archived = 7
}

public enum ProjectMemberStatus
{
    Planned = 1,
    Active = 2,
    Released = 3
}

public enum ProjectAssetAllocationStatus
{
    Planned = 1,
    Allocated = 2,
    PartiallyReturned = 3,
    Returned = 4,
    Cancelled = 5
}

public enum ProjectParameterInputType
{
    Text = 1,
    Number = 2,
    Decimal = 3,
    Date = 4,
    Dropdown = 5,
    Checkbox = 6,
    Textarea = 7,
    Email = 8,
    Phone = 9,
    Url = 10
}
