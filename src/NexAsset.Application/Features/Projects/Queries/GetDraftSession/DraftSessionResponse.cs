namespace NexAsset.Application.Features.Projects.Queries.GetDraftSession;

public sealed record DraftSessionResponse(
    Guid Id,
    Guid UserId,
    Guid OrganizationId,
    string WizardStateJson,
    int CurrentStep,
    DateTime LastSavedAtUtc);
