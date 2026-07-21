# Inventory Management API

A RESTful Inventory Management API built with **.NET 8** following **Clean Architecture** and the **CQRS (Command Query Responsibility Segregation)** pattern.

The application provides authenticated endpoints to manage categories, products, inventory movements and stock levels while enforcing business rules through a rich domain model.

This project was developed as part of a senior backend technical assessment, emphasizing maintainability, scalability, testability and software design best practices.

---

# Features

- JWT Bearer Authentication
- Category Management (CRUD)
- Product Management (CRUD)
- Inventory Entry Registration
- Inventory Exit Registration
- Inventory Movement History
- Automatic Entity Framework Migrations
- Swagger / OpenAPI Documentation
- Docker Support
- Clean Architecture
- CQRS with MediatR
- FluentValidation
- Entity Framework Core (Read Operations)
- Dapper (Write Operations)
- Unit Tests
- Architecture Tests

---

# Architecture

The solution follows the principles of **Clean Architecture**, keeping business logic isolated from infrastructure concerns.

The application implements the **CQRS** pattern using **MediatR**, separating read and write operations into independent use cases.

To satisfy the assessment requirements:

- **Entity Framework Core** is used for read operations and database migrations.
- **Dapper** is used for write operations.
- Business rules are implemented inside the Domain layer.
- Validation is handled through FluentValidation.
- Dependency Injection is used throughout the solution.

---

# Architecture Overview

```
                +-------------------+
                |     HTTP Client   |
                +---------+---------+
                          |
                          v
                +-------------------+
                |   Inventory.Api   |
                +---------+---------+
                          |
                          v
                +-------------------+
                | Inventory.Application |
                | Commands / Queries |
                +---------+---------+
                          |
                          v
                +-------------------+
                | Inventory.Domain  |
                | Business Rules    |
                +---------+---------+
                          |
                          v
                +-------------------+
                | Infrastructure    |
                | EF Core / Dapper  |
                +---------+---------+
                          |
                          v
                +-------------------+
                |   SQL Server      |
                +-------------------+
```

---

# Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Dapper
- MediatR
- FluentValidation
- SQL Server
- JWT Bearer Authentication
- Docker
- Swagger / OpenAPI
- xUnit
- Moq
- FluentAssertions
- NetArchTest

---

# Project Structure

```
InventoryManagement
│
├── src
│   ├── Inventory.Api
│   ├── Inventory.Application
│   ├── Inventory.Domain
│   └── Inventory.Infrastructure
│
├── tests
│   ├── Inventory.Application.Tests
│   ├── Inventory.Domain.Tests
│   └── Inventory.Architecture.Tests
│
├── docker-compose.yml
├── Dockerfile
├── docker-entrypoint.sh
└── wait-for-it.sh
```

---

# Project Responsibilities

| Project | Responsibility |
|----------|----------------|
| Inventory.Api | REST API, Middleware, Authentication, Dependency Injection |
| Inventory.Application | CQRS, Commands, Queries, Validators and Use Cases |
| Inventory.Domain | Entities, Value Objects and Business Rules |
| Inventory.Infrastructure | EF Core, Dapper, Authentication and Persistence |

---

# Business Rules

The domain layer enforces the following business rules:

- Products cannot have negative stock.
- Every inventory movement generates a movement record.
- Product stock is always updated through domain methods.
- Categories are soft deleted.
- Products are soft deleted.
- Domain entities protect their own invariants.

---

# Getting Started

## Prerequisites

Before running the application, install:

- .NET 8 SDK
- Docker Desktop
- Git

---

## Clone the repository

```bash
git clone https://github.com/carlos-crz-flp/inventory-management-api.git

cd inventory-management-api
```

---

## Run with Docker

Build and start all services:

```bash
docker compose up --build
```

The application automatically:

- Starts SQL Server
- Waits until SQL Server becomes available
- Applies pending Entity Framework migrations
- Creates the database if necessary
- Starts the Web API

API

```
http://localhost:8080
```

Swagger

```
http://localhost:8080/swagger
```

Stop containers:

```bash
docker compose down
```

View logs:

```bash
docker compose logs -f
```

---

## Run Locally

Update the SQL Server connection string located in:

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

# Configuration

Application configuration is provided through:

- ConnectionStrings
- Jwt

For production environments these values should be supplied through environment variables.

---

# Authentication

Authentication is implemented using **JWT Bearer Tokens**.

A lightweight authentication flow was implemented for this technical assessment, issuing JWT tokens directly from the API instead of integrating an external OAuth2 Authorization Server.

After authentication include the token in every request:

```
Authorization: Bearer {your_token}
```

---

# API Documentation

Swagger is enabled by default.

```
http://localhost:8080/swagger
```

Swagger allows testing every endpoint directly from the browser.

---

# Testing

The solution includes different types of automated tests:

- Domain Unit Tests
- Application Unit Tests
- FluentValidation Tests
- Architecture Tests

Architecture tests verify:

- Layer dependencies
- CQRS conventions
- Validators
- Handlers
- Naming conventions

Run all tests:

```bash
dotnet test
```

---

# Design Decisions

Some of the most important architectural decisions include:

- Clean Architecture
- CQRS using MediatR
- SOLID Principles
- Repository Pattern
- FluentValidation
- Dependency Injection
- Entity Framework Core for read operations
- Dapper for write operations
- JWT Bearer Authentication
- Docker Compose for local development
- Architecture Tests using NetArchTest and Reflection

---

# Future Improvements

Possible future enhancements include:

- Refresh Token support
- Role-Based Authorization
- Global Exception Handling using ProblemDetails
- Structured Logging with Serilog
- Health Checks
- API Versioning
- Rate Limiting
- Distributed Caching
- CI/CD Pipeline
- Integration Tests

---

# License

This project was created for educational purposes and as part of a technical assessment.