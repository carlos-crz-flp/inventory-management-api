# Inventory Management API

A RESTful Inventory Management API built with **.NET 8** following **Clean Architecture** and the **CQRS** pattern.

The application provides authenticated endpoints to manage product categories, products, and inventory movements while maintaining stock consistency through business rules. It demonstrates modern backend development practices with a focus on maintainability, scalability, and separation of concerns.

---

## Features

- JWT Bearer Authentication
- Category Management
- Product Management
- Inventory Entry Registration
- Inventory Exit Registration
- Inventory Movement History
- Automatic Database Migrations
- Swagger / OpenAPI Documentation
- Docker Support
- Clean Architecture
- CQRS with MediatR
- Entity Framework Core and Dapper integration

---

## Architecture

The solution follows the principles of **Clean Architecture**, separating responsibilities into independent layers.

The application implements the **CQRS (Command Query Responsibility Segregation)** pattern using MediatR, separating read and write operations while keeping business logic isolated from infrastructure concerns.

Entity Framework Core is used for database access and migrations, while Dapper is used for lightweight SQL operations. Authentication is implemented using JWT Bearer Tokens.

---

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Dapper
- MediatR
- SQL Server
- JWT Bearer Authentication
- Docker
- Swagger / OpenAPI

---

## Project Structure

```
InventoryManagementApi/
│
├── Inventory.Api              # Presentation layer
├── Inventory.Application      # Application layer (CQRS, DTOs, Interfaces)
├── Inventory.Domain           # Domain entities and business rules
├── Inventory.Infrastructure   # Persistence, Authentication and Services
├── Inventory.SharedKernel     # Shared abstractions and common components
│
├── docker-compose.yml
├── Dockerfile
├── docker-entrypoint.sh
└── wait-for-it.sh
```

### Project Responsibilities

| Project | Responsibility |
|----------|----------------|
| Inventory.Api | API endpoints, dependency injection and middleware configuration |
| Inventory.Application | Use cases, Commands, Queries, Validators and Interfaces |
| Inventory.Domain | Business entities, domain rules and contracts |
| Inventory.Infrastructure | Database access, repositories, authentication and external services |
| Inventory.SharedKernel | Shared abstractions and common components |

---

## Getting Started

### Prerequisites

Before running the application, ensure you have installed:

- .NET 8 SDK
- Docker Desktop
- Git

---

### Clone the repository

```bash
git clone https://github.com/carlos-crz-flp/inventory-management-api.git

cd inventory-management-api
```

---

### Run with Docker

Build and start the containers:

```bash
docker compose up --build
```

The application will automatically:

- Start SQL Server
- Wait until SQL Server is available
- Apply pending Entity Framework migrations
- Create the database if it does not exist
- Start the Web API

Once running:

API

```
http://localhost:8080
```

Swagger

```
http://localhost:8080/swagger
```

---

### Run locally

Update the SQL Server connection string in:

```
Inventory.Api/appsettings.json
```

Apply migrations:

```bash
dotnet ef database update \
--project Inventory.Infrastructure \
--startup-project Inventory.Api
```

Run the API:

```bash
dotnet run --project Inventory.Api
```

---

## Configuration

The application uses the following configuration sections:

```json
ConnectionStrings

Jwt
```

The default configuration is located in:

```
Inventory.Api/appsettings.json
```

---

## Authentication

Authentication is based on **JWT Bearer Tokens**.

Obtain a token using the authentication endpoint and include it in subsequent requests:

```
Authorization: Bearer {your_token}
```

---

## API Documentation

Swagger is enabled by default.

Once the application is running, access:

```
http://localhost:8080/swagger
```

Swagger allows testing every endpoint directly from the browser.

---

## Design Decisions

Some of the architectural decisions made during development include:

- Clean Architecture to enforce separation of concerns.
- CQRS using MediatR for clear command/query separation.
- Dependency Injection throughout the application.
- Repository Pattern to abstract persistence.
- Unit of Work to coordinate transactional operations.
- Entity Framework Core for persistence and database migrations.
- Dapper for lightweight SQL execution.
- JWT Bearer Authentication for stateless security.
- Docker Compose for simplified local development.

---

## Future Improvements

Possible enhancements include:

- Unit and Integration Testing
- Refresh Token implementation
- Role-Based Authorization
- Global Exception Handling with Problem Details
- Logging with Serilog
- Health Checks
- CI/CD pipeline
- API Versioning
- Rate Limiting
- Caching

---

## License

This project is provided for educational and demonstration purposes.