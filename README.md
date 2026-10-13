# Task Manager API

A REST API for managing projects and tasks, built with ASP.NET Core and Entity Framework Core. Users register, log in with a JWT, and manage their own projects and the tasks inside them. Each user can only see and change their own data.

## Features

- **Authentication**: registration and login with hashed passwords and JWT bearer tokens
- **Projects**: full CRUD, scoped to the logged-in user
- **Tasks**: full CRUD with a status (`ToDo`, `InProgress`, `Done`) and an optional due date
- **Filtering and sorting**: filter tasks by project and status, sort by due date, title or status
- **Validation**: request DTOs are validated (required fields, length limits, and a custom rule that rejects due dates in the past)
- **Data isolation**: another user's project or task returns `404`, the same as a resource that does not exist
- **Consistent errors**: every error response, including unexpected `500`s, uses the standard `ProblemDetails` format with a `traceId`
- **API documentation**: OpenAPI document with an interactive Scalar UI

## Tech stack

- .NET 10, ASP.NET Core Web API (controllers)
- Entity Framework Core with SQLite, code-first migrations
- JWT bearer authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Password hashing with ASP.NET Core Identity's `PasswordHasher`
- OpenAPI + [Scalar](https://scalar.com) for API documentation

## Project structure

| Folder | Responsibility |
|---|---|
| `Controllers` | HTTP layer: read the request, call a service, map the result to a status code |
| `Services` | Business logic and database access (`TaskService`, `ProjectService`, `AuthService`, `TokenService`) |
| `Dtos` | Request and response models, so database entities are never exposed to clients |
| `Mappings` | Extension methods that convert between entities and DTOs |
| `Models` | EF Core entities: `User`, `Project`, `TaskItem` |
| `Data` | `AppDbContext` and model configuration |
| `Validation` | Custom validation attributes (`NotInPast`) |
| `Middleware` | Global exception handler that returns `ProblemDetails` |
| `Extensions` | Helpers, such as reading the user id from the JWT claims |
| `Migrations` | EF Core database migrations |

Controllers never touch the database directly. They read the user id from the token and pass it to a service, and every service query filters by that id.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- EF Core CLI tool:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 1. Clone the repository

```bash
git clone https://github.com/ziomtrzyk/TaskManager.git
cd TaskManager
```

### 2. Set the JWT signing key

The signing key is kept out of the repository using .NET User Secrets. Set your own key, which must be **at least 32 characters** long:

```bash
dotnet user-secrets set "Jwt:Key" "replace-this-with-a-random-key-of-at-least-32-characters"
```

The issuer, audience and token lifetime are in `appsettings.json`.

> User Secrets are only loaded in the `Development` environment. `dotnet run` uses the `Development` profile from `launchSettings.json` by default.

### 3. Create the database

```bash
dotnet ef database update
```

This creates a local `tasks.db` SQLite file.

### 4. Run the API

```bash
dotnet run
```

- API: `http://localhost:5196`
- Interactive documentation: `http://localhost:5196/scalar`
- OpenAPI document: `http://localhost:5196/openapi/v1.json`

## Endpoints

🔒 = requires the `Authorization: Bearer <token>` header.

### Auth

| Method | Path | Description |
|---|---|---|
| `POST` | `/auth/register` | Create an account. Returns `201`, or `409` if the username is taken |
| `POST` | `/auth/login` | Log in. Returns `200` with a JWT, or `401` for invalid credentials |

### Projects 🔒

| Method | Path | Description |
|---|---|---|
| `GET` | `/projects` | List your projects |
| `GET` | `/projects/{id}` | Get one project |
| `GET` | `/projects/{id}/tasks` | List the tasks in a project |
| `POST` | `/projects` | Create a project |
| `PUT` | `/projects/{id}` | Update a project |
| `DELETE` | `/projects/{id}` | Delete a project and its tasks |

### Tasks 🔒

| Method | Path | Description |
|---|---|---|
| `GET` | `/tasks` | List your tasks (see query parameters below) |
| `GET` | `/tasks/{id}` | Get one task |
| `POST` | `/tasks` | Create a task in one of your projects |
| `PUT` | `/tasks/{id}` | Update a task |
| `DELETE` | `/tasks/{id}` | Delete a task |

Query parameters for `GET /tasks`:

| Parameter | Example | Description |
|---|---|---|
| `projectId` | `?projectId=1` | Only tasks from this project |
| `status` | `?status=Done` or `?status=2` | Only tasks with this status |
| `orderBy` | `?orderBy=duedate` | Sort by `duedate` (tasks without a due date last), `title` or `status`. Default: by id |

## Example usage

```http
### 1. Register
POST http://localhost:5196/auth/register
Content-Type: application/json

{ "username": "demo", "password": "secret123" }

### 2. Log in and copy the token from the response
POST http://localhost:5196/auth/login
Content-Type: application/json

{ "username": "demo", "password": "secret123" }

### 3. Create a project
POST http://localhost:5196/projects
Content-Type: application/json
Authorization: Bearer <token>

{ "name": "Learn .NET", "description": "Build a portfolio project" }

### 4. Add a task to the project
POST http://localhost:5196/tasks
Content-Type: application/json
Authorization: Bearer <token>

{ "title": "Write the README", "projectId": 1, "dueDate": "2030-01-01T12:00:00Z" }
```

[`TaskManager.http`](TaskManager.http) has a full set of requests for every endpoint, including error cases, with the expected status code for each one. It can be run from VS Code with the REST Client extension or from Visual Studio.

