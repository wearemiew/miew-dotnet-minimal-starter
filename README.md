<img class="logo" src="https://github.com/wearemiew/.github/raw/main/static/miew-banner.png" alt="Miew Banner"/>

# .NET Minimal API Starter

A clean, modular .NET 9.0 Minimal API starter template following Domain-Driven Design (DDD) principles and clean architecture patterns.

## Features

- ✅ .NET 9.0 Minimal API architecture
- ✅ Domain-Driven Design approach
- ✅ Entity Framework Core with PostgreSQL
- ✅ ASP.NET Core Identity for authentication
- ✅ OpenAPI (Swagger) documentation
- ✅ Structured logging with Serilog
- ✅ Docker and Docker Compose support
- ✅ Unit tests with xUnit
- ✅ Clean, modular architecture

## Project Structure

```
├── minimal api/                        # Solution directory
│   ├── minimal api/                    # Main API project
│   │   ├── Application/                # Application layer
│   │   │   └── Interfaces/             # Repository interfaces
│   │   ├── Configurations/             # App configurations
│   │   ├── Domain/                     # Domain models
│   │   │   ├── Entities/               # Domain entities (User, Post)
│   │   │   ├── Enums/                  # Domain enumerations
│   │   │   └── ValueObjects/           # Value objects
│   │   ├── Features/                   # Feature modules
│   │   │   ├── Extensions/             # Feature extensions
│   │   │   ├── Posts/                  # Posts feature
│   │   │   └── Users/                  # Users feature
│   │   ├── Infrastructure/             # Infrastructure layer
│   │   │   └── Repositories/           # Repository implementations
│   │   └── Program.cs                  # Main application entry point
│   └── minimal api.tests/              # Test project
│       └── [Test classes]              # Tests for domain and features
└── docker-compose.yaml                 # Docker Compose configuration
```

## Prerequisites

- .NET 9.0 SDK
- PostgreSQL database
- Docker (optional, for containerized deployment)

## Getting Started

### Running Locally

1. Update the connection string in `appsettings.json` to point to your PostgreSQL instance.

2. Run the migrations to create the database:
   ```bash
   cd "minimal api/minimal api"
   dotnet ef database update
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Access the API at https://localhost:5001 or http://localhost:5000

### Using Docker

1. Build and run using Docker Compose:
   ```bash
   docker-compose up --build
   ```

2. Access the API at http://localhost:8080

## API Endpoints

The API includes endpoints for managing users and posts:

- **Users:**
  - GET /api/users - List all users
  - GET /api/users/{id} - Get a specific user
  - POST /api/users - Create a new user
  - PUT /api/users/{id} - Update a user
  - DELETE /api/users/{id} - Delete a user

- **Posts:**
  - GET /api/posts - List all posts
  - GET /api/posts/{id} - Get a specific post
  - POST /api/posts - Create a new post
  - PUT /api/posts/{id} - Update a post
  - DELETE /api/posts/{id} - Delete a post

## Testing

Run the tests with:

```bash
cd "minimal api/minimal api.tests"
dotnet test
```

## Logging

Logs are written to the console and to log files in the `logs` directory.
