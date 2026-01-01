using APIMonitizationGateway.APIDatabase;
using APIMonitizationGateway.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIMonitizationGateway.BackgroundJobs
{
    public class MonthlyAggregationJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public MonthlyAggregationJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<APIMonitizationDbContext>();
                var now = DateTime.UtcNow;
                var usage = db.ApiLogs
                .Where(x => x.Timestamp.Month == now.Month)
                .GroupBy(x => x.CustomerId)
                .Select(g => new { g.Key, Count = g.Count() });
                foreach (var u in usage)
                {
                    var customer = db.Customers.Include(c => c.Tier)
                    .First(c => c.Id == u.Key);
                    db.MonthySummary.Add(new MonthlyUsageSummary
                    {
                        CustomerId = u.Key,
                        Year = now.Year,
                        Month = now.Month,
                        TotalRequests = u.Count,
                        Price = customer.Tier.MonthlyPrice
                    });
                }
                await db.SaveChangesAsync();
            }

        }
    }
}