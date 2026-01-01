using APIMonitizationGateway.APIDatabase;
using APIMonitizationGateway.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Threading.RateLimiting;

namespace APIMonitizationGateway.Models.CustomMiddleware
{
    public class RateLimitMiddleware
    {
        private static readonly ConcurrentDictionary<int, RateLimiter> _limiters = new();
        private readonly RequestDelegate _next;
        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, APIMonitizationDbContext db)
        {

            int customerId = int.Parse(context.Request.Headers["X-Customer-Id"]);
            var customer = await db.Customers.Include(c => c.Tier)
            .FirstAsync(c => c.Id == customerId);
            var limiter = _limiters.GetOrAdd(customerId, _ => new RateLimiter());
            if (!limiter.Allow(customer.Tier.RateLimitPerSecond))
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Rate limit exceeded");
                return;
            }
            await _next(context);
            db.ApiLogs.Add(new ApiUsageLog
            {
                CustomerId = customerId,
                Endpoint = context.Request.Path,
                Timestamp = DateTime.UtcNow
            });
            await db.SaveChangesAsync();

        }
    }
}
