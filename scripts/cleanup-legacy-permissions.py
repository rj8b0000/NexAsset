#!/usr/bin/env python3
"""One-time cleanup: removes legacy singular-module permission duplicates
(Asset.*, Employee.*, Organization.Manage, ...) left by the demo seeder and
remaps the demo roles onto the canonical Module.Action permission matrix."""
import json
import urllib.request
import http.cookiejar

BASE = "http://localhost:5080"

CANONICAL = {
    "Organizations": ["View", "Create", "Update", "Delete"],
    "Branches": ["View", "Create", "Update", "Delete"],
    "Departments": ["View", "Create", "Update", "Delete"],
    "Designations": ["View", "Create", "Update", "Delete"],
    "Employees": ["View", "Create", "Update", "Delete", "Activate"],
    "Roles": ["View", "Create", "Update", "Delete", "AssignPermissions"],
    "Users": ["View", "Create", "Update", "ManageRoles", "ResetPassword"],
    "Permissions": ["View", "Create", "Update", "Delete"],
    "AssetCategories": ["View", "Create", "Update", "Delete"],
    "Assets": ["View", "Create", "Update", "Delete"],
    "AssetAssignments": ["Assign", "Unassign", "Transfer", "Return", "ViewHistory"],
    "Vendors": ["View", "Create", "Update", "Delete"],
    "PurchaseRequests": ["View", "Create", "Approve"],
    "PurchaseOrders": ["View", "Create", "UpdateStatus"],
    "Inventory": ["View", "Create", "Update", "RecordMovement"],
    "Consumables": ["View", "Create", "Update"],
    "Maintenance": ["View", "Create", "UpdateStatus"],
    "Customers": ["View", "Create", "Update", "Delete"],
    "ServiceTickets": ["View", "Create", "Update"],
    "Notifications": ["View", "Create", "MarkRead"],
    "AuditLogs": ["View", "Create"],
    "SystemSettings": ["View", "Manage"],
    "Dashboard": ["View"],
    "Reports": ["View"],
    "Finance": ["View"],
}
CANONICAL_CODES = {f"{m}.{a}" for m, acts in CANONICAL.items() for a in acts}

ALL = sorted(CANONICAL_CODES)
VIEW_ALL = [c for c in ALL if c.endswith(".View") or c == "AssetAssignments.ViewHistory"]

ROLE_MAP = {
    # SuperAdmin gets everything explicitly too (enforcement also bypasses it).
    "SuperAdmin": ALL,
    # Managing organizations is SuperAdmin-only: an OrganizationAdmin works inside one
    # organization and must not create, rename, or even list the others.
    "OrganizationAdmin": [
        *[f"Branches.{a}" for a in CANONICAL["Branches"]],
        *[f"Departments.{a}" for a in CANONICAL["Departments"]],
        *[f"Designations.{a}" for a in CANONICAL["Designations"]],
        *[f"Employees.{a}" for a in CANONICAL["Employees"]],
        *[f"AssetCategories.{a}" for a in CANONICAL["AssetCategories"]],
        *[f"Assets.{a}" for a in CANONICAL["Assets"]],
        *[f"AssetAssignments.{a}" for a in CANONICAL["AssetAssignments"]],
        "Roles.View", "Permissions.View", "Reports.View", "Dashboard.View",
        "Notifications.View", "Notifications.MarkRead",
        "AuditLogs.View", "SystemSettings.View", "SystemSettings.Manage",
        "Users.View", "Users.Create", "Users.Update", "Users.ManageRoles", "Users.ResetPassword",
    ],
    "HR Manager": [
        *[f"Employees.{a}" for a in CANONICAL["Employees"]],
        "Organizations.View", "Branches.View", "Departments.View", "Designations.View",
        "Reports.View", "Dashboard.View", "Notifications.View", "Notifications.MarkRead",
    ],
    "Manager": [
        "Organizations.View", "Branches.View", "Departments.View", "Employees.View",
        "Assets.View", "AssetCategories.View",
        *[f"AssetAssignments.{a}" for a in CANONICAL["AssetAssignments"]],
        "Vendors.View", "PurchaseRequests.View", "PurchaseRequests.Create", "PurchaseRequests.Approve",
        "PurchaseOrders.View", "PurchaseOrders.Create", "PurchaseOrders.UpdateStatus",
        "Inventory.View", "Inventory.RecordMovement", "Consumables.View",
        "Maintenance.View", "Maintenance.Create", "Maintenance.UpdateStatus",
        "Customers.View", "ServiceTickets.View", "ServiceTickets.Create", "ServiceTickets.Update",
        "Reports.View", "Dashboard.View", "Notifications.View", "Notifications.MarkRead",
    ],
    "Employee": [
        "Dashboard.View", "Assets.View", "Employees.View", "Inventory.View",
        "PurchaseRequests.View", "PurchaseRequests.Create",
        "ServiceTickets.View", "ServiceTickets.Create",
        "Notifications.View", "Notifications.MarkRead",
    ],
}

jar = http.cookiejar.CookieJar()
opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(jar))


def call(method, path, body=None):
    req = urllib.request.Request(BASE + path, method=method)
    data = None
    if body is not None:
        data = json.dumps(body).encode()
        req.add_header("Content-Type", "application/json")
    try:
        with opener.open(req, data) as r:
            raw = r.read()
            return r.status, json.loads(raw) if raw else None
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode()[:200]


def main():
    st, _ = call("POST", "/api/auth/login", {"email": "admin@nexasset.com", "password": "Admin@123"})
    assert st == 200, f"login failed: {st}"

    st, page = call("GET", "/api/permissions?PageNumber=1&PageSize=500&SortBy=Code&Descending=false")
    assert st == 200, f"list perms failed: {st} {page}"
    perms = page["items"]
    by_code = {p["code"]: p["id"] for p in perms}

    legacy = [p for p in perms if p["code"] not in CANONICAL_CODES]
    print(f"{len(perms)} permissions; deleting {len(legacy)} legacy:")
    for p in legacy:
        st, err = call("DELETE", f"/api/permissions/{p['id']}")
        print(f"  {'OK ' if st in (200, 204) else 'ERR'} {p['code']} ({st}{'' if st in (200,204) else ' ' + str(err)})")

    st, roles = call("GET", "/api/roles?PageNumber=1&PageSize=100&SortBy=Name&Descending=false")
    assert st == 200, f"list roles failed: {st} {roles}"
    role_ids = {r["name"]: r["id"] for r in roles["items"]}
    print("Roles:", ", ".join(role_ids))

    for role_name, codes in ROLE_MAP.items():
        rid = role_ids.get(role_name)
        if not rid:
            print(f"  !! role '{role_name}' not found, skipped")
            continue
        ok = dup = err = 0
        for code in codes:
            pid = by_code.get(code)
            if not pid:
                print(f"  !! unknown code {code}")
                continue
            st, body = call("POST", "/api/permissions/roles/assign", {"roleId": rid, "permissionId": pid})
            if st in (200, 201, 204):
                ok += 1
            elif st in (400, 409):
                dup += 1
            else:
                err += 1
                print(f"  ERR {role_name} <- {code}: {st} {body}")
        print(f"  {role_name}: {ok} assigned, {dup} already had, {err} errors")


if __name__ == "__main__":
    main()
