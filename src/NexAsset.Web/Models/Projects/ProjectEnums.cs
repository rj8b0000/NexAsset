namespace NexAsset.Web.Models.Projects;

public enum ProjectStatus
{
    Draft = 1,
    Planning = 2,
    AwaitingApproval = 3,
    Approved = 4,
    InProgress = 5,
    OnHold = 6,
    Completed = 7,
    Archived = 8,
    Cancelled = 9
}

public enum ProjectPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum ParameterInputType
{
    Text = 1,
    Textarea = 2,
    Number = 3,
    Decimal = 4,
    Date = 5,
    Dropdown = 6,
    Checkbox = 7,
    Email = 8,
    Phone = 9,
    URL = 10
}

public enum DocumentCategory
{
    Agreement = 1,
    Contract = 2,
    Survey = 3,
    BOQ = 4,
    TechnicalDrawing = 5,
    GovernmentApproval = 6,
    InspectionReport = 7,
    CompletionCertificate = 8,
    Invoice = 9,
    PurchaseDocument = 10,
    Photo = 11,
    Video = 12,
    Other = 13
}
