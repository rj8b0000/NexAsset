# NexAsset Architecture Context
> Permanent source of truth. Updated after each architectural discussion.
> Last updated: 2026-07-21 (KT Phase 1–10 complete)

---

## 1. PRODUCT OVERVIEW

**NexAsset** is an Enterprise Asset Management (EAM) and Operations Management Platform.
Target: Solar EPC, electrical contractors, O&M providers, civil construction, infrastructure,
facility management, manufacturing — but intentionally industry-agnostic.

**Core Mission:** Replace spreadsheets, WhatsApp approvals, and paper registers with a
single, multi-tenant, auditable platform that tracks every asset from purchase to retirement.

**Commercial Model:** One-time licensing, AMC, cloud subscription, enterprise SaaS,
customization, implementation, training, support, consulting.

---

## 2. TECHNOLOGY STACK

| Layer | Technology | Version |
|-------|-----------|---------|
| Runtime | .NET | 10 |
| API | ASP.NET Core Minimal APIs | 10 |
| Frontend | Blazor WebAssembly | 10 |
| Database | PostgreSQL | Latest |
| ORM | Entity Framework Core | 10.0.9 |
| Identity | ASP.NET Identity | EF stores |
| CQRS | MediatR | 14.2.0 |
| Validation | FluentValidation | 12.1.1 |
| Mapping | Mapperly (source gen) | 4.3.1 |
| Auth | Cookie-based (Identity) | - |
| Docs | Swagger / OpenAPI | Swashbuckle 10 |

---

## 3. SOLUTION STRUCTURE

```
NexAsset.sln
├── src/NexAsset.API              Minimal API endpoints, middleware, authorization filters
├── src/NexAsset.Application      CQRS features, handlers, validators, DTOs, interfaces
├── src/NexAsset.Domain           Entities, enums, BaseEntity
├── src/NexAsset.Infrastructure   EF Core, Identity, repositories, authorization, seed
├── src/NexAsset.Web              Blazor WASM frontend (SPA)
└── src/docs/                     API, module, workflow, architecture documentation
```

### Layer Dependency Rule
```
API → Application → Domain ← Infrastructure
Web → API (via HTTP only)
```
No layer may bypass its direct dependency. No shortcuts.

---

## 4. ARCHITECTURE PATTERN

### Clean Architecture + CQRS (MediatR)

```
HTTP Request
  → Middleware pipeline (Exception → Auth → Tenant → Permission)
  → Minimal API Endpoint (thin adapter)
  → MediatR Send(Command/Query)
  → ValidationBehavior (FluentValidation pipeline)
  → Command/Query Handler (business logic)
  → Repository (data access abstraction)
  → UnitOfWork.SaveChangesAsync()
  → EF Core → PostgreSQL
  → Result<T> flows back up
  → Endpoint: IsFailure → 4xx | IsSuccess → 2xx
```

### Middleware Stack (order matters)
1. GlobalExceptionMiddleware — catches all unhandled exceptions
2. UseHttpsRedirection
3. UseCors (WasmClient policy)
4. UseAuthentication
5. UseAuthorization
6. TenantResolutionMiddleware — establishes org boundary AFTER auth
7. Endpoints

---

## 5. DOMAIN MODEL

### BaseEntity (all domain entities inherit this)
```
Id           Guid (PK, NewGuid() default)
CreatedAtUtc DateTime (UTC)
UpdatedAtUtc DateTime (UTC)
IsDeleted    bool
DeletedAtUtc DateTime?
```

### Entity Inventory (26 domain tables + Identity tables)

**Foundation (4)**
- Organization — top-level tenant entity (Code globally unique)
- Branch — location per org (Code unique per org)
- Department — functional unit per org (Code unique per org)
- Designation — job title per org, optional dept link (Title unique per org)

**HR / Identity (3 domain + 7 Identity)**
- Employee — org + branch? + dept? + designation? + reportingManager? + identityUserId?
- Permission — global permission catalog (Code = "Module.Action", globally unique)
- RolePermission — join: ApplicationRole ↔ Permission (composite PK, cascade on Permission delete)
- DesignationPermission — join: Designation ↔ Permission (composite PK, cascade on Permission delete)
- ApplicationUser (Identity, extends IdentityUser<Guid>) — + OrganizationId, BranchId, DepartmentId, DesignationId, EmployeeId, IsActive, LoginAtUtc
- ApplicationRole (Identity, extends IdentityRole<Guid>)

**Assets (5)**
- AssetCategory — per org (Code unique per org)
- Asset — per org + category + branch? + dept? + currentEmployee? (Code unique per org, SerialNumber globally unique)
- AssetAssignment — append-only history (AssetId + EmployeeId indexed, snapshot of branch/dept)
- AssetTransfer — append-only history (AssetId indexed, from/to employee/branch/dept)
- AssetReturn — append-only history (AssetId indexed, IsAssetUsable flag)

**Maintenance (1)**
- MaintenanceRecord — per asset, preventive or corrective, lifecycle status

**Enterprise Operations (8)**
- Vendor — per org (Code unique per org)
- PurchaseRequest — per org, RequestNumber unique per org
- PurchaseOrder — per org, OrderNumber unique per org, optional link to PR
- InventoryItem — per org, optional BranchId, AvailableStock = computed
- StockMovement — per InventoryItem, snapshot of StockAfterMovement
- Consumable — per InventoryItem, ConsumableCode globally unique (potential issue)
- Customer — per org (Code unique per org)
- ServiceTicket — per org + customer, TicketNumber unique per org

**System (3)**
- Notification — user-targeted or broadcast (no org query filter)
- AuditLog — entity/action/user metadata (no org query filter)
- SystemSetting — org-specific or global (null OrganizationId = global)

---

## 6. DOMAIN ENUMS (State Machines)

### AssetStatus
Available → Assigned → InTransfer → Available (via return)
Available → Assigned → Damaged (via return with IsAssetUsable=false)
Any → InMaintenance, Retired, Lost

### AssetAssignmentStatus
Active → Returned | Transferred | Unassigned

### ProcurementStatus (used by both PurchaseRequest and PurchaseOrder)
Draft → PendingApproval → Approved → Ordered
PendingApproval → Rejected | Cancelled

### MaintenanceStatus
Requested → Scheduled → InProgress → Completed
Any → Cancelled

### TicketStatus
Open → Assigned → InProgress → Resolved → Closed
Any → Cancelled

### TicketPriority: Low, Medium, High, Critical

### StockMovementType: StockIn, StockOut, Adjustment, Reserved, Released

### MaintenanceType: Preventive, Corrective

### NotificationType: Info, Warning, Success, Error

### EmploymentStatus: Active, Probation, NoticePeriod, Resigned, Terminated

---

## 7. DATABASE DESIGN DECISIONS

### Multi-tenancy (EF Core HasQueryFilter)
- ALL org-scoped entities filtered by `TenantOrganizationId` automatically
- Organization entity filtered by `OrganizationScopeId` (null for SuperAdmin)
- Entities without direct OrgId (AssetTransfer, AssetReturn, Consumable, StockMovement, MaintenanceRecord)
  filtered via parent navigation: `x.Asset.OrganizationId`, `x.InventoryItem.OrganizationId`
- Identity tables (AspNet*): NO query filter — scoped manually in IdentityService

### Soft Delete
- Implemented MANUALLY in each repository via `&& !x.IsDeleted`
- NOT a global query filter (intentional — explicit is preferred over implicit here)
- All repositories consistently apply `!x.IsDeleted` in all reads

### Business Code Uniqueness Pattern
- All codes use composite index: `{OrganizationId, Code}` (unique per org)
- Exception: Permission.Code is globally unique (no org scope)
- Exception: Asset.SerialNumber is globally unique (across all orgs)
- Exception: Consumable.ConsumableCode is globally unique (potential bug — should be org-scoped)
- Exception: Employee.Email is globally unique

### Money Fields: decimal(18,2) — all monetary amounts
### Business Dates: DateOnly — no time component on purchase dates, assignment dates, etc.
### All PKs: Guid (NewGuid() by default in BaseEntity)
### All timestamps: DateTime UTC
### All FKs: DeleteBehavior.Restrict (safe defaults) except RolePermission/DesignationPermission→Permission: Cascade

### Indexes (explicitly defined)
- Organization.Code (unique)
- Branch.{OrganizationId, Code} (unique)
- Department.{OrganizationId, Code} (unique)
- Designation.{OrganizationId, Title} (unique)
- Employee.{OrganizationId, EmployeeCode} (unique)
- Employee.Email (unique)
- AssetCategory.{OrganizationId, Code} (unique)
- Asset.{OrganizationId, AssetCode} (unique)
- Asset.SerialNumber (unique)
- AssetAssignment.AssetId, AssetAssignment.EmployeeId
- AssetReturn.AssetId, AssetTransfer.AssetId
- Vendor.{OrganizationId, Code} (unique)
- PurchaseRequest.{OrganizationId, RequestNumber} (unique)
- PurchaseOrder.{OrganizationId, OrderNumber} (unique)
- InventoryItem.{OrganizationId, ItemCode} (unique)
- StockMovement.InventoryItemId
- Consumable.ConsumableCode (unique — global, potential bug)
- Customer.{OrganizationId, Code} (unique)
- ServiceTicket.{OrganizationId, TicketNumber} (unique)
- SystemSetting.{OrganizationId, Key} (unique)
- Permission.Code (unique)

---

## 8. SECURITY & AUTHORIZATION

### Authentication
- Cookie-based: `NexAsset.Auth`
- 7-day sliding expiration
- Password: min 8 chars, requires digit + upper + lower (no special char required)
- Unique email enforced globally
- `ApplicationUser.IsActive` — inactive accounts cannot log in
- Lock: SetLockoutEndDateAsync (100 years if permanent), unlock by setting null

### Multi-Tenant Isolation
```
TenantContext.Apply() sets:
  FilterOrganizationId = null (SuperAdmin unselected) | orgId (all others)
  OrganizationFilterId = null (SuperAdmin) | orgId (others)

DbContext query filters read these values on EVERY query.
No application code can escape this — only IgnoreQueryFilters() can bypass,
used only in EffectivePermissionService (to avoid circular dependency).
```

### Permission Model
```
Employee present?
  YES → permissions = DesignationPermission codes for employee's designation
         (no designation = no permissions)
  NO  → permissions = RolePermission codes for user's Identity roles

SuperAdmin role → bypasses ALL permission checks
```

### Permission Cache
- Memory cache, 60-second TTL per user
- Key: `effective-permissions:{userId}`
- Permission changes propagate within 60 seconds

### Permission Naming Convention
Format: `Module.Action`
All 97 seeded permissions across 25 modules (see PermissionSeeder.Matrix)

### PermissionRouteConvention
Derives required permission from HTTP method + URL pattern at runtime.
- GET → Module.View
- POST → Module.Create (with special cases for status, assign, transfer, etc.)
- PUT/PATCH → Module.Update
- DELETE → Module.Delete
Special cases: approve, transfer, return, assign, unassign, mark-read, manage-roles, etc.

### SuperAdmin Safeguard
Cannot remove the last SuperAdmin from the SuperAdmin role (enforced in IdentityService).

### CORS
- Policy: `WasmClient`
- Origins: `http://localhost:5174`, `https://localhost:7225`
- AllowCredentials() required for cookie transmission
- AllowAnyHeader(), AllowAnyMethod()

---

## 9. BACKEND PATTERNS

### CQRS Feature Structure
```
Features/{Domain}/
  Commands/
    {Operation}/
      {Operation}Command.cs         — sealed record : IRequest<Result<T>>
      {Operation}CommandHandler.cs  — sealed class : IRequestHandler
      {Operation}CommandValidator.cs — sealed class : AbstractValidator<T>
  Queries/
    {Operation}/
      {Operation}Query.cs           — sealed record : IRequest<Result<T>>
      {Operation}QueryHandler.cs    — sealed class : IRequestHandler
      {Operation}Response.cs        — response DTO
```

### Result<T> Pattern
```csharp
Result<T>.Success(value) | Result<T>.Failure("message")
Result.Success()         | Result.Failure("message")
```
Business failures return Result.Failure() — never throw exceptions.
Exceptions are reserved for infrastructure failures (caught by GlobalExceptionMiddleware).

### Validation (Two-Stage)
1. FluentValidation (format/size/required) — via ValidationBehavior pipeline
2. Business validation (reference checks, duplicates, state) — inside Handler

### Paging
All list endpoints use `PagedRequest` (PageNumber, PageSize, Search, SortBy, Descending)
and return `PagedResponse<T>` (Items, TotalCount, PageNumber, PageSize).
Repositories use AsNoTracking() for all list/history queries.

### Repository Pattern
- Individual repositories for Foundation + HR + Asset domains
- Single `EnterpriseOperationsRepository` (generic) for all enterprise ops entities
- `IUnitOfWork.SaveChangesAsync()` — one implicit EF Core transaction per save
- No explicit Begin/Commit/Rollback on UnitOfWork (relies on EF implicit transactions)

### Endpoint Pattern
```csharp
// Group with auth + permission filter
var group = app.MapGroup("/api/{module}").WithTags("...")
    .RequireAuthorization()
    .AddEndpointFilter<PermissionEnforcementFilter>();

// Thin — delegates to MediatR immediately
group.MapPost("/", async ([FromBody] CreateCmd cmd, ISender sender) => {
    var result = await sender.Send(cmd);
    return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created(...);
});
```

### Error Response Shapes
- Business failure: `"Error message string"` (bare JSON string)
- Validation failure: `{ "Errors": [{ "PropertyName", "ErrorMessage" }] }`
- Unhandled exception: `{ "Message": "An unexpected error occurred." }`
- Permission denied: `{ "Message": "You don't have permission..." }` (403)

---

## 10. ASSET LIFECYCLE (Critical Business Logic)

### Assign
- Guard: no active assignment exists
- Guard: employee belongs to same organization
- Sets: Asset.CurrentEmployeeId = employee.Id
- Sets: Asset.BranchId = employee.BranchId ?? asset.BranchId
- Sets: Asset.DepartmentId = employee.DepartmentId ?? asset.DepartmentId
- Sets: Asset.AssetStatus = Assigned
- Creates: AssetAssignment (snapshot of branch/dept at time of assignment)

### Return
- Guard: active assignment exists
- Sets: Asset.CurrentEmployeeId = null
- Sets: Asset.AssetStatus = Available (if usable) | Damaged (if not usable)
- Updates: AssetAssignment.Status = Returned, UnassignedDate = returnDate
- Creates: AssetReturn (with InspectionNotes, IsAssetUsable)

### Transfer (Approved)
- Guard: active assignment exists
- Updates: Asset.CurrentEmployeeId, BranchId, DepartmentId → new values
- Updates: AssetAssignment.EmployeeId, BranchId, DepartmentId → new values
- Sets: Asset.AssetStatus = Assigned
- Creates: AssetTransfer (from/to snapshot)

### Transfer (Pending/Not Approved)
- Guard: active assignment exists
- Sets: Asset.AssetStatus = InTransfer
- Does NOT update CurrentEmployeeId (still with original holder)
- Creates: AssetTransfer with IsApproved = false

### Delete Asset
- Soft delete only (IsDeleted = true)
- Asset remains in database for audit history

---

## 11. PROCUREMENT WORKFLOW

```
PurchaseRequest: Draft → PendingApproval → Approved → Ordered
                                         → Rejected
                                         → Cancelled

PurchaseOrder:   (same ProcurementStatus enum)
               Can be created: linked to PR (PurchaseRequestId?) OR standalone
               Must reference a Vendor

Special permission: PurchaseRequests.Approve (for status transitions)
Special permission: PurchaseOrders.UpdateStatus (for status transitions)
```

---

## 12. INVENTORY WORKFLOW

```
InventoryItem → tracks CurrentStock, ReservedStock, ReorderLevel
  AvailableStock = CurrentStock - ReservedStock (computed in memory, not stored)

StockMovement types:
  StockIn     → increases CurrentStock
  StockOut    → decreases CurrentStock
  Adjustment  → corrects stock discrepancy
  Reserved    → increases ReservedStock
  Released    → decreases ReservedStock

StockAfterMovement = snapshot stored per movement (running history)
Low stock flagging = when CurrentStock <= ReorderLevel
```

---

## 13. FRONTEND ARCHITECTURE (Blazor WASM)

### API Client Pattern
All typed clients extend `ApiClientBase`. Result type: `ApiResult<T>` (mirrors backend `Result<T>`).
Error shapes handled: bare string, FluentValidation `{ Errors }`, unhandled `{ Message }`.

### Authentication State
- Backed by `GET /api/auth/me` (cookie check)
- Cached in `NexAssetAuthenticationStateProvider`
- Refreshed on login/logout
- Permission set loaded separately from `GET /api/auth/me/permissions`

### Permission Enforcement in UI
- `IPermissionChecker.HasPermission("Module.Action")` — hides UI elements
- Fail-open while unloaded (backend still enforces)
- SuperAdmin bypasses all UI permission checks

### State Management (Singleton)
- `ThemeState` — dark/light mode
- `NotificationState` — toast/banner messages
- `NavigationState` — active menu tracking

### Mock Status
- Finance module: still on `MockDatabaseService` (no backend Finance module yet)
- ApprovalCenter: partially mocked
- Everything else: real typed API clients

---

## 14. MODULE INVENTORY

### API Routes and Permissions

| Module | Route | Notes |
|--------|-------|-------|
| Authentication | /api/auth/* | login, logout, me, me/permissions, register, reset-password |
| Organizations | /api/organizations | Foundation |
| Branches | /api/branches | Foundation |
| Departments | /api/departments | Foundation |
| Designations | /api/designations | Foundation |
| Employees | /api/employees | HR |
| Roles | /api/roles | HR |
| Users | /api/users | HR |
| Permissions | /api/permissions | HR |
| Asset Categories | /api/asset-categories | Assets |
| Assets | /api/assets | Assets |
| Asset Assignments | /api/asset-assignments | Assets lifecycle |
| Asset Transfers | /api/asset-transfers | Assets lifecycle |
| Asset Returns | /api/asset-returns | Assets lifecycle |
| Vendors | /api/enterprise-operations/vendors | Enterprise Ops |
| Customers | /api/enterprise-operations/customers | Enterprise Ops |
| Purchase Requests | /api/enterprise-operations/purchase-requests | Enterprise Ops |
| Purchase Orders | /api/enterprise-operations/purchase-orders | Enterprise Ops |
| Inventory | /api/enterprise-operations/inventory | Enterprise Ops |
| Consumables | /api/enterprise-operations/consumables | Enterprise Ops |
| Maintenance | /api/enterprise-operations/maintenance | Enterprise Ops |
| Service Tickets | /api/enterprise-operations/service-tickets | Enterprise Ops |
| Notifications | /api/enterprise-operations/notifications | Enterprise Ops |
| Audit Logs | /api/enterprise-operations/audit-logs | Enterprise Ops |
| System Settings | /api/enterprise-operations/system-settings | Enterprise Ops |
| Dashboard | /api/enterprise-operations/dashboard/{orgId} | Enterprise Ops |

---

## 15. KNOWN ISSUES & RISKS

### Active Issues to Address
1. **Consumable.ConsumableCode globally unique** — likely should be org-scoped
   File: `EnterpriseOperationsConfiguration.cs` (ConsumableConfiguration)
   Fix: Change to `{OrganizationId/InventoryItemId, ConsumableCode}` composite index

2. **Missing indexes on high-traffic FKs:**
   - Asset.CurrentEmployeeId (common: "assets held by employee X")
   - Notification.UserId (common: "my notifications")
   - ServiceTicket.AssignedToEmployeeId (common: "tickets assigned to me")
   Fix: Add to respective EF Core configurations

3. **Approver navigation properties missing:**
   - PurchaseRequest.ApprovedByEmployeeId has no navigation
   - PurchaseOrder.ApprovedByEmployeeId has no navigation
   - ServiceTicket.AssignedToEmployeeId has no navigation
   Impact: Approval history requires explicit joins

4. **Finance module not implemented** — frontend uses MockDatabaseService
   No backend endpoints for Finance/Invoices exist yet

5. **ApprovalCenter page partially mocked** — needs real API integration

6. **Permission cache is in-process only** — multi-instance deployments will have
   inconsistent permissions for up to 60 seconds after changes

7. **No rate limiting** — API has no rate limiting middleware yet

### Technical Debt (Low Priority)
- UnitOfWork has no explicit transaction API (BeginTransaction/Commit/Rollback)
  Current risk: low (all handlers do single SaveChanges call)
- No dedicated read model / read replica support
- No background jobs / hosted services yet (notifications are stored, not delivered)
- No email delivery for notifications
- No SMTP configuration in current codebase

---

## 16. IMPLEMENTATION STATUS

### Fully Implemented (Backend + Frontend)
- Authentication (login, logout, register, reset password, lock/unlock)
- Organizations, Branches, Departments, Designations
- Employees, Roles, Users, Permissions
- Asset Categories, Assets
- Asset Assignments, Transfers, Returns
- Vendors, Customers
- Purchase Requests, Purchase Orders
- Inventory Items, Stock Movements, Consumables
- Maintenance Records
- Service Tickets
- Notifications, Audit Logs, System Settings
- Dashboard Summary

### Frontend Only (No Backend)
- Finance / Invoices (MockDatabaseService)

### Partially Mocked
- ApprovalCenter (frontend not fully migrated to real API)

### Roadmap (Not Started)
- Phase 2: Native Android/iOS, Advanced Analytics, BI Dashboards,
           Project Management, Task Management, Resource Allocation
- Phase 3: Multi-Tenant SaaS, Cloud Deployment, AI Assistant,
           Predictive Maintenance, RFID Tracking, IoT Integration

---

## 17. CODING STANDARDS

### Naming
- Commands: `{Verb}{Entity}Command` (sealed record)
- Handlers: `{Verb}{Entity}CommandHandler` (sealed class)
- Validators: `{Verb}{Entity}CommandValidator` (sealed class)
- Queries: `Get{Entity}Query`, `Get{Entities}Query`
- Responses: `{Entity}Response`, `{Entity}ListItemResponse`
- Repositories: `I{Entity}Repository`, `{Entity}Repository`
- Endpoints file: `{Entity}Endpoints.cs` with `Map{Entity}Endpoints()`
- Permission codes: `Module.Action`
- API routes: `/api/{kebab-case}` or `/api/enterprise-operations/{module}`

### File Location
- `Application/Features/{Domain}/Commands/{Op}/{File}.cs`
- `Application/Features/{Domain}/Queries/{Op}/{File}.cs`
- `Infrastructure/Repository/{Entity}Repository.cs`
- `Infrastructure/Persistence/Configurations/{Entity}Configuration.cs`
- `API/Endpoints/{Domain}/{Domain}Endpoints.cs`
- `Web/Features/{Domain}/` (Blazor pages)
- `Web/Infrastructure/{Domain}/` (typed API clients)

### Key Rules
- `AsParameters` for GET query binding in minimal APIs
- `[FromBody]` for POST/PUT body binding
- No-tracking reads (`AsNoTracking()`) for all list/history queries
- Tracking reads for single-entity fetches that will be updated
- Always call `asset.UpdatedAtUtc = DateTime.UtcNow` when modifying entities
- Handlers validate org boundaries manually (don't trust IDs from request body alone)
- Never throw for business failures — use Result.Failure("message")
- Never call SaveChanges more than once in a single handler

---

## 18. SEED DATA

### OrganizationSeeder
Seeds a default "NexAsset" organization if no orgs exist.
Code: NEX, India/Gujarat/Ahmedabad, INR currency, Asia/Kolkata timezone.
Idempotent (skips if any org exists).

### PermissionSeeder
Seeds all 97 permissions from the canonical Matrix.
Idempotent — inserts only codes that don't exist yet.
Run on every startup via DatabaseInitializer.InitializeAsync().

---

## 19. ARCHITECTURAL PRINCIPLES (Do Not Violate)

1. All business logic lives in Application handlers — never in endpoints or repositories
2. Endpoints are thin adapters — receive request, send to MediatR, return result
3. No module bypasses the command/query handler for business workflows
4. Organization boundary is enforced at the database level — no application code can escape it
5. Soft delete is explicit — every read must include `&& !x.IsDeleted`
6. Result<T> for business failures, exceptions for infrastructure failures only
7. Validation is a pipeline behavior — not duplicated in handlers
8. Repositories return domain entities — mapping to DTOs happens in handlers via Mapperly
9. UnitOfWork.SaveChangesAsync() is called exactly once per handler
10. Identity tables are tenant-scoped manually in IdentityService, not via query filters

---

## 20. FUTURE DECISIONS TO TRACK

When these decisions are made, update this document:

- [ ] Finance module backend design (Invoices, financial reports)
- [ ] ApprovalCenter workflow implementation (formal multi-step approvals)
- [ ] Notification delivery mechanism (email, push, SignalR)
- [ ] Background job strategy (Hangfire, Quartz, hosted services)
- [ ] Multi-tenant SaaS migration strategy (current: single-DB multi-tenant)
- [ ] Mobile app API contract (REST vs GraphQL for mobile)
- [ ] RFID/IoT integration architecture
- [ ] Distributed caching strategy (Redis) for production deployments
- [ ] Asset barcode/QR generation
- [ ] Report export format (PDF, Excel)
