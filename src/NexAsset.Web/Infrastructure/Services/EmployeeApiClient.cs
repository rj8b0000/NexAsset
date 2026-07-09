using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.Models;
using NexAsset.Web.State;

namespace NexAsset.Web.Infrastructure.Services
{
    public class EmployeeApiClient : IEmployeeApiClient
    {
        private readonly MockDatabaseService _db;
        private readonly NotificationState _notifications;

        public EmployeeApiClient(MockDatabaseService db, NotificationState notifications)
        {
            _db = db;
            _notifications = notifications;
        }

        public Task<List<EmployeeMock>> GetEmployeesAsync()
        {
            return Task.FromResult(_db.Employees.ToList());
        }

        public Task<EmployeeMock?> GetEmployeeAsync(string id)
        {
            return Task.FromResult(_db.Employees.FirstOrDefault(e => e.Id == id));
        }

        public Task CreateEmployeeAsync(EmployeeMock employee)
        {
            employee.Id = $"EMP-{_db.Employees.Count + 101:D3}";
            _db.Employees.Insert(0, employee);

            _db.AddAuditLog(employee.Id, "Employee", "Create", $"Created employee profile: {employee.Name}");
            _notifications.AddActivity("Employee Created", $"New employee profile for {employee.Name} registered.");
            _notifications.AddToast("Employee Registered", $"Successfully registered employee {employee.Id}", ToastType.Success);

            return Task.CompletedTask;
        }

        public Task UpdateEmployeeAsync(EmployeeMock employee)
        {
            var idx = _db.Employees.FindIndex(e => e.Id == employee.Id);
            if (idx >= 0)
            {
                _db.Employees[idx] = employee;
                
                _db.AddAuditLog(employee.Id, "Employee", "Update", $"Updated employee: {employee.Name}");
                _notifications.AddActivity("Employee Updated", $"Employee profile {employee.Id} details updated.");
                _notifications.AddToast("Employee Updated", $"Updated employee details for {employee.Id}", ToastType.Success);
            }
            return Task.CompletedTask;
        }

        public Task DeleteEmployeeAsync(string id)
        {
            var emp = _db.Employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                _db.Employees.Remove(emp);
                _db.AddAuditLog(id, "Employee", "Delete", $"Deleted employee: {emp.Name}");
                _notifications.AddActivity("Employee Deleted", $"Employee profile {id} was permanently removed.");
                _notifications.AddToast("Employee Removed", $"Deleted employee {id}", ToastType.Warning);
            }
            return Task.CompletedTask;
        }
    }
}
