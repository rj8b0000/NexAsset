# Project Workspace

Project Workspace is the operational center of NexAsset. It connects project setup, teams, assets, dynamic parameters, documents and activity history without storing searchable business data in JSON.

## Backend Scope

- Project categories are organization-specific and seeded with industry suggestions.
- Projects support create, update, soft delete, archive and duplicate.
- Wizard drafts support autosave through `ProjectDraft`; the draft state is intended only for temporary UI continuation.
- Project members are normalized in `ProjectMembers`.
- Asset allocations are normalized in `ProjectAssetAllocations` and can be queried as permanent asset project history.
- Dynamic parameter groups and parameters are normalized in `ProjectParameterGroups` and `ProjectParameters`.
- Project documents are normalized in `ProjectDocuments` with version-ready metadata.
- Project activities are written automatically for major workflow events.

## API Routes

Base route:

```text
/api/project-workspace
```

Primary resources:

- `/categories`
- `/projects`
- `/drafts`
- `/projects/{projectId}/members`
- `/projects/{projectId}/asset-allocations`
- `/projects/{projectId}/parameter-groups`
- `/projects/{projectId}/parameters`
- `/projects/{projectId}/documents`
- `/projects/{projectId}/activities`
- `/assets/{assetId}/history`

## Frontend Scope

The Blazor page lives at:

```text
/projects
```

It provides:

- Server-driven project table.
- Project creation wizard.
- Draft autosave state indicators.
- Workspace detail panel.
- Team, asset allocation, parameter, document and activity tabs.

## Data Design Notes

Business data is relational. JSON is limited to the `ProjectDraft.DraftState` field, which is temporary wizard state and not authoritative business data.

Tenant boundaries are enforced through `ApplicationDbContext` query filters:

- Direct organization scope for `ProjectCategory`, `Project`, and `ProjectDraft`.
- Parent project scope for project child tables.

## Future Extensions

- Task management and project timelines.
- Budget line items and cost controls.
- Procurement links from purchase requests and purchase orders.
- Document storage provider integration.
- AI project assistant and reporting projections.
