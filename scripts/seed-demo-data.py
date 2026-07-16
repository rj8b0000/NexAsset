#!/usr/bin/env python3
"""Seed NexAsset with demo data across every module.

Creates organizations, branches, departments, designations, permissions (mapped to the
seeded roles), employees with login users of different roles (password: Password@123),
asset categories, assets, vendors, customers, inventory (+ stock movements), consumables,
purchase requests/orders with workflow statuses, maintenance records, service tickets,
notifications, and system settings.

Codes are unique per run-family; re-running against an already-seeded database will
report conflicts for duplicate codes — that is expected.

Usage: python3 scripts/seed-demo-data.py [base_url]   (default http://localhost:5080)
"""
import json
import sys
import urllib.request

BASE = (sys.argv[1] if len(sys.argv) > 1 else "http://localhost:5080").rstrip("/")
EO = f"{BASE}/api/enterprise-operations"
created, failed = 0, 0


def call(method, url, body=None):
    global created, failed
    data = json.dumps(body).encode() if body is not None else None
    req = urllib.request.Request(url, data=data, method=method,
                                 headers={"Content-Type": "application/json"})
    try:
        with urllib.request.urlopen(req) as resp:
            raw = resp.read().decode()
            created += 1
            return json.loads(raw) if raw else {}
    except urllib.error.HTTPError as e:
        failed += 1
        print(f"  !! {method} {url.replace(BASE, '')} -> {e.code}: {e.read().decode()[:120]}")
        return {}


def post(url, body):
    return call("POST", url, body)


print("== Organizations ==")
orgs = {}
for code, name in [("GLOBEX", "Globex Industries"), ("INITECH", "Initech Solutions")]:
    r = post(f"{BASE}/api/organizations", {
        "code": code, "name": name, "legalName": f"{name} LLC",
        "email": f"info@{code.lower()}.com", "currency": "USD", "timeZone": "UTC",
        "country": "USA", "city": "Austin"})
    if r.get("id"):
        orgs[code] = r["id"]
# include existing ACME if present
existing = call("GET", f"{BASE}/api/organizations?PageNumber=1&PageSize=50&Descending=false")
for item in existing.get("items", []):
    orgs.setdefault(item["code"], item["id"])
org_ids = list(orgs.values())
print(f"  organizations available: {list(orgs.keys())}")

print("== Branches ==")
branches = {}
for code, name, org in [
    ("GLB-HQ", "Globex HQ", "GLOBEX"), ("GLB-W", "Globex Warehouse", "GLOBEX"),
    ("INI-HQ", "Initech HQ", "INITECH"), ("INI-LAB", "Initech Lab", "INITECH"),
    ("ACME-E", "Acme East Office", "ACME"),
]:
    if org not in orgs:
        continue
    r = post(f"{BASE}/api/branches", {"organizationId": orgs[org], "code": code, "name": name,
                                      "city": "Austin", "country": "USA"})
    if r.get("id"):
        branches[code] = r["id"]

print("== Departments ==")
departments = {}
for code, name, org in [
    ("GLB-IT", "IT", "GLOBEX"), ("GLB-FIN", "Finance", "GLOBEX"), ("GLB-OPS", "Operations", "GLOBEX"),
    ("INI-ENG", "Engineering", "INITECH"), ("INI-HR", "Human Resources", "INITECH"),
    ("ACME-IT", "IT", "ACME"),
]:
    if org not in orgs:
        continue
    r = post(f"{BASE}/api/departments", {"organizationId": orgs[org], "code": code, "name": name})
    if r.get("id"):
        departments[code] = r["id"]

print("== Designations ==")
designations = {}
for title, org, dept in [
    ("Software Engineer", "INITECH", "INI-ENG"), ("HR Executive", "INITECH", "INI-HR"),
    ("IT Administrator", "GLOBEX", "GLB-IT"), ("Accountant", "GLOBEX", "GLB-FIN"),
    ("Operations Lead", "GLOBEX", "GLB-OPS"), ("Support Engineer", "ACME", "ACME-IT"),
]:
    if org not in orgs:
        continue
    r = post(f"{BASE}/api/designations", {
        "organizationId": orgs[org], "departmentId": departments.get(dept), "title": title})
    if r.get("id"):
        designations[title] = r["id"]

print("== Permissions + role mapping ==")
perm_defs = [
    ("Organization.Manage", "Manage Organizations"), ("Employee.Manage", "Manage Employees"),
    ("Employee.View", "View Employees"), ("Asset.Manage", "Manage Assets"),
    ("Asset.Assign", "Assign Assets"), ("Procurement.Approve", "Approve Purchase Requests"),
    ("Procurement.Create", "Create Purchase Requests"), ("Inventory.Manage", "Manage Inventory"),
    ("Ticket.Resolve", "Resolve Service Tickets"), ("Report.View", "View Reports"),
]
perms = {}
for code, name in perm_defs:
    r = post(f"{BASE}/api/permissions", {"code": code, "name": name, "description": name})
    if r.get("id"):
        perms[code] = r["id"]

roles = {i["name"]: i["id"] for i in
         call("GET", f"{BASE}/api/roles?PageNumber=1&PageSize=50&Descending=false").get("items", [])}
role_perm_map = {
    "SuperAdmin": list(perms.keys()),
    "OrganizationAdmin": ["Organization.Manage", "Employee.Manage", "Asset.Manage", "Report.View"],
    "HR Manager": ["Employee.Manage", "Employee.View", "Report.View"],
    "Manager": ["Employee.View", "Asset.Assign", "Procurement.Approve", "Report.View"],
    "Employee": ["Employee.View", "Procurement.Create"],
}
for role, plist in role_perm_map.items():
    if role not in roles:
        continue
    for p in plist:
        if p in perms:
            post(f"{BASE}/api/permissions/roles/assign", {"roleId": roles[role], "permissionId": perms[p]})

print("== Employees (login users with roles, password: Password@123) ==")
employees = {}
emp_defs = [
    ("GLB-001", "Hank", "Scorpio", "GLOBEX", "GLB-HQ", "GLB-OPS", "Operations Lead", ["OrganizationAdmin"]),
    ("GLB-002", "Frank", "Grimes", "GLOBEX", "GLB-HQ", "GLB-FIN", "Accountant", ["Employee"]),
    ("GLB-003", "Mindy", "Simmons", "GLOBEX", "GLB-W", "GLB-IT", "IT Administrator", ["Manager"]),
    ("INI-001", "Peter", "Gibbons", "INITECH", "INI-HQ", "INI-ENG", "Software Engineer", ["Employee"]),
    ("INI-002", "Bill", "Lumbergh", "INITECH", "INI-HQ", "INI-ENG", "Software Engineer", ["Manager"]),
    ("INI-003", "Joanna", "Chotchkie", "INITECH", "INI-LAB", "INI-HR", "HR Executive", ["HR Manager"]),
    ("INI-004", "Milton", "Waddams", "INITECH", "INI-LAB", "INI-ENG", "Software Engineer", ["Employee"]),
    ("ACME-01", "Wile", "Coyote", "ACME", "ACME-E", "ACME-IT", "Support Engineer", ["Employee"]),
    ("ACME-02", "Road", "Runner", "ACME", "ACME-E", "ACME-IT", "Support Engineer", ["Manager"]),
]
for code, first, last, org, br, dept, desig, rls in emp_defs:
    if org not in orgs:
        continue
    r = post(f"{BASE}/api/employees", {
        "employeeCode": code, "firstName": first, "lastName": last,
        "email": f"{first.lower()}.{last.lower()}@{org.lower()}.com", "password": "Password@123",
        "phone": "+1-555-0100", "organizationId": orgs[org], "branchId": branches.get(br),
        "departmentId": departments.get(dept), "designationId": designations.get(desig),
        "joiningDate": "2025-01-15", "employmentStatus": 1, "roles": rls})
    if r.get("id"):
        employees[code] = r["id"]

print("== Asset categories ==")
categories = {}
for code, name, org in [
    ("GLB-IT-EQ", "IT Equipment", "GLOBEX"), ("GLB-FURN", "Furniture", "GLOBEX"),
    ("INI-COMP", "Computers", "INITECH"), ("INI-NET", "Networking", "INITECH"),
    ("ACME-TOOL", "Tools", "ACME"),
]:
    if org not in orgs:
        continue
    r = post(f"{BASE}/api/asset-categories", {"organizationId": orgs[org], "code": code, "name": name})
    if r.get("id"):
        categories[code] = r["id"]

print("== Assets ==")
assets = {}
asset_defs = [
    ("GLB-AST-01", "Dell OptiPlex 7010", "GLOBEX", "GLB-IT-EQ", "GLB-HQ", "Dell", 1200),
    ("GLB-AST-02", "HP LaserJet Pro", "GLOBEX", "GLB-IT-EQ", "GLB-HQ", "HP", 450),
    ("GLB-AST-03", "Standing Desk", "GLOBEX", "GLB-FURN", "GLB-W", "Ikea", 600),
    ("GLB-AST-04", "Herman Miller Chair", "GLOBEX", "GLB-FURN", "GLB-W", "HM", 950),
    ("INI-AST-01", "MacBook Air M3", "INITECH", "INI-COMP", "INI-HQ", "Apple", 1400),
    ("INI-AST-02", "ThinkPad X1 Carbon", "INITECH", "INI-COMP", "INI-HQ", "Lenovo", 1700),
    ("INI-AST-03", "Cisco Switch 24p", "INITECH", "INI-NET", "INI-LAB", "Cisco", 2200),
    ("INI-AST-04", "Ubiquiti AP Pro", "INITECH", "INI-NET", "INI-LAB", "Ubiquiti", 250),
    ("ACME-AST-1", "Anvil Press", "ACME", "ACME-TOOL", "ACME-E", "Acme", 5000),
    ("ACME-AST-2", "Rocket Skates", "ACME", "ACME-TOOL", "ACME-E", "Acme", 800),
]
for code, name, org, cat, br, brand, cost in asset_defs:
    if org not in orgs or cat not in categories:
        continue
    r = post(f"{BASE}/api/assets", {
        "organizationId": orgs[org], "categoryId": categories[cat], "branchId": branches.get(br),
        "assetCode": code, "assetName": name, "brand": brand, "purchaseCost": cost,
        "currentValue": cost * 0.8, "purchaseDate": "2025-03-01", "assetStatus": 1,
        "serialNumber": f"SN-{code}"})
    if r.get("id"):
        assets[code] = r["id"]

print("== Asset assignments ==")
for asset, emp in [("GLB-AST-01", "GLB-002"), ("INI-AST-01", "INI-001"), ("INI-AST-02", "INI-002")]:
    if asset in assets and emp in employees:
        post(f"{BASE}/api/asset-assignments/assign", {
            "assetId": assets[asset], "employeeId": employees[emp],
            "assignedDate": "2025-04-01", "remarks": "Initial issue"})

print("== Vendors ==")
vendors = {}
for code, name, org in [
    ("GLB-VEN-1", "Dell Enterprise", "GLOBEX"), ("GLB-VEN-2", "Staples Business", "GLOBEX"),
    ("INI-VEN-1", "Apple Business", "INITECH"), ("INI-VEN-2", "CDW Networks", "INITECH"),
    ("ACME-VEN1", "Roadrunner Supplies", "ACME"),
]:
    if org not in orgs:
        continue
    r = post(f"{EO}/vendors", {"organizationId": orgs[org], "code": code, "name": name,
                               "email": f"sales@{code.lower().replace('-', '')}.com",
                               "taxNumber": f"GST-{code}"})
    if r.get("id"):
        vendors[code] = r["id"]

print("== Customers ==")
customers = {}
for code, name, org in [
    ("GLB-CUS-1", "Springfield Power", "GLOBEX"), ("GLB-CUS-2", "Kwik-E-Mart", "GLOBEX"),
    ("INI-CUS-1", "Chotchkies Restaurant", "INITECH"), ("INI-CUS-2", "Flingers Inc", "INITECH"),
    ("ACME-CUS1", "Desert Outfitters", "ACME"),
]:
    if org not in orgs:
        continue
    r = post(f"{EO}/customers", {"organizationId": orgs[org], "code": code, "name": name,
                                 "email": f"contact@{code.lower().replace('-', '')}.com"})
    if r.get("id"):
        customers[code] = r["id"]

print("== Inventory + stock movements ==")
inventory = {}
inv_defs = [
    ("GLB-INV-1", "HDMI Cables", "GLOBEX", "GLB-W", 100, 20),
    ("GLB-INV-2", "Laptop Chargers", "GLOBEX", "GLB-W", 40, 10),
    ("GLB-INV-3", "Printer Toner", "GLOBEX", "GLB-HQ", 8, 10),
    ("INI-INV-1", "Cat6 Patch Cords", "INITECH", "INI-LAB", 200, 50),
    ("INI-INV-2", "SSD 1TB", "INITECH", "INI-LAB", 25, 5),
    ("INI-INV-3", "Keyboards", "INITECH", "INI-HQ", 30, 10),
    ("ACME-INV1", "TNT Sticks", "ACME", "ACME-E", 500, 100),
]
for code, name, org, br, stock, reorder in inv_defs:
    if org not in orgs:
        continue
    r = post(f"{EO}/inventory", {
        "organizationId": orgs[org], "branchId": branches.get(br), "itemCode": code,
        "itemName": name, "currentStock": stock, "reservedStock": 0,
        "reorderLevel": reorder, "unitOfMeasure": "pcs"})
    if r.get("id"):
        inventory[code] = r["id"]
for code, mtype, qty in [("GLB-INV-1", 2, 30), ("GLB-INV-2", 1, 10), ("INI-INV-1", 2, 60), ("INI-INV-2", 2, 5)]:
    if code in inventory:
        post(f"{EO}/inventory/stock-movements", {
            "inventoryItemId": inventory[code], "movementType": mtype, "quantity": qty,
            "referenceNumber": f"MV-{code}", "remarks": "Seeded movement"})

print("== Consumables ==")
for code, name, inv in [
    ("GLB-CON-1", "HDMI Cable 2m", "GLB-INV-1"), ("GLB-CON-2", "65W USB-C Charger", "GLB-INV-2"),
    ("INI-CON-1", "Patch Cord 1m", "INI-INV-1"), ("INI-CON-2", "Mechanical Keyboard", "INI-INV-3"),
]:
    if inv in inventory:
        post(f"{EO}/consumables", {"inventoryItemId": inventory[inv], "consumableCode": code, "name": name})

print("== Purchase requests + orders ==")
prs = {}
pr_defs = [
    ("GLB-PR-01", "Replacement monitors", "GLOBEX", "GLB-002", 3500, 3),
    ("GLB-PR-02", "Office chairs restock", "GLOBEX", "GLB-002", 5200, 2),
    ("INI-PR-01", "Dev laptops batch", "INITECH", "INI-001", 12000, 3),
    ("INI-PR-02", "Rack upgrade", "INITECH", "INI-002", 8000, 2),
    ("ACME-PR-1", "Gunpowder resupply", "ACME", "ACME-01", 950, 1),
]
for code, title, org, emp, amount, status in pr_defs:
    if org not in orgs or emp not in employees:
        continue
    r = post(f"{EO}/purchase-requests", {
        "organizationId": orgs[org], "requestNumber": code, "title": title,
        "requestedByEmployeeId": employees[emp], "requestDate": "2026-07-01",
        "estimatedAmount": amount})
    if r.get("id"):
        prs[code] = r["id"]
        if status != 1:
            post(f"{EO}/purchase-requests/{r['id']}/status", {
                "status": status, "approvalRemarks": "Seeded decision"})
po_defs = [
    ("GLB-PO-01", "GLOBEX", "GLB-PR-01", "GLB-VEN-1", 3400, 6),
    ("INI-PO-01", "INITECH", "INI-PR-01", "INI-VEN-1", 11800, 6),
    ("INI-PO-02", "INITECH", None, "INI-VEN-2", 2100, 2),
]
for code, org, pr, ven, amount, status in po_defs:
    if org not in orgs or ven not in vendors:
        continue
    r = post(f"{EO}/purchase-orders", {
        "organizationId": orgs[org], "orderNumber": code, "purchaseRequestId": prs.get(pr),
        "vendorId": vendors[ven], "orderDate": "2026-07-05",
        "expectedDeliveryDate": "2026-07-25", "totalAmount": amount})
    if r.get("id") and status != 1:
        post(f"{EO}/purchase-orders/{r['id']}/status", {"status": status, "remarks": "Seeded"})

print("== Maintenance ==")
for asset, mtype, title, status in [
    ("GLB-AST-02", 2, "Paper jam mechanism repair", 2),
    ("GLB-AST-01", 1, "Annual PC health check", 1),
    ("INI-AST-03", 1, "Firmware upgrade window", 2),
    ("ACME-AST-1", 2, "Hydraulic seal replacement", 3),
]:
    if asset not in assets:
        continue
    r = post(f"{EO}/maintenance", {
        "assetId": assets[asset], "maintenanceType": mtype, "requestedDate": "2026-07-10",
        "title": title, "cost": 150})
    if r.get("id") and status != 1:
        post(f"{EO}/maintenance/{r['id']}/status", {"status": status, "scheduledDate": "2026-07-20"})

print("== Service tickets ==")
for code, title, org, cust, emp, prio, status in [
    ("GLB-ST-01", "Power meter offline", "GLOBEX", "GLB-CUS-1", "GLB-003", 4, 3),
    ("GLB-ST-02", "POS printer error", "GLOBEX", "GLB-CUS-2", "GLB-003", 2, 1),
    ("INI-ST-01", "Menu kiosk frozen", "INITECH", "INI-CUS-1", "INI-002", 3, 2),
    ("INI-ST-02", "WiFi drops in kitchen", "INITECH", "INI-CUS-2", None, 2, 1),
    ("ACME-ST-1", "Anvil delivery damaged", "ACME", "ACME-CUS1", "ACME-02", 3, 4),
]:
    if org not in orgs or cust not in customers:
        continue
    r = post(f"{EO}/service-tickets", {
        "organizationId": orgs[org], "customerId": customers[cust], "ticketNumber": code,
        "title": title, "priority": prio})
    if r.get("id") and (emp or status != 1):
        call("PUT", f"{EO}/service-tickets/{r['id']}", {
            "assignedToEmployeeId": employees.get(emp), "priority": prio, "status": status,
            "resolution": "Resolved during seeding" if status == 4 else None})

print("== Notifications + settings ==")
for title, msg, ntype in [
    ("Low stock alert", "Printer Toner is below its reorder level.", 2),
    ("PO delivered", "GLB-PO-01 arrived at Globex Warehouse.", 3),
    ("New employee onboarded", "Peter Gibbons joined Initech Engineering.", 1),
    ("Maintenance overdue", "Anvil Press repair passed its scheduled date.", 4),
]:
    post(f"{EO}/notifications", {"title": title, "message": msg, "notificationType": ntype})
for key, value, desc in [
    ("Asset.DepreciationMethod", "StraightLine", "Default depreciation method"),
    ("Procurement.ApprovalThreshold", "5000", "Amount above which PRs need approval"),
    ("Inventory.LowStockAlerts", "true", "Send alerts when stock hits reorder level"),
    ("Tickets.AutoAssign", "false", "Auto-assign new tickets to on-call engineer"),
]:
    post(f"{EO}/system-settings", {"key": key, "value": value, "description": desc})

print(f"\nDone: {created} requests succeeded, {failed} failed.")
