# Enterprise Operations API

Base route: `/api/enterprise-operations`

## Main Routes

- `/vendors`
- `/customers`
- `/purchase-requests`
- `/purchase-orders`
- `/inventory`
- `/inventory/stock-movements`
- `/inventory/{inventoryItemId}/stock-history`
- `/consumables`
- `/maintenance`
- `/service-tickets`
- `/notifications`
- `/audit-logs`
- `/system-settings`
- `/dashboard/{organizationId}`

Paged list endpoints support:

- `pageNumber`
- `pageSize`
- `search`
- `sortBy`
- `descending`
