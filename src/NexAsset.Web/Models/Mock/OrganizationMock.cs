using System;

namespace NexAsset.Web.Models.Mock
{
    /// <summary>
    /// In-memory placeholder shape for an organization record.
    /// Backed today by <see cref="NexAsset.Web.Infrastructure.Services.Mock.MockDatabaseService"/> only.
    /// Not wired to NexAsset.API.
    /// </summary>
    public class OrganizationMock
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int HeadCount { get; set; }
        public string Location { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
