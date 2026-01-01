# API Monetization Gateway (.NET)

This project implements a **Programmable API Monetization Gateway** using **ASP.NET Core (.NET 8)**, **Entity Framework Core**, and **Docker**.

The gateway sits in front of internal APIs and enforces:
- Tier-based rate limiting
- Monthly request quotas
- API usage tracking
- Monthly usage aggregation for billing

---

## Architecture Overview

Client requests pass through the API Gateway which:
1. Identifies the customer
2. Applies rate limits based on subscription tier
3. Enforces monthly quotas
4. Logs successful API usage
5. Aggregates monthly usage via background job

---

## Subscription Tiers

| Tier | Rate Limit | Monthly Quota | Price |
|----|----|----|----|
| Free | 2 req/sec | 100 | $0 |
| Pro | 10 req/sec | 100,000 | $50 |

---

## Tech Stack

- .NET 8 Web API
- ASP.NET Core Middleware
- Entity Framework Core
- SQL Server
- BackgroundService
- Docker & Docker Compose

---

## Run Using Docker

### Prerequisites
- Docker
- Docker Compose

### Start Application

```bash
docker-compose up --build
```

API will be available at:
```
https://localhost:7172
```

---

## Sample API Request

Endpoint:
```
GET /api/gateway/SendApiRequest
```

Header:
```
X-Customer-Id: 1
```

Example:
```bash
curl -H "X-Customer-Id: 1" https://localhost:7172/api/gateway/SendApiRequest
```

---

## Expected Responses

- 200 OK – Request allowed
- 429 Too Many Requests – Rate limit or quota exceeded

---

## Database Initialization

On startup:
- EF Core migrations are applied automatically
- Default tiers and customers are seeded

---

## Background Job

A background service periodically:
- Aggregates monthly usage
- Calculates billing summaries
- Persists monthly usage data

---

## Notes

- Rate limiting and quota enforcement are implemented using middleware
- The solution focuses on gateway and monetization logic

---
Example:
```
X-Customer-Id: 1  # Free tier
X-Customer-Id: 2  # Pro tier
```

## Author

Developed as part of a backend coding challenge.


```
