# Database Relationships

Enterprise Operations builds on the existing foundation, HR, and asset modules.

- `Vendor` belongs to an organization.
- `PurchaseRequest` belongs to an organization and is requested by an employee.
- `PurchaseOrder` belongs to an organization, references a vendor, and may reference a purchase request.
- `InventoryItem` belongs to an organization and may be scoped to a branch.
- `StockMovement` belongs to an inventory item.
- `Consumable` is backed by an inventory item.
- `MaintenanceRecord` belongs to an asset.
- `Customer` belongs to an organization.
- `ServiceTicket` belongs to an organization and customer, and may be assigned to an employee.
- `Notification` may target a user.
- `AuditLog` captures entity/action/user metadata.
- `SystemSetting` is global or organization-scoped.
