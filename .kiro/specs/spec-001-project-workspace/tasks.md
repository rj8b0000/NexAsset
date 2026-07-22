# Implementation Plan: Project Workspace Module

## Overview

This implementation plan covers the complete Project Workspace module including 13 domain entities, enums, EF Core configurations, repositories, CQRS features (Commands and Queries), API endpoints, frontend pages and components, permission seeding, and integration with existing Asset, Employee, Customer, and Organization modules.

**Implementation Language:** C# with ASP.NET Core Minimal APIs, Blazor WebAssembly
**Architecture:** Clean Architecture + CQRS (MediatR)
**Database:** PostgreSQL with Entity Framework Core 10

The tasks are organized in logical dependency order:
1. Domain Layer (Entities + Enums)
2. Infrastructure Layer (EF Configurations + Repositories + Migration)
3. Application Layer (CQRS Features)
4. API Layer (Endpoints)
5. Frontend Layer (Pages + Components + API Clients)
6. Permission Seeding
7. Testing
8. Integration & Documentation

---

## Tasks

### 1. Domain Layer — Enums and Entities

- [x] 1.1 Create ProjectEnums.cs with all enumeration types
  - Create `NexAsset.Domain/Enums/ProjectEnums.cs`
  - Define all enums: ProjectStatus, ProjectPriority, ParameterInputType, TeamMemberStatus, AllocationStatus, DocumentCategory, RiskCategory, RiskProbability, RiskImpact, RiskSeverity, RiskStatus, TimelineEventType, ActivityType, NotificationPriority, ProjectNotificationType
  - _Requirements: 2.4, 2.5, 4.5, 5.2, 14.1-14.7, 15.2-15.6, 12.3-12.22, 13.3-13.22, 17.1-17.16_

- [x] 1.2 Create ProjectCategory entity
  - Create `NexAsset.Domain/Entities/ProjectCategory.cs` inheriting from BaseEntity
  - Properties: Name (string, 100), Description (string?, 500), IsActive (bool), OrganizationId (Guid), Organization navigation
  - _Requirements: 1.4, 1.5_


- [x] 1.3 Create Project entity
  - Create `NexAsset.Domain/Entities/Project.cs` inheriting from BaseEntity
  - Properties: ProjectCode (string, 50), ProjectName (string, 200), Description (string?, 1000), Notes (string?, 2000), InternalRemarks (string?, 2000), Status (ProjectStatus), Priority (ProjectPriority), StartDate (DateOnly?), EndDate (DateOnly?), ExpectedCompletionDate (DateOnly?), OrganizationId, CustomerId?, CategoryId, BranchId?, DepartmentId?, ProjectManagerEmployeeId?, TaskModuleId?, MilestoneModuleId?
  - Navigation properties: Organization, Customer, Category, Branch, Department, ProjectManager
  - _Requirements: 2.1, 2.2, 2.3, 27.1-27.6_

- [x] 1.4 Create DraftSession entity
  - Create `NexAsset.Domain/Entities/DraftSession.cs` inheriting from BaseEntity
  - Properties: UserId (Guid), OrganizationId (Guid), WizardStateJson (string), CurrentStep (int), LastSavedAtUtc (DateTime)
  - _Requirements: 3.2, 3.3, 3.7_

- [x] 1.5 Create ProjectParameterSection entity
  - Create `NexAsset.Domain/Entities/ProjectParameterSection.cs` inheriting from BaseEntity
  - Properties: Name (string, 100), DisplayOrder (int), ProjectId (Guid), Project navigation, Parameters collection
  - _Requirements: 4.1, 4.2_

- [x] 1.6 Create ProjectParameter entity
  - Create `NexAsset.Domain/Entities/ProjectParameter.cs` inheriting from BaseEntity
  - Properties: ParameterName (string, 100), InputType (ParameterInputType), Unit (string?, 50), IsRequired (bool), DisplayOrder (int), DropdownOptionsJson (string?), SectionId (Guid), Section navigation
  - _Requirements: 4.4, 4.5, 4.8_

- [x] 1.7 Create ProjectParameterValue entity
  - Create `NexAsset.Domain/Entities/ProjectParameterValue.cs` inheriting from BaseEntity
  - Properties: Value (string?, 2000), ProjectId (Guid), ParameterId (Guid), Project navigation, Parameter navigation
  - _Requirements: 4.9_

- [x] 1.8 Create ProjectTeamMember entity
  - Create `NexAsset.Domain/Entities/ProjectTeamMember.cs` inheriting from BaseEntity
  - Properties: ProjectRole (string, 100), AllocationPercentage (int), JoinedDate (DateOnly), ReleasedDate (DateOnly?), Status (TeamMemberStatus), Remarks (string?, 500), SnapshotBranchId (Guid?), SnapshotDepartmentId (Guid?), ProjectId (Guid), EmployeeId (Guid), Project navigation, Employee navigation
  - _Requirements: 6.1, 6.2, 27.3_


- [x] 1.9 Create ProjectAssetAllocation entity
  - Create `NexAsset.Domain/Entities/ProjectAssetAllocation.cs` inheriting from BaseEntity
  - Properties: AllocationDate (DateOnly), ReturnDate (DateOnly?), AllocatedQuantity (int), ReturnedQuantity (int), Status (AllocationStatus), Remarks (string?, 500), ProjectId (Guid), AssetId (Guid), Project navigation, Asset navigation
  - _Requirements: 7.1, 7.4, 7.5_

- [x] 1.10 Create ProjectDocument entity
  - Create `NexAsset.Domain/Entities/ProjectDocument.cs` inheriting from BaseEntity
  - Properties: DocumentName (string, 200), Category (DocumentCategory), Description (string?, 500), FileReference (string, 1000), UploadedAtUtc (DateTime), Version (int), Remarks (string?, 500), ExpiryDate (DateOnly?), ProjectId (Guid), UploadedByEmployeeId (Guid?), Project navigation, UploadedByEmployee navigation
  - _Requirements: 5.1, 5.2, 5.3_

- [x] 1.11 Create ProjectBudget entity
  - Create `NexAsset.Domain/Entities/ProjectBudget.cs` inheriting from BaseEntity
  - Properties: EstimatedBudget (decimal), ApprovedBudget (decimal), ActualCost (decimal), ProcurementCost (decimal), MaintenanceCost (decimal), LabourCost (decimal), MiscellaneousCost (decimal), UpdatedByUserId (Guid), FinanceInvoiceId (Guid?), ProjectId (Guid), Project navigation
  - _Requirements: 14.1, 14.2, 14.3, 14.14_

- [x] 1.12 Create ProjectRisk entity
  - Create `NexAsset.Domain/Entities/ProjectRisk.cs` inheriting from BaseEntity
  - Properties: Title (string, 200), Description (string?, 1000), Category (RiskCategory), Probability (RiskProbability), Impact (RiskImpact), Severity (RiskSeverity), Status (RiskStatus), MitigationPlan (string?, 1000), Remarks (string?, 500), ClosedAtUtc (DateTime?), ProjectId (Guid), OwnerEmployeeId (Guid?), Project navigation, OwnerEmployee navigation
  - _Requirements: 15.1, 15.2-15.6, 15.9_

- [x] 1.13 Create ProjectTimelineEvent entity
  - Create `NexAsset.Domain/Entities/ProjectTimelineEvent.cs` inheriting from BaseEntity
  - Properties: EventType (TimelineEventType), EntityType (string, 100), EntityId (Guid?), Description (string, 500), Timestamp (DateTime), UserId (Guid?), IconType (string, 50), ProjectId (Guid), Project navigation
  - _Requirements: 12.2, 12.3-12.21_

- [x] 1.14 Create ProjectActivityRecord entity
  - Create `NexAsset.Domain/Entities/ProjectActivityRecord.cs` inheriting from BaseEntity
  - Properties: ActivityType (ActivityType), UserId (Guid?), Action (string, 500), TargetEntity (string, 100), TargetEntityId (Guid?), Timestamp (DateTime), Remarks (string?, 500), ProjectId (Guid), Project navigation
  - _Requirements: 13.2, 13.3-13.22_


- [x] 1.15 Create SavedFilter entity
  - Create `NexAsset.Domain/Entities/SavedFilter.cs` inheriting from BaseEntity
  - Properties: FilterName (string, 100), EntityType (string, 100), SearchKeyword (string?, 200), FilterCriteriaJson (string), UserId (Guid), OrganizationId (Guid), Organization navigation
  - _Requirements: 18.17, 18.18_

- [ ] 1.16 Add ProjectId to Asset entity (existing entity modification)
  - Open `NexAsset.Domain/Entities/Asset.cs`
  - Add property: `public Guid? ProjectId { get; set; }`
  - Add navigation: `public Project? Project { get; set; }`
  - _Requirements: 7.5, 7.6, 27.8, 27.9_

- [x] 1.17 Create RiskSeverityHelper static class
  - Create `NexAsset.Domain/Helpers/RiskSeverityHelper.cs` with static method `ComputeSeverity(RiskProbability p, RiskImpact i)`
  - Implement the 3x3 probability-impact severity matrix as per design section 3.3
  - _Requirements: 15.5, 15.13_

- [x] 1.18 Create ProjectStatusTransition static class
  - Create `NexAsset.Domain/Helpers/ProjectStatusTransition.cs` with static method `IsAllowed(ProjectStatus from, ProjectStatus to)`
  - Implement the allowed transitions dictionary as per design section 3.4
  - _Requirements: 2.4, 2.5_

### 2. Infrastructure Layer — Configurations, Repositories, Migration

- [x] 2.1 Create EF Core configuration for ProjectCategory
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectCategoryConfiguration.cs` implementing `IEntityTypeConfiguration<ProjectCategory>`
  - Configure table name, primary key, max lengths, unique index on `{OrganizationId, Name}` with IsDeleted filter, foreign key to Organization with Restrict, HasQueryFilter by OrganizationId
  - _Requirements: 1.5, 1.6, 9.1_

- [x] 2.2 Create EF Core configuration for Project
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectConfiguration.cs` implementing `IEntityTypeConfiguration<Project>`
  - Configure table name, primary key, max lengths, unique index on `{OrganizationId, ProjectCode}` with IsDeleted filter, indexes on `{OrganizationId, Status}`, `ProjectManagerEmployeeId`, `CategoryId`, foreign keys with Restrict, HasQueryFilter by OrganizationId
  - _Requirements: 2.2, 2.3, 9.1_


- [x] 2.3 Create EF Core configuration for DraftSession
  - Create `NexAsset.Infrastructure/Persistence/Configurations/DraftSessionConfiguration.cs`
  - Configure table name, primary key, max lengths, index on `{UserId, OrganizationId}` with IsDeleted filter, HasQueryFilter by OrganizationId
  - _Requirements: 3.2, 9.1_

- [x] 2.4 Create EF Core configuration for ProjectParameterSection
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectParameterSectionConfiguration.cs`
  - Configure table name, primary key, max lengths, foreign key to Project with Restrict, index on ProjectId, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 4.1, 9.1_

- [ ] 2.5 Create EF Core configuration for ProjectParameter
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectParameterConfiguration.cs`
  - Configure table name, primary key, max lengths, foreign key to ProjectParameterSection with Restrict, index on SectionId, HasQueryFilter via `Section.Project.OrganizationId`
  - _Requirements: 4.4, 9.1_

- [ ] 2.6 Create EF Core configuration for ProjectParameterValue
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectParameterValueConfiguration.cs`
  - Configure table name, primary key, max lengths, unique index on `{ProjectId, ParameterId}` with IsDeleted filter, foreign keys with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 4.9, 9.1_

- [ ] 2.7 Create EF Core configuration for ProjectTeamMember
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectTeamMemberConfiguration.cs`
  - Configure table name, primary key, max lengths, indexes on `{ProjectId, Status}` and `EmployeeId`, foreign keys with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 6.1, 9.1_

- [ ] 2.8 Create EF Core configuration for ProjectAssetAllocation
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectAssetAllocationConfiguration.cs`
  - Configure table name, primary key, indexes on `{ProjectId, Status}` and `AssetId`, foreign keys with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 7.1, 9.1_

- [ ] 2.9 Create EF Core configuration for ProjectDocument
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectDocumentConfiguration.cs`
  - Configure table name, primary key, max lengths, index on `{ProjectId, Category}`, foreign keys with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 5.1, 9.1_


- [ ] 2.10 Create EF Core configuration for ProjectBudget
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectBudgetConfiguration.cs`
  - Configure table name, primary key, decimal(18,2) for all budget fields, index on `{ProjectId, CreatedAtUtc DESC}`, foreign key to Project with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 14.1, 14.10, 14.14, 9.1_

- [ ] 2.11 Create EF Core configuration for ProjectRisk
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectRiskConfiguration.cs`
  - Configure table name, primary key, max lengths, indexes on `{ProjectId, Status}`, `{ProjectId, Severity}`, `OwnerEmployeeId`, foreign keys with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 15.1, 9.1_

- [ ] 2.12 Create EF Core configuration for ProjectTimelineEvent
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectTimelineEventConfiguration.cs`
  - Configure table name, primary key, max lengths, index on `{ProjectId, Timestamp DESC}`, foreign key to Project with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 12.2, 9.1_

- [ ] 2.13 Create EF Core configuration for ProjectActivityRecord
  - Create `NexAsset.Infrastructure/Persistence/Configurations/ProjectActivityRecordConfiguration.cs`
  - Configure table name, primary key, max lengths, index on `{ProjectId, Timestamp DESC}`, foreign key to Project with Restrict, HasQueryFilter via `Project.OrganizationId`
  - _Requirements: 13.2, 9.1_

- [ ] 2.14 Create EF Core configuration for SavedFilter
  - Create `NexAsset.Infrastructure/Persistence/Configurations/SavedFilterConfiguration.cs`
  - Configure table name, primary key, max lengths, index on `{UserId, OrganizationId, EntityType}`, foreign key to Organization with Restrict, HasQueryFilter by OrganizationId
  - _Requirements: 18.18, 9.1_

- [ ] 2.15 Update AssetConfiguration to add ProjectId FK
  - Open `NexAsset.Infrastructure/Persistence/Configurations/AssetConfiguration.cs`
  - Add FK configuration: `builder.HasOne(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);`
  - Add index on ProjectId: `builder.HasIndex(x => x.ProjectId);`
  - _Requirements: 27.8, 27.9_


- [ ] 2.16 Register all new DbSets in ApplicationDbContext
  - Open `NexAsset.Infrastructure/Persistence/ApplicationDbContext.cs`
  - Add DbSet properties for: ProjectCategory, Project, DraftSession, ProjectParameterSection, ProjectParameter, ProjectParameterValue, ProjectTeamMember, ProjectAssetAllocation, ProjectDocument, ProjectBudget, ProjectRisk, ProjectTimelineEvent, ProjectActivityRecord, SavedFilter
  - Apply all 14 new configurations in OnModelCreating
  - _Requirements: All entity requirements_

- [ ] 2.17 Create IProjectCategoryRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectCategoryRepository.cs`
  - Define methods: ExistsByNameAsync (org, name, ct), ExistsByNameAsync (org, name, excludeId, ct), AddAsync, GetByIdAsync, GetPagedAsync (PagedRequest, org, isActive?, ct), Update
  - _Requirements: 1.5, 1.7, 1.8, 1.9, 1.10_

- [ ] 2.18 Create IProjectRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectRepository.cs`
  - Define methods: ExistsByCodeAsync (org, code, ct), ExistsByCodeAsync (org, code, excludeId, ct), AddAsync, GetByIdAsync, GetByIdWithDetailsAsync (includes Category, Branch, Dept, Manager, Customer), GetPagedAsync (GetProjectsQuery, ct), Update
  - _Requirements: 2.2, 2.6, 2.7, 2.13, 2.14, 2.15, 2.16_

- [ ] 2.19 Create IDraftSessionRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IDraftSessionRepository.cs`
  - Define methods: GetByUserAsync (userId, orgId, ct), AddAsync, Update, Remove
  - _Requirements: 3.2, 3.3, 3.7, 3.10_

- [ ] 2.20 Create IProjectTeamRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectTeamRepository.cs`
  - Define methods: AddAsync, GetByIdAsync, HasActiveAsync (projectId, employeeId, ct), GetPagedAsync (projectId, status?, PagedRequest, ct), Update, Remove
  - _Requirements: 6.1, 6.3, 6.7, 6.8_

- [ ] 2.21 Create IProjectAssetRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectAssetRepository.cs`
  - Define methods: AddAsync, GetByIdAsync, HasActiveAsync (projectId, assetId, ct), GetPagedAsync (projectId, status?, PagedRequest, ct), GetByAssetIdAsync (assetId, ct), Update
  - _Requirements: 7.1, 7.4, 7.7, 7.8, 7.9_


- [ ] 2.22 Create IProjectDocumentRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectDocumentRepository.cs`
  - Define methods: AddAsync, GetByIdAsync, GetPagedAsync (projectId, category?, search?, PagedRequest, ct), GetVersionHistoryAsync (projectId, documentName, ct), Update
  - _Requirements: 5.3, 5.4, 5.8, 5.9, 5.10_

- [ ] 2.23 Create IProjectParameterRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectParameterRepository.cs`
  - Define methods: AddSectionAsync, GetSectionByIdAsync, GetSectionsAsync (projectId, ct), AddParameterAsync, GetParameterByIdAsync, UpsertValueAsync, GetValuesByProjectAsync, UpdateSection, UpdateParameter, RemoveSection, RemoveParameter
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.7, 4.9_

- [ ] 2.24 Create IProjectBudgetRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectBudgetRepository.cs`
  - Define methods: AddAsync, GetLatestByProjectAsync (projectId, ct), GetHistoryAsync (projectId, PagedRequest, ct)
  - _Requirements: 14.10, 14.13_

- [ ] 2.25 Create IProjectRiskRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectRiskRepository.cs`
  - Define methods: AddAsync, GetByIdAsync, GetPagedAsync (GetRisksQuery, ct), Update, Remove
  - _Requirements: 15.9, 15.14, 15.15, 15.16, 15.23_

- [ ] 2.26 Create IProjectTimelineRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectTimelineRepository.cs`
  - Define methods: AddAsync, GetPagedAsync (projectId, type?, keyword?, pageNumber, pageSize, ct)
  - _Requirements: 12.1, 12.23, 12.24, 12.25, 12.26_

- [ ] 2.27 Create IProjectActivityRepository interface
  - Create `NexAsset.Application/Common/Interfaces/IProjectActivityRepository.cs`
  - Define methods: AddAsync, GetPagedAsync (GetActivitiesQuery, ct), GetRecentAsync (projectId, count, ct)
  - _Requirements: 13.1, 13.26, 13.27, 13.28, 13.29, 13.30_

- [ ] 2.28 Create ISavedFilterRepository interface
  - Create `NexAsset.Application/Common/Interfaces/ISavedFilterRepository.cs`
  - Define methods: AddAsync, GetByIdAsync, GetByUserAsync (userId, orgId, entityType?, ct), Remove
  - _Requirements: 18.18, 18.19, 18.20_


- [ ] 2.29 Implement ProjectCategoryRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectCategoryRepository.cs` implementing `IProjectCategoryRepository`
  - Implement all methods with AsNoTracking for list/read-only, tracking for single-entity-for-update, soft delete filtering `!x.IsDeleted`
  - _Requirements: 1.5, 1.7, 1.8, 1.9, 1.10, 1.11, 1.12_

- [ ] 2.30 Implement ProjectRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectRepository.cs` implementing `IProjectRepository`
  - Implement all methods with AsNoTracking for list/read-only, tracking for single-entity-for-update, soft delete filtering, eager loading with Include/ThenInclude for GetByIdWithDetailsAsync
  - _Requirements: 2.2, 2.6, 2.7, 2.13, 2.14, 2.15, 2.16_

- [ ] 2.31 Implement DraftSessionRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/DraftSessionRepository.cs` implementing `IDraftSessionRepository`
  - Implement all methods with soft delete filtering
  - _Requirements: 3.2, 3.3, 3.7, 3.10_

- [ ] 2.32 Implement ProjectTeamRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectTeamRepository.cs` implementing `IProjectTeamRepository`
  - Implement all methods with AsNoTracking for list queries, soft delete filtering, Include Employee for display name resolution
  - _Requirements: 6.1, 6.3, 6.7, 6.8_

- [ ] 2.33 Implement ProjectAssetRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectAssetRepository.cs` implementing `IProjectAssetRepository`
  - Implement all methods with AsNoTracking for list/history queries, soft delete filtering, Include Asset for display resolution
  - _Requirements: 7.1, 7.4, 7.7, 7.8, 7.9_

- [ ] 2.34 Implement ProjectDocumentRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectDocumentRepository.cs` implementing `IProjectDocumentRepository`
  - Implement all methods with AsNoTracking for list/history queries, soft delete filtering, Include UploadedByEmployee for display resolution
  - _Requirements: 5.3, 5.4, 5.8, 5.9, 5.10, 5.12_


- [ ] 2.35 Implement ProjectParameterRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectParameterRepository.cs` implementing `IProjectParameterRepository`
  - Implement all methods with AsNoTracking for queries, Include navigation properties for section/parameter/value relationships, soft delete filtering
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.7, 4.9_

- [ ] 2.36 Implement ProjectBudgetRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectBudgetRepository.cs` implementing `IProjectBudgetRepository`
  - Implement all methods with AsNoTracking for list/history, append-only pattern (never UPDATE budget records, always INSERT new version)
  - _Requirements: 14.10, 14.13_

- [ ] 2.37 Implement ProjectRiskRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectRiskRepository.cs` implementing `IProjectRiskRepository`
  - Implement all methods with AsNoTracking for list queries, Include OwnerEmployee for display resolution, soft delete filtering
  - _Requirements: 15.9, 15.14, 15.15, 15.16, 15.23_

- [ ] 2.38 Implement ProjectTimelineRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectTimelineRepository.cs` implementing `IProjectTimelineRepository`
  - Implement all methods with AsNoTracking for all queries (timeline events are immutable after insert), soft delete filtering
  - _Requirements: 12.1, 12.23, 12.24, 12.25, 12.26_

- [ ] 2.39 Implement ProjectActivityRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/ProjectActivityRepository.cs` implementing `IProjectActivityRepository`
  - Implement all methods with AsNoTracking for all queries (activity records are immutable after insert), soft delete filtering
  - _Requirements: 13.1, 13.26, 13.27, 13.28, 13.29, 13.30_

- [ ] 2.40 Implement SavedFilterRepository
  - Create `NexAsset.Infrastructure/Persistence/Repositories/SavedFilterRepository.cs` implementing `ISavedFilterRepository`
  - Implement all methods with AsNoTracking for queries, soft delete filtering
  - _Requirements: 18.18, 18.19, 18.20_

- [ ] 2.41 Register all repository implementations in DI container
  - Open `NexAsset.Infrastructure/DependencyInjection.cs` (or wherever services are registered)
  - Add scoped registrations for all 12 new repository interfaces and implementations
  - _Requirements: All repository requirements_


- [ ] 2.42 Create EF Core migration for Project Workspace
  - Run `dotnet ef migrations add Add_ProjectWorkspace_Module --project src/NexAsset.Infrastructure --startup-project src/NexAsset.API --output-dir Persistence/Migrations`
  - Review generated migration to confirm 14 new tables + nullable ProjectId column added to Assets table + nullable ProjectId columns added to MaintenanceRecords, PurchaseRequests, PurchaseOrders (future integration)
  - _Requirements: All entity requirements, 27.15, 27.16, 27.17_

- [ ] 2.43 Create IFileStorageService interface
  - Create `NexAsset.Application/Common/Interfaces/IFileStorageService.cs`
  - Define methods: SaveAsync (Stream content, fileName, contentType, ct), GetAsync (fileReference, ct), DeleteAsync (fileReference, ct)
  - _Requirements: 5.3, 5.4_

- [ ] 2.44 Implement LocalFileStorageService
  - Create `NexAsset.Infrastructure/Services/LocalFileStorageService.cs` implementing `IFileStorageService`
  - Implement disk-based storage with configurable root path, generate unique file references, handle file upload/download/delete
  - _Requirements: 5.3, 5.4_

- [ ] 2.45 Register LocalFileStorageService in DI container
  - Register IFileStorageService → LocalFileStorageService in DI container
  - _Requirements: 5.3_

### 3. Application Layer — CQRS Features (ProjectCategories)

- [ ] 3.1 Create CreateProjectCategoryCommand and handler
  - Create `NexAsset.Application/Features/ProjectCategories/Commands/CreateProjectCategory/CreateProjectCategoryCommand.cs` (sealed record : IRequest<Result<ProjectCategoryResponse>>)
  - Create `CreateProjectCategoryCommandHandler.cs` (sealed class : IRequestHandler<...>)
  - Handler: Validate org exists, validate category name uniqueness, create entity, save, return mapped response
  - _Requirements: 1.1, 1.5, 1.6, 9.3_

- [ ] 3.2 Create CreateProjectCategoryCommandValidator
  - Create `CreateProjectCategoryCommandValidator.cs` (sealed class : AbstractValidator<CreateProjectCategoryCommand>)
  - Rules: Name not empty, max 100 chars; Description max 500 chars
  - _Requirements: 10.3, 22.15, 22.16, 22.17_


- [ ] 3.3 Create UpdateProjectCategoryCommand and handler
  - Create `UpdateProjectCategory/UpdateProjectCategoryCommand.cs` and `UpdateProjectCategoryCommandHandler.cs`
  - Handler: Load category by ID, validate org boundary, validate name uniqueness (excluding current ID), update fields, save
  - _Requirements: 1.5, 1.6, 9.3_

- [ ] 3.4 Create UpdateProjectCategoryCommandValidator
  - Create `UpdateProjectCategoryCommandValidator.cs`
  - Rules: Same as Create validator
  - _Requirements: 22.15, 22.16, 22.17_

- [ ] 3.5 Create DeleteProjectCategoryCommand and handler
  - Create `DeleteProjectCategory/DeleteProjectCategoryCommand.cs` (sealed record : IRequest<Result>)
  - Create `DeleteProjectCategoryCommandHandler.cs`
  - Handler: Load category by ID, soft delete (IsDeleted = true, DeletedAtUtc = now), save
  - _Requirements: 1.11, 9.3_

- [ ] 3.6 Create GetProjectCategoryQuery and handler
  - Create `Queries/GetProjectCategory/GetProjectCategoryQuery.cs` (sealed record : IRequest<Result<ProjectCategoryResponse>>)
  - Create `GetProjectCategoryQueryHandler.cs`
  - Handler: GetByIdAsync, return mapped response
  - _Requirements: 1.7_

- [ ] 3.7 Create ProjectCategoryResponse DTO
  - Create `ProjectCategoryResponse.cs` (sealed record with all category fields)
  - _Requirements: 1.4_

- [ ] 3.8 Create GetProjectCategoriesQuery and handler
  - Create `GetProjectCategories/GetProjectCategoriesQuery.cs` (extending PagedRequest : IRequest<Result<PagedResponse<ProjectCategoryResponse>>>)
  - Add filter properties: IsActive?
  - Create `GetProjectCategoriesQueryHandler.cs`
  - Handler: Call GetPagedAsync, return mapped paged response
  - _Requirements: 1.7, 1.8, 1.9, 1.10_

### 4. Application Layer — CQRS Features (Projects Core)

- [ ] 4.1 Create CreateProjectCommand and handler
  - Create `Features/Projects/Commands/CreateProject/CreateProjectCommand.cs` (all project fields)
  - Create `CreateProjectCommandHandler.cs`
  - Handler: Validate org, category, customer, branch, dept, project manager all belong to same org; validate ProjectCode uniqueness; validate StartDate <= EndDate; create Project entity with Status=Draft; create ProjectTimelineEvent (ProjectCreated); create ProjectActivityRecord (ProjectCreated); create AuditLog (Created); if ProjectManagerEmployeeId provided: create Notification (ProjectAssigned); save
  - _Requirements: 2.1, 2.2, 2.3, 10.1, 10.2, 12.3, 13.3, 17.1, 23.1_


- [ ] 4.2 Create CreateProjectCommandValidator
  - FluentValidation rules: ProjectCode not empty, max 50; ProjectName not empty, max 200; Description max 1000; Notes max 2000; InternalRemarks max 2000; StartDate <= EndDate when both provided; OrgId, CategoryId not empty
  - _Requirements: 10.1, 10.2, 22.1-22.14_

- [ ] 4.3 Create ProjectResponse DTO
  - Sealed record with all project fields, resolved names for Customer, Category, Branch, Department, ProjectManager
  - _Requirements: 2.1, 8.1_

- [ ] 4.4 Create UpdateProjectCommand, handler, and validator
  - Similar to Create but allows updating all non-identity fields
  - Handler: Load project, validate org boundary, validate uniqueness (excluding current), update, create ActivityRecord (ProjectUpdated), AuditLog (Updated), save
  - _Requirements: 2.7, 13.4, 23.2_

- [ ] 4.5 Create DeleteProjectCommand and handler
  - Soft delete: IsDeleted = true, DeletedAtUtc = now, create AuditLog (Deleted)
  - _Requirements: 2.11, 23.7_

- [ ] 4.6 Create TransitionProjectStatusCommand and handler
  - Validate allowed transition using ProjectStatusTransition.IsAllowed
  - Create TimelineEvent, ActivityRecord (StatusChanged), AuditLog (StatusChanged with OldValue/NewValue)
  - Generate notifications per status (AwaitingApproval → ApprovalRequested to Projects.Approve users; Approved → ApprovalCompleted to ProjectManager; Completed → ProjectCompleted to all active team members)
  - _Requirements: 2.4, 2.5, 2.8, 12.4-12.6, 13.5, 17.2, 17.3, 17.16, 23.3, 23.4, 23.5, 23.6_

- [ ] 4.7 Create TransitionProjectStatusCommandValidator
  - Rules: Id not empty, NewStatus is valid enum value
  - _Requirements: 22.1_

- [ ] 4.8 Create DuplicateProjectCommand and handler
  - Copy: ProjectName, Description, CategoryId, BranchId, DepartmentId, Priority, ProjectParameterSections + Parameters
  - Blank: CustomerId, ProjectManagerEmployeeId, StartDate, EndDate
  - New: ProjectCode (system-generated), Status = Draft
  - _Requirements: 2.10, 4.10_


- [ ] 4.9 Create GetProjectQuery and handler
  - GetByIdWithDetailsAsync, return ProjectResponse with all resolved names
  - _Requirements: 2.13, 8.1_

- [ ] 4.10 Create GetProjectsQuery and handler
  - Extend PagedRequest, add filter properties: Status?, Priority?, CategoryId?, BranchId?, DepartmentId?, ProjectManagerEmployeeId?
  - Handler: GetPagedAsync with filters, search (ProjectCode, ProjectName), sort (ProjectName, StartDate, EndDate, CreatedAtUtc)
  - _Requirements: 2.13, 2.14, 2.15, 2.16_

- [ ] 4.11 Create ProjectListItemResponse DTO
  - Sealed record: Id, ProjectCode, ProjectName, Status, Priority, ProjectManagerName?, StartDate?, EndDate?, CategoryName, CreatedAtUtc
  - _Requirements: 2.16_

### 5. Application Layer — CQRS Features (Draft Session)

- [ ] 5.1 Create UpsertDraftSessionCommand and handler
  - If draft exists: Update WizardStateJson, CurrentStep, LastSavedAtUtc
  - If not exists: Create new DraftSession
  - _Requirements: 3.2, 3.3_

- [ ] 5.2 Create GetDraftSessionQuery and handler
  - GetByUserAsync, return DraftSessionResponse (UserId, OrgId, WizardStateJson, CurrentStep, LastSavedAtUtc)
  - _Requirements: 3.8_

- [ ] 5.3 Create DeleteDraftSessionCommand and handler
  - Remove draft session (hard delete is OK here since draft is transient)
  - _Requirements: 3.10_

### 6. Application Layer — CQRS Features (Project Team)

- [ ] 6.1 Create AddTeamMemberCommand and handler
  - Validate: EmployeeId belongs to org, AllocationPercentage 1-100, JoinedDate <= ReleasedDate, no duplicate active membership
  - Capture snapshots: SnapshotBranchId, SnapshotDepartmentId from Employee at time of assignment
  - Create TeamMember (Status=Active), TimelineEvent (TeamMemberAdded), ActivityRecord (EmployeeAdded), AuditLog (TeamMemberAdded), Notification (TeamMemberAdded to EmployeeId)
  - _Requirements: 6.1, 6.2, 6.3, 10.4, 12.7, 13.7, 17.4, 23.11, 27.3, 27.5_


- [ ] 6.2 Create AddTeamMemberCommandValidator
  - Rules: ProjectRole not empty, max 100; AllocationPercentage 1-100; JoinedDate <= ReleasedDate when both provided
  - _Requirements: 10.4, 22.18-22.24_

- [ ] 6.3 Create TeamMemberResponse DTO
  - Id, ProjectId, EmployeeId, EmployeeName, ProjectRole, AllocationPercentage, JoinedDate, ReleasedDate?, Status, Remarks?
  - _Requirements: 6.1_

- [ ] 6.4 Create UpdateTeamMemberCommand and handler
  - Validate: Status=Active (cannot edit released members except Remarks)
  - Update: ProjectRole, AllocationPercentage, Remarks (if Active); only Remarks (if Released)
  - _Requirements: 6.6, 6.8_

- [ ] 6.5 Create ReleaseTeamMemberCommand and handler
  - Set Status=Released, ReleasedDate=provided date
  - Create TimelineEvent (TeamMemberReleased), ActivityRecord (EmployeeReleased), AuditLog (TeamMemberReleased), Notification (TeamMemberReleased to EmployeeId)
  - _Requirements: 6.5, 12.8, 13.8, 17.5, 23.12_

- [ ] 6.6 Create RemoveTeamMemberCommand and handler
  - Soft delete team member record
  - _Requirements: 6.8_

- [ ] 6.7 Create GetTeamMembersQuery and handler
  - PagedRequest with filter: Status?
  - Return TeamMemberResponse list with resolved EmployeeName
  - _Requirements: 6.7_

### 7. Application Layer — CQRS Features (Project Assets)

- [ ] 7.1 Create AllocateAssetCommand and handler
  - Validate: Asset belongs to org, Asset.AssetStatus == Available, AllocatedQuantity > 0, no existing active allocation for same asset+project
  - Create ProjectAssetAllocation (Status=Active)
  - Update Asset: AssetStatus = Assigned, ProjectId = projectId
  - Create TimelineEvent (AssetAllocated), ActivityRecord (AssetAllocated), AuditLog (AssetAllocated), Notification (AssetAllocated to ProjectManagerEmployeeId)
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 10.5, 10.6, 12.9, 13.9, 17.6, 23.13, 27.8, 27.9_


- [ ] 7.2 Create AllocateAssetCommandValidator
  - Rules: AllocatedQuantity > 0, AllocationDate provided
  - _Requirements: 10.5, 10.6, 22.25-22.28_

- [ ] 7.3 Create AssetAllocationResponse DTO
  - Id, ProjectId, AssetId, AssetCode, AssetName, AllocationDate, ReturnDate?, AllocatedQuantity, ReturnedQuantity, Status, Remarks?
  - _Requirements: 7.1_

- [ ] 7.4 Create ReturnAssetCommand and handler
  - Validate: Allocation exists, ReturnedQuantity <= AllocatedQuantity
  - Update Allocation: ReturnedQuantity, ReturnDate, Status (Returned if full return, PartiallyReturned if partial)
  - Update Asset: ProjectId = null (if full return), AssetStatus = Available (or Damaged based on condition)
  - Create TimelineEvent (AssetReturned), ActivityRecord (AssetReturned), AuditLog (AssetReturned), Notification (AssetReturned to ProjectManagerEmployeeId)
  - _Requirements: 7.7, 12.10, 13.10, 17.7, 23.14, 27.10_

- [ ] 7.5 Create GetAssetAllocationsQuery and handler
  - PagedRequest with filter: Status?
  - Return AssetAllocationResponse list with resolved AssetCode, AssetName
  - _Requirements: 7.9_

### 8. Application Layer — CQRS Features (Project Documents)

- [ ] 8.1 Create UploadDocumentCommand and handler
  - Validate: DocumentName not empty, max 200; Category valid enum; file size <= 50MB (if validation done here); file type in allowed list
  - Create ProjectDocument (Version = 1)
  - Create TimelineEvent (DocumentUploaded), ActivityRecord (DocumentUploaded), AuditLog (DocumentUploaded), Notification (DocumentUploaded to ProjectManagerEmployeeId)
  - _Requirements: 5.1, 5.2, 5.3, 10.7, 12.11, 13.11, 17.13, 22.29-22.35, 23.15_

- [ ] 8.2 Create UploadDocumentCommandValidator
  - Rules: DocumentName not empty, max 200; Description max 500; Category valid enum
  - _Requirements: 10.7, 22.29-22.31_

- [ ] 8.3 Create DocumentResponse DTO
  - Id, DocumentName, Category, Description?, FileReference, UploadedByEmployeeName?, UploadedAtUtc, Version, Remarks?, ExpiryDate?
  - _Requirements: 5.1, 5.12_


- [ ] 8.4 Create ReplaceDocumentCommand and handler
  - Load existing document, increment Version, create new ProjectDocument record with new FileReference
  - Create TimelineEvent (DocumentReplaced), ActivityRecord (DocumentReplaced), AuditLog (DocumentReplaced with OldValue/NewValue versions)
  - _Requirements: 5.4, 12.12, 13.12, 23.16_

- [ ] 8.5 Create DeleteDocumentCommand and handler
  - Soft delete: IsDeleted = true, DeletedAtUtc = now
  - Create TimelineEvent (DocumentDeleted), ActivityRecord (DocumentDeleted), AuditLog (DocumentDeleted)
  - _Requirements: 5.7, 12.13, 13.13, 23.17_

- [ ] 8.6 Create GetDocumentsQuery and handler
  - PagedRequest with filters: Category?, search (DocumentName)
  - Return DocumentResponse list with resolved UploadedByEmployeeName
  - _Requirements: 5.9, 5.10, 5.11_

- [ ] 8.7 Create GetDocumentVersionHistoryQuery and handler
  - GetVersionHistoryAsync (projectId, documentName), return all versions descending by Version
  - _Requirements: 5.8_

### 9. Application Layer — CQRS Features (Project Parameters)

- [ ] 9.1 Create CreateParameterSectionCommand and handler
  - Create ProjectParameterSection (Name, DisplayOrder, ProjectId)
  - Create TimelineEvent, ActivityRecord (SectionCreated), AuditLog (SectionAdded)
  - _Requirements: 4.1, 4.2, 13.17, 23.21_

- [ ] 9.2 Create UpdateParameterSectionCommand and handler
  - Update Name, DisplayOrder
  - _Requirements: 4.3_

- [ ] 9.3 Create DeleteParameterSectionCommand and handler
  - Soft delete section (cascades to parameters if configured, or handle explicitly)
  - Create ActivityRecord (SectionDeleted), AuditLog (SectionDeleted)
  - _Requirements: 4.3, 13.18, 23.22_

- [ ] 9.4 Create AddParameterCommand and handler
  - Create ProjectParameter (ParameterName, InputType, Unit?, IsRequired, DisplayOrder, DropdownOptionsJson?, SectionId)
  - Create TimelineEvent (ParameterCreated), ActivityRecord (ParameterCreated), AuditLog (ParameterAdded)
  - _Requirements: 4.4, 4.5, 4.8, 12.17, 13.14, 23.18_


- [ ] 9.5 Create AddParameterCommandValidator
  - Rules: ParameterName not empty, max 100; InputType valid enum; DisplayOrder positive; DropdownOptionsJson required if InputType=Dropdown
  - _Requirements: 22.36-22.39_

- [ ] 9.6 Create UpdateParameterCommand and handler
  - Update ParameterName, InputType, Unit, IsRequired, DisplayOrder, DropdownOptionsJson
  - _Requirements: 4.7_

- [ ] 9.7 Create DeleteParameterCommand and handler
  - Soft delete parameter
  - Create TimelineEvent (ParameterDeleted), ActivityRecord (ParameterDeleted), AuditLog (ParameterDeleted)
  - _Requirements: 4.7, 12.18, 13.16, 23.20_

- [ ] 9.8 Create SaveParameterValuesCommand and handler
  - Accept list of ParameterValueItem (ParameterId, Value?)
  - Validate: Email format if InputType=Email, Phone format if InputType=Phone, URL format if InputType=URL, numeric if InputType=Number/Decimal, required if IsRequired=true
  - Upsert ProjectParameterValue records (update if exists, insert if not)
  - Create TimelineEvent (ParameterUpdated), ActivityRecord (ParameterUpdated), AuditLog (ParameterUpdated with OldValue/NewValue)
  - _Requirements: 4.6, 4.9, 10.8, 10.9, 12.19, 13.15, 22.40-22.45, 23.19_

- [ ] 9.9 Create GetParameterSectionsQuery and handler
  - GetSectionsAsync (projectId), return all sections with nested parameters and values, ordered by DisplayOrder
  - _Requirements: 4.1_

### 10. Application Layer — CQRS Features (Project Budget)

- [ ] 10.1 Create UpdateBudgetCommand and handler
  - Validate: All budget fields >= 0
  - Create new ProjectBudget record (append-only pattern, never update existing)
  - Compute in-memory: RemainingBudget = ApprovedBudget - ActualCost, BudgetPercentageUsed = (ActualCost / ApprovedBudget) * 100 (if ApprovedBudget > 0 else 0), BudgetStatus = Under/On/Over Budget
  - Create TimelineEvent (BudgetUpdated), ActivityRecord (BudgetUpdated), AuditLog (BudgetUpdated)
  - Generate notifications: if BudgetPercentageUsed >= 80% and < 100%: Notification (BudgetThresholdCrossed, Warning); if >= 100%: Notification (BudgetThresholdCrossed, Critical)
  - _Requirements: 14.1-14.9, 14.10, 14.11, 14.12, 17.8, 17.9, 22.46-22.48, 23.23_


- [ ] 10.2 Create UpdateBudgetCommandValidator
  - Rules: EstimatedBudget >= 0, ApprovedBudget >= 0, ActualCost >= 0, all cost fields >= 0
  - _Requirements: 14.8, 14.9, 22.46-22.47_

- [ ] 10.3 Create BudgetResponse DTO
  - Sealed record: Id, ProjectId, EstimatedBudget, ApprovedBudget, ActualCost, ProcurementCost, MaintenanceCost, LabourCost, MiscellaneousCost, RemainingBudget (computed), BudgetPercentageUsed (computed), BudgetStatus (string), CreatedAtUtc
  - _Requirements: 14.1, 14.2, 14.3, 14.5, 14.6, 14.7_

- [ ] 10.4 Create GetCurrentBudgetQuery and handler
  - GetLatestByProjectAsync, return BudgetResponse with all computed fields
  - _Requirements: 14.1_

- [ ] 10.5 Create GetBudgetHistoryQuery and handler
  - GetHistoryAsync (projectId, PagedRequest), return all budget versions descending by CreatedAtUtc
  - _Requirements: 14.13_

### 11. Application Layer — CQRS Features (Project Risks)

- [ ] 11.1 Create CreateRiskCommand and handler
  - Validate: Title not empty, max 200; OwnerEmployeeId belongs to org (if provided)
  - Compute: Severity = RiskSeverityHelper.ComputeSeverity(Probability, Impact)
  - Create ProjectRisk (Status=Open, CreatedAtUtc=now)
  - Create TimelineEvent (RiskAdded), ActivityRecord (RiskAdded), AuditLog (RiskCreated)
  - Generate notifications: Notification (RiskCreated to ProjectManagerEmployeeId); if Severity == Critical: Notification (RiskCritical, Critical to ProjectManagerEmployeeId AND OwnerEmployeeId)
  - _Requirements: 15.7, 15.8, 15.9, 12.15, 13.20, 17.10, 17.11, 22.49-22.56, 23.24_

- [ ] 11.2 Create CreateRiskCommandValidator
  - Rules: Title not empty, max 200; Description max 1000; MitigationPlan max 1000; Category, Probability, Impact valid enum values
  - _Requirements: 15.7, 22.49-22.56_

- [ ] 11.3 Create UpdateRiskCommand and handler
  - Update: Title, Description, Category, Probability, Impact, OwnerEmployeeId, MitigationPlan, Status, Remarks
  - Recompute: Severity = RiskSeverityHelper.ComputeSeverity(Probability, Impact)
  - Create ActivityRecord (RiskUpdated), AuditLog (RiskUpdated)
  - _Requirements: 15.12, 15.13, 13.21, 23.25_

- [ ] 11.4 Create CloseRiskCommand and handler
  - Set Status=Closed, ClosedAtUtc=now
  - Create TimelineEvent (RiskClosed), ActivityRecord (RiskClosed), AuditLog (RiskClosed), Notification (RiskClosed to ProjectManagerEmployeeId)
  - _Requirements: 15.10, 12.16, 13.22, 17.12, 23.26_

- [ ] 11.5 Create DeleteRiskCommand and handler
  - Soft delete: IsDeleted=true, DeletedAtUtc=now
  - _Requirements: 15.23_

- [ ] 11.6 Create GetRisksQuery and handler
  - Extend PagedRequest with filters: Category?, Probability?, Impact?, Severity?, Status?, OwnerEmployeeId?
  - Return RiskResponse list with resolved OwnerName
  - _Requirements: 15.14, 15.15, 15.16_

- [ ] 11.7 Create GetRiskQuery and handler
  - GetByIdAsync, return RiskResponse with resolved OwnerName
  - _Requirements: 15.14_

- [ ] 11.8 Create RiskResponse DTO
  - Sealed record: Id, Title, Description?, Category, Probability, Impact, Severity, Status, OwnerEmployeeId?, OwnerName?, MitigationPlan?, Remarks?, ClosedAtUtc?, CreatedAtUtc
  - _Requirements: 15.1_

### 12. Application Layer — CQRS Features (Timeline & Activities)

- [ ] 12.1 Create GetTimelineQuery and handler
  - Accept: ProjectId, EventType?, Keyword?, PageNumber, PageSize
  - GetPagedAsync with filters, search by keyword in Description
  - Return TimelineEventResponse list with resolved UserName
  - _Requirements: 12.22, 12.23, 12.24, 12.25, 12.26_

- [ ] 12.2 Create TimelineEventResponse DTO
  - Sealed record: Id, EventType, Description, Timestamp, EntityType, EntityId?, UserId?, UserName?, IconType
  - _Requirements: 12.2_

- [ ] 12.3 Create GetActivitiesQuery and handler
  - Accept: ProjectId, ActivityType?, UserId?, From (DateTime?), To (DateTime?), Keyword?, PageNumber, PageSize
  - GetPagedAsync with filters, search by keyword in Action and Remarks
  - Return ActivityRecordResponse list with resolved UserName
  - _Requirements: 13.26, 13.27, 13.28, 13.29, 13.30_

- [ ] 12.4 Create ActivityRecordResponse DTO
  - Sealed record: Id, ActivityType, Action, UserId?, UserName?, TargetEntity, TargetEntityId?, Timestamp, Remarks?
  - _Requirements: 13.2, 13.23, 13.24, 13.25_

### 13. Application Layer — CQRS Features (Dashboard)

- [ ] 13.1 Create GetProjectDashboardQuery and handler
  - Execute aggregate queries for: team count (active), asset count (by status), document count (by category), risk count (by severity), budget summary (latest budget with computed fields), recent 10 activities
  - Compute: days elapsed since StartDate, days remaining until ExpectedCompletionDate, has upcoming deadline (ExpectedCompletionDate within 30 days), is pending approval (Status == AwaitingApproval)
  - Return ProjectDashboardResponse
  - _Requirements: 11.1-11.13_

- [ ] 13.2 Create ProjectDashboardResponse DTO
  - Sealed record with: ProjectResponse GeneralInfo, TeamSummary, AssetSummary, DocumentSummary, RiskSummary, BudgetSummary, List<ActivityRecordResponse> RecentActivities, bool HasUpcomingDeadline, int? DaysUntilDeadline, bool IsPendingApproval, int DaysElapsed, int? DaysRemaining
  - _Requirements: 11.2-11.13_

### 14. Application Layer — CQRS Features (Reports)

- [ ] 14.1 Create GetProjectSummaryReportQuery and handler
  - Fetch project with all general info, resolve all foreign keys (Customer, Category, Branch, Department, ProjectManager)
  - Return ProjectSummaryReportResponse
  - _Requirements: 16.1_

- [ ] 14.2 Create GetTeamAllocationReportQuery and handler
  - Fetch all team members with resolved Employee names, roles, allocation %
  - Return TeamAllocationReportResponse
  - _Requirements: 16.2_

- [ ] 14.3 Create GetAssetAllocationReportQuery and handler
  - Fetch all asset allocations with resolved Asset code/name, allocation dates, quantities
  - Return AssetAllocationReportResponse
  - _Requirements: 16.3_

- [ ] 14.4 Create GetBudgetReportQuery and handler
  - Fetch budget history with all versions, compute RemainingBudget, BudgetPercentageUsed
  - Return BudgetReportResponse
  - _Requirements: 16.4_

- [ ] 14.5 Create GetRiskReportQuery and handler
  - Fetch all risks with resolved Owner names, categories, severities
  - Return RiskReportResponse
  - _Requirements: 16.5_

- [ ] 14.6 Create GetDocumentRegisterReportQuery and handler
  - Fetch all documents with resolved UploaderNames, categories, versions
  - Return DocumentRegisterReportResponse
  - _Requirements: 16.6_

- [ ] 14.7 Create GetActivityReportQuery and handler
  - Accept filters: ActivityType?, UserId?, FromDate, ToDate
  - Fetch filtered activities with resolved UserNames
  - Return ActivityReportResponse
  - _Requirements: 16.7_

- [ ] 14.8 Create GetTimelineReportQuery and handler
  - Accept filters: EventType?, FromDate, ToDate
  - Fetch filtered timeline events with resolved UserNames
  - Return TimelineReportResponse
  - _Requirements: 16.8_

- [ ] 14.9 Create GetParameterReportQuery and handler
  - Fetch all parameter sections with nested parameters and values, ordered by DisplayOrder
  - Return ParameterReportResponse
  - _Requirements: 16.9_

- [ ] 14.10 Create ExportReportPdfCommand and handler
  - Accept: ReportType enum, ProjectId, filter parameters
  - Generate PDF using existing NexAsset PDF generation library
  - Include: Organization logo, report generation timestamp, user who generated
  - Return file stream
  - _Requirements: 16.10, 16.16, 16.17, 16.18_

- [ ] 14.11 Create ExportReportExcelCommand and handler
  - Accept: ReportType enum, ProjectId, filter parameters
  - Generate Excel using existing NexAsset Excel generation library
  - Include: Organization logo, report generation timestamp, user who generated
  - Return file stream
  - _Requirements: 16.11, 16.16, 16.17, 16.18_

### 15. Application Layer — CQRS Features (Search & Filters)

- [ ] 15.1 Create GlobalSearchQuery and handler
  - Accept: OrganizationId, Keyword, PageNumber, PageSize
  - Search across: Projects (ProjectCode, ProjectName), ProjectParameters (ParameterName), ProjectParameterValues (Value), ProjectDocuments (DocumentName, Description, Remarks), ProjectTeamMembers (resolved EmployeeName), ProjectAssetAllocations (resolved AssetCode, AssetName), Risks (Title, Description, MitigationPlan), ActivityRecords (Action, Remarks)
  - Group results by EntityType with match count per type
  - Return GlobalSearchResponse
  - _Requirements: 18.1-18.9, 18.10, 18.11_

- [ ] 15.2 Create SaveFilterCommand and handler
  - Create SavedFilter (UserId, FilterName, EntityType, SearchKeyword?, FilterCriteriaJson)
  - _Requirements: 18.17, 18.18_

- [ ] 15.3 Create GetSavedFiltersQuery and handler
  - GetByUserAsync (userId, orgId, entityType?)
  - Return list of SavedFilter records
  - _Requirements: 18.19_

- [ ] 15.4 Create DeleteSavedFilterCommand and handler
  - Remove SavedFilter by Id
  - _Requirements: 18.20_

- [ ] 15.5 Create GlobalSearchResponse DTO
  - Sealed record: List<SearchResultGroup> Groups, int TotalCount, int PageNumber, int PageSize
  - SearchResultGroup: string EntityType, int MatchCount, List<SearchResultItem> Items
  - SearchResultItem: Guid Id, string DisplayText, string Highlight, string NavigationUrl
  - _Requirements: 18.10, 18.11_

### 16. API Layer — Endpoints

- [ ] 16.1 Create ProjectCategoryEndpoints
  - Create `NexAsset.API/Endpoints/ProjectCategories/ProjectCategoryEndpoints.cs`
  - Map: GET /api/project-categories, GET /api/project-categories/{id}, POST /api/project-categories, PUT /api/project-categories/{id}, DELETE /api/project-categories/{id}
  - Use ISender (MediatR), PermissionRouteConvention, PermissionEnforcementFilter
  - _Requirements: 9.2, 21.1, 21.23-21.26, 28.1-28.25_

- [ ] 16.2 Create ProjectEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectEndpoints.cs`
  - Map: GET /api/projects, GET /api/projects/{id}, POST /api/projects, PUT /api/projects/{id}, DELETE /api/projects/{id}, POST /api/projects/{id}/status, POST /api/projects/{id}/duplicate
  - Special cases: POST /{id}/status requires Projects.Approve (for Approved transition), Projects.Archive (for Archived), Projects.Restore (for InProgress from Archived); POST /{id}/duplicate requires Projects.Duplicate
  - _Requirements: 9.2, 21.1-21.9, 28.1-28.25_

- [ ] 16.3 Create ProjectTeamEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectTeamEndpoints.cs`
  - Map: GET /api/projects/{id}/team, POST /api/projects/{id}/team, PUT /api/projects/{id}/team/{memberId}, POST /api/projects/{id}/team/{memberId}/release, DELETE /api/projects/{id}/team/{memberId}
  - All require Projects.ManageTeam except GET (requires Projects.View)
  - _Requirements: 21.10, 28.1-28.25_

- [ ] 16.4 Create ProjectAssetEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectAssetEndpoints.cs`
  - Map: GET /api/projects/{id}/assets, POST /api/projects/{id}/assets, POST /api/projects/{id}/assets/{allocationId}/return
  - All require Projects.ManageAssets except GET (requires Projects.View)
  - _Requirements: 21.11, 28.1-28.25_

- [ ] 16.5 Create ProjectDocumentEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectDocumentEndpoints.cs`
  - Map: GET /api/projects/{id}/documents, POST /api/projects/{id}/documents, POST /api/projects/{id}/documents/{docId}/replace, DELETE /api/projects/{id}/documents/{docId}, GET /api/projects/{id}/documents/{docId}/versions
  - All require Projects.ManageDocuments except GET (requires Projects.View)
  - _Requirements: 21.13, 28.1-28.25_

- [ ] 16.6 Create ProjectParameterEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectParameterEndpoints.cs`
  - Map: GET /api/projects/{id}/parameters, POST /api/projects/{id}/parameters/sections, PUT /api/projects/{id}/parameters/sections/{sectionId}, DELETE /api/projects/{id}/parameters/sections/{sectionId}, POST /api/projects/{id}/parameters/sections/{sectionId}/parameters, PUT /api/projects/{id}/parameters/sections/{sectionId}/parameters/{paramId}, DELETE /api/projects/{id}/parameters/sections/{sectionId}/parameters/{paramId}, PUT /api/projects/{id}/parameters/values
  - All require Projects.ManageParameters except GET (requires Projects.View)
  - _Requirements: 21.12, 28.1-28.25_

- [ ] 16.7 Create ProjectBudgetEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectBudgetEndpoints.cs`
  - Map: GET /api/projects/{id}/budget, PUT /api/projects/{id}/budget, GET /api/projects/{id}/budget/history
  - GET requires Projects.ViewBudget, PUT requires Projects.ManageBudget
  - _Requirements: 21.14, 21.15, 28.1-28.25_

- [ ] 16.8 Create ProjectRiskEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectRiskEndpoints.cs`
  - Map: GET /api/projects/{id}/risks, GET /api/projects/{id}/risks/{riskId}, POST /api/projects/{id}/risks, PUT /api/projects/{id}/risks/{riskId}, POST /api/projects/{id}/risks/{riskId}/close, DELETE /api/projects/{id}/risks/{riskId}
  - GET requires Projects.ViewRisks, all others require Projects.ManageRisks
  - _Requirements: 21.16, 21.17, 28.1-28.25_

- [ ] 16.9 Create ProjectTimelineEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectTimelineEndpoints.cs`
  - Map: GET /api/projects/{id}/timeline
  - Requires Projects.ViewTimeline
  - _Requirements: 21.20, 28.1-28.25_

- [ ] 16.10 Create ProjectActivityEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectActivityEndpoints.cs`
  - Map: GET /api/projects/{id}/activities
  - Requires Projects.ViewActivities
  - _Requirements: 21.21, 28.1-28.25_

- [ ] 16.11 Create ProjectDashboardEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectDashboardEndpoints.cs`
  - Map: GET /api/projects/{id}/dashboard
  - Requires Projects.View
  - _Requirements: 21.1, 28.1-28.25_

- [ ] 16.12 Create ProjectReportEndpoints
  - Create `NexAsset.API/Endpoints/Projects/ProjectReportEndpoints.cs`
  - Map: GET /api/projects/{id}/reports/summary, GET /api/projects/{id}/reports/team, GET /api/projects/{id}/reports/assets, GET /api/projects/{id}/reports/budget, GET /api/projects/{id}/reports/risks, GET /api/projects/{id}/reports/documents, GET /api/projects/{id}/reports/activities, GET /api/projects/{id}/reports/timeline, GET /api/projects/{id}/reports/parameters, POST /api/projects/{id}/reports/export/pdf, POST /api/projects/{id}/reports/export/excel
  - GET requires Projects.ViewReports, POST export requires Projects.ExportReports
  - _Requirements: 21.18, 21.19, 28.1-28.25_

- [ ] 16.13 Create DraftSessionEndpoints
  - Create `NexAsset.API/Endpoints/Projects/DraftSessionEndpoints.cs`
  - Map: GET /api/projects/drafts, PUT /api/projects/drafts, DELETE /api/projects/drafts
  - All require Projects.Create
  - _Requirements: 21.3, 28.1-28.25_

- [ ] 16.14 Create GlobalSearchEndpoints
  - Create `NexAsset.API/Endpoints/Projects/GlobalSearchEndpoints.cs`
  - Map: GET /api/projects/search
  - Requires Projects.View
  - _Requirements: 21.1, 28.1-28.25_

- [ ] 16.15 Create SavedFilterEndpoints
  - Create `NexAsset.API/Endpoints/Projects/SavedFilterEndpoints.cs`
  - Map: GET /api/projects/saved-filters, POST /api/projects/saved-filters, DELETE /api/projects/saved-filters/{filterId}
  - All require Projects.View
  - _Requirements: 21.1, 28.1-28.25_

### 17. Frontend Layer — Pages

- [ ] 17.1 Create ProjectListPage.razor
  - Path: `/projects`
  - Display table: ProjectCode, ProjectName, Status, Priority, ProjectManager, StartDate, EndDate with pagination
  - Search box (debounced 300ms) by ProjectCode/ProjectName
  - Filters: Status, Priority, Category, Branch, Department, ProjectManager (dropdowns)
  - Sort: clickable column headers (ProjectName, StartDate, EndDate, CreatedAtUtc)
  - Row actions: View, Edit, Delete (conditional on permissions)
  - _Requirements: 29.1-29.5, 24.23-24.30_

- [ ] 17.2 Create ProjectDetailPage.razor
  - Path: `/projects/{id}`
  - Display tabbed/sectioned layout: Overview, General Info, Team, Assets, Parameters, Documents, Timeline, Activity, Budget, Risks, Reports, Settings
  - Default to Overview section
  - Lazy loading: load only Overview initially, defer other sections until user navigates to them
  - Breadcrumb: Organization → Projects → ProjectName → Section
  - _Requirements: 29.6-29.8, 25.7, 19.1-19.13, 24.19-24.48_

- [ ] 17.3 Create ProjectWizardPage.razor
  - Path: `/projects/new`
  - 7-step wizard: (1) General Information, (2) Project Team, (3) Asset Allocation, (4) Project Parameters, (5) Documents, (6) Review, (7) Save
  - Step indicator at top showing current step
  - Navigation: Back, Next, Save buttons (Back disabled on Step 1, Next disabled on Step 7, Save enabled only on Step 7)
  - Autosave: 5-second debounce, save draft to DraftSession after pause
  - Autosave indicator: "Saving...", "Saved at HH:MM:SS", "Unsaved Changes"
  - Resume draft: prompt user to resume existing draft or start new
  - Validate current step before proceeding to next
  - _Requirements: 3.1, 3.2, 3.3-3.6, 3.7, 3.8, 3.9, 3.11, 29.9-29.12, 24.14-24.17_

- [ ] 17.4 Create ProjectCategoryListPage.razor
  - Path: `/project-categories`
  - Display table: Name, Description, Status, CreatedAtUtc
  - Search, filter by Status, sort by Name/CreatedAtUtc
  - Row actions: Edit, Delete
  - _Requirements: 1.7, 1.8, 1.9_

### 17. Frontend Layer — Components (Dashboard)

- [ ] 17.5 Create ProjectDashboard.razor
  - Dashboard shell component, loads GetProjectDashboardQuery
  - Display card grid: OverviewCard, TeamSummaryCard, AssetSummaryCard, DocumentSummaryCard, RiskSummaryCard, BudgetSummaryCard (3 columns desktop, 2 tablet, 1 mobile)
  - Display RecentActivitiesWidget (10 most recent)
  - Display QuickActionsToolbar (Edit Project, Add Team Member, Allocate Asset, Upload Document, Add Risk)
  - Display UpcomingDeadlinesBanner (if ExpectedCompletionDate within 30 days)
  - _Requirements: 11.1-11.12, 29.13-29.15, 24.19, 24.23_

- [ ] 17.6 Create OverviewCard.razor
  - Display: Status badge, Priority badge, Health indicator, Progress percentage
  - _Requirements: 11.2, 8.3_

- [ ] 17.7 Create TeamSummaryCard.razor
  - Display: total member count, active allocation count
  - _Requirements: 11.3_

- [ ] 17.8 Create AssetSummaryCard.razor
  - Display: total allocated count, breakdown by asset status
  - _Requirements: 11.4_

- [ ] 17.9 Create DocumentSummaryCard.razor
  - Display: total document count, breakdown by category
  - _Requirements: 11.5_

- [ ] 17.10 Create RiskSummaryCard.razor
  - Display: open risk count, breakdown by severity
  - _Requirements: 11.6_

- [ ] 17.11 Create BudgetSummaryCard.razor
  - Display: EstimatedBudget, ApprovedBudget, ActualCost, RemainingBudget, BudgetPercentageUsed
  - _Requirements: 11.7_

- [ ] 17.12 Create RecentActivitiesWidget.razor
  - Display: 10 most recent Activity_Records with user avatar, action text, relative timestamp
  - _Requirements: 11.8_

- [ ] 17.13 Create QuickActionsToolbar.razor
  - Display: Quick action buttons (Edit Project, Add Team Member, Allocate Asset, Upload Document, Add Risk)
  - _Requirements: 11.12, 29.15_

- [ ] 17.14 Create RiskMatrixChart.razor
  - Display: 3x3 grid visualization (Probability x Impact), color-coded by Severity
  - _Requirements: 15.17, 15.18_

- [ ] 17.15 Create BudgetProgressChart.razor
  - Display: Visual chart showing ApprovedBudget, ActualCost, RemainingBudget with progress bar
  - _Requirements: 11.7_

- [ ] 17.16 Create TimelineViewer.razor
  - Display: Vertical timeline with event icons, descriptions, timestamps, grouped by date
  - _Requirements: 12.22, 12.23, 12.24, 12.25, 12.26, 12.27, 12.28, 29.16, 29.17, 29.18_

- [ ] 17.17 Create ActivityFeedList.razor
  - Display: Activity list with user avatars, action descriptions, relative timestamps
  - _Requirements: 13.23, 13.24, 13.25, 13.26, 13.27, 13.28, 13.29, 13.30, 29.19, 29.20_

- [ ] 17.18 Create TeamMembersTable.razor
  - Display: Team members table with EmployeeName, ProjectRole, AllocationPercentage, JoinedDate, Status, row actions
  - _Requirements: 6.7_

- [ ] 17.19 Create AddTeamMemberModal.razor
  - Display: Modal form with fields: EmployeeId (dropdown), ProjectRole, AllocationPercentage, JoinedDate, Remarks
  - _Requirements: 6.1, 6.2_

- [ ] 17.20 Create EditTeamMemberModal.razor
  - Display: Modal form for editing active team member or read-only view for released member
  - _Requirements: 6.6, 6.8_

- [ ] 17.21 Create AssetAllocationsTable.razor
  - Display: Asset allocations table with AssetCode, AssetName, AllocationDate, ReturnDate, AllocatedQuantity, Status, row actions
  - _Requirements: 7.9_

- [ ] 17.22 Create AllocateAssetModal.razor
  - Display: Modal form with fields: AssetId (dropdown with availability indicator), AllocationDate, AllocatedQuantity, Remarks
  - _Requirements: 7.1, 7.2, 7.3_

- [ ] 17.23 Create ReturnAssetModal.razor
  - Display: Modal form with fields: ReturnDate, ReturnedQuantity (<=AllocatedQuantity), Remarks
  - _Requirements: 7.7_

- [ ] 17.24 Create ParameterSectionCard.razor
  - Display: Collapsible card showing section name as header, list of parameters with values inside
  - _Requirements: 4.1, 29.30_

- [ ] 17.25 Create AddSectionModal.razor
  - Display: Modal form with field: Name
  - _Requirements: 4.1, 4.2_

- [ ] 17.26 Create AddParameterModal.razor
  - Display: Modal form with fields: ParameterName, InputType (dropdown), Unit, IsRequired (checkbox), DisplayOrder, DropdownOptionsJson (conditional)
  - _Requirements: 4.4, 4.5, 4.8_

- [ ] 17.27 Create ParameterValueInput.razor
  - Component: Renders correct input control per InputType (Text, Textarea, Number, Decimal, Date, Dropdown, Checkbox, Email, Phone, URL)
  - _Requirements: 4.6, 29.33_

- [ ] 17.28 Create DocumentGrid.razor
  - Display: Card grid or table view (user-selectable) with document cards/rows showing thumbnail, DocumentName, Category, Version, UploadedBy, UploadedAtUtc, actions
  - _Requirements: 5.9, 5.10, 5.11, 29.26_

- [ ] 17.29 Create UploadDocumentModal.razor
  - Display: Modal with drag-and-drop file upload or file picker, fields for DocumentName, Category, Description, Remarks
  - _Requirements: 5.1, 5.2, 5.3, 29.27_

- [ ] 17.30 Create DocumentPreviewModal.razor
  - Display: Modal overlay for previewing PDF and image files with zoom controls and page navigation
  - _Requirements: 5.6, 29.29_

- [ ] 17.31 Create VersionHistoryModal.razor
  - Display: Modal showing all document versions in descending order with download links
  - _Requirements: 5.8, 29.28_

- [ ] 17.32 Create BudgetForm.razor
  - Display: Form with input fields for all budget fields (EstimatedBudget, ApprovedBudget, ActualCost, ProcurementCost, MaintenanceCost, LabourCost, MiscellaneousCost), computed read-only fields (RemainingBudget, BudgetPercentageUsed, BudgetStatus)
  - _Requirements: 14.1, 14.2, 14.3, 14.4, 14.5, 14.6, 14.7, 29.21_

- [ ] 17.33 Create BudgetHistoryTable.razor
  - Display: Table showing all previous budget versions with timestamps and user who made the change
  - _Requirements: 14.13, 29.22_

- [ ] 17.34 Create RisksTable.razor
  - Display: Table with columns: Title, Category, Probability, Impact, Severity (color-coded), Owner, Status, row actions
  - _Requirements: 15.14, 15.15, 15.16, 29.23_

- [ ] 17.35 Create AddRiskModal.razor
  - Display: Modal form with fields: Title, Description, Category, Probability, Impact, Owner, MitigationPlan
  - _Requirements: 15.7, 15.8, 15.9, 29.25_

- [ ] 17.36 Create EditRiskModal.razor
  - Display: Modal form for editing risk with automatic Severity recomputation
  - _Requirements: 15.12, 15.13_

- [ ] 17.37 Create ReportSelector.razor
  - Display: Dropdown with report types and filter form for selected report
  - _Requirements: 16.10, 16.11, 29.35, 29.36_

- [ ] 17.38 Create ReportPreview.razor
  - Display: Read-only rendering of report based on selected filters
  - _Requirements: 16.12, 29.37_

- [ ] 17.39 Create GlobalSearchBar.razor
  - Display: Search input with debounced typing (300ms)
  - _Requirements: 18.1, 18.2_

- [ ] 17.40 Create SearchResultsGrouped.razor
  - Display: Search results grouped by entity type with match counts
  - _Requirements: 18.9, 18.10, 18.11_

### 18. Permission Seeding

- [ ] 18.1 Add 24 new permissions to PermissionSeeder.Matrix
  - Add permissions: Projects.View, Projects.Create, Projects.Update, Projects.Delete, Projects.Archive, Projects.Restore, Projects.Duplicate, Projects.Approve, Projects.ManageTeam, Projects.ManageAssets, Projects.ManageParameters, Projects.ManageDocuments, Projects.ViewBudget, Projects.ManageBudget, Projects.ViewRisks, Projects.ManageRisks, Projects.ViewReports, Projects.ExportReports, Projects.ViewTimeline, Projects.ViewActivities, Projects.ManageSettings, ProjectCategories.View, ProjectCategories.Create, ProjectCategories.Update, ProjectCategories.Delete
  - _Requirements: 9.2, 21.1_

### 19. Testing

- [ ] 19.1 Write unit tests for domain helpers
  - Test RiskSeverityHelper.ComputeSeverity for all 9 probability-impact combinations
  - Test ProjectStatusTransition.IsAllowed for all valid and invalid transitions
  - Test budget computed fields (RemainingBudget, BudgetPercentageUsed, BudgetStatus)
  - _Requirements: 15.5, 2.4, 14.2, 14.3, 14.4, 14.5, 14.6, 14.7_

- [ ] 19.2 Write unit tests for FluentValidation validators
  - Test CreateProjectCommandValidator (ProjectCode, ProjectName, Description, Notes, InternalRemarks, StartDate <= EndDate)
  - Test AddTeamMemberCommandValidator (ProjectRole, AllocationPercentage 1-100, JoinedDate <= ReleasedDate)
  - Test AllocateAssetCommandValidator (AllocatedQuantity > 0)
  - Test UpdateBudgetCommandValidator (all cost fields >= 0)
  - Test CreateRiskCommandValidator (Title, Description, MitigationPlan lengths, Category/Probability/Impact valid enums)
  - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5, 10.6, 10.7, 10.8, 10.9_

- [ ] 19.3 Write integration tests for CQRS handlers
  - Test end-to-end CreateProjectCommand → Project entity created + Timeline + Activity + AuditLog
  - Test TransitionProjectStatusCommand for all valid transitions with permission checks
  - Test AllocateAssetCommand → Asset.ProjectId updated + Asset.AssetStatus = Assigned
  - Test ReturnAssetCommand → Asset.ProjectId cleared + Asset.AssetStatus = Available
  - Test UpdateBudgetCommand → append-only behavior (new record created, not updated)
  - Test CreateRiskCommand → Severity computed correctly + Notifications generated for Critical
  - Test DuplicateProjectCommand → fields copied/blanked correctly
  - Test multi-tenant isolation (org A cannot access org B projects)
  - _Requirements: 2.1, 2.4, 2.5, 2.8, 2.10, 7.1, 7.2, 7.3, 7.5, 7.7, 14.10, 15.7, 15.8, 15.9, 15.11, 9.1_

- [ ] 19.4 Write API integration tests (Postman or automated)
  - Test happy path for every endpoint (200/201/204 responses)
  - Test validation failures (400 with FluentValidation shape)
  - Test authorization failures (403 for each permission)
  - Test unauthenticated requests (401)
  - Test invalid IDs (404)
  - Test cross-tenant isolation (org A cannot access org B projects via API)
  - _Requirements: All API requirements_

### 20. Documentation

- [ ] 20.1 Update API documentation and generate Swagger docs
  - Update Swagger annotations for all new endpoints
  - Generate API documentation with endpoint descriptions, request/response schemas, permission requirements
  - _Requirements: All module requirements_

## Task Dependency Graph

```json
{
  "waves": [
    { "id": 0, "tasks": ["1.1"] },
    { "id": 1, "tasks": ["1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", "1.10", "1.11", "1.12", "1.13", "1.14", "1.15", "1.16", "1.17", "1.18"] },
    { "id": 2, "tasks": ["2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12", "2.13", "2.14", "2.15", "2.16"] },
    { "id": 3, "tasks": ["2.17", "2.18", "2.19", "2.20", "2.21", "2.22", "2.23", "2.24", "2.25", "2.26", "2.27", "2.28"] },
    { "id": 4, "tasks": ["2.29", "2.30", "2.31", "2.32", "2.33", "2.34", "2.35", "2.36", "2.37", "2.38", "2.39", "2.40", "2.41", "2.43", "2.44", "2.45"] },
    { "id": 5, "tasks": ["2.42"] },
    { "id": 6, "tasks": ["3.1", "3.6", "3.7"] },
    { "id": 7, "tasks": ["3.2", "3.3", "3.4", "3.5", "3.8"] },
    { "id": 8, "tasks": ["4.1", "4.3", "4.5", "4.6", "4.9", "4.10", "4.11"] },
    { "id": 9, "tasks": ["4.2", "4.4", "4.7", "4.8"] },
    { "id": 10, "tasks": ["5.1", "5.2", "5.3"] },
    { "id": 11, "tasks": ["6.1", "6.3", "6.6", "6.7"] },
    { "id": 12, "tasks": ["6.2", "6.4", "6.5"] },
    { "id": 13, "tasks": ["7.1", "7.4", "7.5"] },
    { "id": 14, "tasks": ["7.2", "7.3"] },
    { "id": 15, "tasks": ["8.1", "8.3", "8.5", "8.6", "8.7"] },
    { "id": 16, "tasks": ["8.2", "8.4"] },
    { "id": 17, "tasks": ["9.1", "9.2", "9.3", "9.4", "9.6", "9.7", "9.8", "9.9"] },
    { "id": 18, "tasks": ["9.5"] },
    { "id": 19, "tasks": ["10.1", "10.3", "10.4", "10.5"] },
    { "id": 20, "tasks": ["10.2"] },
    { "id": 21, "tasks": ["11.1", "11.3", "11.4", "11.5", "11.6", "11.7", "11.8"] },
    { "id": 22, "tasks": ["11.2"] },
    { "id": 23, "tasks": ["12.1", "12.2", "12.3", "12.4"] },
    { "id": 24, "tasks": ["13.1", "13.2"] },
    { "id": 25, "tasks": ["14.1", "14.2", "14.3", "14.4", "14.5", "14.6", "14.7", "14.8", "14.9"] },
    { "id": 26, "tasks": ["14.10", "14.11"] },
    { "id": 27, "tasks": ["15.1", "15.2", "15.3", "15.4", "15.5"] },
    { "id": 28, "tasks": ["16.1", "16.2", "16.3", "16.4", "16.5", "16.6", "16.7", "16.8", "16.9", "16.10", "16.11", "16.12", "16.13", "16.14", "16.15"] },
    { "id": 29, "tasks": ["17.1", "17.2", "17.3", "17.4"] },
    { "id": 30, "tasks": ["17.5", "17.6", "17.7", "17.8", "17.9", "17.10", "17.11", "17.12", "17.13", "17.14", "17.15", "17.16", "17.17", "17.18", "17.19", "17.20", "17.21", "17.22", "17.23", "17.24", "17.25", "17.26", "17.27", "17.28", "17.29", "17.30", "17.31", "17.32", "17.33", "17.34", "17.35", "17.36", "17.37", "17.38", "17.39", "17.40"] },
    { "id": 31, "tasks": ["18.1"] },
    { "id": 32, "tasks": ["19.1", "19.2", "19.3", "19.4"] },
    { "id": 33, "tasks": ["20.1"] }
  ]
}
```

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- The dependency graph enables parallel execution of independent tasks
- Testing tasks (19.1-19.4) are marked optional but highly recommended
- Wave 0: Enums foundation
- Waves 1-2: Domain entities and EF configurations
- Waves 3-5: Repository interfaces, implementations, and migration
- Waves 6-27: CQRS features (Commands, Queries, Handlers, Validators, DTOs)
- Waves 28-30: API endpoints and frontend UI
- Wave 31: Permission seeding
- Wave 32: Testing
- Wave 33: Documentation
