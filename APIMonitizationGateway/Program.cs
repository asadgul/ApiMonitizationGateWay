using APIMonitizationGateway.APIDatabase;
using APIMonitizationGateway.BackgroundJobs;
using APIMonitizationGateway.Entities;
using APIMonitizationGateway.Models.CustomMiddleware;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<APIMonitizationDbContext>(option=>option.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbConnection")));
builder.Services.AddHostedService<MonthlyAggregationJob>();
builder.Services.AddControllers();
var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<APIMonitizationDbContext>();
//    db.Database.Migrate();
//}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<APIMonitizationDbContext>();
    db.Database.Migrate();
    // ---- SEED TIERS ----
    if (!db.Tiers.Any())
    {
        var freeTier = new Tier
        {
            Name = "Free",
            MonthlyQuota = 100,
            RateLimitPerSecond = 2,
            MonthlyPrice = 0
        };

        var proTier = new Tier
        {
            Name = "Pro",
            MonthlyQuota = 100_000,
            RateLimitPerSecond = 10,
            MonthlyPrice = 50
        };

        db.Tiers.AddRange(freeTier, proTier);
        db.SaveChanges();
    }

    // ---- SEED CUSTOMERS ----
    if (!db.Customers.Any())
    {
        var freeTierId = db.Tiers.First(t => t.Name == "Free").Id;
        var proTierId = db.Tiers.First(t => t.Name == "Pro").Id;

        db.Customers.AddRange(
            new Customer
            {
                Name = "Free Customer",
                TierId = freeTierId
            },
            new Customer
            {
                Name = "Pro Customer",
                TierId = proTierId
            }
        );

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<RateLimitMiddleware>();
app.MapControllers();

app.Run();
