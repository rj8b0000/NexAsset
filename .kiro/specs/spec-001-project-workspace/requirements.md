# Requirements Document

**Specification ID:** SPEC-001  
**Title:** Project Workspace Module

---

## Introduction

The Project Workspace module is the operational center of NexAsset. It transforms a project from a simple record into a fully contextualized workspace where organizations manage all aspects of project execution — employees, assets, documents, dynamic technical parameters, budget, risks, and reports.

The module is designed to be industry-agnostic, serving Solar EPC, Electrical Contractors, HT/LT Panel Manufacturing, Electrical Testing, O&M Companies, Civil Construction, Mechanical Engineering, Infrastructure, Facility Management, Manufacturing, and Industrial Automation organizations on the same platform without hardcoded industry-specific fields.

The module integrates with existing NexAsset entities: Customer, Employee, Asset, Branch, Department, and Organization.

---

## Glossary

- **Project_Workspace**: The NexAsset module that provides a centralized workspace for managing all aspects of a project.
- **Project**: A unit of business work executed by an organization, containing team, assets, documents, and parameters.
- **Project_Category**: An organization-scoped classification label applied to projects.
- **Project_Status**: The current lifecycle stage of a project (Draft, Planning, AwaitingApproval, Approved, InProgress, OnHold, Completed, Archived, Cancelled).
- **Project_Priority**: Urgency level of a project (Low, Medium, High, Critical).
- **Project_Wizard**: A multi-step guided interface for creating a project, with autosave support.
- **Draft_Session**: A persisted, incomplete project creation attempt that can be resumed.
- **Project_Parameter_Section**: A user-defined named group of project parameters.
- **Project_Parameter**: A user-defined field within a section, with a name, input type, unit, required flag, and display order.
- **Parameter_Input_Type**: One of: Text, Textarea, Number, Decimal, Date, Dropdown, Checkbox, Email, Phone, URL.
- **Project_Parameter_Value**: The actual value entered for a Project_Parameter on a specific project.
- **Project_Team_Member**: An employee assigned to a project with a role, allocation percentage, and date range.
- **Project_Asset_Allocation**: An association between an asset and a project, tracking quantity, dates, and status.
- **Project_Document**: A file attached to a project with metadata including category, version, and uploader.
- **Document_Category**: The classification of a project document (Agreement, Contract, Survey, BOQ, Technical Drawing, Government Approval, Inspection Report, Completion Certificate, Invoice, Purchase Document, Photo, Video, Other).
- **Autosave**: The automatic persistence of wizard form state without explicit user action.
- **Organization**: The top-level tenant entity in NexAsset, scoping all project data.
- **Employee**: An HR entity used as a project team member or project manager.
- **Customer**: An Enterprise Operations entity linked to a project as the client.
- **Asset**: An asset management entity that can be allocated to a project.
- **Branch**: A Foundation entity representing an organizational location.
- **Department**: A Foundation entity representing a functional unit within an organization.
- **Project_Dashboard**: The landing page workspace view providing executive summary, KPIs, and quick actions.
- **Timeline_Event**: A chronological record of project lifecycle changes, team changes, asset operations, and document operations.
- **Activity_Record**: A detailed history entry tracking user actions on project entities.
- **Project_Budget**: A lightweight project-level budget structure tracking estimated, approved, actual, and remaining costs.
- **Risk**: A project risk entry with probability, impact, severity, owner, and mitigation plan.
- **Risk_Category**: The classification of a risk (Technical, Financial, Operational, External, Regulatory, Safety, Resource).
- **Risk_Probability**: The likelihood of a risk occurring (Low, Medium, High).
- **Risk_Impact**: The severity of a risk if it occurs (Low, Medium, High).
- **Risk_Severity**: A computed level based on probability and impact matrix (Low, Medium, High, Critical).
- **Risk_Status**: The lifecycle state of a risk (Open, InProgress, Mitigated, Closed).
- **Project_Report**: A document generated from project data, exportable in multiple formats.
- **Notification**: A user-targeted message triggered by project events.
- **Notification_Priority**: The importance level of a notification (Info, Warning, Critical).
- **Global_Search**: A full-text search capability spanning all project entities.
- **Saved_Filter**: A user-saved search query with filter combinations for reuse.
- **Extension_Point**: A reserved design structure enabling future modules without breaking changes.
- **Permission_Matrix**: A complete matrix of permission codes defining authorization boundaries for all module features.
- **Validation_Rule**: A business constraint or format enforcement rule applied to data submission.
- **Audit_Event**: A logged record of a significant business operation, captured for compliance and history.
- **Loading_State**: A UI state displaying a loading indicator while asynchronous data is being fetched.
- **Empty_State**: A UI state displayed when no data exists for a view, with guidance or call-to-action.
- **Error_State**: A UI state displayed when an operation fails, with recovery actions.
- **Confirmation_Dialog**: A modal dialog requiring user confirmation before proceeding with a destructive or significant action.
- **Autosave_Indicator**: A visual element showing the status of background save operations (Saving, Saved, Unsaved Changes).
- **Responsive_Layout**: A UI layout that adapts to mobile, tablet, and desktop screen sizes.
- **WCAG**: Web Content Accessibility Guidelines 2.1 Level AA — accessibility compliance standard.
- **Pagination**: A technique for dividing large datasets into pages to improve performance and user experience.
- **Lazy_Loading**: A technique for deferring data loading until it is needed.
- **Server_Side_Operation**: Data processing (search, filter, sort) performed on the backend rather than the frontend.
- **Concurrency_Conflict**: A condition where two users attempt to modify the same entity simultaneously.
- **Permission_Inheritance**: Behavior where permissions acquired via Designation or Role apply to all operations in a module.
- **Multi_Tenant_Isolation**: Enforcement of organization-level data boundaries to prevent cross-tenant access.
- **Integration_Point**: A defined interface where the Project Workspace module interacts with other NexAsset modules.
- **API_Convention**: A standardized pattern for API endpoint design, response format, and error handling.
- **UI_Component_Pattern**: A reusable UI component design following existing NexAsset design system.
- **Module_Acceptance_Criteria**: The complete set of verification criteria that must pass for the module to be considered production-ready.

---

## Requirements

### Requirement 1: Project Category Management

**User Story:** As an organization administrator, I want to create and manage project categories, so that projects can be classified according to our business domain.

#### Acceptance Criteria

1. THE Project_Workspace SHALL provide a searchable list of suggested category names when a user begins creating a new Project_Category, including suggestions: Solar EPC, Solar O&M, Commercial Solar, Residential Solar, Electrical Contractor, Electrical Testing, Transmission Line, Distribution Line, Underground Cable, HT/LT Panel, Civil Construction, Mechanical Works, Manufacturing, Infrastructure, Facility Management, Water Treatment, Industrial Automation.
2. WHEN a user submits a new Project_Category with a name that matches a suggestion exactly (case-insensitive), THE Project_Workspace SHALL accept the submission without error.
3. WHEN a user submits a new Project_Category with a custom name not present in the suggestion list, THE Project_Workspace SHALL accept the submission, allowing unlimited custom categories per organization.
4. THE Project_Category SHALL contain: Name, Description, Status (Active/Inactive), CreatedAtUtc, UpdatedAtUtc, IsDeleted, DeletedAtUtc.
5. WHEN a Project_Category is created, THE Project_Workspace SHALL enforce that Name is unique within the same Organization.
6. IF a Project_Category creation request contains a duplicate Name within the same Organization, THEN THE Project_Workspace SHALL return a descriptive validation error identifying the conflict.
7. THE Project_Workspace SHALL support searching Project_Categories by Name.
8. THE Project_Workspace SHALL support sorting Project_Categories by Name and CreatedAtUtc.
9. THE Project_Workspace SHALL support filtering Project_Categories by Status.
10. THE Project_Workspace SHALL return Project_Category list results using PagedRequest and PagedResponse.
11. WHEN a Project_Category is soft-deleted, THE Project_Workspace SHALL set IsDeleted to true and record DeletedAtUtc, retaining the record for audit purposes.
12. IF a Project_Category with IsDeleted equal to true is requested in list endpoints, THEN THE Project_Workspace SHALL exclude it from results unless the request explicitly includes deleted items.
13. WHERE a Project_Category has an Inactive Status, THE Project_Workspace SHALL prevent it from being selected when creating or editing a Project.

---

### Requirement 2: Project Lifecycle Management

**User Story:** As a project manager, I want to create and manage projects through their full lifecycle, so that my organization has a single source of truth for all active and historical project work.

#### Acceptance Criteria

1. THE Project SHALL contain: ProjectCode, ProjectName, Description, CustomerId, CategoryId, BranchId, DepartmentId, ProjectManagerEmployeeId, Priority, Status, StartDate, EndDate, ExpectedCompletionDate, Notes, InternalRemarks, CreatedAtUtc, UpdatedAtUtc, IsDeleted, DeletedAtUtc.
2. WHEN a Project is created within an Organization, THE Project_Workspace SHALL enforce that ProjectCode is unique within the same Organization.
3. IF a Project creation or edit request contains a duplicate ProjectCode within the same Organization, THEN THE Project_Workspace SHALL return a descriptive validation error.
4. THE Project_Workspace SHALL support the following status transitions only:
   - Draft → Planning
   - Planning → AwaitingApproval
   - Planning → Cancelled
   - AwaitingApproval → Approved
   - AwaitingApproval → Planning (rejection returns to planning)
   - Approved → InProgress
   - Approved → Cancelled
   - InProgress → OnHold
   - InProgress → Completed
   - OnHold → InProgress
   - OnHold → Cancelled
   - Completed → Archived
   - Archived → InProgress (restore)
   - Any non-terminal status → Cancelled
5. IF a requested status transition is not in the allowed set defined in Acceptance Criterion 4, THEN THE Project_Workspace SHALL return a descriptive error indicating the invalid transition.
6. THE Project_Workspace SHALL support creating a Project (Create operation).
7. THE Project_Workspace SHALL support editing a Project's non-identity fields (Edit operation).
8. WHEN a Project is archived, THE Project_Workspace SHALL set Status to Archived and record the transition in the audit log.
9. WHEN an archived Project is restored, THE Project_Workspace SHALL set Status to InProgress and record the transition in the audit log.
10. WHEN a Project is duplicated, THE Project_Workspace SHALL create a new Project in Draft status, copying ProjectName, Description, CategoryId, BranchId, DepartmentId, Priority, and ProjectParameterSections and their Parameters, but assigning a new system-generated ProjectCode and leaving CustomerId, ProjectManagerEmployeeId, StartDate, EndDate blank.
11. WHEN a Project is soft-deleted, THE Project_Workspace SHALL set IsDeleted to true and record DeletedAtUtc.
11. WHEN a Project entity is created or modified, THE Project_Workspace SHALL record an entry in the AuditLog including the entity type, entity ID, action performed, and the UserId of the actor.
12. IF audit logging fails during a project operation, THEN THE Project_Workspace SHALL allow the project operation to complete, log the audit failure through the application logging mechanism, and notify administrators of the audit logging failure.
13. THE Project_Workspace SHALL support searching Projects by ProjectCode and ProjectName.
14. THE Project_Workspace SHALL support filtering Projects by Status, Priority, CategoryId, BranchId, DepartmentId, and ProjectManagerEmployeeId.
15. THE Project_Workspace SHALL support sorting Projects by ProjectName, StartDate, EndDate, and CreatedAtUtc.
16. THE Project_Workspace SHALL return Project list results using PagedRequest and PagedResponse.

---

### Requirement 3: Project Creation Wizard with Autosave

**User Story:** As a user, I want to create a project through a guided multi-step wizard with automatic progress saving, so that I never lose entered information if I navigate away.

#### Acceptance Criteria

1. THE Project_Wizard SHALL present project creation in exactly seven sequential steps: (1) General Information, (2) Project Team, (3) Asset Allocation, (4) Project Parameters, (5) Documents, (6) Review, (7) Save.
2. WHEN a user begins a new project creation wizard, THE Project_Workspace SHALL create a Draft_Session record associated with the authenticated user and organization.
3. WHEN a user pauses editing any step of the Project_Wizard for 5 seconds without further input, THE Project_Workspace SHALL automatically persist the current wizard state to the Draft_Session.
4. WHEN a user resumes editing the Project_Wizard after a pause, THE Project_Workspace SHALL restart the autosave timer.
4. WHEN a user pauses editing any step of the Project_Wizard for 5 seconds without further input, THE Project_Workspace SHALL restart the autosave timer.
5. WHEN autosave is in progress, THE Project_Wizard SHALL display a "Saving..." indicator.
5. WHEN autosave completes successfully, THE Project_Wizard SHALL display a "Saved" indicator with the timestamp of the last successful save.
6. WHEN a user has unsaved local changes not yet persisted to the Draft_Session, THE Project_Wizard SHALL display an "Unsaved Changes" indicator.
7. WHEN a user navigates away from the Project_Wizard before completing Step 7, THE Project_Workspace SHALL retain the Draft_Session with all entered data.
8. WHEN a user returns to the Project_Wizard after having navigated away, THE Project_Workspace SHALL automatically restore all previously entered wizard state from the Draft_Session.
9. IF a Draft_Session exists for a user when the user initiates a new project creation, THEN THE Project_Workspace SHALL prompt the user to resume the existing draft or start a new one.
10. WHEN a user completes Step 7 (Save), THE Project_Workspace SHALL persist the Project and all associated data, then delete the Draft_Session.
11. THE Project_Wizard SHALL allow the user to navigate backward to any previously completed step without losing data entered in subsequent steps.

---

### Requirement 4: Dynamic Project Parameters

**User Story:** As an organization administrator, I want to define custom project parameters organized into sections, so that our projects capture the technical information relevant to our industry without being constrained by hardcoded fields.

#### Acceptance Criteria

1. THE Project_Workspace SHALL allow users to define Project_Parameter_Sections on a per-project basis, with each section having a Name.
2. WHEN a user creates a Project_Parameter_Section, THE Project_Workspace SHALL add it as a collapsible section within Wizard Step 4.
3. THE Project_Workspace SHALL allow users to rename, delete, and reorder Project_Parameter_Sections within a project.
4. WHEN a user adds a Project_Parameter to a section, THE Project_Workspace SHALL record: ParameterName, InputType, Unit, IsRequired, DisplayOrder.
5. THE Project_Parameter InputType SHALL be one of: Text, Textarea, Number, Decimal, Date, Dropdown, Checkbox, Email, Phone, URL.
6. IF a Project_Parameter has IsRequired equal to true and no value is provided at project save time, THEN THE Project_Workspace SHALL return a validation error identifying the missing required parameter by name.
7. THE Project_Workspace SHALL allow users to edit, delete, and reorder Project_Parameters within a section.
8. WHEN a Project_Parameter of type Dropdown is defined, THE Project_Workspace SHALL allow the user to specify the list of selectable option values for that parameter.
9. WHEN a project is saved, THE Project_Workspace SHALL persist all Project_Parameter_Values as key-value records linked to the project and the corresponding Project_Parameter.
10. WHEN a Project is duplicated, THE Project_Workspace SHALL copy all Project_Parameter_Sections and Project_Parameters to the new project, but SHALL NOT copy the Project_Parameter_Values.

---

### Requirement 5: Project Document Management

**User Story:** As a project manager, I want to upload and manage documents associated with a project, so that all project-related files are organized and accessible in one place.

#### Acceptance Criteria

1. THE Project_Document SHALL contain: DocumentName, Category, Description, FileReference, UploadedByEmployeeId, UploadedAtUtc, Version, Remarks, IsDeleted, DeletedAtUtc.
2. THE Project_Document Category SHALL be one of: Agreement, Contract, Survey, BOQ, Technical Drawing, Government Approval, Inspection Report, Completion Certificate, Invoice, Purchase Document, Photo, Video, Other.
3. WHEN a user uploads a document to a project, THE Project_Workspace SHALL create a Project_Document record with Version initialized to 1.
4. WHEN a user replaces an existing document, THE Project_Workspace SHALL create a new Project_Document record with an incremented Version number, retaining all previous versions in the document history.
5. THE Project_Workspace SHALL allow users to download a Project_Document by providing the stored file reference.
6. THE Project_Workspace SHALL allow users to preview supported file types (PDF, images) directly within the application.
7. WHEN a Project_Document is deleted, THE Project_Workspace SHALL perform a soft delete by setting IsDeleted to true and recording DeletedAtUtc.
8. THE Project_Workspace SHALL provide a complete version history for each Project_Document, listing all versions in descending version number order.
9. THE Project_Workspace SHALL support filtering Project_Documents by Category.
10. THE Project_Workspace SHALL support searching Project_Documents by DocumentName.
11. THE Project_Workspace SHALL display UploadedAtUtc as a formatted timestamp for each document entry.
12. WHILE the Project_Document management feature is active, THE Project_Workspace SHALL resolve and display UploadedByEmployeeId as the employee's display name for each document entry.

---

### Requirement 6: Project Team Management

**User Story:** As a project manager, I want to add employees to my project team with defined roles and allocation details, so that staffing responsibility is tracked within the project.

#### Acceptance Criteria

1. THE Project_Team_Member SHALL contain: ProjectId, EmployeeId, ProjectRole, AllocationPercentage, JoinedDate, ReleasedDate, Status (Active/Released), Remarks.
2. WHEN a user adds a team member to a project, THE Project_Workspace SHALL validate that the EmployeeId belongs to the same Organization as the Project.
3. IF an EmployeeId is submitted for a project that already has an Active team membership for the same employee, THEN THE Project_Workspace SHALL return an error preventing duplicate active membership.
4. THE Project_Workspace SHALL require AllocationPercentage to be between 1% and 100% inclusive for all active team members.
5. WHEN a team member is released from a project, THE Project_Workspace SHALL set Status to Released and record ReleasedDate.
6. WHEN a team member has Status equal to Released, THE Project_Workspace SHALL allow editing of Remarks only, and SHALL prevent editing of ProjectRole, AllocationPercentage, JoinedDate, and ReleasedDate.
7. IF a released team member must be reassigned to the same project, THEN THE Project_Workspace SHALL create a new Project_Team_Member record rather than modifying the existing released record.
8. THE Project_Workspace SHALL allow users to edit the ProjectRole, AllocationPercentage, and Remarks of an existing team member with Status equal to Active.
6. THE Project_Workspace SHALL maintain a complete allocation history for each employee on a project, including all previous memberships with their joined and released dates.
7. THE Project_Workspace SHALL allow filtering project team members by Status.
8. WHEN a team member record is removed, THE Project_Workspace SHALL perform a soft delete, retaining the record for allocation history purposes.

---

### Requirement 7: Project Asset Allocation

**User Story:** As a project manager, I want to allocate organization assets to my project, so that asset utilization is tracked and assets are associated with the project throughout its lifecycle.

#### Acceptance Criteria

1. THE Project_Asset_Allocation SHALL contain: ProjectId, AssetId, AllocationDate, ReturnDate, AllocatedQuantity, ReturnedQuantity, Status (Active/Returned/PartiallyReturned), Remarks.
2. WHEN a user selects an asset to allocate, THE Project_Workspace SHALL display the asset's current availability status and, where applicable, the available quantity before confirming allocation.
3. IF the available quantity for an asset is zero, THEN THE Project_Workspace SHALL prevent allocation and return a descriptive error indicating zero availability.
4. WHEN an asset is allocated to a project, THE Project_Workspace SHALL create a Project_Asset_Allocation record and record the AllocationDate and AllocatedQuantity.
4. WHEN an asset is allocated to a project, THE Project_Workspace SHALL create a Project_Asset_Allocation record and record the AllocationDate and AllocatedQuantity.
5. IF an asset with AssetStatus not equal to Available is submitted for project allocation, THEN THE Project_Workspace SHALL return a descriptive error indicating the asset is not available.
5. WHEN a Project_Asset_Allocation is created, THE Project_Workspace SHALL update the Asset record to reflect the active project association, including ProjectId, AllocationDate, and Status.
6. THE Project_Workspace SHALL display on the Asset detail view: associated ProjectName, AllocationDate, ReturnDate, AllocatedQuantity, ReturnedQuantity, Status, and Remarks for all active and historical project allocations.
7. WHEN an asset is returned from a project, THE Project_Workspace SHALL update the Project_Asset_Allocation Status to Returned or PartiallyReturned based on ReturnedQuantity relative to AllocatedQuantity, and record ReturnDate.
8. THE Project_Workspace SHALL maintain a complete allocation history for each asset across all projects.
9. THE Project_Workspace SHALL allow filtering project asset allocations by Status.

---

### Requirement 8: Project Workspace Summary View

**User Story:** As a project manager, I want a single workspace view for each project that surfaces key information across all sub-modules, so that I can monitor project health without navigating multiple screens.

#### Acceptance Criteria

1. THE Project_Workspace SHALL provide a project detail workspace view that consolidates: General Information, team member count and active allocations, asset allocation count, document count by category, and dynamic parameter sections with their values.
2. WHEN a user accesses the project workspace view, THE Project_Workspace SHALL display the current Status prominently with a visual indicator corresponding to the lifecycle stage.
3. THE Project_Workspace SHALL display the complete audit trail for a project, listing each status transition with the actor's name and timestamp.
4. THE Project_Workspace SHALL display the Project Priority with a visual distinction between Low, Medium, High, and Critical levels.

---

### Requirement 9: Permission and Access Control

**User Story:** As an organization administrator, I want project data to be protected by the existing NexAsset permission model, so that only authorized users can create, edit, or delete projects and their related data.

#### Acceptance Criteria

1. THE Project_Workspace SHALL enforce organization-level data isolation using the existing multi-tenant query filter, ensuring users can only access Projects, Project_Categories, Project_Team_Members, Project_Asset_Allocations, and Project_Documents belonging to their Organization.
2. THE Project_Workspace SHALL register the following permissions in the permission catalog: Projects.View, Projects.Create, Projects.Update, Projects.Delete, Projects.Approve, ProjectCategories.View, ProjectCategories.Create, ProjectCategories.Update, ProjectCategories.Delete.
3. WHEN a user without Projects.Create permission attempts to create a project, THE Project_Workspace SHALL return a 403 response.
4. WHEN a user without Projects.Approve permission attempts to transition a project to Approved status, THE Project_Workspace SHALL return a 403 response.
5. THE Project_Workspace SHALL integrate with the existing PermissionRouteConvention so that standard HTTP method-to-permission mappings apply to all project endpoints.
6. IF a permission check fails or cannot be completed, THEN THE Project_Workspace SHALL block the operation and return an authorization error response.

---

### Requirement 10: Data Integrity and Validation

**User Story:** As a system administrator, I want all project data to be validated at the boundary, so that the database never contains inconsistent or malformed records.

#### Acceptance Criteria

1. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate: ProjectCode is not empty and does not exceed 50 characters, ProjectName is not empty and does not exceed 200 characters, StartDate is not later than EndDate when both are provided, ProjectManagerEmployeeId belongs to the same Organization.
2. IF StartDate is provided and is later than EndDate, THEN THE Project_Workspace SHALL return a validation error identifying the date conflict, regardless of whether other validation rules pass or fail.
3. WHEN a Project_Category creation or update request is received, THE Project_Workspace SHALL validate: Name is not empty and does not exceed 100 characters.
4. WHEN a Project_Team_Member addition request is received, THE Project_Workspace SHALL validate: AllocationPercentage is between 1 and 100 inclusive, JoinedDate is not later than ReleasedDate when both are provided.
5. WHEN a Project_Asset_Allocation creation request is received, THE Project_Workspace SHALL validate: AllocatedQuantity is greater than 0, AllocationDate is provided.
6. IF a Project_Asset_Allocation creation request contains AllocatedQuantity equal to 0, THEN THE Project_Workspace SHALL return a validation error preventing the allocation.
6. IF a Project_Asset_Allocation creation request contains AllocatedQuantity equal to 0, THEN THE Project_Workspace SHALL return a validation error preventing the allocation.
7. WHEN a Project_Document upload request is received, THE Project_Workspace SHALL validate: DocumentName is not empty and does not exceed 200 characters, Category is one of the defined Document_Category values.
8. IF a Project_Parameter of type Number or Decimal receives a non-numeric value, THEN THE Project_Workspace SHALL return a validation error identifying the parameter name and the expected type.
9. IF a Project_Parameter of type Email receives a value that does not conform to email format, THEN THE Project_Workspace SHALL return a validation error identifying the parameter name.
10. THE Project_Workspace SHALL return all validation errors using the existing FluentValidation error shape: `{ "Errors": [{ "PropertyName", "ErrorMessage" }] }`.
11. IF a request contains multiple validation errors, THEN THE Project_Workspace SHALL return all detected errors in a single validation response using the FluentValidation format.
12. IF a request fails validation, THEN THE Project_Workspace SHALL NOT process or persist the request.

---

### Requirement 11: Project Dashboard

**User Story:** As a project manager, I want a comprehensive executive dashboard as the landing page for each project, so that I can quickly assess project health without navigating multiple sections.

#### Acceptance Criteria

1. THE Project_Dashboard SHALL be the default landing page WHEN a user opens a project workspace.
2. THE Project_Dashboard SHALL display Overview Cards containing: Status, Priority, Health, Progress.
3. THE Project_Dashboard SHALL display a Team Summary containing: total member count, active allocation count.
4. THE Project_Dashboard SHALL display an Asset Summary containing: total allocated count, breakdown by asset status.
5. THE Project_Dashboard SHALL display a Document Summary containing: total document count, breakdown by category.
6. THE Project_Dashboard SHALL display a Risk Summary containing: open risk count, breakdown by severity.
7. THE Project_Dashboard SHALL display a Budget Summary containing: EstimatedBudget, ApprovedBudget, ActualCost, RemainingBudget, BudgetPercentageUsed.
8. THE Project_Dashboard SHALL display the 10 most recent Activity_Records in a Recent Activities widget.
9. THE Project_Dashboard SHALL display Upcoming Deadlines if ExpectedCompletionDate is within 30 days.
10. THE Project_Dashboard SHALL display a Pending Approvals count if the project Status is AwaitingApproval.
11. THE Project_Dashboard SHALL display Project Statistics including: days elapsed since StartDate, days remaining until ExpectedCompletionDate.
12. THE Project_Dashboard SHALL provide Quick Actions including: Edit Project, Add Team Member, Allocate Asset, Upload Document.
13. THE Project_Dashboard SHALL display Recent Changes listing the 5 most recent modifications to project entities.
14. THE Project_Dashboard SHALL support adding future widget components without requiring redesign of the layout structure.

---

### Requirement 12: Project Timeline

**User Story:** As a project manager, I want a complete chronological timeline of all project events, so that I can audit the full history of the project lifecycle.

#### Acceptance Criteria

1. THE Project_Workspace SHALL maintain a Project Timeline containing all Timeline_Events in chronological order.
2. THE Timeline_Event SHALL contain: EventType, EntityType, EntityId, Description, Timestamp, UserId, IconType.
3. THE Project_Timeline SHALL record a Timeline_Event WHEN a project is created, with EventType equal to ProjectCreated.
4. THE Project_Timeline SHALL record a Timeline_Event WHEN a project Status transitions to Planning, with EventType equal to PlanningStarted.
5. THE Project_Timeline SHALL record a Timeline_Event WHEN a project Status transitions to AwaitingApproval, with EventType equal to ApprovalRequested.
6. THE Project_Timeline SHALL record a Timeline_Event WHEN a project Status transitions to Approved, with EventType equal to Approved.
7. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Team_Member is added, with EventType equal to TeamMemberAdded.
8. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Team_Member is released, with EventType equal to TeamMemberReleased.
9. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Asset_Allocation is created, with EventType equal to AssetAllocated.
10. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Asset_Allocation Status is set to Returned, with EventType equal to AssetReturned.
11. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Document is uploaded, with EventType equal to DocumentUploaded.
12. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Document version is incremented, with EventType equal to DocumentReplaced.
13. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Document is soft-deleted, with EventType equal to DocumentDeleted.
14. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Budget field is updated, with EventType equal to BudgetUpdated.
15. THE Project_Timeline SHALL record a Timeline_Event WHEN a Risk is created, with EventType equal to RiskAdded.
16. THE Project_Timeline SHALL record a Timeline_Event WHEN a Risk Status transitions to Closed, with EventType equal to RiskClosed.
17. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Parameter_Value is created, with EventType equal to ParameterCreated.
18. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Parameter_Value is modified, with EventType equal to ParameterUpdated.
19. THE Project_Timeline SHALL record a Timeline_Event WHEN a Project_Parameter is deleted, with EventType equal to ParameterDeleted.
20. THE Project_Timeline SHALL record a Timeline_Event WHEN a project Status transitions to Completed, with EventType equal to ProjectCompleted.
21. THE Project_Timeline SHALL record a Timeline_Event WHEN a project Status transitions to Archived, with EventType equal to ProjectArchived.
22. THE Project_Workspace SHALL display Timeline_Events with a visual icon corresponding to EventType.
23. THE Project_Workspace SHALL support grouping Timeline_Events by month.
24. THE Project_Workspace SHALL support filtering Timeline_Events by EventType.
25. THE Project_Workspace SHALL support searching Timeline_Events by keyword within Description.
26. THE Project_Workspace SHALL display Timeline_Events in descending chronological order by default.
27. THE Project_Workspace SHALL support expanding and collapsing Timeline_Event details.
28. THE Project_Timeline SHALL reserve EventType values for future integration with Task and Milestone modules without requiring schema changes.


---

### Requirement 13: Activity Feed

**User Story:** As a project manager, I want a detailed activity history similar to GitHub's activity feed, so that every business action on the project is tracked and auditable.

#### Acceptance Criteria

1. THE Project_Workspace SHALL maintain an Activity Feed containing all Activity_Records for the project.
2. THE Activity_Record SHALL contain: ActivityType, UserId, Action, TargetEntity, TargetEntityId, Timestamp, Remarks.
3. THE Project_Workspace SHALL create an Activity_Record WHEN a project is created, with ActivityType equal to ProjectCreated.
4. THE Project_Workspace SHALL create an Activity_Record WHEN a project's non-identity fields are updated, with ActivityType equal to ProjectUpdated.
5. THE Project_Workspace SHALL create an Activity_Record WHEN a project Status is changed, with ActivityType equal to StatusChanged.
6. THE Project_Workspace SHALL create an Activity_Record WHEN a project Priority is changed, with ActivityType equal to PriorityChanged.
7. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Team_Member is added, with ActivityType equal to EmployeeAdded.
8. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Team_Member is released, with ActivityType equal to EmployeeReleased.
9. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Asset_Allocation is created, with ActivityType equal to AssetAllocated.
10. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Asset_Allocation Status transitions to Returned or PartiallyReturned, with ActivityType equal to AssetReturned.
11. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Document is uploaded, with ActivityType equal to DocumentUploaded.
12. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Document version is incremented, with ActivityType equal to DocumentReplaced.
13. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Document is soft-deleted, with ActivityType equal to DocumentDeleted.
14. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Parameter is created, with ActivityType equal to ParameterCreated.
15. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Parameter_Value is updated, with ActivityType equal to ParameterUpdated.
16. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Parameter is deleted, with ActivityType equal to ParameterDeleted.
17. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Parameter_Section is created, with ActivityType equal to SectionCreated.
18. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Parameter_Section is deleted, with ActivityType equal to SectionDeleted.
19. THE Project_Workspace SHALL create an Activity_Record WHEN a Project_Budget field is updated, with ActivityType equal to BudgetUpdated.
20. THE Project_Workspace SHALL create an Activity_Record WHEN a Risk is created, with ActivityType equal to RiskAdded.
21. THE Project_Workspace SHALL create an Activity_Record WHEN a Risk's non-identity fields are updated, with ActivityType equal to RiskUpdated.
22. THE Project_Workspace SHALL create an Activity_Record WHEN a Risk Status transitions to Closed, with ActivityType equal to RiskClosed.
23. THE Activity_Record SHALL resolve and display UserId as the user's display name in the Activity Feed view.
24. THE Activity_Record SHALL display the Action as a human-readable past-tense verb phrase.
25. THE Activity_Record SHALL display TargetEntity and TargetEntityId as a clickable link to the entity detail view WHERE applicable.
26. THE Project_Workspace SHALL support filtering Activity_Records by ActivityType.
27. THE Project_Workspace SHALL support filtering Activity_Records by UserId.
28. THE Project_Workspace SHALL support filtering Activity_Records by date range.
29. THE Project_Workspace SHALL support searching Activity_Records by keyword within Action and Remarks fields.
30. THE Project_Workspace SHALL support pagination of Activity_Records using PagedRequest and PagedResponse.
31. THE Project_Workspace SHALL reserve ActivityType values for future notification integration without requiring schema changes.

---

### Requirement 14: Project Budget

**User Story:** As a project manager, I want to track project-level budget information, so that I can monitor estimated, approved, and actual costs without requiring the full Finance module.

#### Acceptance Criteria

1. THE Project_Budget SHALL contain: EstimatedBudget, ApprovedBudget, ActualCost, ProcurementCost, MaintenanceCost, LabourCost, MiscellaneousCost, CreatedAtUtc, UpdatedAtUtc.
2. THE Project_Budget SHALL compute RemainingBudget as ApprovedBudget minus ActualCost.
3. THE Project_Budget SHALL compute BudgetPercentageUsed as (ActualCost divided by ApprovedBudget) multiplied by 100, WHERE ApprovedBudget is greater than zero.
4. IF ApprovedBudget is zero, THEN THE Project_Budget SHALL display BudgetPercentageUsed as zero.
5. THE Project_Budget SHALL derive BudgetStatus as Under Budget WHEN ActualCost is less than ApprovedBudget.
6. THE Project_Budget SHALL derive BudgetStatus as On Budget WHEN ActualCost is equal to ApprovedBudget.
7. THE Project_Budget SHALL derive BudgetStatus as Over Budget WHEN ActualCost is greater than ApprovedBudget.
8. THE Project_Workspace SHALL validate that EstimatedBudget, ApprovedBudget, ActualCost, ProcurementCost, MaintenanceCost, LabourCost, and MiscellaneousCost are all greater than or equal to zero.
9. IF a budget update request contains a negative value for any cost field, THEN THE Project_Workspace SHALL return a validation error identifying the invalid field.
10. THE Project_Workspace SHALL maintain a complete budget history by storing each budget modification as a separate versioned record linked to the project.
11. WHEN a Project_Budget field is updated, THE Project_Workspace SHALL create an Activity_Record with ActivityType equal to BudgetUpdated.
12. WHEN a Project_Budget field is updated, THE Project_Workspace SHALL create a Timeline_Event with EventType equal to BudgetUpdated.
13. THE Project_Workspace SHALL support retrieving the complete budget history for a project in descending order by UpdatedAtUtc.
14. THE Project_Budget SHALL use decimal(18,2) for all monetary fields, consistent with the NexAsset money field standard.
15. THE Project_Workspace SHALL reserve extension points in the Project_Budget structure for future integration with the Finance module without requiring schema migration.


---

### Requirement 15: Risk Register

**User Story:** As a project manager, I want to maintain a risk register for my project, so that I can identify, assess, and mitigate risks throughout the project lifecycle.

#### Acceptance Criteria

1. THE Risk SHALL contain: ProjectId, Title, Description, Category, Probability, Impact, OwnerEmployeeId, MitigationPlan, Status, CreatedAtUtc, ClosedAtUtc, Remarks, IsDeleted, DeletedAtUtc.
2. THE Risk Category SHALL be one of: Technical, Financial, Operational, External, Regulatory, Safety, Resource.
3. THE Risk Probability SHALL be one of: Low, Medium, High.
4. THE Risk Impact SHALL be one of: Low, Medium, High.
5. THE Risk Severity SHALL be computed using a probability-impact matrix as follows: (Probability=Low AND Impact=Low) yields Severity=Low, (Probability=Low AND Impact=Medium) yields Severity=Low, (Probability=Low AND Impact=High) yields Severity=Medium, (Probability=Medium AND Impact=Low) yields Severity=Low, (Probability=Medium AND Impact=Medium) yields Severity=Medium, (Probability=Medium AND Impact=High) yields Severity=High, (Probability=High AND Impact=Low) yields Severity=Medium, (Probability=High AND Impact=Medium) yields Severity=High, (Probability=High AND Impact=High) yields Severity=Critical.
6. THE Risk Status SHALL be one of: Open, InProgress, Mitigated, Closed.
7. WHEN a user creates a Risk, THE Project_Workspace SHALL validate that Title is not empty and does not exceed 200 characters.
8. WHEN a user creates a Risk, THE Project_Workspace SHALL validate that OwnerEmployeeId belongs to the same Organization as the Project.
9. WHEN a user creates a Risk, THE Project_Workspace SHALL set Status to Open and record CreatedAtUtc.
10. WHEN a Risk Status is transitioned to Closed, THE Project_Workspace SHALL record ClosedAtUtc.
11. WHEN a Risk Status is transitioned from Closed to Open, THE Project_Workspace SHALL set ClosedAtUtc to null.
12. THE Project_Workspace SHALL allow users to edit a Risk's Title, Description, Category, Probability, Impact, OwnerEmployeeId, MitigationPlan, Status, and Remarks.
13. WHEN a Risk's Probability or Impact is modified, THE Project_Workspace SHALL automatically recompute Severity using the matrix defined in Acceptance Criterion 5.
14. THE Project_Workspace SHALL support searching Risks by Title and Description.
15. THE Project_Workspace SHALL support filtering Risks by Category, Probability, Impact, Severity, Status, and OwnerEmployeeId.
16. THE Project_Workspace SHALL support sorting Risks by Severity, CreatedAtUtc, and ClosedAtUtc.
17. THE Project_Workspace SHALL display a Risk Matrix visualization showing all risks positioned by Probability and Impact axes with color coding by Severity.
18. THE Risk Matrix SHALL use color coding: Low=Green, Medium=Yellow, High=Orange, Critical=Red.
19. THE Project_Workspace SHALL maintain a complete risk modification history, recording all changes to risk fields with the actor UserId and timestamp.
20. WHEN a Risk is created, THE Project_Workspace SHALL create an Activity_Record with ActivityType equal to RiskAdded.
21. WHEN a Risk is updated, THE Project_Workspace SHALL create an Activity_Record with ActivityType equal to RiskUpdated.
22. WHEN a Risk Status transitions to Closed, THE Project_Workspace SHALL create an Activity_Record with ActivityType equal to RiskClosed.
23. WHEN a Risk is soft-deleted, THE Project_Workspace SHALL set IsDeleted to true and record DeletedAtUtc.
24. THE Project_Workspace SHALL support reopening a closed Risk by transitioning Status from Closed to Open.
25. THE Project_Workspace SHALL reserve extension points for future AI-powered risk recommendation features without requiring schema changes.

---

### Requirement 16: Reports

**User Story:** As a project manager, I want to generate comprehensive project reports in multiple formats, so that I can share project information with stakeholders.

#### Acceptance Criteria

1. THE Project_Workspace SHALL provide a Project Summary Report containing: all project General Information, Status, Priority, ProjectManagerEmployeeId resolved as name, CustomerId resolved as name, CategoryId resolved as name, BranchId resolved as name, DepartmentId resolved as name, StartDate, EndDate, ExpectedCompletionDate, Notes, InternalRemarks.
2. THE Project_Workspace SHALL provide a Team Allocation Report containing: all Project_Team_Members with EmployeeId resolved as name, ProjectRole, AllocationPercentage, JoinedDate, ReleasedDate, Status.
3. THE Project_Workspace SHALL provide an Asset Allocation Report containing: all Project_Asset_Allocations with AssetId resolved as asset code and name, AllocationDate, ReturnDate, AllocatedQuantity, ReturnedQuantity, Status, Remarks.
4. THE Project_Workspace SHALL provide a Budget Report containing: EstimatedBudget, ApprovedBudget, ActualCost, RemainingBudget, BudgetPercentageUsed, ProcurementCost, MaintenanceCost, LabourCost, MiscellaneousCost, complete budget history with timestamps.
5. THE Project_Workspace SHALL provide a Risk Report containing: all Risks with Title, Description, Category, Probability, Impact, Severity, OwnerEmployeeId resolved as name, MitigationPlan, Status, CreatedAtUtc, ClosedAtUtc, Remarks.
6. THE Project_Workspace SHALL provide a Document Register containing: all Project_Documents with DocumentName, Category, Description, Version, UploadedByEmployeeId resolved as name, UploadedAtUtc, Remarks.
7. THE Project_Workspace SHALL provide an Activity Report containing: all Activity_Records with UserId resolved as name, Action, TargetEntity, Timestamp, Remarks, filtered by user-selected ActivityType and date range.
8. THE Project_Workspace SHALL provide a Timeline Report containing: all Timeline_Events with EventType, Description, Timestamp, UserId resolved as name, filtered by user-selected EventType and date range.
9. THE Project_Workspace SHALL provide a Parameter Report containing: all Project_Parameter_Sections with their Project_Parameters and Project_Parameter_Values, organized by section and DisplayOrder.
10. THE Project_Workspace SHALL support exporting all reports in PDF format.
11. THE Project_Workspace SHALL support exporting all reports in Excel format.
12. THE Project_Workspace SHALL provide a print preview for all reports before export.
13. THE Project_Workspace SHALL support filtering report data by relevant criteria before generating the report.
14. THE Project_Workspace SHALL support sorting report data by relevant fields before generating the report.
15. THE Project_Workspace SHALL support selecting a date range for Activity Report and Timeline Report.
16. THE Project_Workspace SHALL include the Organization logo and branding on all exported reports WHERE the Organization has uploaded a logo.
17. THE Project_Workspace SHALL display the report generation timestamp on all exported reports.
18. THE Project_Workspace SHALL display the UserId who generated the report on all exported reports.


---

### Requirement 17: Notifications

**User Story:** As a project stakeholder, I want to receive notifications for important project events, so that I stay informed without manually checking the project workspace.

#### Acceptance Criteria

1. THE Project_Workspace SHALL generate a Notification WHEN a project is assigned to a user as ProjectManagerEmployeeId, with notification type ProjectAssigned.
2. THE Project_Workspace SHALL generate a Notification WHEN a project Status transitions to AwaitingApproval, addressed to users with Projects.Approve permission, with notification type ApprovalRequested.
3. THE Project_Workspace SHALL generate a Notification WHEN a project Status transitions to Approved or when AwaitingApproval transitions back to Planning, addressed to ProjectManagerEmployeeId, with notification type ApprovalCompleted.
4. THE Project_Workspace SHALL generate a Notification WHEN a Project_Team_Member is added, addressed to the EmployeeId of the added member, with notification type TeamMemberAdded.
5. THE Project_Workspace SHALL generate a Notification WHEN a Project_Team_Member Status transitions to Released, addressed to the EmployeeId of the released member, with notification type TeamMemberReleased.
6. THE Project_Workspace SHALL generate a Notification WHEN a Project_Asset_Allocation is created, addressed to ProjectManagerEmployeeId, with notification type AssetAllocated.
7. THE Project_Workspace SHALL generate a Notification WHEN a Project_Asset_Allocation Status transitions to Returned or PartiallyReturned, addressed to ProjectManagerEmployeeId, with notification type AssetReturned.
8. THE Project_Workspace SHALL generate a Notification WHEN BudgetPercentageUsed exceeds 80%, addressed to ProjectManagerEmployeeId, with notification type BudgetThresholdCrossed and priority Warning.
9. THE Project_Workspace SHALL generate a Notification WHEN BudgetPercentageUsed exceeds 100%, addressed to ProjectManagerEmployeeId, with notification type BudgetThresholdCrossed and priority Critical.
10. THE Project_Workspace SHALL generate a Notification WHEN a Risk is created, addressed to ProjectManagerEmployeeId, with notification type RiskCreated.
11. THE Project_Workspace SHALL generate a Notification WHEN a Risk Severity is computed as Critical, addressed to ProjectManagerEmployeeId and OwnerEmployeeId, with notification type RiskCritical and priority Critical.
12. THE Project_Workspace SHALL generate a Notification WHEN a Risk Status transitions to Closed, addressed to ProjectManagerEmployeeId, with notification type RiskClosed.
13. THE Project_Workspace SHALL generate a Notification WHEN a Project_Document is uploaded, addressed to ProjectManagerEmployeeId, with notification type DocumentUploaded.
14. THE Project_Workspace SHALL generate a Notification WHEN a Project_Document with an ExpiryDate field is within 30 days of expiry, addressed to ProjectManagerEmployeeId, with notification type DocumentExpiring and priority Warning.
15. THE Project_Workspace SHALL generate a Notification WHEN ExpectedCompletionDate is within 7 days of the current date, addressed to ProjectManagerEmployeeId, with notification type UpcomingDeadline and priority Warning.
16. THE Project_Workspace SHALL generate a Notification WHEN a project Status transitions to Completed, addressed to all Project_Team_Members with Status equal to Active, with notification type ProjectCompleted.
17. THE Notification SHALL contain: NotificationType, Priority, UserId, ProjectId, EntityType, EntityId, Message, IsRead, CreatedAtUtc.
18. THE Notification Priority SHALL be one of: Info, Warning, Critical.
19. WHEN a Notification is displayed to the user, THE Project_Workspace SHALL allow the user to mark it as read by setting IsRead to true.
20. THE Project_Workspace SHALL retain all Notifications in the history regardless of IsRead status.
21. THE Project_Workspace SHALL support filtering Notifications by IsRead status.
22. THE Project_Workspace SHALL support filtering Notifications by Priority.
23. THE Project_Workspace SHALL support filtering Notifications by NotificationType.
24. THE Project_Workspace SHALL display Notifications in descending order by CreatedAtUtc by default.
25. THE Project_Workspace SHALL reserve extension points for future email delivery of notifications without requiring schema changes.
26. THE Project_Workspace SHALL reserve extension points for future push notification delivery without requiring schema changes.
27. THE Project_Workspace SHALL reserve extension points for future mobile notification delivery without requiring schema changes.

---

### Requirement 18: Search & Filtering

**User Story:** As a project user, I want to search and filter across all project entities from a single search interface, so that I can quickly locate relevant information.

#### Acceptance Criteria

1. THE Project_Workspace SHALL provide a Global_Search feature accepting a keyword input.
2. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Projects by ProjectCode and ProjectName.
3. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Project_Parameter names and Project_Parameter_Value values.
4. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Project_Documents by DocumentName, Description, and Remarks.
5. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Project_Team_Members by resolving EmployeeId to employee name and searching the resolved name.
6. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Project_Asset_Allocations by resolving AssetId to asset code and asset name and searching the resolved values.
7. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Risks by Title, Description, and MitigationPlan.
8. WHEN a user enters a keyword in Global_Search, THE Project_Workspace SHALL search across Activity_Records by Action and Remarks.
9. THE Global_Search SHALL return results grouped by entity type with a count of matches per entity type.
10. THE Global_Search SHALL display search results with highlighting on matched keywords.
11. THE Global_Search SHALL support pagination of search results using PagedRequest and PagedResponse.
12. THE Project_Workspace SHALL provide Advanced Filters allowing entity-specific filtering criteria.
13. THE Advanced Filters for Projects SHALL support filtering by Status, Priority, Category, Branch, Department, ProjectManager.
14. THE Advanced Filters for Risks SHALL support filtering by Category, Probability, Impact, Severity, Status, Owner.
15. THE Advanced Filters for Activity_Records SHALL support filtering by ActivityType, UserId, date range.
16. THE Advanced Filters for Project_Documents SHALL support filtering by Category, UploadedBy, date range.
17. THE Project_Workspace SHALL allow users to save a combination of search keyword and filter criteria as a Saved_Filter with a user-defined name.
18. THE Saved_Filter SHALL contain: UserId, FilterName, EntityType, SearchKeyword, FilterCriteria as JSON, CreatedAtUtc.
19. THE Project_Workspace SHALL allow users to retrieve and apply a Saved_Filter by selecting it from a saved filters list.
20. THE Project_Workspace SHALL allow users to delete a Saved_Filter.
21. THE Project_Workspace SHALL support sorting search results by relevance score, date fields, and name fields depending on the entity type.
22. THE Project_Workspace SHALL support full-text search capabilities using database full-text search features WHERE available.
23. THE Project_Workspace SHALL provide search suggestions based on previously searched keywords for the authenticated user.


---

### Requirement 19: Project Workspace Navigation

**User Story:** As a project user, I want a consistent and intuitive navigation structure within the project workspace, so that I can efficiently access all project sections.

#### Acceptance Criteria

1. THE Project_Workspace SHALL provide a navigation structure containing the following sections in order: Overview, General Information, Team, Assets, Project Parameters, Documents, Timeline, Activity Feed, Budget, Risk Register, Reports, Settings.
2. WHEN a user opens a project workspace, THE Project_Workspace SHALL display the Overview section (Project_Dashboard) as the default landing page.
3. THE Project_Workspace navigation SHALL highlight the currently active section with a visual indicator.
4. THE Project_Workspace SHALL support deep linking to specific sections using URL fragments or path segments.
5. WHEN a user navigates to a specific section via a deep link, THE Project_Workspace SHALL display that section and update the navigation highlight accordingly.
6. THE Project_Workspace navigation SHALL display badge counts WHERE applicable, including: unread Activity_Records count on Activity Feed, open Risks count on Risk Register, pending approvals count on Overview.
7. THE Project_Workspace navigation SHALL be responsive for mobile and tablet viewports, collapsing into a hamburger menu or drawer on small screens.
8. THE Project_Workspace navigation SHALL remain consistent with existing NexAsset UI patterns used in Foundation, HR, Assets, and Enterprise Operations modules.
9. THE Project_Workspace SHALL use either sidebar navigation or tab navigation based on the existing NexAsset design system.
10. THE Project_Workspace navigation SHALL support keyboard navigation using Tab, Enter, and arrow keys for accessibility compliance.
11. THE Project_Workspace navigation SHALL provide breadcrumb navigation showing: Organization → Projects → ProjectName → Section.
12. THE Project_Workspace navigation SHALL allow users to return to the project list view by clicking the Projects breadcrumb.
13. THE Project_Workspace navigation SHALL reserve space for future sections including Tasks, Milestones, and Gantt Chart without requiring navigation redesign.

---

### Requirement 20: Future Extensibility

**User Story:** As a system architect, I want the Project Workspace design to accommodate future modules and features, so that enhancements can be added without breaking existing functionality or requiring major refactoring.

#### Acceptance Criteria

1. THE Project_Workspace entity schema SHALL use extensible structures that can accommodate additional fields and relationships from future modules without requiring migration of existing project data.
2. THE Project_Workspace SHALL avoid hardcoded limits on the number of Project_Parameter_Sections, Project_Parameters, Project_Team_Members, Project_Asset_Allocations, Project_Documents, Risks, Timeline_Events, and Activity_Records per project.
3. THE Project_Workspace API endpoints SHALL be versioned using URL path versioning (e.g., /api/v1/projects) to support backward compatibility when future API changes are introduced.
4. THE Project_Workspace SHALL reserve extension points in Timeline_Event EventType for future event types including: TaskCreated, TaskCompleted, MilestoneReached, GanttUpdated, IssueOpened, IssueClosed, MeetingScheduled, ProgressReportSubmitted, RFIDScanRecorded, IoTDataReceived, AIRecommendationGenerated.
5. THE Project_Workspace SHALL reserve extension points in Activity_Record ActivityType for future activity types corresponding to the reserved Timeline_Event types.
6. THE Project_Workspace SHALL reserve extension points in Notification NotificationType for future notification types including: TaskAssigned, TaskOverdue, MilestoneApproaching, IssueAssigned, MeetingReminder, ProgressReportDue, RFIDAnomaly, IoTAlert, AIInsightAvailable.
7. THE Project_Workspace database schema SHALL allow nullable foreign key fields for future module entities without requiring NOT NULL constraints that would block future integration.
8. THE Project_Workspace SHALL support adding future API endpoints for Task Management, Milestones, Gantt Charts, Daily Progress Reports, Project Issues, Meetings, Resource Planning, RFID Integration, IoT Devices, and AI Project Assistant without breaking existing endpoint contracts.
9. THE Project_Workspace navigation structure SHALL support adding future sections without redesigning the existing navigation layout or removing current sections.
10. THE Project_Workspace SHALL maintain backward compatibility with existing data structures when future modules are added, ensuring that projects created before module additions remain fully functional.
11. THE Project_Workspace SHALL reserve extension points for future integration with external systems including: Customer Portals (external customer access to project information), Vendor Portals (external vendor access to procurement and delivery tracking), Native Mobile Applications (iOS/Android apps), Offline Synchronization (offline data access and sync), Multi-Tenant SaaS (cloud-based multi-tenant deployment).
12. THE Project_Workspace SHALL document all reserved extension points in the technical specification and API documentation to guide future feature development.
13. THE Project_Workspace SHALL ensure that all future modules integrate using the existing permission model without requiring a redesigned authorization framework.
14. THE Project_Workspace SHALL ensure that all future modules integrate using the existing multi-tenant isolation mechanism without requiring changes to the TenantContext or query filter logic.
15. THE Project_Workspace SHALL ensure that all future modules use the existing Result<T> pattern, CQRS structure, FluentValidation pipeline, and UnitOfWork transaction management without introducing conflicting architectural patterns.



### Requirement 21: Permissions & Authorization

**User Story:** As an organization administrator, I want complete authorization controls for Project Workspace operations, so that only authorized users can access and modify project data according to their assigned roles and designations.

#### Acceptance Criteria

1. THE Project_Workspace SHALL register the following permissions in the permission catalog: Projects.View, Projects.Create, Projects.Update, Projects.Delete, Projects.Archive, Projects.Restore, Projects.Duplicate, Projects.Approve, Projects.ManageTeam, Projects.ManageAssets, Projects.ManageParameters, Projects.ManageDocuments, Projects.ViewBudget, Projects.ManageBudget, Projects.ViewRisks, Projects.ManageRisks, Projects.ViewReports, Projects.ExportReports, Projects.ViewTimeline, Projects.ViewActivities, Projects.ManageSettings, ProjectCategories.View, ProjectCategories.Create, ProjectCategories.Update, ProjectCategories.Delete.
2. WHEN a user without Projects.View permission attempts to access any project list or project detail endpoint, THE Project_Workspace SHALL return a 403 response with a descriptive authorization error message.
3. WHEN a user without Projects.Create permission attempts to create a project, THE Project_Workspace SHALL return a 403 response.
4. WHEN a user without Projects.Update permission attempts to modify a project, THE Project_Workspace SHALL return a 403 response.
5. WHEN a user without Projects.Delete permission attempts to soft-delete a project, THE Project_Workspace SHALL return a 403 response.
6. WHEN a user without Projects.Archive permission attempts to transition a project to Archived status, THE Project_Workspace SHALL return a 403 response.
7. WHEN a user without Projects.Restore permission attempts to restore an archived project, THE Project_Workspace SHALL return a 403 response.
8. WHEN a user without Projects.Duplicate permission attempts to duplicate a project, THE Project_Workspace SHALL return a 403 response.
9. WHEN a user without Projects.Approve permission attempts to transition a project to Approved status, THE Project_Workspace SHALL return a 403 response.
10. WHEN a user without Projects.ManageTeam permission attempts to add, edit, or release a Project_Team_Member, THE Project_Workspace SHALL return a 403 response.
11. WHEN a user without Projects.ManageAssets permission attempts to allocate or return a Project_Asset_Allocation, THE Project_Workspace SHALL return a 403 response.
12. WHEN a user without Projects.ManageParameters permission attempts to add, edit, delete, or reorder Project_Parameters or Project_Parameter_Sections, THE Project_Workspace SHALL return a 403 response.
13. WHEN a user without Projects.ManageDocuments permission attempts to upload, replace, or delete a Project_Document, THE Project_Workspace SHALL return a 403 response.
14. WHEN a user without Projects.ViewBudget permission attempts to access project budget information, THE Project_Workspace SHALL return a 403 response.
15. WHEN a user without Projects.ManageBudget permission attempts to modify project budget fields, THE Project_Workspace SHALL return a 403 response.
16. WHEN a user without Projects.ViewRisks permission attempts to access the risk register or risk details, THE Project_Workspace SHALL return a 403 response.
17. WHEN a user without Projects.ManageRisks permission attempts to create, edit, close, or delete a Risk, THE Project_Workspace SHALL return a 403 response.
18. WHEN a user without Projects.ViewReports permission attempts to access report generation features, THE Project_Workspace SHALL return a 403 response.
19. WHEN a user without Projects.ExportReports permission attempts to export a report to PDF or Excel, THE Project_Workspace SHALL return a 403 response.
20. WHEN a user without Projects.ViewTimeline permission attempts to access the project timeline, THE Project_Workspace SHALL return a 403 response.
21. WHEN a user without Projects.ViewActivities permission attempts to access the activity feed, THE Project_Workspace SHALL return a 403 response.
22. WHEN a user without Projects.ManageSettings permission attempts to modify project-level settings, THE Project_Workspace SHALL return a 403 response.
23. WHEN a user without ProjectCategories.View permission attempts to access the project category list or category details, THE Project_Workspace SHALL return a 403 response.
24. WHEN a user without ProjectCategories.Create permission attempts to create a new project category, THE Project_Workspace SHALL return a 403 response.
25. WHEN a user without ProjectCategories.Update permission attempts to modify an existing project category, THE Project_Workspace SHALL return a 403 response.
26. WHEN a user without ProjectCategories.Delete permission attempts to soft-delete a project category, THE Project_Workspace SHALL return a 403 response.
27. IF a user's ApplicationRole contains the SuperAdmin role, THEN THE Project_Workspace SHALL bypass all permission checks and allow all operations.
28. THE Project_Workspace frontend SHALL hide UI elements corresponding to operations the user does not have permission to perform.
29. WHEN a user does not have Projects.Update permission, THE Project_Workspace frontend SHALL display project detail views in read-only mode, preventing inline editing.
30. THE Project_Workspace SHALL integrate with the existing PermissionRouteConvention so that standard HTTP method-to-permission mappings automatically apply to all project endpoints.
31. THE Project_Workspace SHALL integrate with the existing PermissionEnforcementFilter so that permission checks occur before handler execution for all protected endpoints.


---

### Requirement 22: Validation Rules

**User Story:** As a system administrator, I want comprehensive business validation rules enforced for all Project Workspace entities, so that the database remains consistent and data quality is maintained.

#### Acceptance Criteria

1. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that ProjectCode is not empty, does not exceed 50 characters, and is unique within the same Organization.
2. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that ProjectName is not empty and does not exceed 200 characters.
3. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that Description, when provided, does not exceed 1000 characters.
4. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that Notes, when provided, does not exceed 2000 characters.
5. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that InternalRemarks, when provided, does not exceed 2000 characters.
6. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that StartDate is not later than EndDate when both are provided.
7. IF StartDate is later than EndDate, THEN THE Project_Workspace SHALL return a validation error: "Start date cannot be after end date."
8. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that ProjectManagerEmployeeId, when provided, belongs to the same Organization as the Project.
9. IF ProjectManagerEmployeeId does not belong to the Organization, THEN THE Project_Workspace SHALL return a validation error: "Project manager must belong to the same organization."
10. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that CustomerId, when provided, belongs to the same Organization as the Project.
11. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that CategoryId references an existing Project_Category with Status equal to Active.
12. IF CategoryId does not reference an active category, THEN THE Project_Workspace SHALL return a validation error: "Selected category is inactive or does not exist."
13. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that BranchId, when provided, belongs to the same Organization.
14. WHEN a Project creation or update request is received, THE Project_Workspace SHALL validate that DepartmentId, when provided, belongs to the same Organization.
15. WHEN a Project_Category creation or update request is received, THE Project_Workspace SHALL validate that Name is not empty, does not exceed 100 characters, and is unique within the same Organization.
16. IF a duplicate Project_Category Name is detected within the Organization, THEN THE Project_Workspace SHALL return a validation error: "A category with this name already exists in your organization."
17. WHEN a Project_Category creation or update request is received, THE Project_Workspace SHALL validate that Description, when provided, does not exceed 500 characters.
18. WHEN a Project_Team_Member addition request is received, THE Project_Workspace SHALL validate that EmployeeId belongs to the same Organization as the Project.
19. WHEN a Project_Team_Member addition request is received, THE Project_Workspace SHALL validate that ProjectRole is not empty and does not exceed 100 characters.
20. WHEN a Project_Team_Member addition request is received, THE Project_Workspace SHALL validate that AllocationPercentage is between 1 and 100 inclusive.
21. IF AllocationPercentage is less than 1 or greater than 100, THEN THE Project_Workspace SHALL return a validation error: "Allocation percentage must be between 1% and 100%."
22. WHEN a Project_Team_Member addition request is received, THE Project_Workspace SHALL validate that JoinedDate is not later than ReleasedDate when both are provided.
23. WHEN a Project_Team_Member addition request is received, THE Project_Workspace SHALL validate that no other active Project_Team_Member record exists for the same EmployeeId on the same Project.
24. IF a duplicate active team membership is detected, THEN THE Project_Workspace SHALL return a validation error: "This employee is already an active member of the project."
25. WHEN a Project_Asset_Allocation creation request is received, THE Project_Workspace SHALL validate that AssetId belongs to the same Organization as the Project.
26. WHEN a Project_Asset_Allocation creation request is received, THE Project_Workspace SHALL validate that AllocatedQuantity is greater than 0.
27. IF AllocatedQuantity is zero or negative, THEN THE Project_Workspace SHALL return a validation error: "Allocated quantity must be greater than zero."
28. WHEN a Project_Asset_Allocation creation request is received, THE Project_Workspace SHALL validate that AllocationDate is provided and is not in the future.
29. WHEN a Project_Document upload request is received, THE Project_Workspace SHALL validate that DocumentName is not empty and does not exceed 200 characters.
30. WHEN a Project_Document upload request is received, THE Project_Workspace SHALL validate that Category is one of the defined Document_Category enumeration values.
31. WHEN a Project_Document upload request is received, THE Project_Workspace SHALL validate that Description, when provided, does not exceed 500 characters.
32. WHEN a Project_Document upload request is received, THE Project_Workspace SHALL validate that the uploaded file size does not exceed 50 MB.
33. IF the uploaded file size exceeds 50 MB, THEN THE Project_Workspace SHALL return a validation error: "File size must not exceed 50 MB."
34. WHEN a Project_Document upload request is received, THE Project_Workspace SHALL validate that the file type is one of: PDF, DOCX, XLSX, PNG, JPG, JPEG, DWG, ZIP.
35. IF the file type is not in the allowed list, THEN THE Project_Workspace SHALL return a validation error: "File type not supported. Allowed types: PDF, DOCX, XLSX, PNG, JPG, JPEG, DWG, ZIP."
36. WHEN a Project_Parameter is created or updated, THE Project_Workspace SHALL validate that ParameterName is not empty and does not exceed 100 characters.
37. WHEN a Project_Parameter is created or updated, THE Project_Workspace SHALL validate that InputType is one of the defined Parameter_Input_Type enumeration values.
38. WHEN a Project_Parameter is created or updated, THE Project_Workspace SHALL validate that DisplayOrder is a positive integer.
39. WHEN a Project_Parameter_Section is created or updated, THE Project_Workspace SHALL validate that Name is not empty and does not exceed 100 characters.
40. WHEN a Project_Parameter_Value with InputType equal to Email is submitted, THE Project_Workspace SHALL validate that the value conforms to standard email format.
41. IF an Email parameter value does not conform to email format, THEN THE Project_Workspace SHALL return a validation error: "[ParameterName] must be a valid email address."
42. WHEN a Project_Parameter_Value with InputType equal to Phone is submitted, THE Project_Workspace SHALL validate that the value contains only digits, spaces, hyphens, and parentheses, and is between 8 and 20 characters.
43. WHEN a Project_Parameter_Value with InputType equal to URL is submitted, THE Project_Workspace SHALL validate that the value conforms to valid URL format.
44. WHEN a Project_Parameter_Value with InputType equal to Number or Decimal is submitted, THE Project_Workspace SHALL validate that the value is numeric.
45. IF a Number or Decimal parameter value is not numeric, THEN THE Project_Workspace SHALL return a validation error: "[ParameterName] must be a numeric value."
46. WHEN a Project_Budget field is updated, THE Project_Workspace SHALL validate that EstimatedBudget, ApprovedBudget, ActualCost, ProcurementCost, MaintenanceCost, LabourCost, and MiscellaneousCost are all greater than or equal to zero.
47. IF any budget field contains a negative value, THEN THE Project_Workspace SHALL return a validation error: "[FieldName] must be greater than or equal to zero."
48. THE Project_Workspace SHALL enforce decimal(18,2) precision for all budget monetary fields.
49. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that Title is not empty and does not exceed 200 characters.
50. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that Description, when provided, does not exceed 1000 characters.
51. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that MitigationPlan, when provided, does not exceed 1000 characters.
52. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that OwnerEmployeeId belongs to the same Organization as the Project.
53. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that Category is one of the defined Risk_Category enumeration values.
54. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that Probability is one of the defined Risk_Probability enumeration values.
55. WHEN a Risk creation or update request is received, THE Project_Workspace SHALL validate that Impact is one of the defined Risk_Impact enumeration values.
56. THE Project_Workspace SHALL return all validation errors using the existing FluentValidation error shape: `{ "Errors": [{ "PropertyName", "ErrorMessage" }] }`.
57. IF a request contains multiple validation errors, THEN THE Project_Workspace SHALL return all detected errors in a single validation response.
58. IF a request fails validation, THEN THE Project_Workspace SHALL NOT process, persist, or execute any side effects from the request.

---

### Requirement 23: Audit Logging

**User Story:** As a compliance officer, I want every significant Project Workspace operation to generate an audit record, so that all project changes are traceable and auditable for regulatory compliance.

#### Acceptance Criteria

1. WHEN a Project is created, THE Project_Workspace SHALL create an AuditLog record with Action equal to Created, EntityType equal to Project, EntityId equal to the project's ID, and NewValue containing the project's initial state serialized as JSON.
2. WHEN a Project's non-identity fields are updated, THE Project_Workspace SHALL create an AuditLog record with Action equal to Updated, EntityType equal to Project, OldValue containing the previous state serialized as JSON, and NewValue containing the new state serialized as JSON.
3. WHEN a Project Status is transitioned, THE Project_Workspace SHALL create an AuditLog record with Action equal to StatusChanged, EntityType equal to Project, OldValue equal to the previous Status, and NewValue equal to the new Status.
4. WHEN a Project Priority is changed, THE Project_Workspace SHALL create an AuditLog record with Action equal to PriorityChanged, EntityType equal to Project, OldValue equal to the previous Priority, and NewValue equal to the new Priority.
5. WHEN a Project is archived, THE Project_Workspace SHALL create an AuditLog record with Action equal to Archived, EntityType equal to Project.
6. WHEN a Project is restored from Archived status, THE Project_Workspace SHALL create an AuditLog record with Action equal to Restored, EntityType equal to Project.
7. WHEN a Project is soft-deleted, THE Project_Workspace SHALL create an AuditLog record with Action equal to Deleted, EntityType equal to Project.
8. WHEN a Project_Category is created, THE Project_Workspace SHALL create an AuditLog record with Action equal to Created, EntityType equal to ProjectCategory, EntityId equal to the category's ID.
9. WHEN a Project_Category is updated, THE Project_Workspace SHALL create an AuditLog record with Action equal to Updated, EntityType equal to ProjectCategory, OldValue and NewValue containing the changed state.
10. WHEN a Project_Category is soft-deleted, THE Project_Workspace SHALL create an AuditLog record with Action equal to Deleted, EntityType equal to ProjectCategory.
11. WHEN a Project_Team_Member is added to a project, THE Project_Workspace SHALL create an AuditLog record with Action equal to TeamMemberAdded, EntityType equal to ProjectTeamMember, EntityId equal to the team member's ID, NewValue containing EmployeeId and ProjectRole.
12. WHEN a Project_Team_Member is released from a project, THE Project_Workspace SHALL create an AuditLog record with Action equal to TeamMemberReleased, EntityType equal to ProjectTeamMember, NewValue containing ReleasedDate.
13. WHEN a Project_Asset_Allocation is created, THE Project_Workspace SHALL create an AuditLog record with Action equal to AssetAllocated, EntityType equal to ProjectAssetAllocation, EntityId equal to the allocation's ID, NewValue containing AssetId and AllocationDate.
14. WHEN a Project_Asset_Allocation Status transitions to Returned or PartiallyReturned, THE Project_Workspace SHALL create an AuditLog record with Action equal to AssetReturned, EntityType equal to ProjectAssetAllocation, NewValue containing ReturnDate and ReturnedQuantity.
15. WHEN a Project_Document is uploaded, THE Project_Workspace SHALL create an AuditLog record with Action equal to DocumentUploaded, EntityType equal to ProjectDocument, EntityId equal to the document's ID, NewValue containing DocumentName and Category.
16. WHEN a Project_Document version is incremented, THE Project_Workspace SHALL create an AuditLog record with Action equal to DocumentReplaced, EntityType equal to ProjectDocument, OldValue containing the previous Version, NewValue containing the new Version.
17. WHEN a Project_Document is soft-deleted, THE Project_Workspace SHALL create an AuditLog record with Action equal to DocumentDeleted, EntityType equal to ProjectDocument.
18. WHEN a Project_Parameter is added, THE Project_Workspace SHALL create an AuditLog record with Action equal to ParameterAdded, EntityType equal to ProjectParameter, NewValue containing ParameterName and InputType.
19. WHEN a Project_Parameter_Value is created or updated, THE Project_Workspace SHALL create an AuditLog record with Action equal to ParameterUpdated, EntityType equal to ProjectParameterValue, OldValue and NewValue containing the parameter value change.
20. WHEN a Project_Parameter is deleted, THE Project_Workspace SHALL create an AuditLog record with Action equal to ParameterDeleted, EntityType equal to ProjectParameter.
21. WHEN a Project_Parameter_Section is created, THE Project_Workspace SHALL create an AuditLog record with Action equal to SectionAdded, EntityType equal to ProjectParameterSection, NewValue containing the section Name.
22. WHEN a Project_Parameter_Section is deleted, THE Project_Workspace SHALL create an AuditLog record with Action equal to SectionDeleted, EntityType equal to ProjectParameterSection.
23. WHEN a Project_Budget field is updated, THE Project_Workspace SHALL create an AuditLog record with Action equal to BudgetUpdated, EntityType equal to ProjectBudget, OldValue and NewValue containing the changed budget field and amount.
24. WHEN a Risk is created, THE Project_Workspace SHALL create an AuditLog record with Action equal to RiskCreated, EntityType equal to Risk, NewValue containing Title, Category, Probability, and Impact.
25. WHEN a Risk is updated, THE Project_Workspace SHALL create an AuditLog record with Action equal to RiskUpdated, EntityType equal to Risk, OldValue and NewValue containing the changed risk fields.
26. WHEN a Risk Status transitions to Closed, THE Project_Workspace SHALL create an AuditLog record with Action equal to RiskClosed, EntityType equal to Risk, NewValue containing ClosedAtUtc.
27. THE AuditLog record SHALL contain: UserId (the authenticated user who performed the action), Timestamp (the UTC timestamp of the action), Action, EntityType, EntityId, OldValue, NewValue, OrganizationId.
28. THE AuditLog record SHALL include an IP Address field reserved for future security auditing, populated as null in the current implementation.
29. THE Project_Workspace SHALL integrate with the existing NexAsset AuditLog infrastructure, using the same AuditLog entity and table.
30. IF audit logging fails during a project operation, THEN THE Project_Workspace SHALL allow the project operation to complete, log the audit failure through the application logging mechanism, and notify administrators of the audit logging failure.
31. THE Project_Workspace SHALL NOT block business operations due to audit logging failures.
32. THE Project_Workspace SHALL support querying all AuditLog records for a specific project by EntityType and EntityId.
33. THE Project_Workspace SHALL support filtering AuditLog records by UserId, Action, EntityType, and date range.

---

### Requirement 24: User Experience

**User Story:** As a project user, I want a modern, intuitive, and responsive user interface for the Project Workspace, so that I can efficiently work with project data across desktop, tablet, and mobile devices.

#### Acceptance Criteria

1. WHEN a project list or project detail view is loading data, THE Project_Workspace SHALL display skeleton loaders matching the expected table or card layout structure.
2. WHEN no projects exist in an organization, THE Project_Workspace SHALL display an empty state with the message: "No projects yet. Create your first project to get started." with a prominent Create Project button.
3. WHEN no team members exist on a project, THE Project_Workspace SHALL display an empty state with the message: "No team members assigned yet. Add team members to start collaborating." with an Add Team Member button.
4. WHEN no assets are allocated to a project, THE Project_Workspace SHALL display an empty state with the message: "No assets allocated yet. Allocate assets to track project resources." with an Allocate Asset button.
5. WHEN no documents are uploaded to a project, THE Project_Workspace SHALL display an empty state with the message: "No documents uploaded yet. Upload project documents to centralize your files." with an Upload Document button.
6. WHEN no risks are recorded for a project, THE Project_Workspace SHALL display an empty state with the message: "No risks identified yet. Add risks to track and mitigate project threats." with an Add Risk button.
7. WHEN an API request fails due to validation errors, THE Project_Workspace SHALL display field-level error messages below each invalid field, highlighting the invalid fields with a red border.
8. WHEN an API request fails due to authorization errors (403), THE Project_Workspace SHALL display a user-friendly error message: "You don't have permission to perform this action. Contact your administrator for access."
9. WHEN an API request fails due to a server error (500), THE Project_Workspace SHALL display a generic error message: "Something went wrong. Our team has been notified. Please try again later." along with an error reference ID for support.
10. WHEN a user successfully creates a project, THE Project_Workspace SHALL display a success notification toast in the top-right corner: "Project [ProjectName] created successfully." with auto-dismiss after 5 seconds.
11. WHEN a user successfully updates a project, THE Project_Workspace SHALL display a success notification toast: "Project updated successfully."
12. WHEN a user attempts to delete a project, THE Project_Workspace SHALL display a confirmation dialog with the message: "Are you sure you want to delete [ProjectName]? This action cannot be undone." with Cancel and Delete buttons.
13. WHEN a user attempts to archive a project, THE Project_Workspace SHALL display a confirmation dialog with the message: "Archive [ProjectName]? Archived projects can be restored later." with Cancel and Archive buttons.
14. WHEN a user has unsaved changes in the Project_Wizard and attempts to navigate away, THE Project_Workspace SHALL display a confirmation dialog: "You have unsaved changes. Do you want to save your draft or discard changes?" with Save Draft, Discard, and Cancel buttons.
15. WHEN the Project_Wizard autosave operation is in progress, THE Project_Workspace SHALL display an indicator: "Saving..." with a spinning icon.
16. WHEN the Project_Wizard autosave operation completes successfully, THE Project_Workspace SHALL display an indicator: "Saved" with a checkmark icon and the timestamp "Last saved at HH:MM:SS".
17. WHEN the Project_Workspace frontend detects user inactivity followed by activity resumption, THE Project_Workspace SHALL check for autosaved draft and prompt the user: "Resume your draft from [timestamp]?" with Resume and Start New options.
18. THE Project_Workspace SHALL display responsive layouts optimized for three viewport categories: mobile (<768px), tablet (768px-1023px), desktop (≥1024px).
19. WHEN viewed on mobile devices, THE Project_Workspace navigation SHALL collapse into a hamburger menu, tables SHALL scroll horizontally or transform into card layouts, and multi-column forms SHALL stack vertically.
20. THE Project_Workspace SHALL provide tooltips on complex or technical fields, appearing on hover (desktop) or tap (mobile), with contextual help text.
21. THE Project_Workspace SHALL support keyboard navigation using Tab to move between fields, Enter to submit forms, Escape to close dialogs, and arrow keys to navigate lists.
22. THE Project_Workspace SHALL provide ARIA labels, roles, and live regions for screen reader compatibility, targeting WCAG 2.1 Level AA compliance.
23. THE Project_Workspace SHALL maintain consistent spacing using the existing NexAsset spacing scale: 4px, 8px, 12px, 16px, 24px, 32px, 48px.
24. THE Project_Workspace SHALL use existing NexAsset component behaviors: primary buttons for primary actions, secondary buttons for cancel/back actions, icon buttons for quick actions, danger buttons for destructive actions.
25. THE Project_Workspace page transitions SHALL be smooth, preserving scroll position when navigating between project sections.
26. WHEN a long-running operation (report generation, document upload) is in progress, THE Project_Workspace SHALL display a progress indicator with percentage or estimated time remaining WHERE available.
27. WHEN a network request times out, THE Project_Workspace SHALL display an error message: "The request timed out. Please check your connection and try again." with a Retry button.
28. WHEN the application detects the user is offline, THE Project_Workspace SHALL display a persistent banner: "You are offline. Some features may not be available." and disable actions requiring network access.
29. THE Project_Workspace SHALL follow the existing NexAsset design language: color palette, typography scale, button styles, card layouts, form patterns, table designs.
30. THE Project_Workspace SHALL NOT introduce new UI patterns that conflict with existing NexAsset modules (Foundation, HR, Assets, Enterprise Operations).


---

### Requirement 25: Performance Requirements

**User Story:** As a system administrator, I want the Project Workspace to maintain acceptable performance even with large datasets, so that users experience fast response times regardless of project size.

#### Acceptance Criteria

1. THE Project_Workspace SHALL support projects containing 100 or more active Project_Team_Members without performance degradation.
2. THE Project_Workspace SHALL support projects containing 500 or more Project_Asset_Allocations without performance degradation.
3. THE Project_Workspace SHALL support projects containing 1000 or more Project_Documents without performance degradation.
4. THE Project_Workspace SHALL support projects containing 500 or more Project_Parameters across all sections without performance degradation.
5. THE Project_Workspace SHALL support projects containing 200 or more open Risks without performance degradation.
6. THE Project_Workspace SHALL implement server-side pagination for all list endpoints, with a default page size of 20 items and a configurable maximum page size of 100 items.
7. WHEN a project detail view is opened, THE Project_Workspace SHALL load section data using lazy loading, fetching only the Overview section data initially and deferring Team, Assets, Parameters, Documents, Budget, and Risks until the user navigates to those sections.
8. WHEN a user performs a search operation on a large dataset (>100 items), THE Project_Workspace SHALL execute the search on the server side using LINQ `.Where()` with `.Contains()` for text matching, returning paginated results.
9. WHEN a user applies filters to a large dataset, THE Project_Workspace SHALL execute filtering on the server side using LINQ `.Where()` with exact matches or range checks, returning paginated results.
10. WHEN a user sorts a large dataset, THE Project_Workspace SHALL execute sorting on the server side using LINQ `.OrderBy()` or `.OrderByDescending()`, returning paginated results.
11. THE Project_Dashboard SHALL load and render within 2 seconds for projects of typical size (up to 20 team members, 50 assets, 100 documents, 50 parameters, 20 risks).
12. THE Project_Workspace API endpoints SHALL respond within 500 milliseconds for the 95th percentile of requests under normal load conditions.
13. THE Project_Workspace SHALL support 100 or more concurrent users per organization without response time degradation beyond acceptable thresholds.
14. THE Project_Workspace database queries SHALL use indexes on all frequently queried foreign keys: ProjectId on all project-related entities, OrganizationId on all tenant-scoped entities, EmployeeId on team member queries, AssetId on allocation queries.
15. THE Project_Workspace repositories SHALL use `.AsNoTracking()` for all list queries, history queries, and read-only detail queries to reduce EF Core memory overhead.
16. THE Project_Workspace repositories SHALL use tracking queries (default EF Core behavior) only for single-entity fetches that will be modified within the same handler operation.
17. THE Project_Workspace SHALL avoid N+1 query problems by using `.Include()` and `.ThenInclude()` to eagerly load required navigation properties in single queries WHERE the cardinality is low (1:1 or 1:few).
18. THE Project_Workspace SHALL avoid loading large related collections in a single query; instead, related collections (team members, assets, documents) SHALL be loaded via separate paginated queries when accessed.
19. THE Project_Workspace SHALL support scaling to 10,000 or more projects per organization without requiring schema changes or major refactoring.
20. THE Project_Workspace frontend SHALL implement virtual scrolling or infinite scroll for lists containing more than 100 items to reduce DOM size and improve rendering performance.
21. THE Project_Workspace frontend SHALL debounce search input with a 300ms delay to reduce the number of API requests triggered by typing.
22. THE Project_Workspace frontend SHALL cache frequently accessed reference data (project categories, employees, assets) in browser memory with a 5-minute TTL to reduce redundant API calls.

---

### Requirement 26: Error Handling

**User Story:** As a project user, I want clear and actionable error messages when operations fail, so that I understand what went wrong and how to resolve the issue.

#### Acceptance Criteria

1. WHEN a validation failure occurs, THE Project_Workspace SHALL display field-level error messages below each invalid field in the form, highlighting invalid fields with a red border and an error icon.
2. WHEN a validation failure occurs, THE Project_Workspace SHALL preserve all user-entered input values so that the user can correct errors without re-entering valid data.
3. WHEN a validation failure occurs, THE Project_Workspace SHALL focus the first invalid field and scroll it into view for accessibility.
4. WHEN an authorization failure (403) occurs, THE Project_Workspace SHALL display a user-friendly error message: "You don't have permission to perform this action. Contact your administrator if you believe this is an error."
5. WHEN a resource not found error (404) occurs, THE Project_Workspace SHALL display an error message: "The requested [entity type] could not be found. It may have been deleted or you may not have access."
6. WHEN a concurrency conflict is detected (two users editing the same entity), THE Project_Workspace SHALL display a conflict resolution dialog with options: View Latest Changes, Overwrite with My Changes, Cancel.
7. WHEN the user selects View Latest Changes in a concurrency conflict dialog, THE Project_Workspace SHALL reload the entity with the latest data from the server and display a warning: "This [entity] was modified by [UserName] at [timestamp]. Your unsaved changes have been discarded."
8. WHEN the user selects Overwrite with My Changes in a concurrency conflict dialog, THE Project_Workspace SHALL retry the save operation and display a confirmation: "Your changes have been saved, overwriting the previous version."
9. WHEN autosave fails due to a network interruption, THE Project_Workspace SHALL retry the autosave operation using exponential backoff: 1 second, 2 seconds, 4 seconds, 8 seconds.
10. WHEN autosave fails persistently after 4 retry attempts, THE Project_Workspace SHALL display a warning banner: "Unable to save your changes automatically. Please check your connection or save manually." with a Save Now button.
11. WHEN a document upload fails due to network interruption, THE Project_Workspace SHALL display a Retry button allowing the user to resume the upload without re-selecting the file.
12. WHEN a document upload fails due to server error or validation error, THE Project_Workspace SHALL display the error message returned by the server and allow the user to correct the issue and retry.
13. WHEN an unexpected system error (500) occurs, THE Project_Workspace SHALL display a generic error message: "An unexpected error occurred. Our team has been notified." along with a unique error reference ID (e.g., Error ID: ABC123) that can be provided to support for troubleshooting.
14. WHEN an unexpected system error occurs, THE Project_Workspace backend SHALL log the full exception stack trace, request details, and user context to the application logging system with a severity level of Error.
15. WHEN a temporary server failure or gateway timeout (502, 503, 504) occurs, THE Project_Workspace SHALL display an error message: "The service is temporarily unavailable. Please try again in a moment." with a Retry button.
16. WHEN the user clicks a Retry button after a temporary failure, THE Project_Workspace SHALL retry the failed operation immediately without requiring the user to re-enter data.
17. WHEN a request timeout occurs, THE Project_Workspace SHALL display a timeout message: "The request took too long to complete. Please try again or contact support if the issue persists." with a Retry button.
18. THE Project_Workspace SHALL NOT display raw exception stack traces, database error messages, or internal system details to end users.
19. THE Project_Workspace SHALL provide user-friendly error messages that explain what went wrong in plain language without technical jargon.
20. THE Project_Workspace SHALL provide actionable recovery guidance in error messages, such as: "Check your input and try again", "Retry the operation", "Contact support with Error ID: XYZ", "Refresh the page".
21. WHEN an error occurs during a background operation (autosave, report generation), THE Project_Workspace SHALL log the error to the application logging system and display a non-intrusive notification to the user.
22. THE Project_Workspace error logging SHALL include: timestamp, user ID, organization ID, operation attempted, error type, error message, stack trace (backend only), request payload (sanitized to remove sensitive data).

---

### Requirement 27: Integration Requirements

**User Story:** As a system architect, I want the Project Workspace to integrate seamlessly with existing NexAsset modules, so that project data leverages organization, employee, asset, and customer entities without duplicating data or breaking existing functionality.

#### Acceptance Criteria

1. THE Project_Workspace SHALL integrate with the Organization entity to enforce multi-tenant isolation, scoping all project queries by OrganizationId using the existing EF Core HasQueryFilter mechanism.
2. THE Project_Workspace SHALL integrate with the Branch entity, allowing a Project to reference a BranchId and displaying the Branch name in project detail views and reports.
3. WHEN a Project_Team_Member is created, THE Project_Workspace SHALL capture and snapshot the team member's BranchId and DepartmentId at the time of assignment, preserving allocation history even if the employee's branch or department changes later.
4. THE Project_Workspace SHALL integrate with the Department entity, allowing a Project to reference a DepartmentId and displaying the Department name in project detail views and reports.
5. WHEN a Project_Team_Member is created, THE Project_Workspace SHALL capture and snapshot the team member's DepartmentId at the time of assignment, preserving allocation history even if the employee's department changes later.
6. THE Project_Workspace SHALL integrate with the Customer entity, allowing a Project to reference a CustomerId and displaying the Customer name and code in project detail views and reports.
7. THE Project_Workspace SHALL integrate with the Employee entity, allowing ProjectManagerEmployeeId, Project_Team_Member.EmployeeId, and Risk.OwnerEmployeeId to reference employees, and resolving employee names for display in all views.
8. THE Project_Workspace SHALL integrate with the Asset entity, allowing Project_Asset_Allocation to reference AssetId and displaying asset code, name, category, and current status in allocation views.
9. WHEN an asset is allocated to a project, THE Project_Workspace SHALL update the Asset record to set ProjectId, AllocationDate, and AssetStatus, maintaining bidirectional consistency between Project_Asset_Allocation and Asset.
10. WHEN an asset is returned from a project, THE Project_Workspace SHALL update the Asset record to clear ProjectId and set AssetStatus to Available or Damaged based on the return condition, maintaining bidirectional consistency.
11. THE Asset detail view in the Assets module SHALL display all project allocations for the asset, showing ProjectName, AllocationDate, ReturnDate, AllocatedQuantity, ReturnedQuantity, and Status as a historical list.
12. THE Project_Workspace SHALL integrate with the AssetCategory entity, allowing filtering of assets by category in the asset allocation UI.
13. THE Project_Workspace SHALL reserve integration points for future Inventory module integration, allowing projects to track consumable usage (e.g., linking InventoryItem consumption to specific projects).
14. THE Project_Workspace SHALL reserve integration points for future Consumable module integration, allowing projects to track consumable consumption per project without requiring schema changes.
15. THE MaintenanceRecord entity in the Maintenance module SHALL support an optional ProjectId field, allowing maintenance records to be linked to projects for project-specific maintenance tracking.
16. THE Project_Workspace SHALL reserve integration points for future PurchaseRequest integration, allowing purchase requests to be linked to projects (e.g., PurchaseRequest.ProjectId nullable FK).
17. THE Project_Workspace SHALL reserve integration points for future PurchaseOrder integration, allowing purchase orders to be linked to projects (e.g., PurchaseOrder.ProjectId nullable FK).
18. THE Project_Workspace SHALL reserve integration points for future Vendor integration, allowing projects to track preferred vendors or vendor performance per project.
19. THE Project_Workspace SHALL integrate with the Notification entity, creating user-targeted notifications for project events and displaying them in the main NexAsset notification center.
20. THE Project_Workspace SHALL integrate with the AuditLog entity, creating audit records for all significant project operations and displaying them in the global audit log with EntityType filtering.
21. THE Project_Workspace SHALL integrate with the existing report export infrastructure, using the same PDF and Excel generation libraries used by other NexAsset modules.
22. THE Project_Workspace SHALL integrate with the existing cookie-based authentication system, using the same authentication middleware and ApplicationUser identity.
23. THE Project_Workspace SHALL integrate with the existing permission model, using DesignationPermission and RolePermission to resolve user permissions and PermissionRouteConvention to enforce authorization.
24. THE Project_Workspace SHALL NOT redesign or replace any existing NexAsset entities, repositories, services, or UI components.
25. THE Project_Workspace SHALL NOT introduce breaking changes to existing Foundation, HR, Assets, or Enterprise Operations modules.

---

### Requirement 28: API Behavior

**User Story:** As a frontend developer, I want consistent and predictable API behavior for the Project Workspace, so that API integration follows the same patterns used across all NexAsset modules.

#### Acceptance Criteria

1. THE Project_Workspace API endpoints SHALL use the URL prefix `/api/projects` for all project-related operations and `/api/project-categories` for category operations.
2. THE Project_Workspace API endpoints SHALL use sub-resources for nested entities: `/api/projects/{id}/team`, `/api/projects/{id}/assets`, `/api/projects/{id}/parameters`, `/api/projects/{id}/documents`, `/api/projects/{id}/budget`, `/api/projects/{id}/risks`, `/api/projects/{id}/timeline`, `/api/projects/{id}/activities`, `/api/projects/{id}/reports`.
3. THE Project_Workspace SHALL use HTTP GET for read operations, returning 200 OK with data in the response body.
4. THE Project_Workspace SHALL use HTTP POST for create operations, returning 201 Created with the created resource in the response body and a Location header pointing to the new resource.
5. THE Project_Workspace SHALL use HTTP PUT or PATCH for update operations, returning 200 OK with the updated resource in the response body or 204 No Content if no body is returned.
6. THE Project_Workspace SHALL use HTTP DELETE for soft-delete operations, returning 204 No Content on success.
7. THE Project_Workspace SHALL validate all incoming requests using the FluentValidation pipeline via ValidationBehavior, returning 400 Bad Request with validation errors in FluentValidation format if validation fails.
8. THE Project_Workspace SHALL return business failures as Result.Failure("message") from handlers, mapped to 400 Bad Request responses with the error message as a bare JSON string.
9. THE Project_Workspace SHALL return 401 Unauthorized when a request is made without a valid authentication cookie.
10. THE Project_Workspace SHALL return 403 Forbidden with a descriptive message when a request is made by an authenticated user without the required permission.
11. THE Project_Workspace SHALL return 404 Not Found when a requested resource does not exist or the user does not have access to it.
12. THE Project_Workspace SHALL return 500 Internal Server Error for unhandled exceptions, with a generic error message and error reference ID, logging the full exception details on the server.
13. THE Project_Workspace SHALL use the existing Result<T> pattern for all command and query handlers, returning Result<T>.Success(value) for successful operations and Result<T>.Failure("message") for business failures.
14. THE Project_Workspace list endpoints SHALL accept PagedRequest (PageNumber, PageSize, Search, SortBy, Descending) as query parameters using the `[AsParameters]` attribute.
15. THE Project_Workspace list endpoints SHALL return PagedResponse<T> (Items, TotalCount, PageNumber, PageSize) containing the requested page of data.
16. THE Project_Workspace search operations SHALL use server-side LINQ `.Where()` with `.Contains()` for case-insensitive partial text matching across searchable fields.
17. THE Project_Workspace filter operations SHALL use server-side LINQ `.Where()` with exact matches for enum fields, foreign keys, and boolean fields, and range checks for date and numeric fields.
18. THE Project_Workspace sort operations SHALL use server-side LINQ `.OrderBy()` or `.OrderByDescending()` based on the SortBy field and Descending flag.
19. THE Project_Workspace SHALL integrate with PermissionRouteConvention to automatically derive the required permission from the HTTP method and URL pattern for all project endpoints.
20. THE Project_Workspace SHALL use PermissionEnforcementFilter to enforce permission checks before handler execution for all authenticated endpoints.
21. THE Project_Workspace SHALL support bulk operations WHERE appropriate, such as bulk delete for risks or bulk status update for team members, returning a summary result indicating success count and failure count.
22. THE Project_Workspace SHALL support future API versioning using the URL path prefix `/api/v1/` to maintain backward compatibility when breaking changes are introduced in future versions.
23. THE Project_Workspace API responses SHALL use consistent JSON casing: camelCase for all property names in request and response bodies.
24. THE Project_Workspace API responses SHALL include standard headers: Content-Type: application/json, Cache-Control directives for cacheable resources.
25. THE Project_Workspace API SHALL support OPTIONS requests for CORS preflight checks, returning allowed methods and headers.

---

### Requirement 29: Frontend Behavior

**User Story:** As a project user, I want a complete and intuitive frontend interface for the Project Workspace, so that I can efficiently manage all aspects of my projects without confusion or friction.

#### Acceptance Criteria

1. THE Project List page SHALL display a table with columns: Project Code, Project Name, Status, Priority, Project Manager (resolved name), Start Date, End Date, with pagination controls at the bottom.
2. THE Project List page SHALL support server-side search by typing in a search box, filtering results by ProjectCode or ProjectName as the user types (debounced 300ms).
3. THE Project List page SHALL support filtering projects by Status (dropdown), Priority (dropdown), Category (dropdown), Branch (dropdown), Department (dropdown), and Project Manager (searchable dropdown).
4. THE Project List page SHALL support sorting by clicking column headers: ProjectName, StartDate, EndDate, with ascending/descending toggle on each click.
5. THE Project List page table rows SHALL provide quick actions: View (eye icon), Edit (pencil icon), Delete (trash icon), displayed on row hover or as a context menu (three dots).
6. THE Project Detail page SHALL display a tabbed or sectioned layout with sections: Overview, General Info, Team, Assets, Parameters, Documents, Timeline, Activity, Budget, Risks, Reports, Settings.
7. THE Project Detail page SHALL display Overview (Project_Dashboard) as the default active section when the page loads.
8. THE Project Detail page navigation tabs SHALL highlight the currently active section and allow clicking to switch between sections without full page reload.
9. THE Project_Wizard SHALL display a step indicator at the top showing: (1) General Information, (2) Project Team, (3) Asset Allocation, (4) Project Parameters, (5) Documents, (6) Review, (7) Save, with the current step highlighted.
10. THE Project_Wizard SHALL display navigation buttons: Back, Next, and Save, with Back disabled on Step 1, Next disabled on Step 7, and Save enabled only on Step 7.
11. THE Project_Wizard SHALL display autosave status indicators: "Saving...", "Saved at HH:MM:SS", or "Unsaved Changes" in a fixed position (e.g., bottom-right corner).
12. THE Project_Wizard SHALL validate the current step before allowing the user to proceed to the next step, displaying field-level validation errors if validation fails.
13. THE Project_Dashboard SHALL display a card grid layout with cards for: Overview (Status, Priority, Health, Progress), Team Summary, Asset Summary, Document Summary, Risk Summary, Budget Summary, in a responsive grid (3 columns on desktop, 2 on tablet, 1 on mobile).
14. THE Project_Dashboard SHALL display a Recent Activities widget showing the 10 most recent Activity_Records with timestamps, actions, and entity links.
15. THE Project_Dashboard SHALL display a Quick Actions toolbar with buttons: Edit Project, Add Team Member, Allocate Asset, Upload Document, Add Risk.
16. THE Project Timeline page SHALL display a vertical timeline with event icons on the left, event descriptions in the center, and timestamps on the right, grouped by date.
17. THE Project Timeline page SHALL support expanding and collapsing event details by clicking on timeline entries.
18. THE Project Timeline page SHALL support filtering events by EventType using a dropdown filter, with options: All, Project Created, Status Changed, Team Member Added, Asset Allocated, Document Uploaded, etc.
19. THE Activity Feed page SHALL display a list view with user avatars on the left, action descriptions in the center (e.g., "John Doe added a team member"), and relative timestamps on the right (e.g., "2 hours ago").
20. THE Activity Feed page SHALL support infinite scroll or pagination to load additional activity records as the user scrolls down.
21. THE Budget page SHALL display a form with input fields: Estimated Budget, Approved Budget, Actual Cost, Procurement Cost, Maintenance Cost, Labour Cost, Miscellaneous Cost, with computed read-only fields: Remaining Budget, Budget Percentage Used, Budget Status.
22. THE Budget page SHALL display a budget history table below the form, showing all previous budget versions with timestamps and the user who made the change.
23. THE Risk Register page SHALL display a table view with columns: Title, Category, Probability, Impact, Severity (color-coded), Owner (resolved name), Status, with row actions: View, Edit, Close, Delete.
24. THE Risk Register page SHALL display a Risk Matrix visualization showing a 3x3 grid (Probability: Low/Medium/High on Y-axis, Impact: Low/Medium/High on X-axis) with risks positioned in the corresponding cell and color-coded by Severity (Green=Low, Yellow=Medium, Orange=High, Red=Critical).
25. THE Risk Register page SHALL support adding a risk via an Add Risk button that opens a modal form with fields: Title, Description, Category, Probability, Impact, Owner, Mitigation Plan.
26. THE Documents page SHALL display a card grid view or table view (user-selectable) with document cards showing: thumbnail/icon, DocumentName, Category, Version, Uploaded By, Uploaded Date, with actions: Download, Preview, Replace, Delete.
27. THE Documents page SHALL support uploading documents via an Upload Document button that opens a modal with drag-and-drop file upload or file picker, fields for DocumentName, Category, Description, Remarks.
28. THE Documents page SHALL display a version history modal when the user clicks on a document's version number, showing all versions in descending order with download links.
29. THE Documents page SHALL support previewing PDF and image files in a modal overlay without downloading, with zoom controls and page navigation for multi-page PDFs.
30. THE Dynamic Parameters page SHALL display parameter sections as collapsible cards, each showing the section name as the card header and the list of parameters inside.
31. THE Dynamic Parameters page SHALL support adding a parameter section via an Add Section button that opens an inline form or modal to enter the section name.
32. THE Dynamic Parameters page SHALL support adding a parameter via an Add Parameter button within each section, opening a modal form with fields: Parameter Name, Input Type (dropdown), Unit, Is Required (checkbox), Display Order (number input), and Dropdown Options (conditional field visible only if Input Type = Dropdown).
33. THE Dynamic Parameters page SHALL support inline editing of parameter values directly in the section cards, with input controls matching the InputType (text box, textarea, number spinner, date picker, dropdown, checkbox, etc.).
34. THE Dynamic Parameters page SHALL support reordering parameters via drag-and-drop within a section, updating the DisplayOrder field automatically.
35. THE Reports page SHALL display a report selection dropdown with options: Project Summary, Team Allocation, Asset Allocation, Budget Report, Risk Report, Document Register, Activity Report, Timeline Report, Parameter Report.
36. THE Reports page SHALL display a filter form with fields relevant to the selected report type (e.g., date range for Activity Report, EventType filter for Timeline Report).
37. THE Reports page SHALL display a preview area showing a read-only rendering of the report based on the selected filters.
38. THE Reports page SHALL display export buttons: Export to PDF, Export to Excel, Print, triggering the respective report generation and download.
39. THE Settings page SHALL display a form for project-level settings (reserved for future use), with a Save button and validation feedback.
40. THE Project_Workspace UI components SHALL use existing NexAsset patterns: data tables with sortable columns, paginated footers, row actions; modal dialogs with header, body, footer, close button; form layouts with label-input pairs, validation messages below fields, required field indicators (*); cards with header, body, action buttons; breadcrumbs at the top of each page.
41. THE Project_Workspace UI components SHALL use existing NexAsset button styles: primary (blue), secondary (gray), danger (red), success (green), with consistent sizing (small, medium, large) and icon support.
42. THE Project_Workspace UI components SHALL use existing NexAsset form controls: text input, textarea, number input, date picker, dropdown/select, multi-select, checkbox, radio button, file upload, with consistent styling and focus states.
43. THE Project_Workspace layouts SHALL follow existing NexAsset responsive breakpoints: mobile (<768px, single column, stacked forms, hamburger menu), tablet (768px-1023px, two columns, collapsible sidebar), desktop (≥1024px, three columns, persistent sidebar).
44. THE Project_Workspace navigation flow SHALL be smooth, preserving scroll position when navigating between sections within a project, and resetting scroll position when navigating between different projects.
45. THE Project_Workspace loading states SHALL display skeleton loaders matching the expected content layout (table rows, card grids, form fields) while data is being fetched.
46. THE Project_Workspace empty states SHALL display centered messages with an icon, descriptive text, and a primary action button (e.g., "Create your first project").
47. THE Project_Workspace error states SHALL display an error icon, error message, and recovery action buttons (e.g., Retry, Go Back, Contact Support).
48. THE Project_Workspace success notifications SHALL display as toast/snackbar messages in the top-right corner, auto-dismissing after 5 seconds, with a close button for manual dismissal.

---

### Requirement 30: Module Acceptance Criteria

**User Story:** As a project stakeholder, I want a clear definition of what constitutes a complete and production-ready Project Workspace module, so that acceptance testing and sign-off are objective and traceable.

#### Acceptance Criteria

**Functional Completeness:**
1. THE Project_Workspace SHALL implement all CRUD operations (Create, Read, Update, Delete) for Projects, ProjectCategories, Project_Team_Members, Project_Asset_Allocations, Project_Documents, Project_Parameters, Project_Parameter_Sections, Project_Budget, and Risks.
2. THE Project_Wizard SHALL implement all 7 steps (General Information, Project Team, Asset Allocation, Project Parameters, Documents, Review, Save) with autosave functionality and draft session recovery.
3. THE Project_Dashboard SHALL display all defined widgets: Overview Cards (Status, Priority, Health, Progress), Team Summary, Asset Summary, Document Summary, Risk Summary, Budget Summary, Recent Activities, Upcoming Deadlines, Pending Approvals, Project Statistics, Quick Actions, Recent Changes.
4. THE Project Timeline SHALL record and display all defined Timeline_Event types: ProjectCreated, PlanningStarted, ApprovalRequested, Approved, TeamMemberAdded, TeamMemberReleased, AssetAllocated, AssetReturned, DocumentUploaded, DocumentReplaced, DocumentDeleted, BudgetUpdated, RiskAdded, RiskClosed, ParameterCreated, ParameterUpdated, ParameterDeleted, ProjectCompleted, ProjectArchived.
5. THE Activity Feed SHALL record and display all defined Activity_Record types: ProjectCreated, ProjectUpdated, StatusChanged, PriorityChanged, EmployeeAdded, EmployeeReleased, AssetAllocated, AssetReturned, DocumentUploaded, DocumentReplaced, DocumentDeleted, ParameterCreated, ParameterUpdated, ParameterDeleted, SectionCreated, SectionDeleted, BudgetUpdated, RiskAdded, RiskUpdated, RiskClosed.
6. THE Project_Budget SHALL correctly compute RemainingBudget as ApprovedBudget minus ActualCost, BudgetPercentageUsed as (ActualCost / ApprovedBudget) * 100, and BudgetStatus as Under Budget, On Budget, or Over Budget based on the comparison of ActualCost and ApprovedBudget.
7. THE Risk Severity matrix SHALL correctly compute severity levels: (Low, Low)→Low, (Low, Medium)→Low, (Low, High)→Medium, (Medium, Low)→Low, (Medium, Medium)→Medium, (Medium, High)→High, (High, Low)→Medium, (High, Medium)→High, (High, High)→Critical.
8. THE Project_Workspace SHALL generate all 9 report types: Project Summary, Team Allocation, Asset Allocation, Budget Report, Risk Report, Document Register, Activity Report, Timeline Report, Parameter Report, with PDF and Excel export functionality.
9. THE Project_Workspace SHALL generate notifications for all defined event types: ProjectAssigned, ApprovalRequested, ApprovalCompleted, TeamMemberAdded, TeamMemberReleased, AssetAllocated, AssetReturned, BudgetThresholdCrossed (80%, 100%), RiskCreated, RiskCritical, RiskClosed, DocumentUploaded, DocumentExpiring, UpcomingDeadline, ProjectCompleted.
10. THE Global_Search SHALL search across Projects, Project_Parameters, Project_Parameter_Values, Project_Documents, Project_Team_Members, Project_Asset_Allocations, Risks, and Activity_Records, returning results grouped by entity type with match counts.
11. THE Advanced Filters SHALL support filtering Projects by Status, Priority, Category, Branch, Department, ProjectManager; Risks by Category, Probability, Impact, Severity, Status, Owner; Activity_Records by ActivityType, UserId, date range; Project_Documents by Category, UploadedBy, date range.
12. THE Saved_Filter feature SHALL allow users to save filter combinations with a user-defined name, retrieve saved filters, and delete saved filters.
13. THE Project_Workspace navigation SHALL support deep linking to all sections: Overview, General Info, Team, Assets, Parameters, Documents, Timeline, Activity, Budget, Risks, Reports, Settings.

**Permission Enforcement:**
14. ALL 24 defined Project_Workspace permissions SHALL be registered in the permission catalog: Projects.View, Projects.Create, Projects.Update, Projects.Delete, Projects.Archive, Projects.Restore, Projects.Duplicate, Projects.Approve, Projects.ManageTeam, Projects.ManageAssets, Projects.ManageParameters, Projects.ManageDocuments, Projects.ViewBudget, Projects.ManageBudget, Projects.ViewRisks, Projects.ManageRisks, Projects.ViewReports, Projects.ExportReports, Projects.ViewTimeline, Projects.ViewActivities, Projects.ManageSettings, ProjectCategories.View, ProjectCategories.Create, ProjectCategories.Update, ProjectCategories.Delete.
15. Permission checks SHALL be enforced at the API level for all endpoints, returning 403 Forbidden for unauthorized access attempts.
16. Permission checks SHALL be enforced at the UI level, hiding unauthorized actions and disabling unauthorized buttons.
17. THE SuperAdmin role SHALL bypass all permission checks and have unrestricted access to all Project_Workspace operations.

**Validation Enforcement:**
18. ALL mandatory fields SHALL be enforced with "required" validation errors.
19. ALL length limits SHALL be enforced (ProjectCode 50 chars, ProjectName 200 chars, Description 1000 chars, etc.) with "exceeds maximum length" validation errors.
20. ALL allowed value enumerations SHALL be enforced (Status, Priority, Category, Probability, Impact, Document_Category, Input_Type) with "invalid value" validation errors.
21. ALL business constraints SHALL be enforced (StartDate ≤ EndDate, AllocationPercentage 1-100, ProjectCode unique per org, no duplicate active team members) with descriptive validation errors.
22. ALL cross-field validations SHALL be enforced (date range checks, relationship existence checks, uniqueness constraints).
23. Validation errors SHALL be returned in FluentValidation format: `{ "Errors": [{ "PropertyName", "ErrorMessage" }] }`.

**Audit Logging:**
24. ALL defined audit events SHALL generate AuditLog records with the correct Action, EntityType, EntityId, OldValue, NewValue, UserId, Timestamp, OrganizationId.
25. Audit log failures SHALL NOT block business operations, and audit failures SHALL be logged to the application logging system.
26. Audit records SHALL be queryable by ProjectId, EntityType, UserId, Action, and date range.

**User Experience:**
27. Loading states SHALL display skeleton loaders for all async operations (project list loading, project detail loading, report generation).
28. Empty states SHALL display helpful messages and call-to-action buttons when no data exists (no projects, no team members, no assets, no documents, no risks).
29. Error states SHALL display user-friendly error messages with recovery actions (Retry, Go Back, Contact Support).
30. Success notifications SHALL display toast/snackbar messages for successful operations (project created, team member added, document uploaded) with auto-dismiss after 5 seconds.
31. Confirmation dialogs SHALL display before destructive actions (delete project, archive project, delete document, delete risk).
32. Unsaved changes warnings SHALL display before navigating away from forms with unsaved data (wizard, edit forms).
33. Autosave indicators SHALL display during wizard editing: "Saving...", "Saved at HH:MM:SS", "Unsaved Changes".
34. Responsive layouts SHALL work correctly on mobile (<768px), tablet (768px-1023px), and desktop (≥1024px) viewports.
35. Keyboard navigation SHALL work for all interactive elements (Tab, Enter, Escape, arrow keys).
36. Screen reader accessibility SHALL be functional with ARIA labels, roles, and live regions for all dynamic content.

**Performance:**
37. Pagination SHALL work for all list endpoints with default page size 20 and maximum page size 100.
38. Lazy loading SHALL work for project detail sections (load Overview initially, defer Team, Assets, Parameters, Documents, Budget, Risks until accessed).
39. Server-side search, filter, and sort SHALL work correctly for all list views.
40. Dashboard loading time SHALL be under 2 seconds for typical projects (20 team members, 50 assets, 100 documents, 50 parameters, 20 risks).
41. API response times SHALL be under 500ms for 95th percentile under normal load.
42. NO N+1 query issues SHALL be present in any list or detail query.

**Error Handling:**
43. Validation failures SHALL display field-level errors with preserved user input.
44. Authorization failures (403) SHALL display friendly "You don't have permission" messages.
45. Concurrency conflicts SHALL be detected and SHALL offer View Latest Changes or Overwrite options.
46. Autosave failures SHALL retry with exponential backoff and SHALL display a warning banner after 4 failed attempts.
47. Upload failures SHALL allow retry without re-selecting the file.
48. Unexpected errors (500) SHALL display generic error messages with error reference IDs and SHALL log full exception details on the server.
49. Network interruptions SHALL be detected and SHALL display "You are offline" banner with disabled actions.

**Integration:**
50. Multi-tenant isolation SHALL be enforced (all queries scoped by OrganizationId via HasQueryFilter).
51. Integration with Customers, Employees, Assets, Branches, Departments, AssetCategories SHALL work correctly (foreign key resolution, display name resolution, filtering).
52. Integration with Notifications system SHALL work correctly (notification generation, display in main notification center).
53. Integration with AuditLog system SHALL work correctly (audit record creation, display in global audit log).
54. Integration with existing authentication system SHALL work correctly (cookie-based auth, ApplicationUser identity).
55. Integration with existing permission system SHALL work correctly (DesignationPermission, RolePermission, PermissionRouteConvention, PermissionEnforcementFilter, SuperAdmin bypass).
56. Integration with existing report export infrastructure SHALL work correctly (PDF and Excel generation using existing libraries).

**API Consistency:**
57. ALL endpoints SHALL follow REST conventions (GET for read, POST for create, PUT/PATCH for update, DELETE for soft delete).
58. ALL endpoints SHALL use Result<T> pattern (Result.Success or Result.Failure).
59. ALL list endpoints SHALL use PagedRequest and PagedResponse.
60. ALL endpoints SHALL enforce authorization via PermissionRouteConvention and PermissionEnforcementFilter.
61. ALL endpoints SHALL return consistent error responses (400 validation, 401 unauthenticated, 403 unauthorized, 404 not found, 500 server error).
62. ALL endpoints SHALL return consistent success responses (200 OK, 201 Created, 204 No Content).

**Frontend Consistency:**
63. ALL UI components SHALL follow existing NexAsset design language (buttons, forms, tables, cards, modals, breadcrumbs, tabs).
64. ALL layouts SHALL match existing NexAsset spacing scale and typography scale.
65. ALL colors, icons, and visual indicators SHALL match existing NexAsset theme.
66. NO new UI patterns SHALL be introduced that conflict with existing modules.

**Architectural Compliance:**
67. Implementation SHALL follow Clean Architecture with Application, Domain, Infrastructure, API, and Web layers.
68. ALL commands and queries SHALL use MediatR (IRequest<Result<T>>, IRequestHandler).
69. ALL validation SHALL use FluentValidation pipeline (AbstractValidator, ValidationBehavior).
70. ALL entities SHALL inherit from BaseEntity (Id, CreatedAtUtc, UpdatedAtUtc, IsDeleted, DeletedAtUtc).
71. ALL repositories SHALL use UnitOfWork pattern (IUnitOfWork.SaveChangesAsync).
72. ALL API endpoints SHALL use Minimal APIs pattern with thin endpoint adapters.
73. ALL soft deletes SHALL be enforced (IsDeleted flag, manual filtering in repositories).
74. ALL timestamps SHALL be in UTC.
75. ALL multi-tenant query filters SHALL be enforced via EF Core HasQueryFilter.

**Final Verification Checklist:**
76. ✓ Every functional requirement (1-20) implemented and tested.
77. ✓ Every permission (21) enforced and tested.
78. ✓ Every validation rule (22) enforced and tested.
79. ✓ Every audit event (23) generates correctly.
80. ✓ Every UX state (24) displays correctly.
81. ✓ Every performance expectation (25) met.
82. ✓ Every error scenario (26) handled correctly.
83. ✓ Every integration point (27) works correctly.
84. ✓ Every API endpoint (28) works correctly.
85. ✓ Every UI screen (29) works correctly.
86. ✓ NO existing NexAsset functionality broken by this module.
87. ✓ Documentation complete: API documentation (Swagger), user guides, admin guides, developer guides.

THE completed SPEC-001 Project Workspace module SHALL be considered production-ready and acceptable for deployment only when all acceptance criteria in Requirements 1 through 30 have been verified and the Final Verification Checklist items 76-87 have been confirmed as passing.

