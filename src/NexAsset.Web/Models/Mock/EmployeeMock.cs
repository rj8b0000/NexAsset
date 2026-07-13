namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for an employee record.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API. Expect this shape to be replaced by the real
    /// GetEmployeeResponse contract (Guid-keyed, organization/branch/department/designation relations)
    /// once API integration begins.
    /// </summary>
    public class EmployeeMock
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public int AssetsAssigned { get; set; }
        public string Status { get; set; } = "Active"; // Active, On Leave, Terminated
    }
}
