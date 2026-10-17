# FitnessTracker API

A fitness-tracking REST API built with **ASP.NET Core (.NET 10)**: log food, exercise and hydration, and get a live daily dashboard of calories in, calories out and water intake. Includes JWT auth, an external nutrition API integration, a background worker, and real-time push notifications over SignalR.

![CI](https://github.com/<your-username>/<your-repo>/actions/workflows/ci.yml/badge.svg)

**Live demo:** https://fitnesstracker-jzj5.onrender.com/swagger

> Hosted on a free tier that sleeps when idle, so the first request after a quiet period can take a while. To try it: `POST /api/users/register`, then `POST /api/users/login`, click **Authorize** in Swagger and paste the token.

## Features

- **Auth:** registration and login with hashed passwords (`PasswordHasher<T>`) and JWT bearer tokens. Every log endpoint is ownership-scoped, so users can only read and delete their own data.
- **Exercise logging:** calories burned are calculated server-side from a seeded exercise catalog (MET values) and the user's weight.
- **Food logging:** search the [USDA FoodData Central](https://fdc.nal.usda.gov/) API, cache resolved foods in the local database, or add a food manually. Search results are cached in memory, and the API degrades to a `503` with a manual-entry hint if USDA is unreachable.
- **Hydration logging** with a daily goal derived from body weight.
- **Dashboard:** per-day calories in, exercise burn, BMR accrued so far, net calories, hydration vs. goal.
- **Pagination and date-range filtering** on all list endpoints.
- **Background reminders:** a `BackgroundService` finds users who are behind on hydration, stores a notification, and pushes it live through SignalR.
- **Validation and error handling:** data-annotation validation, a `Result<T>` pattern for expected failures (`400` vs `409`), and a global exception handler that never leaks stack traces.

## Tech stack

| Area | Choice |
|---|---|
| Framework | ASP.NET Core Web API, controllers, .NET 10 |
| Data | EF Core + PostgreSQL (Neon), code-first migrations with seeded data |
| Auth | Manual JWT bearer auth (deliberately not ASP.NET Identity) |
| Real-time | SignalR |
| Background work | `BackgroundService` + `PeriodicTimer` |
| External API | USDA FoodData Central via a typed `HttpClient` + `IMemoryCache` |
| Testing | xUnit, SQLite in-memory, `FakeTimeProvider` |
| Delivery | Docker (multi-stage), GitHub Actions CI, Render |

## Architecture

```
Client ──HTTP──► Controllers ──► Services ──► AppDbContext ──► PostgreSQL (Neon)
                                    │
                                    └──► UsdaFoodClient ──► USDA FoodData Central
                                         (typed HttpClient + IMemoryCache)

HydrationReminderWorker ──► ReminderService ──► Notifications table
                                    │
                                    └──► SignalR hub ──► connected client (live push)
```

## API overview

All endpoints except register, login and `/health` require `Authorization: Bearer <token>`.

| Endpoint | Purpose |
|---|---|
| `POST /api/users/register` | Create an account |
| `POST /api/users/login` | Get a JWT |
| `GET/POST/DELETE /api/exerciselog` | Exercise entries (calories burned computed server-side) |
| `GET /api/fooditems/search?query=` | Search USDA (top matches) |
| `POST /api/fooditems/from-usda?fdcId=` | Resolve and cache a USDA food locally |
| `POST /api/fooditems/manual` | Add a food manually (fallback) |
| `GET/POST/DELETE /api/foodlog` | Food entries |
| `GET/POST/DELETE /api/hydrationlog` | Hydration entries |
| `GET /api/dashboard?date=` | Daily summary (defaults to today, UTC) |
| `GET /api/notifications`, `POST /api/notifications/{id}/read` | Stored reminders |
| `/hubs/notifications` | SignalR hub, event name `notification` |
| `GET /health` | Health check |

List endpoints accept `from`, `to`, `page` and `pageSize` and return `{ items, page, pageSize, totalCount }`.

**SignalR auth:** browsers can't set headers on WebSockets, so the JWT is sent as `?access_token=` for `/hubs` paths only.

## Run locally

Prerequisites: .NET 10 SDK, a PostgreSQL database (a free [Neon](https://neon.tech) project works), and a free [USDA API key](https://fdc.nal.usda.gov/api-key-signup).

```bash
cd FitnessTracker
dotnet user-secrets set "ConnectionStrings:Default" "Host=...;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true"
dotnet user-secrets set "Jwt:Key" "<random secret, at least 32 characters>"
dotnet user-secrets set "Usda:ApiKey" "<your key>"
dotnet ef database update
dotnet run
```

Swagger is served at `/swagger` in Development. Note that the connection string must be in Npgsql's `key=value` format, not the `postgresql://` URI that Neon shows by default.

## Configuration

Set via User Secrets locally and environment variables in production (`__` replaces `:`).

| Key | Purpose |
|---|---|
| `ConnectionStrings__Default` | PostgreSQL connection string |
| `Jwt__Key` | JWT signing secret (32+ characters) |
| `Usda__ApiKey` | USDA FoodData Central key |
| `Reminders__IntervalMinutes` / `Reminders__MinGapMinutes` | Worker timing (defaults 60 / 180) |
| `Swagger__Enabled` | Expose Swagger outside Development |

## Tests

```bash
dotnet test
```

- Pure calculators (MET calorie burn, BMR, age) with edge cases and `[Theory]` data.
- Service tests against in-memory SQLite with a frozen clock: registration (hashing, duplicate email, age rules), dashboard math, and reminder rules including cooldown and push behavior via a fake publisher.

## Docker

```bash
docker build -t fitnesstracker-api .
docker run --rm -p 8080:8080 --env-file .env.docker fitnesstracker-api
```

The Dockerfile is multi-stage (SDK image builds, ASP.NET runtime image runs as a non-root user). Configuration comes from environment variables, and no secrets are baked into the image.

## CI/CD and deployment

- **CI:** GitHub Actions restores, builds and tests the solution, then builds the Docker image, on every push and pull request to `main`.
- **Hosting:** Render (Docker web service, free tier) with a separate production database on Neon.
- **CD:** on every push to main, once build and tests pass, the pipeline applies EF Core migrations through a compiled migration bundle and then triggers a Render deploy via a deploy hook.

## Design decisions

- **Manual JWT instead of ASP.NET Identity**, to understand the mechanics: hashing, claims, validation, and 401 vs 403 semantics.
- **Service layer talks to `DbContext` directly.** EF Core's `DbContext` is already a unit of work, so an extra repository layer adds little here.
- **Response DTOs everywhere.** Entities are never returned, which avoids serialization cycles and accidental leaks such as password hashes.
- **`Result<T>` for expected failures**, exceptions only for unexpected ones.
- **Time is injected (`TimeProvider`)**, so time-dependent logic is deterministic in tests.
- **Compute on read:** BMR accrual is calculated from elapsed time at request time instead of being stored or scheduled.
- **The database is the source of truth for notifications**; the SignalR push is best-effort and the REST endpoint covers anything missed.

## Known limitations

- A "day" is a UTC day; there are no per-user time zones yet.
- Hydration is intake against a goal (about 35 ml per kg), not a time-decay model.
- On the free host the app sleeps when idle, so the background worker only runs while it is awake. Running multiple instances would need a proper job scheduler.
- Tests use SQLite, which cannot catch Postgres-specific behavior.