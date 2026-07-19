#!/usr/bin/env python3
"""Seed realistic demo data for Rudraksh Projects Pvt Ltd — a solar EPC / electrical
contracting business (solar EPC, substations, O&M, electrical testing, cable laying,
HT/LT panels, rooftop & ground-mount solar).

Populates asset categories, assets, vendors, customers, inventory (+ movements),
consumables, employees (with logins), plus a few purchase requests/orders, maintenance
records and service tickets — all inside the existing Rudraksh organization, against its
existing branches/departments/designations.

Signs in as the SuperAdmin (account administration + all data is permission-gated now).
Codes are unique to this script; re-running reports conflicts for duplicates (expected).

Usage: python3 scripts/seed-rudraksh.py [base_url]   (default http://localhost:5080)
"""
import http.cookiejar
import json
import sys
import urllib.error
import urllib.request

BASE = (sys.argv[1] if len(sys.argv) > 1 else "http://localhost:5080").rstrip("/")
EO = f"{BASE}/api/enterprise-operations"

ADMIN_EMAIL = "admin@nexasset.com"
ADMIN_PASSWORD = "Admin@123"

ORG = "6a8b2534-816a-4953-b46b-d9c4ab834f19"  # Rudraksh Projects Pvt Ltd

# Existing branches (code -> id)
BR = {
    "AHMD": "38655190-fe7a-4439-8404-bb938895fc0e",
    "BLR": "e94e1ffe-ffa9-4a7b-a580-aa2e26afcb41",
    "MUM": "3701ca6c-16e3-4adc-adaf-540ba519a7cb",
    "NOIDA": "12282375-dca4-4899-a0d6-46a900e10e14",
}
# Existing departments (short -> id)
DEPT = {
    "ADMIN": "56abbd20-1989-4c19-b0c1-8a6cb6e72dd9",
    "ENG": "fea84e57-c8c8-46fc-bfae-6e1bae8a3219",
    "FIN": "e3a36581-d13f-49b0-b8ad-c7c1f38dca8a",
    "HR": "ccf4fe5a-1b77-4a24-83d4-8167f81b0ba8",
    "IT": "570de16c-e8ba-446a-99cc-340081fc6f53",
    "OPS": "f6376fe1-b72d-4e69-b61e-4544a08b410a",
    "PROC": "35d8fdaf-fefd-4b75-b6e8-df3fa0e6af15",
    "SALES": "b854436c-9fd7-44e3-8496-59f73674bee5",
    "TECH": "90aa45a6-b857-403e-b7ed-c24b43c79228",
}
# Existing designations (title -> id)
DESIG = {
    "CEO": "d91f284a-3584-45e1-9ab1-287a4fcad0cd",
    "COO": "9598b314-c33e-42f2-a395-a420ff70cf0e",
    "Managing Director": "571c5fa7-c659-4be6-81b4-f6162351755a",
    "General Manager": "e8deb774-82c8-4cf4-a379-2072472f8cb2",
    "Chief Engineer": "5cce67fd-d4b2-4f9c-b4af-8cbd1cc4ef73",
    "Senior Electrical Engineer": "6b83bcf5-742c-4b01-b499-2a0b64c3d2fe",
    "Electrical Engineer": "9fcdfd72-d710-4d50-9124-b50347c079e3",
    "Solar Design Engineer": "685ae72a-46a2-4712-a186-a55fe8c6ef7c",
    "Site Engineer": "9d91a154-fa64-45e3-95da-19f00f7bb051",
    "Junior Engineer": "780c56b2-fa6b-49bf-9fe1-e3e55782ac49",
    "O&M Engineer": "6c4655a1-cade-487d-8a20-73b77a39d865",
    "Project Manager": "02209bb5-6667-4344-bfd6-31cf404ca160",
    "Project Coordinator": "57dc5806-6aa3-41ed-88a0-464923d1ef20",
    "Site Supervisor": "d1aaf35c-7fe4-4373-a994-70da4feb5d0a",
    "Operations Manager": "3ef64173-e8eb-4b7a-a33c-2509fd4f7ce5",
    "Procurement Manager": "a04d5e70-3b01-41fd-bbe5-7383354c34ac",
    "Accountant": "6bafd54b-b794-4846-b5a5-2cac4bdccff6",
    "HR Manager": "f0bfefbb-e6de-4996-bdcf-8984e9501643",
    "Sales Executive": "90b0c939-65f2-4104-9f76-fde007692bd2",
}

created, failed = 0, 0
_jar = http.cookiejar.CookieJar()
_opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(_jar))


def call(method, url, body=None):
    global created, failed
    data = json.dumps(body).encode() if body is not None else None
    req = urllib.request.Request(url, data=data, method=method,
                                 headers={"Content-Type": "application/json"})
    try:
        with _opener.open(req) as resp:
            raw = resp.read().decode()
            created += 1
            return json.loads(raw) if raw else {}
    except urllib.error.HTTPError as e:
        failed += 1
        print(f"  !! {method} {url.replace(BASE, '')} -> {e.code}: {e.read().decode()[:140]}")
        return {}


def post(url, body):
    return call("POST", url, body)


def login():
    st = call("POST", f"{BASE}/api/auth/login", {"email": ADMIN_EMAIL, "password": ADMIN_PASSWORD})
    if not st:
        print("!! login failed — cannot continue")
        sys.exit(1)
    print(f"Signed in as {ADMIN_EMAIL}")


login()

print("== Employees ==")
employees = {}
# (code, first, last, branch, dept, designation)  — branch None = company-wide (CEO/COO/MD)
emp_defs = [
    ("RP-EMP-001", "Rajesh", "Mehta", None, "ADMIN", "Managing Director"),
    ("RP-EMP-002", "Anjali", "Verma", None, "ADMIN", "CEO"),
    ("RP-EMP-003", "Vikram", "Shah", None, "ADMIN", "COO"),
    ("RP-EMP-004", "Suresh", "Patel", "AHMD", "ADMIN", "General Manager"),
    ("RP-EMP-005", "Deepak", "Sharma", "AHMD", "ENG", "Chief Engineer"),
    ("RP-EMP-006", "Amit", "Kumar", "MUM", "ENG", "Senior Electrical Engineer"),
    ("RP-EMP-007", "Sneha", "Reddy", "BLR", "ENG", "Solar Design Engineer"),
    ("RP-EMP-008", "Ravi", "Iyer", "NOIDA", "ENG", "Electrical Engineer"),
    ("RP-EMP-009", "Pooja", "Desai", "AHMD", "ENG", "Site Engineer"),
    ("RP-EMP-010", "Sanjay", "Rao", "NOIDA", "ENG", "Junior Engineer"),
    ("RP-EMP-011", "Manish", "Gupta", "MUM", "OPS", "Project Manager"),
    ("RP-EMP-012", "Arjun", "Singh", "BLR", "OPS", "O&M Engineer"),
    ("RP-EMP-013", "Neha", "Kulkarni", "MUM", "OPS", "Site Supervisor"),
    ("RP-EMP-014", "Kavita", "Bhat", "AHMD", "OPS", "Project Coordinator"),
    ("RP-EMP-015", "Kunal", "Joshi", "AHMD", "PROC", "Procurement Manager"),
    ("RP-EMP-016", "Rohit", "Agarwal", "AHMD", "FIN", "Accountant"),
    ("RP-EMP-017", "Priya", "Nair", "AHMD", "HR", "HR Manager"),
    ("RP-EMP-018", "Divya", "Menon", "BLR", "SALES", "Sales Executive"),
]
for code, first, last, br, dept, desig in emp_defs:
    r = post(f"{BASE}/api/employees", {
        "employeeCode": code, "firstName": first, "lastName": last,
        "email": f"{first.lower()}.{last.lower()}@rudrakshprojects.in",
        "password": "Password@123", "phone": "+91-98250-00000",
        "organizationId": ORG, "branchId": BR.get(br) if br else None,
        "departmentId": DEPT[dept], "designationId": DESIG[desig],
        "joiningDate": "2024-06-01", "employmentStatus": 1, "roles": ["Employee"]})
    if r.get("id"):
        employees[code] = r["id"]

print("== Asset categories ==")
categories = {}
cat_defs = [
    ("RP-CAT-SOLTEST", "Solar Testing Instruments"),
    ("RP-CAT-ELETEST", "Electrical Testing Equipment"),
    ("RP-CAT-POWTOOL", "Power Tools"),
    ("RP-CAT-VEHICLE", "Site Vehicles"),
    ("RP-CAT-SURVEY", "Survey & Measurement Instruments"),
    ("RP-CAT-SAFETY", "Safety Equipment"),
    ("RP-CAT-ITOFF", "IT & Office Equipment"),
    ("RP-CAT-HEAVY", "Heavy Machinery"),
    ("RP-CAT-CABLE", "Cable Laying Equipment"),
    ("RP-CAT-PANEL", "Panel & Switchgear Tools"),
]
for code, name in cat_defs:
    r = post(f"{BASE}/api/asset-categories", {"organizationId": ORG, "code": code, "name": name})
    if r.get("id"):
        categories[name] = r["id"]

print("== Assets ==")
assets = {}
# (code, name, category, branch, brand, cost, status)
asset_defs = [
    ("RP-AST-001", "IV Curve Tracer PV200", "Solar Testing Instruments", "AHMD", "Seaward", 350000, 1),
    ("RP-AST-002", "Solar Irradiance Meter", "Solar Testing Instruments", "BLR", "Kipp & Zonen", 85000, 1),
    ("RP-AST-003", "PV Module I-V Analyzer", "Solar Testing Instruments", "MUM", "HT Instruments", 220000, 1),
    ("RP-AST-004", "Insulation Resistance Tester MIT525", "Electrical Testing Equipment", "AHMD", "Megger", 180000, 1),
    ("RP-AST-005", "Earth Resistance Tester 4105A", "Electrical Testing Equipment", "NOIDA", "Kyoritsu", 45000, 1),
    ("RP-AST-006", "Micro-ohmmeter DLRO10", "Electrical Testing Equipment", "MUM", "Megger", 260000, 1),
    ("RP-AST-007", "HV Test Kit 100kV", "Electrical Testing Equipment", "MUM", "Hipotronics", 950000, 1),
    ("RP-AST-008", "Relay Test Kit CMC356", "Electrical Testing Equipment", "AHMD", "Omicron", 1800000, 4),
    ("RP-AST-009", "Transformer Turns Ratio Tester TTR330", "Electrical Testing Equipment", "NOIDA", "Megger", 420000, 1),
    ("RP-AST-010", "Power Quality Analyzer Fluke 435-II", "Electrical Testing Equipment", "BLR", "Fluke", 320000, 1),
    ("RP-AST-011", "Digital Clamp Meter Fluke 376", "Electrical Testing Equipment", "AHMD", "Fluke", 28000, 1),
    ("RP-AST-012", "Cable Fault Locator", "Cable Laying Equipment", "MUM", "Megger", 780000, 1),
    ("RP-AST-013", "Cable Drum Jack Set 10T", "Cable Laying Equipment", "AHMD", "Local", 45000, 1),
    ("RP-AST-014", "Hydraulic Crimping Tool 400sqmm", "Power Tools", "AHMD", "Dowells", 35000, 1),
    ("RP-AST-015", "Cordless Impact Drill GSB18V", "Power Tools", "NOIDA", "Bosch", 18000, 1),
    ("RP-AST-016", "Angle Grinder 100mm", "Power Tools", "MUM", "Makita", 9500, 1),
    ("RP-AST-017", "Torque Wrench Set", "Power Tools", "BLR", "Taparia", 22000, 1),
    ("RP-AST-018", "Tata Ace Gold (Site Van)", "Site Vehicles", "AHMD", "Tata Motors", 650000, 2),
    ("RP-AST-019", "Mahindra Bolero Pickup", "Site Vehicles", "MUM", "Mahindra", 950000, 2),
    ("RP-AST-020", "Total Station TS07", "Survey & Measurement Instruments", "BLR", "Leica", 550000, 1),
    ("RP-AST-021", "Laser Distance Meter GLM250", "Survey & Measurement Instruments", "AHMD", "Bosch", 15000, 1),
    ("RP-AST-022", "JCB 3DX Backhoe Loader", "Heavy Machinery", "NOIDA", "JCB", 3500000, 1),
    ("RP-AST-023", "Diesel Generator 125kVA", "Heavy Machinery", "MUM", "Cummins", 850000, 1),
    ("RP-AST-024", "Design Workstation (PVsyst/AutoCAD)", "IT & Office Equipment", "BLR", "Dell Precision", 180000, 2),
    ("RP-AST-025", "HP DesignJet Plotter T650", "IT & Office Equipment", "AHMD", "HP", 220000, 1),
]
for code, name, cat, br, brand, cost, status in asset_defs:
    if cat not in categories:
        continue
    r = post(f"{BASE}/api/assets", {
        "organizationId": ORG, "categoryId": categories[cat], "branchId": BR.get(br),
        "assetCode": code, "assetName": name, "brand": brand,
        "purchaseCost": cost, "currentValue": round(cost * 0.82),
        "purchaseDate": "2024-04-01", "assetStatus": status if status != 2 else 1,
        "serialNumber": f"SN-{code}"})
    if r.get("id"):
        assets[code] = (r["id"], status)

print("== Asset assignments ==")
# Assign the vans / workstation to relevant staff (drives status Assigned properly).
for asset_code, emp_code in [
    ("RP-AST-018", "RP-EMP-009"),  # Site van -> Site Engineer (Ahmedabad)
    ("RP-AST-019", "RP-EMP-013"),  # Pickup -> Site Supervisor (Mumbai)
    ("RP-AST-024", "RP-EMP-007"),  # Design workstation -> Solar Design Engineer (Bangalore)
]:
    if asset_code in assets and emp_code in employees:
        post(f"{BASE}/api/asset-assignments/assign", {
            "assetId": assets[asset_code][0], "employeeId": employees[emp_code],
            "assignedDate": "2024-07-01", "remarks": "Issued for site operations"})

print("== Vendors ==")
vendors = {}
ven_defs = [
    ("RP-VEN-01", "Waaree Energies Ltd", "Solar PV modules"),
    ("RP-VEN-02", "Adani Solar (Mundra)", "Solar PV modules"),
    ("RP-VEN-03", "Sungrow India Pvt Ltd", "String & central inverters"),
    ("RP-VEN-04", "Polycab India Ltd", "LT/HT & solar DC cables"),
    ("RP-VEN-05", "KEI Industries Ltd", "HT power cables"),
    ("RP-VEN-06", "Havells India Ltd", "Switchgear & MCBs"),
    ("RP-VEN-07", "L&T Electrical & Automation", "HT/LT panels & switchgear"),
    ("RP-VEN-08", "Schneider Electric India", "Panels & protection"),
    ("RP-VEN-09", "ABB India Ltd", "Transformers & breakers"),
    ("RP-VEN-10", "Fluke Technologies India", "Testing instruments"),
    ("RP-VEN-11", "Karam Industries", "Safety PPE & harnesses"),
    ("RP-VEN-12", "Exide Industries Ltd", "Batteries & BESS"),
]
for code, name, _desc in ven_defs:
    r = post(f"{EO}/vendors", {
        "organizationId": ORG, "code": code, "name": name,
        "email": f"sales@{code.lower().replace('-', '')}.example.in",
        "phone": "+91-79-4000-0000", "taxNumber": f"24{code.replace('-', '')}Z1"})
    if r.get("id"):
        vendors[code] = r["id"]

print("== Customers ==")
customers = {}
cust_defs = [
    ("RP-CUS-01", "Gujarat Urja Vikas Nigam Ltd (GUVNL)"),
    ("RP-CUS-02", "Adani Green Energy Ltd"),
    ("RP-CUS-03", "Torrent Power Ltd"),
    ("RP-CUS-04", "Maharashtra State Electricity Distribution (MSEDCL)"),
    ("RP-CUS-05", "Tata Power Solar Systems Ltd"),
    ("RP-CUS-06", "Reliance Industries Ltd (Captive Solar)"),
]
for code, name in cust_defs:
    r = post(f"{EO}/customers", {
        "organizationId": ORG, "code": code, "name": name,
        "email": f"projects@{code.lower().replace('-', '')}.example.in",
        "phone": "+91-79-2600-0000"})
    if r.get("id"):
        customers[code] = r["id"]

print("== Inventory ==")
inventory = {}
# (code, name, branch, stock, reorder, uom)
inv_defs = [
    ("RP-INV-01", "Solar PV Module 540Wp Mono PERC", "AHMD", 480, 100, "pcs"),
    ("RP-INV-02", "String Inverter 25kW", "AHMD", 24, 6, "pcs"),
    ("RP-INV-03", "Solar DC Cable 4sqmm (Red)", "MUM", 8000, 2000, "mtr"),
    ("RP-INV-04", "Solar DC Cable 4sqmm (Black)", "MUM", 7500, 2000, "mtr"),
    ("RP-INV-05", "LT XLPE Cable 3.5C x 240sqmm", "NOIDA", 3000, 500, "mtr"),
    ("RP-INV-06", "HT XLPE Cable 3C x 240sqmm 11kV", "NOIDA", 1200, 300, "mtr"),
    ("RP-INV-07", "MC4 Connector Pair", "AHMD", 950, 200, "pairs"),
    ("RP-INV-08", "GI Earthing Strip 25x3mm", "BLR", 1500, 300, "mtr"),
    ("RP-INV-09", "Copper Lug 240sqmm", "MUM", 600, 150, "pcs"),
    ("RP-INV-10", "Module Mounting Structure Set", "AHMD", 120, 30, "sets"),
    ("RP-INV-11", "Cable Gland 240sqmm", "NOIDA", 400, 100, "pcs"),
    ("RP-INV-12", "MCCB 250A 4-Pole", "AHMD", 45, 10, "pcs"),
    ("RP-INV-13", "Solar DC Fuse 15A 1000V", "BLR", 800, 200, "pcs"),
    ("RP-INV-14", "Lightning Arrestor", "MUM", 60, 15, "pcs"),
    ("RP-INV-15", "Earthing Electrode 3mtr", "AHMD", 200, 50, "pcs"),
    ("RP-INV-16", "Cable Tie 300mm (pack of 100)", "AHMD", 300, 50, "packs"),
]
for code, name, br, stock, reorder, uom in inv_defs:
    r = post(f"{EO}/inventory", {
        "organizationId": ORG, "branchId": BR.get(br), "itemCode": code, "itemName": name,
        "currentStock": stock, "reservedStock": 0, "reorderLevel": reorder, "unitOfMeasure": uom})
    if r.get("id"):
        inventory[code] = r["id"]

print("== Stock movements ==")
# StockIn=1, StockOut=2
for code, mtype, qty, ref in [
    ("RP-INV-01", 1, 120, "GRN-SOLAR-2401"),   # modules received
    ("RP-INV-01", 2, 200, "ISSUE-SITE-GUVNL"),  # issued to site
    ("RP-INV-03", 2, 3000, "ISSUE-DCRUN-01"),
    ("RP-INV-07", 2, 400, "ISSUE-MC4-01"),
    ("RP-INV-02", 1, 12, "GRN-INV-2402"),
]:
    if code in inventory:
        post(f"{EO}/inventory/stock-movements", {
            "inventoryItemId": inventory[code], "movementType": mtype, "quantity": qty,
            "referenceNumber": ref, "remarks": "Seeded movement"})

print("== Consumables ==")
for code, name, inv in [
    ("RP-CON-01", "MC4 Connector Pair", "RP-INV-07"),
    ("RP-CON-02", "Solar DC Fuse 15A", "RP-INV-13"),
    ("RP-CON-03", "Copper Lug 240sqmm", "RP-INV-09"),
    ("RP-CON-04", "Cable Tie 300mm", "RP-INV-16"),
    ("RP-CON-05", "Earthing Electrode 3mtr", "RP-INV-15"),
    ("RP-CON-06", "Cable Gland 240sqmm", "RP-INV-11"),
]:
    if inv in inventory:
        post(f"{EO}/consumables", {
            "inventoryItemId": inventory[inv], "consumableCode": code, "name": name})

print("== Purchase requests + orders ==")
prs = {}
# (code, title, requestedBy, amount, status)  status: 1 Draft, 3 Approved
pr_defs = [
    ("RP-PR-01", "540Wp modules for GUVNL 2MW project", "RP-EMP-011", 4200000, 3),
    ("RP-PR-02", "11kV HT cable for substation package", "RP-EMP-006", 1850000, 3),
    ("RP-PR-03", "Safety harnesses & PPE restock", "RP-EMP-014", 320000, 1),
    ("RP-PR-04", "String inverters 25kW x 20", "RP-EMP-007", 3600000, 3),
]
for code, title, emp, amount, status in pr_defs:
    if emp not in employees:
        continue
    r = post(f"{EO}/purchase-requests", {
        "organizationId": ORG, "requestNumber": code, "title": title,
        "requestedByEmployeeId": employees[emp], "requestDate": "2026-06-20",
        "estimatedAmount": amount})
    if r.get("id"):
        prs[code] = r["id"]
        if status != 1:
            post(f"{EO}/purchase-requests/{r['id']}/status", {
                "status": status, "approvalRemarks": "Approved by projects head"})

# (code, pr, vendor, amount, status)  status: 6 Ordered
po_defs = [
    ("RP-PO-01", "RP-PR-01", "RP-VEN-01", 4150000, 6),
    ("RP-PO-02", "RP-PR-02", "RP-VEN-05", 1820000, 6),
    ("RP-PO-03", "RP-PR-04", "RP-VEN-03", 3550000, 2),
]
for code, pr, ven, amount, status in po_defs:
    if ven not in vendors:
        continue
    r = post(f"{EO}/purchase-orders", {
        "organizationId": ORG, "orderNumber": code, "purchaseRequestId": prs.get(pr),
        "vendorId": vendors[ven], "orderDate": "2026-06-28",
        "expectedDeliveryDate": "2026-07-20", "totalAmount": amount})
    if r.get("id") and status != 1:
        post(f"{EO}/purchase-orders/{r['id']}/status", {"status": status, "remarks": "PO released"})

print("== Maintenance ==")
# (asset, type, title, status)  Preventive=1 Corrective=2 ; Scheduled=2 InProgress=3
for asset_code, mtype, title, status in [
    ("RP-AST-008", 2, "Relay test kit annual calibration (NABL)", 2),
    ("RP-AST-022", 1, "JCB 250-hour service", 2),
    ("RP-AST-023", 1, "DG set servicing & load test", 3),
    ("RP-AST-007", 1, "HV test kit calibration", 2),
]:
    if asset_code in assets:
        r = post(f"{EO}/maintenance", {
            "assetId": assets[asset_code][0], "maintenanceType": mtype,
            "requestedDate": "2026-07-05", "title": title, "cost": 25000})
        if r.get("id") and status != 1:
            post(f"{EO}/maintenance/{r['id']}/status", {"status": status, "scheduledDate": "2026-07-18"})

print("== Service tickets ==")
# (code, title, customer, assignedEmp, priority, status)
for code, title, cust, emp, prio, status in [
    ("RP-ST-01", "Inverter fault at GUVNL 2MW plant", "RP-CUS-01", "RP-EMP-012", 4, 2),
    ("RP-ST-02", "String underperformance - rooftop site", "RP-CUS-05", "RP-EMP-012", 2, 2),
    ("RP-ST-03", "Earthing check requested at substation", "RP-CUS-04", None, 3, 1),
    ("RP-ST-04", "Thermography of HT panel", "RP-CUS-03", "RP-EMP-006", 2, 2),
]:
    if cust not in customers:
        continue
    r = post(f"{EO}/service-tickets", {
        "organizationId": ORG, "customerId": customers[cust], "ticketNumber": code,
        "title": title, "priority": prio})
    if r.get("id") and (emp or status != 1):
        call("PUT", f"{EO}/service-tickets/{r['id']}", {
            "assignedToEmployeeId": employees.get(emp), "priority": prio, "status": status,
            "resolution": None})

print(f"\nDone: {created} requests succeeded, {failed} failed.")
