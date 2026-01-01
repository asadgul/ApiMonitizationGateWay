using APIMonitizationGateway.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIMonitizationGateway.APIDatabase
{
    public class APIMonitizationDbContext: DbContext
    {

        public APIMonitizationDbContext(DbContextOptions<APIMonitizationDbContext> dbContextOptions):base (dbContextOptions)
        {
            
        }

        public DbSet<Tier> Tiers { get; set; }
        public DbSet<ApiUsageLog> ApiLogs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MonthlyUsageSummary> MonthySummary { get; set; }


    }
}
