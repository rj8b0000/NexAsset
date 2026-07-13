# Enterprise Operations Modules

This phase adds the remaining ERP/EAM operations surface while preserving the existing NexAsset request flow:

Minimal API endpoint -> MediatR -> validation pipeline -> handler -> repository -> unit of work -> EF Core.

## Modules

- Vendors
- Purchase Requests
- Purchase Orders
- Inventory Items
- Stock Movements
- Consumables
- Maintenance Records
- Customers
- Service Tickets
- Notifications
- Audit Logs
- System Settings
- Dashboard Summary

## Workflow Rules

- Purchase requests and purchase orders move through `PendingApproval`, `Approved`, `Rejected`, and `Cancelled` states.
- Stock movements update the current inventory quantity and store movement history.
- Maintenance records track preventive and corrective work with schedule and completion dates.
- Service tickets track assignment, priority, status, resolution, and comments.
- System settings are upserted by organization and key.
- Dashboard summary is repository-backed and uses current persisted data.
