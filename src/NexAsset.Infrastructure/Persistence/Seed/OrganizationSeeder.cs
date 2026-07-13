using Microsoft.EntityFrameworkCore;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Seed;

public static class OrganizationSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Organizations.AnyAsync(cancellationToken))
            return;

        var organization = new Organization
        {
            Code = "NEX",
            Name = "NexAsset",
            LegalName = "NexAsset Technologies Pvt Ltd",
            Email = "admin@nexasset.com",
            Phone = "+91 9999999999",
            Website = "https://nexasset.local",
            Country = "India",
            State = "Gujarat",
            City = "Ahmedabad",
            PostalCode = "380001",
            Currency = "INR",
            TimeZone = "Asia/Kolkata",
            IsActive = true
        };
        
        dbContext.Organizations.Add(organization);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}