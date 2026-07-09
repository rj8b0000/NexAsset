using System;
using System.Collections.Generic;
using System.Linq;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.Infrastructure.Services
{
    public class MockDatabaseService
    {
        public List<AssetMock> Assets { get; } = new();
        public List<EmployeeMock> Employees { get; } = new();
        public List<OrganizationMock> OrganizationDetails { get; } = new();
        public List<ProcurementMock> ProcurementRequests { get; } = new();
        public List<MaintenanceMock> MaintenanceTickets { get; } = new();
        public List<InvoiceMock> Invoices { get; } = new();
        public List<AuditLogMock> AuditLogs { get; } = new();

        public List<string> Organizations { get; } = new() { "NexCorp Global", "NexCorp APAC", "NexCorp EMEA" };
        public List<string> Branches { get; } = new() { "HQ - New York", "London Office", "Tokyo Hub", "Paris Branch" };

        public MockDatabaseService()
        {
            InitializeMockData();
        }

        public void AddAuditLog(string entityId, string entityType, string action, string details)
        {
            AuditLogs.Insert(0, new AuditLogMock
            {
                Id = $"AUD-{AuditLogs.Count + 1001}",
                Timestamp = DateTime.Now,
                EntityId = entityId,
                EntityType = entityType,
                Action = action,
                Details = details,
                User = "Admin User"
            });
        }

        private void InitializeMockData()
        {
            // Seed Assets
            Assets.Add(new AssetMock { Id = "AST-001", Name = "MacBook Pro 16\" M3 Max", Category = "IT Equipment", Serial = "C02GY999XHG", Status = "Assigned", Value = 3499.00m, Location = "HQ - New York", AssignedTo = "Alice Smith", PurchaseDate = DateTime.Now.AddMonths(-6) });
            Assets.Add(new AssetMock { Id = "AST-002", Name = "Dell UltraSharp 32\" 4K Monitor", Category = "IT Equipment", Serial = "CN-0XX998-1293", Status = "Assigned", Value = 899.00m, Location = "HQ - New York", AssignedTo = "Alice Smith", PurchaseDate = DateTime.Now.AddMonths(-5) });
            Assets.Add(new AssetMock { Id = "AST-003", Name = "iPhone 15 Pro Max 256GB", Category = "Mobile Devices", Serial = "DNPD9999FGF", Status = "Available", Value = 1199.00m, Location = "London Office", AssignedTo = null, PurchaseDate = DateTime.Now.AddMonths(-2) });
            Assets.Add(new AssetMock { Id = "AST-004", Name = "Cisco Catalyst 9300 Switch", Category = "Networking", Serial = "FCW2239L0XP", Status = "Assigned", Value = 4500.00m, Location = "Tokyo Hub", AssignedTo = "Kenji Sato", PurchaseDate = DateTime.Now.AddYears(-1) });
            Assets.Add(new AssetMock { Id = "AST-005", Name = "HP LaserJet Enterprise Printer", Category = "Office Equipment", Serial = "JPB9911X23", Status = "Maintenance", Value = 1250.00m, Location = "London Office", AssignedTo = null, PurchaseDate = DateTime.Now.AddMonths(-18) });
            Assets.Add(new AssetMock { Id = "AST-006", Name = "MacBook Air 15\" M2", Category = "IT Equipment", Serial = "C02HK38F9X", Status = "Available", Value = 1499.00m, Location = "Paris Branch", AssignedTo = null, PurchaseDate = DateTime.Now.AddMonths(-3) });
            Assets.Add(new AssetMock { Id = "AST-007", Name = "Tesla Model 3 (Fleet Vehicle)", Category = "Vehicles", Serial = "5YJ3E1EBXLFXXXXXX", Status = "Assigned", Value = 42000.00m, Location = "HQ - New York", AssignedTo = "David Carter", PurchaseDate = DateTime.Now.AddYears(-2) });
            Assets.Add(new AssetMock { Id = "AST-008", Name = "Oculus Quest 3 VR Headset", Category = "R&D Equipment", Serial = "1F23G45678", Status = "Available", Value = 649.00m, Location = "HQ - New York", AssignedTo = null, PurchaseDate = DateTime.Now.AddDays(-14) });

            // Seed Employees
            Employees.Add(new EmployeeMock { Id = "EMP-001", Name = "Alice Smith", Department = "Engineering", Email = "alice.smith@nexasset.com", Role = "Principal Engineer", AssetsAssigned = 2, Status = "Active" });
            Employees.Add(new EmployeeMock { Id = "EMP-002", Name = "Kenji Sato", Department = "IT Operations", Email = "kenji.sato@nexasset.com", Role = "Network Architect", AssetsAssigned = 1, Status = "Active" });
            Employees.Add(new EmployeeMock { Id = "EMP-003", Name = "David Carter", Department = "Logistics", Email = "david.carter@nexasset.com", Role = "Fleet Manager", AssetsAssigned = 1, Status = "Active" });
            Employees.Add(new EmployeeMock { Id = "EMP-004", Name = "Sarah Jenkins", Department = "Finance", Email = "sarah.jenkins@nexasset.com", Role = "Billing lead", AssetsAssigned = 0, Status = "Active" });
            Employees.Add(new EmployeeMock { Id = "EMP-005", Name = "Pierre Dubois", Department = "Sales", Email = "pierre.dubois@nexasset.com", Role = "Regional Director", AssetsAssigned = 0, Status = "On Leave" });

            // Seed Organizations
            OrganizationDetails.Add(new OrganizationMock { Id = "ORG-001", Name = "NexCorp Global", HeadCount = 450, Location = "New York, USA", CreatedDate = DateTime.Now.AddYears(-5) });
            OrganizationDetails.Add(new OrganizationMock { Id = "ORG-002", Name = "NexCorp APAC", HeadCount = 180, Location = "Tokyo, Japan", CreatedDate = DateTime.Now.AddYears(-3) });
            OrganizationDetails.Add(new OrganizationMock { Id = "ORG-003", Name = "NexCorp EMEA", HeadCount = 220, Location = "London, UK", CreatedDate = DateTime.Now.AddYears(-3) });

            // Seed Procurement
            ProcurementRequests.Add(new ProcurementMock { Id = "PR-001", ItemName = "Logitech MX Master 3S Mouse", Quantity = 10, UnitPrice = 99.00m, TotalValue = 990.00m, Requester = "Alice Smith", Status = "Approved" });
            ProcurementRequests.Add(new ProcurementMock { Id = "PR-002", ItemName = "Dell Latitude 5440 Laptops", Quantity = 15, UnitPrice = 1200.00m, TotalValue = 18000.00m, Requester = "Kenji Sato", Status = "Pending" });
            ProcurementRequests.Add(new ProcurementMock { Id = "PR-003", ItemName = "Herman Miller Aeron Chair", Quantity = 5, UnitPrice = 1450.00m, TotalValue = 7250.00m, Requester = "Sarah Jenkins", Status = "Pending" });
            ProcurementRequests.Add(new ProcurementMock { Id = "PR-004", ItemName = "Conference Room Speaker System", Quantity = 2, UnitPrice = 850.00m, TotalValue = 1700.00m, Requester = "David Carter", Status = "Rejected" });

            // Seed Maintenance
            MaintenanceTickets.Add(new MaintenanceMock { Id = "MT-001", AssetName = "HP LaserJet Enterprise Printer", Issue = "Fuser roller replacement & general tune up", Type = "Corrective", Urgency = "Medium", Status = "In Progress" });
            MaintenanceTickets.Add(new MaintenanceMock { Id = "MT-002", AssetName = "Tesla Model 3 (Fleet Vehicle)", Issue = "Routine 10k mile tire rotation and brake inspection", Type = "Preventive", Urgency = "Low", Status = "Open" });
            MaintenanceTickets.Add(new MaintenanceMock { Id = "MT-003", AssetName = "Cisco Catalyst Switch (AST-004)", Issue = "Overheating logs on Port 24", Type = "Corrective", Urgency = "High", Status = "Resolved" });

            // Seed Invoices
            Invoices.Add(new InvoiceMock { Id = "INV-001", Vendor = "CDW Government", Amount = 18900.00m, DueDate = DateTime.Now.AddDays(15), Status = "Unpaid" });
            Invoices.Add(new InvoiceMock { Id = "INV-002", Vendor = "Apple Store Business", Amount = 6998.00m, DueDate = DateTime.Now.AddDays(-5), Status = "Paid" });
            Invoices.Add(new InvoiceMock { Id = "INV-003", Vendor = "Tesla Fleet Finance", Amount = 1250.00m, DueDate = DateTime.Now.AddDays(30), Status = "Unpaid" });
            Invoices.Add(new InvoiceMock { Id = "INV-004", Vendor = "Grainger Industrial Support", Amount = 450.00m, DueDate = DateTime.Now.AddDays(-10), Status = "Overdue" });

            // Seed Audit Logs
            AddAuditLog("AST-001", "Asset", "Assign", "Assigned MacBook Pro to Alice Smith");
            AddAuditLog("AST-004", "Asset", "Create", "Created network switch asset");
            AddAuditLog("MT-003", "Maintenance", "Resolve", "Cisco Switch overheating issue resolved by replacing fans");
        }
    }
}
