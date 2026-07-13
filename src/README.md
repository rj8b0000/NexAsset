# NexAsset

NexAsset is an enterprise asset management ERP built with ASP.NET Core, Blazor, PostgreSQL, CQRS, MediatR, FluentValidation, Mapperly, ASP.NET Identity, repositories and a Unit of Work. The application is organized around foundation data, HR, asset lifecycle management and enterprise operations.

## Features

- Authentication and account administration with ASP.NET Identity.
- Foundation modules for organizations, branches, departments and designations.
- HR modules for employees, roles and permissions.
- Asset management for categories, assets, assignments, transfers and returns.
- Enterprise operations for vendors, procurement, inventory, consumables, maintenance, customers, service tickets, notifications, audit logs, system settings and dashboard summaries.
- Minimal API endpoints with Swagger metadata and a centralized Postman collection.
- Pagination, search, sorting and validation across list and command workflows.

## Architecture

The project follows the existing layered architecture:

```text
Minimal API Endpoint
MediatR
Validation Pipeline
Command / Query Handler
Repository
Unit Of Work
EF Core
PostgreSQL
```

No module should bypass the command/query handler layer for business workflows.

## Technology Stack

- .NET 10
- ASP.NET Core Minimal APIs
- ASP.NET Identity
- Entity Framework Core
- PostgreSQL
- MediatR
- FluentValidation
- Mapperly
- Blazor Web
- Swagger / OpenAPI
- Postman

## Folder Structure

```text
NexAsset.API              HTTP API, Minimal API endpoint groups, Swagger
NexAsset.Application      CQRS requests, handlers, validators, DTOs, mapping contracts
NexAsset.Domain           Entities, enums and domain primitives
NexAsset.Infrastructure   EF Core, Identity, repositories, persistence and DI
NexAsset.Web              Blazor frontend shell and feature pages
docs                      API, module, workflow, database and Postman documentation
```

## Installation

1. Install the .NET 10 SDK.
2. Install PostgreSQL.
3. Configure the API connection string in `NexAsset.API/appsettings.Development.json` or user secrets.
4. Restore packages from the solution folder.

```bash
dotnet restore NexAsset.sln
```

## Database Setup

The API uses PostgreSQL through Entity Framework Core. Apply migrations from the solution folder:

```bash
dotnet ef database update --project src/NexAsset.Infrastructure --startup-project src/NexAsset.API
```

Migration files live under:

```text
NexAsset.Infrastructure/Persistence/Migrations
```

## Running

Backend API:

```bash
dotnet run --project src/NexAsset.API
```

Frontend Web:

```bash
dotnet run --project src/NexAsset.Web
```

## API Documentation

Swagger is exposed by the API project in development. The centralized Postman assets are available at:

```text
docs/postman/NexAsset.postman_collection.json
docs/postman/NexAsset.postman_environment.json
```

The collection is organized into Authentication, Foundation, HR, Asset Management and Enterprise Operations folders and uses environment variables for IDs and tokens.

## Authentication Flow

1. Register or seed an administrator account.
2. Sign in through the Authentication endpoints.
3. Store the returned access token in the `AccessToken` Postman environment variable.
4. Use bearer authentication for protected module endpoints.

## Demo Credentials

Use the seeded administrator credentials configured by the infrastructure seed data or local development settings. Rotate these values before any shared or hosted deployment.

## Business Rules

- Business validation returns `Result<T>` failures rather than throwing exceptions.
- Soft-deleted records are excluded from normal reads and paged lists.
- List endpoints support `PageNumber`, `PageSize`, `Search`, `SortBy` and `Descending`.
- Repositories use no-tracking reads for list and history queries.

## Documentation

Additional documentation:

- `docs/api-enterprise-operations.md`
- `docs/architecture/database-relationships.md`
- `docs/modules/enterprise-operations.md`
- `docs/workflows/enterprise-operations-workflows.md`

## Deployment

Set production values for:

- PostgreSQL connection string
- JWT signing configuration
- Allowed CORS origins
- ASP.NET Core environment
- Any SMTP, storage or integration settings introduced later

Run migrations before exposing the API to users, then verify Swagger, authentication, authorization, CRUD workflows, pagination, search, sorting and validation.

## Screenshots

Screenshots can be captured from the Blazor web project during the portfolio demo preparation step.

## Future Enhancements

- Approval workflow visualization.
- Email and in-app notification delivery adapters.
- Asset barcode and QR label generation.
- Advanced reporting and export workflows.
- Multi-tenant organization administration.
