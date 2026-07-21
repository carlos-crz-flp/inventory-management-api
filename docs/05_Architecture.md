# 05_Architecture.md

Version: 1.0
Status: Approved

---

# Revision History

| Version | Date | Description |
|----------|------|-------------|
| 1.0 | 2026-07-20 | Initial approved version. |

---

# Based On

This document derives its architectural decisions exclusively from the following approved specifications:

- 01_ProjectVision.md v0.2
- 02_FunctionalSpecification.md v1.2
- 03_DomainModel.md v1.0
- 04_APIContract.md v1.0
- Technical Assessment Specification

No functional requirements, business rules, API contracts, or externally observable behaviors are introduced or modified by this document.

---

# Table of Contents

1. Purpose
2. Scope
3. Architectural Goals
4. Architectural Drivers
5. Architectural Principles
6. High-Level Architecture
7. Solution Structure
8. Layer Responsibilities
9. Request Processing Flow
10. Domain Layer Design
11. Application Layer Design
12. Infrastructure Layer Design
13. Persistence Strategy
14. Security Architecture
15. Validation Strategy
16. Testing Strategy
17. Logging and Observability
18. Dependency Injection
19. Configuration Management
20. Containerization Strategy
21. Deployment Considerations
22. Architectural Constraints
23. Traceability Matrix

# 1. Purpose

This document defines the architecture used to implement the approved functional specifications of the Inventory Management System.

Its purpose is to describe the internal organization of the solution, the responsibilities of each architectural component, and the technologies selected to satisfy the approved requirements.

This document does not define business rules, functional requirements, domain concepts, or externally observable API behavior.

Those concerns remain exclusively defined by the approved specification documents.

---

# 2. Scope

This document specifies:

- Architectural style.
- Project organization.
- Layer responsibilities.
- Technology stack.
- Persistence strategy.
- Security architecture.
- Validation strategy.
- Dependency management.
- Containerization.
- Deployment considerations.

This document does not redefine:

- Functional Requirements.
- Business Rules.
- Domain Model.
- REST Contract.

All architectural decisions shall remain consistent with the approved specifications.

---

# 3. Architectural Goals

The architecture has the following primary goals:

- Maintainability
- Testability
- Scalability
- Separation of Concerns
- Extensibility
- Consistency
- Security
- Readability
- Predictability

Architectural decisions shall prioritize long-term maintainability over implementation convenience.

---

# 4. Architectural Drivers

The architecture is primarily driven by the requirements established by the Technical Assessment Specification and the approved specification documents.

The principal architectural drivers are:

- Implementation using .NET 8.
- RESTful API design.
- SQL Server as the relational database.
- Dockerized application.
- Dockerized SQL Server instance.
- Docker Compose for local orchestration.
- Clean Architecture.
- SOLID principles.
- Clean Code practices.
- CQRS where applicable.
- Entity Framework for read operations.
- Dapper for write operations.
- Unit testing.
- Swagger documentation.

Where multiple implementation alternatives exist, the solution shall favor the approach that best satisfies these architectural drivers.

# 5. Architectural Principles

The solution adopts a layered architecture based on the principles of Clean Architecture.

The following principles govern the organization of the solution:

- Separation of Concerns.
- Dependency Inversion.
- Single Responsibility Principle.
- Explicit Dependencies.
- High Cohesion.
- Low Coupling.
- Composition over Inheritance where appropriate.
- Infrastructure Independence.
- Framework Independence.
- Testability by Design.

Business rules shall remain independent of infrastructure concerns.

External frameworks and technologies shall be considered implementation details.

Dependencies shall always point toward the center of the architecture.

---

# 6. High-Level Architecture

The solution follows a layered architecture organized around the Domain Model.

```
                +----------------------+
                |   Presentation       |
                |      (REST API)      |
                +----------+-----------+
                           |
                           v
                +----------------------+
                |    Application       |
                | CQRS / MediatR       |
                +----------+-----------+
                           |
                           v
                +----------------------+
                |       Domain         |
                | Business Rules       |
                +----------+-----------+
                           ^
                           |
                +----------+-----------+
                |    Infrastructure    |
                | EF / Dapper / SQL    |
                +----------------------+
```

The Domain Layer does not depend on any other layer.

The Application Layer coordinates use cases but does not implement business rules.

The Infrastructure Layer provides technical implementations required by the Domain and Application layers.

The Presentation Layer exposes the REST API defined by the approved API Contract.

---

# 7. Solution Structure

The solution is organized into independent projects.

```
Inventory.sln

src/
│
├── Inventory.Api
├── Inventory.Application
├── Inventory.Domain
├── Inventory.Infrastructure
├── Inventory.Persistence
├── Inventory.SharedKernel

tests/
│
├── Inventory.UnitTests
├── Inventory.ArchitectureTests
```

Each project has a single architectural responsibility.

Dependencies between projects shall follow the dependency rules defined by this document.

---

## 7.1 Inventory.Api

Responsibilities:

- Expose REST endpoints.
- Configure middleware.
- Configure authentication.
- Configure dependency injection.
- Configure Swagger.
- Handle HTTP concerns.

The API project shall not contain business rules.

---

## 7.2 Inventory.Application

Responsibilities:

- Implement CQRS.
- Coordinate use cases.
- Execute Commands.
- Execute Queries.
- Define application interfaces.
- Coordinate transactions.
- Publish domain events when applicable.

The Application Layer shall not contain infrastructure logic.

---

## 7.3 Inventory.Domain

Responsibilities:

- Entities.
- Value Objects.
- Domain Services.
- Business Rules.
- Domain Events.
- Repository Contracts.

The Domain Layer is independent of frameworks.

---

## 7.4 Inventory.Infrastructure

Responsibilities:

- External integrations.
- Authentication providers.
- Logging.
- File system access.
- Email services.
- Third-party services.

Infrastructure provides implementations required by the Application Layer.

---

## 7.5 Inventory.Persistence

Responsibilities:

- SQL Server access.
- Entity Framework configuration.
- Dapper configuration.
- Repository implementations.
- Unit of Work implementation.
- Database migrations.

Persistence isolates all database-specific concerns from the remaining layers.

---

## 7.6 Inventory.SharedKernel

Responsibilities:

- Shared abstractions.
- Base classes.
- Common exceptions.
- Shared result models.
- Shared constants.
- Cross-cutting utilities.

Only components with genuine cross-cutting responsibilities shall reside in this project.

---

# 8. Layer Responsibilities

The following dependency rules shall be enforced.

| Layer | May Depend On | Shall Not Depend On |
|---------|---------------|---------------------|
| Api | Application | Infrastructure, Persistence |
| Application | Domain | Api |
| Domain | None | Any other project |
| Infrastructure | Domain, Application | Api |
| Persistence | Domain, Application | Api |
| SharedKernel | None | Any project |

No architectural rule may be violated without an approved Architectural Decision Record (ADR).

# 9. Request Processing Flow

Every incoming HTTP request follows a predictable processing pipeline that separates presentation, application, domain, and persistence concerns.

The objective of this flow is to ensure that each layer has a single responsibility while maintaining loose coupling between architectural components.

The following diagram illustrates the request lifecycle.

```
                 HTTP Request
                      │
                      ▼
              ASP.NET Controller
                      │
                      ▼
             MediatR Dispatcher
                      │
          ┌───────────┴───────────┐
          │                       │
          ▼                       ▼
   Command Handler         Query Handler
          │                       │
          ▼                       ▼
      Domain Model         Read Model
          │                       │
          ▼                       ▼
   Persistence Layer      Persistence Layer
      (Dapper)            (Entity Framework)
          │                       │
          └───────────┬───────────┘
                      ▼
                 SQL Server
                      │
                      ▼
               HTTP Response
```

The request processing pipeline consists of the following stages.

---

## 9.1 Request Reception

The API receives an HTTP request through an ASP.NET Core Controller.

Controllers are responsible only for HTTP concerns, including:

- Route resolution.
- Model binding.
- Request validation triggering.
- HTTP response generation.

Controllers shall not contain business logic.

---

## 9.2 Application Dispatching

Controllers delegate execution to the Application Layer using MediatR.

Depending on the requested operation, MediatR dispatches either:

- A Command.
- A Query.

Each request is handled by exactly one Handler.

---

## 9.3 Command Processing

Commands represent operations that modify the system state.

Typical command operations include:

- Create Category.
- Update Category.
- Delete Category.
- Create Product.
- Update Product.
- Delete Product.
- Register Inventory Entry.
- Register Inventory Exit.

Command Handlers coordinate application logic and invoke the Persistence Layer to execute write operations using Dapper.

---

## 9.4 Query Processing

Queries represent operations that retrieve information without modifying the system state.

Typical query operations include:

- Retrieve Categories.
- Retrieve Category.
- Retrieve Products.
- Retrieve Product.
- Retrieve Inventory Movement History.

Query Handlers coordinate read operations using Entity Framework.

Queries shall not modify domain state.

---

## 9.5 Domain Interaction

Business rules are implemented within the Domain Layer.

Application Handlers invoke Domain components whenever business validation or domain behavior is required.

The Domain Layer remains independent from infrastructure technologies.

---

## 9.6 Persistence

The Persistence Layer is responsible for all database interactions.

Responsibilities include:

- Executing read operations through Entity Framework.
- Executing write operations through Dapper.
- Managing SQL Server connectivity.
- Managing transactions.
- Applying database migrations.

No database access shall occur outside the Persistence Layer.

---

## 9.7 Response Generation

After the requested operation completes successfully, the Application Layer returns the corresponding result to the API.

The API produces an HTTP response consistent with the approved API Contract.

Successful responses and error responses shall follow the representations defined in the API Contract.

---

## 9.8 Error Flow

Any exception occurring during request processing shall be translated into the standardized error representation defined by the API Contract.

Unhandled exceptions shall never expose implementation details to API consumers.

Error handling shall remain consistent across all endpoints.

# 10. Domain Layer Design

The Domain Layer represents the core of the application.

It contains the business concepts, business rules, and domain behaviors defined by the approved specifications.

The Domain Layer shall remain independent from frameworks, infrastructure technologies, persistence mechanisms, and presentation concerns.

---

## 10.1 Responsibilities

The Domain Layer is responsible for:

- Representing the business model.
- Enforcing business rules.
- Protecting domain consistency.
- Encapsulating domain behavior.
- Defining repository contracts.
- Publishing domain events when applicable.

The Domain Layer shall not contain infrastructure-specific implementations.

---

## 10.2 Domain Components

The Domain Layer is composed of the following building blocks:

- Entities
- Value Objects
- Domain Services
- Repository Interfaces
- Domain Events
- Enumerations
- Domain Exceptions

Each component has a single well-defined responsibility.

---

## 10.3 Entities

Entities represent business concepts with a unique identity.

The Inventory Management System contains the following entities:

- Category
- Product
- InventoryMovement

Entity identities are immutable.

Entities encapsulate business behavior rather than acting solely as data containers.

---

## 10.4 Value Objects

Value Objects represent concepts identified exclusively by their values.

Value Objects are immutable.

Whenever possible, business concepts without identity should be modeled as Value Objects.

Examples include:

- ProductSku
- ProductName
- CategoryName
- Quantity

---

## 10.5 Domain Services

Domain Services encapsulate business behavior that does not naturally belong to a single Entity.

A Domain Service shall be introduced only when business logic cannot be assigned to an Entity or Value Object without violating the Single Responsibility Principle.

---

## 10.6 Repository Interfaces

Repository Interfaces define the persistence contracts required by the Domain Layer.

Repositories expose business-oriented operations without revealing persistence implementation details.

Repository implementations belong exclusively to the Persistence project.

---

## 10.7 Domain Events

Domain Events represent significant business events that occur within the domain.

Examples include:

- ProductCreated
- ProductUpdated
- CategoryCreated
- InventoryEntryRegistered
- InventoryExitRegistered

Domain Events are immutable.

Publishing a Domain Event does not imply asynchronous processing.

The processing strategy is an implementation detail defined by the Application Layer.

---

## 10.8 Enumerations

Enumerations define constrained sets of business values.

Examples include:

- InventoryMovementType
- UserRole

Enumerations shall represent stable business concepts.

---

## 10.9 Domain Exceptions

Domain Exceptions represent violations of business rules.

Domain Exceptions shall communicate business failures without exposing infrastructure concerns.

Infrastructure exceptions shall never be propagated directly into the Domain Layer.

---

## 10.10 Dependency Rules

The Domain Layer:

- Shall not reference ASP.NET Core.
- Shall not reference Entity Framework.
- Shall not reference Dapper.
- Shall not reference SQL Server libraries.
- Shall not reference MediatR.
- Shall not reference dependency injection frameworks.

The Domain Layer may reference only:

- .NET Base Class Library.
- Inventory.SharedKernel.

---

## 10.11 Domain Model Integrity

Business invariants shall always be protected by the Domain Layer.

Invalid domain states shall never be created through public APIs.

Entities are responsible for maintaining their own consistency.

Application Services coordinate business operations but shall not bypass domain validation.

---

## 10.12 Architectural Objective

The Domain Layer is the most stable component of the solution.

Changes in infrastructure, persistence, frameworks, or presentation technologies shall not require modifications to the Domain Layer.

This principle preserves long-term maintainability and supports the goals established by the approved architecture.

# 11. Application Layer Design

The Application Layer orchestrates all use cases exposed by the REST API.

It coordinates business operations without containing business rules.

Business rules remain exclusively within the Domain Layer.

The Application Layer acts as the bridge between the Presentation Layer and the Domain Layer.

---

## 11.1 Responsibilities

The Application Layer is responsible for:

- Coordinating use cases.
- Executing Commands.
- Executing Queries.
- Invoking Domain Entities.
- Managing transactions.
- Coordinating repository usage.
- Publishing Domain Events when applicable.
- Returning application results.

The Application Layer shall not implement business rules.

---

## 11.2 CQRS

The solution adopts the Command Query Responsibility Segregation (CQRS) pattern.

Operations are divided into two categories:

- Commands
- Queries

Commands modify the system state.

Queries retrieve information without modifying the system state.

Every use case belongs to exactly one category.

---

## 11.3 Commands

Commands represent write operations.

Each Command is implemented as an independent request object.

Every Command has exactly one Handler.

Examples include:

- CreateCategoryCommand
- UpdateCategoryCommand
- DeleteCategoryCommand
- CreateProductCommand
- UpdateProductCommand
- DeleteProductCommand
- RegisterInventoryEntryCommand
- RegisterInventoryExitCommand

Command Handlers coordinate domain behavior and execute write operations through the Persistence Layer using Dapper.

Commands do not return domain entities.

---

## 11.4 Queries

Queries represent read operations.

Each Query is implemented as an independent request object.

Every Query has exactly one Handler.

Examples include:

- GetCategoriesQuery
- GetCategoryByIdQuery
- GetProductsQuery
- GetProductByIdQuery
- GetInventoryMovementsQuery

Query Handlers retrieve information through the Persistence Layer using Entity Framework.

Queries never modify domain state.

---

## 11.5 MediatR

All Commands and Queries are dispatched through MediatR.

Controllers communicate only with MediatR.

Controllers shall never invoke Handlers directly.

Handlers shall never invoke other Handlers.

Each request is processed by exactly one Handler.

---

## 11.6 Validation

Application validation occurs before business execution.

Validation responsibilities include:

- Required fields.
- Input format.
- Request consistency.
- Basic request constraints.

Business validation remains the responsibility of the Domain Layer.

Validation shall be implemented using FluentValidation.

---

## 11.7 Transactions

Commands requiring database modifications execute within a transaction.

Successful completion commits the transaction.

Failures cause the transaction to be rolled back.

Queries shall not create transactions unless explicitly required by the persistence mechanism.

---

## 11.8 Repository Usage

Application Handlers interact with persistence through repository interfaces.

Handlers never access SQL Server directly.

Persistence implementation details remain isolated from the Application Layer.

---

## 11.9 Result Objects

Application operations return explicit result objects.

Results communicate:

- Successful execution.
- Validation failures.
- Business failures.
- Unexpected failures.

The Application Layer shall not expose persistence models.

---

## 11.10 Dependency Rules

The Application Layer may reference:

- Inventory.Domain
- Inventory.SharedKernel

The Application Layer shall not depend directly on:

- ASP.NET Core
- SQL Server
- Entity Framework implementation
- Dapper implementation

Infrastructure concerns shall remain outside the Application Layer.

---

## 11.11 Architectural Objective

The Application Layer coordinates every use case without becoming the owner of business rules.

This separation preserves maintainability, testability, and compliance with Clean Architecture principles.

# 12. Infrastructure Layer Design

The Infrastructure Layer provides technical implementations required by the Application and Domain Layers.

It encapsulates interactions with external systems, framework-specific components, and cross-cutting technical services.

The Infrastructure Layer shall not contain business rules.

---

## 12.1 Responsibilities

The Infrastructure Layer is responsible for:

- Authentication services.
- Authorization services.
- Logging.
- External service integrations.
- File system access.
- Email services.
- Cross-cutting technical components.

Infrastructure implementations remain transparent to the Domain Layer.

---

## 12.2 External Dependencies

The Infrastructure Layer is the only architectural layer responsible for interacting with external technologies beyond the persistence mechanism.

Examples include:

- Authentication providers.
- Logging frameworks.
- External APIs.
- File storage.
- Email providers.

Changes to external technologies shall not affect the Domain Layer.

---

## 12.3 Authentication

Authentication services required by the application are implemented within the Infrastructure Layer.

These services are responsible for:

- User credential validation.
- Access token generation.
- Token validation.
- Authentication-related technical operations.

Authentication behavior exposed by the REST API shall remain consistent with the approved API Contract.

---

## 12.4 Authorization

Authorization determines whether an authenticated user is allowed to execute a requested operation.

Authorization rules are enforced outside the Domain Layer.

Authorization mechanisms shall remain independent from business logic.

---

## 12.5 Logging

Infrastructure provides the technical implementation required for application logging.

Logging responsibilities include:

- Recording application events.
- Recording unexpected exceptions.
- Recording diagnostic information.
- Supporting operational troubleshooting.

Business behavior shall not depend on logging.

---

## 12.6 External Services

If the application integrates with external systems, those integrations shall be implemented within the Infrastructure Layer.

Examples include:

- Email notifications.
- External REST APIs.
- Third-party authentication providers.
- Cloud services.

Application and Domain Layers remain independent from these implementations.

---

## 12.7 File System Access

Any interaction with the operating system or file system shall be isolated within the Infrastructure Layer.

The Domain Layer shall never access files directly.

---

## 12.8 Dependency Rules

The Infrastructure Layer may reference:

- Inventory.Application
- Inventory.Domain
- Inventory.SharedKernel

The Infrastructure Layer shall not reference:

- Inventory.Api

Infrastructure implementations satisfy interfaces defined by the Application or Domain Layers.

---

## 12.9 Architectural Objective

The Infrastructure Layer isolates framework-specific and technology-specific implementations from the core business model.

This separation allows infrastructure technologies to evolve independently without affecting business behavior.

# 13. Persistence Strategy

The Persistence Layer encapsulates all database access required by the application.

It is the only architectural layer responsible for interacting with SQL Server.

Persistence implementations remain isolated from the Domain, Application, and Presentation layers.

---

## 13.1 Responsibilities

The Persistence Layer is responsible for:

- SQL Server connectivity.
- Database access.
- Entity Framework configuration.
- Dapper configuration.
- Repository implementations.
- Unit of Work implementation.
- Entity configurations.
- Database context.
- Database migrations.
- Transaction management.

No other layer shall access the database directly.

---

## 13.2 Database Technology

The application uses Microsoft SQL Server as its relational database management system.

All persistent business data is stored in SQL Server.

Database-specific implementations remain isolated within the Persistence project.

---

## 13.3 Read Operations

Read operations are implemented using Entity Framework.

Entity Framework is responsible for:

- Entity retrieval.
- Relationship loading.
- LINQ queries.
- Read model materialization.

Read operations shall not modify persistent data.

---

## 13.4 Write Operations

Write operations are implemented using Dapper.

Write operations include:

- INSERT
- UPDATE
- DELETE

Dapper is responsible for executing SQL statements that modify the database.

Write operations shall preserve transactional consistency.

---

## 13.5 Repository Pattern

Repository implementations encapsulate all persistence logic.

Repositories expose business-oriented operations while hiding database implementation details.

Repository interfaces are defined by the Domain Layer.

Repository implementations belong exclusively to the Persistence Layer.

---

## 13.6 Unit of Work

The Unit of Work coordinates database operations executed during a single application use case.

Its responsibilities include:

- Managing transactions.
- Coordinating repository operations.
- Committing successful operations.
- Rolling back failed operations.

Application Handlers coordinate persistence through the Unit of Work.

---

## 13.7 Database Context

The Entity Framework DbContext represents the application's database session for read operations.

The DbContext is responsible for:

- Entity mapping.
- Relationship mapping.
- Query execution.
- Entity tracking when required.

DbContext configuration remains isolated within the Persistence project.

---

## 13.8 Entity Configurations

Entity mappings are defined using the Entity Framework Fluent API.

Configuration classes remain separate from domain entities.

Domain entities shall not contain persistence-specific configuration.

---

## 13.9 Transactions

Commands that modify persistent data shall execute within a database transaction.

If an operation completes successfully, the transaction shall be committed.

If an operation fails, the transaction shall be rolled back.

Queries shall not modify transactional state.

---

## 13.10 Database Migrations

Database schema evolution is managed through Entity Framework Migrations.

Migration files belong exclusively to the Persistence project.

Schema modifications shall be version controlled together with the application source code.

---

## 13.11 Dependency Rules

The Persistence Layer may reference:

- Inventory.Application
- Inventory.Domain
- Inventory.SharedKernel

The Persistence Layer shall not reference:

- Inventory.Api

The Application Layer shall depend only on repository abstractions and shall remain independent from persistence implementations.

---

## 13.12 Architectural Objective

The Persistence Layer isolates all database-specific concerns from the remaining architecture.

This separation enables the business model and application logic to evolve independently from persistence technologies while maintaining compliance with the approved architectural principles.

# 14. Security Architecture

The application implements a security architecture based on JWT Bearer authentication.

Security mechanisms are responsible for authenticating users, protecting API resources, and ensuring that only authorized requests are processed.

Security concerns remain separated from business logic.

---

## 14.1 Security Objectives

The security architecture pursues the following objectives:

- Authenticate users.
- Protect API resources.
- Prevent unauthorized access.
- Ensure stateless request processing.
- Support secure communication between clients and the API.

---

## 14.2 Authentication

Authentication is implemented using JWT Bearer authentication.

Clients authenticate by providing valid credentials through the Authentication endpoint defined by the approved API Contract.

Upon successful authentication, the system issues an Access Token.

Unauthenticated requests attempting to access protected resources shall be rejected.

---

## 14.3 Access Tokens

Authenticated clients receive an Access Token after successful authentication.

The Access Token is included in subsequent requests using the HTTP Authorization header.

Protected endpoints require a valid Access Token before request processing begins.

Token validation occurs before the request reaches the Application Layer.

---

## 14.4 Authorization

Authorization determines whether an authenticated user is permitted to execute a requested operation.

Authorization is enforced by the Presentation Layer before delegating execution to the Application Layer.

Business rules remain independent from authorization mechanisms.

---

## 14.5 Protected Resources

All API endpoints are protected unless explicitly configured otherwise.

The Authentication endpoint is publicly accessible.

All remaining endpoints require successful authentication.

---

## 14.6 Password Handling

User credentials shall never be stored or transmitted in plain text.

Credential verification shall be performed using secure password hashing mechanisms.

The implementation of the hashing algorithm remains an infrastructure concern.

---

## 14.7 Transport Security

All communication between clients and the API should occur over HTTPS.

Transport encryption protects credentials, access tokens, and application data while in transit.

---

## 14.8 Authentication Failures

Authentication failures shall return the appropriate HTTP status code defined by the approved API Contract.

Authentication error responses shall not disclose sensitive implementation details.

---

## 14.9 Authorization Failures

Requests from authenticated users without sufficient permissions shall be rejected.

Authorization failures shall not expose internal authorization rules or infrastructure details.

---

## 14.10 Dependency Rules

Security implementations belong to the Infrastructure Layer.

The Domain Layer shall remain completely independent from authentication and authorization technologies.

Application use cases shall rely on authenticated identities without depending on security implementation details.

---

## 14.11 Architectural Objective

The security architecture isolates authentication and authorization concerns from business logic while protecting all application resources through a consistent JWT Bearer authentication security model.

# 15. Validation Strategy

Validation is performed at multiple architectural levels.

Each layer validates only the concerns that belong to its responsibility.

This separation prevents duplicated logic while preserving domain integrity.

---

## 15.1 Validation Objectives

The validation strategy pursues the following objectives:

- Ensure request correctness.
- Protect domain consistency.
- Detect invalid input as early as possible.
- Prevent invalid business states.
- Maintain separation of concerns.

---

## 15.2 Validation Levels

Validation is divided into the following levels:

- Request Validation.
- Domain Validation.
- Persistence Validation.

Each validation level addresses a different concern.

---

## 15.3 Request Validation

Request Validation verifies that incoming requests satisfy the expected input requirements before application processing begins.

Typical validations include:

- Required fields.
- String length.
- Numeric ranges.
- Data types.
- Collection constraints.
- Basic format validation.

Request Validation shall be implemented using FluentValidation.

Validation failures prevent the request from reaching the business logic.

---

## 15.4 Domain Validation

Domain Validation enforces business rules defined by the Domain Model.

Examples include:

- Business invariants.
- Entity consistency.
- Value Object constraints.
- Domain-specific rules.

Domain Validation shall always be executed regardless of the request source.

Business rules shall never be implemented exclusively as request validation.

---

## 15.5 Persistence Validation

The Persistence Layer is responsible only for validating persistence-related concerns.

Examples include:

- Database constraints.
- Foreign key violations.
- Unique indexes.
- Transaction failures.

Persistence validation does not replace Domain Validation.

---

## 15.6 Validation Failures

Validation failures shall produce standardized error responses consistent with the approved API Contract.

Validation errors shall clearly identify the invalid fields without exposing implementation details.

---

## 15.7 Exception Handling

Unexpected exceptions shall be handled separately from validation failures.

Validation errors represent expected application behavior and shall not be treated as system failures.

Unhandled exceptions shall be translated into standardized error responses.

---

## 15.8 Dependency Rules

Validation components belong to the Application Layer.

Business validation remains the responsibility of the Domain Layer.

Validation components shall not depend on infrastructure-specific implementations.

---

## 15.9 Architectural Objective

The validation strategy ensures that invalid requests are rejected as early as possible while preserving the Domain Layer as the ultimate authority for business correctness.

This approach minimizes duplicated validation logic and maintains a clear separation between input validation and business rule enforcement.

# 16. Testing Strategy

Testing is an integral part of the application's architecture.

The testing strategy aims to verify that business behavior, application logic, and architectural constraints remain correct as the solution evolves.

Tests shall provide confidence without becoming tightly coupled to implementation details.

---

## 16.1 Objectives

The testing strategy pursues the following objectives:

- Verify business behavior.
- Detect regressions.
- Validate architectural integrity.
- Support maintainability.
- Facilitate refactoring.

Testing shall focus on observable behavior rather than internal implementation.

---

## 16.2 Test Projects

The solution includes the following testing projects:

- Inventory.UnitTests
- Inventory.ArchitectureTests

Each project has a specific architectural responsibility.

---

## 16.3 Unit Tests

Unit Tests verify the behavior of individual components in isolation.

Typical candidates include:

- Domain Entities.
- Value Objects.
- Domain Services.
- Application Handlers.
- Validation components.

Unit Tests shall execute without requiring SQL Server or external services.

---

## 16.4 Architecture Tests

Architecture Tests verify compliance with the architectural rules defined in this document.

Typical architectural validations include:

- Dependency direction.
- Layer isolation.
- Project references.
- Architectural boundaries.

Architecture Tests help prevent unintended violations of the established architecture.

---

## 16.5 Isolation

Tests shall isolate the component under evaluation.

External dependencies shall be replaced by test doubles when necessary.

Unit Tests shall not depend on:

- SQL Server.
- HTTP endpoints.
- External APIs.
- File system resources.

---

## 16.6 Test Data

Each test shall define only the data required for the scenario being verified.

Tests shall remain independent from one another.

No test shall depend on the execution order of another test.

---

## 16.7 Maintainability

Tests shall be:

- Readable.
- Deterministic.
- Repeatable.
- Independent.
- Easy to maintain.

A failing test shall clearly communicate the reason for the failure.

---

## 16.8 Continuous Verification

All tests shall be executable as part of the build process.

The solution shall not require manual intervention to execute automated tests.

---

## 16.9 Architectural Objective

The testing strategy ensures that business behavior and architectural integrity can be verified continuously throughout the lifecycle of the application.

Automated tests provide confidence that future changes preserve the intended design and functionality.

# 17. Logging and Observability

Logging and observability provide visibility into the application's execution and operational health.

The objective is to facilitate troubleshooting, monitor application behavior, and support maintenance activities without affecting business logic.

Logging is considered a cross-cutting concern and is implemented within the Infrastructure Layer.

---

## 17.1 Objectives

The logging and observability strategy pursues the following objectives:

- Record application events.
- Capture unexpected failures.
- Support operational diagnostics.
- Facilitate troubleshooting.
- Improve system maintainability.

Logging shall never modify application behavior.

---

## 17.2 Logging Levels

Log messages shall be categorized according to their severity.

Typical logging levels include:

- Trace
- Debug
- Information
- Warning
- Error
- Critical

The appropriate logging level shall be selected according to the significance of the recorded event.

---

## 17.3 Application Events

The application may record significant technical events, including:

- Application startup.
- Application shutdown.
- Successful authentication.
- Unexpected exceptions.
- External service failures.
- Database connectivity failures.

Business operations shall not depend on logging.

---

## 17.4 Exception Logging

Unexpected exceptions shall be logged before a standardized error response is returned to the client.

Logged exception information may include:

- Timestamp.
- Exception type.
- Error message.
- Stack trace.
- Request path.
- Correlation information when available.

Sensitive information shall not be exposed to API consumers.

---

## 17.5 Structured Logging

Log entries should be recorded using structured data whenever possible.

Structured logging facilitates:

- Searching.
- Filtering.
- Correlation.
- Monitoring.
- Operational analysis.

---

## 17.6 Observability

Observability mechanisms provide visibility into the runtime behavior of the application.

Examples include:

- Application logs.
- Performance metrics.
- Health information.

These mechanisms support operational monitoring without introducing business logic.

---

## 17.7 Security Considerations

Logs shall never expose sensitive information, including:

- User passwords.
- Access tokens.
- Authentication credentials.
- Confidential application data.

Sensitive information shall be masked or omitted before being recorded.

---

## 17.8 Dependency Rules

Logging implementations belong exclusively to the Infrastructure Layer.

The Domain Layer shall remain completely independent from logging frameworks.

Application components may request logging services through abstractions without depending on specific implementations.

---

## 17.9 Architectural Objective

The logging and observability strategy provides operational visibility while preserving the separation between technical infrastructure and business behavior.

Logging supports diagnostics and maintenance without affecting the functional behavior of the application.

# 18. Dependency Injection

Dependency Injection is used to compose the application and provide runtime implementations of application services.

The objective is to decouple abstractions from concrete implementations while preserving the dependency rules established by the architecture.

Dependencies shall be resolved during application startup.

---

## 18.1 Objectives

The dependency injection strategy pursues the following objectives:

- Decouple implementations from abstractions.
- Support maintainability.
- Facilitate unit testing.
- Simplify service composition.
- Preserve architectural boundaries.

Dependency Injection supports the Dependency Inversion Principle defined by SOLID.

---

## 18.2 Service Registration

Application services shall be registered during application startup.

Typical registrations include:

- Application services.
- Repository implementations.
- Persistence services.
- Infrastructure services.
- Authentication services.
- Validation services.

Service registration shall remain centralized.

---

## 18.3 Dependency Resolution

Application components shall depend on abstractions rather than concrete implementations.

Concrete implementations are resolved automatically by the Dependency Injection container.

No component shall instantiate its own dependencies directly.

---

## 18.4 Lifetime Management

Service lifetimes shall be selected according to the responsibility of each component.

Typical lifetimes include:

- Singleton.
- Scoped.
- Transient.

The selected lifetime shall ensure correct behavior while avoiding unnecessary resource consumption.

---

## 18.5 Layer Independence

Dependency Injection enables communication between architectural layers without violating dependency direction.

The Domain Layer remains independent from the Dependency Injection framework.

The Application Layer depends only on abstractions defined by the architecture.

Infrastructure and Persistence projects provide the concrete implementations required at runtime.

---

## 18.6 Constructor Injection

Dependencies shall be provided through constructor injection.

Constructor injection makes component dependencies explicit and facilitates testing.

Property injection and service locator patterns shall not be used.

---

## 18.7 Composition Root

The application's composition root resides in the API project.

During application startup, the API configures all required dependencies and composes the complete object graph.

Other projects shall not perform dependency registration.

---

## 18.8 Testing Considerations

Dependency Injection enables test projects to replace runtime implementations with test doubles when required.

This supports isolated testing of application and domain components without modifying production code.

---

## 18.9 Dependency Rules

Dependency Injection shall preserve the architectural rules defined by this document.

No dependency registration shall introduce a reference that violates the approved layer dependencies.

The Dependency Injection container shall never be used to bypass architectural boundaries.

---

## 18.10 Architectural Objective

The dependency injection strategy enables flexible application composition while preserving loose coupling, testability, and compliance with the Dependency Inversion Principle.

Runtime implementations remain replaceable without affecting the business model or application behavior.

# 19. Configuration Management

Application configuration is managed independently from business logic.

Configuration values provide the runtime behavior required by the application while remaining external to the compiled source code.

The application shall support different execution environments without requiring code modifications.

---

## 19.1 Objectives

The configuration management strategy pursues the following objectives:

- Separate configuration from implementation.
- Support multiple execution environments.
- Simplify deployment.
- Protect sensitive configuration values.
- Promote maintainability.

Configuration changes shall not require recompilation of the application.

---

## 19.2 Configuration Sources

Application configuration may be obtained from multiple sources.

Typical configuration sources include:

- Application configuration files.
- Environment variables.
- Command-line arguments.

The configuration system shall support the standard configuration providers available in ASP.NET Core.

---

## 19.3 Environment-Specific Configuration

Different environments may require different configuration values.

Typical environments include:

- Development.
- Testing.
- Production.

Environment-specific configuration shall remain isolated from the application source code.

---

## 19.4 Database Configuration

Database configuration includes the information required to establish connectivity with SQL Server.

Typical database configuration includes:

- Connection string.
- Database server.
- Database name.
- Authentication settings.

Database configuration shall remain external to the application code.

---

## 19.5 Authentication Configuration

Authentication-related configuration includes the parameters required to support JWT Bearer authentication.

Examples include:

- Token settings.
- Authentication options.
- Security parameters.

Security configuration shall remain external to the compiled application.

---

## 19.6 Swagger Configuration

Swagger configuration controls the generation and presentation of the API documentation.

Documentation settings shall remain configurable without affecting application behavior.

---

## 19.7 Docker Configuration

Container-specific configuration shall remain independent from business logic.

Runtime configuration required by Docker containers shall be provided through supported configuration mechanisms.

Container images shall not contain environment-specific values.

---

## 19.8 Sensitive Configuration

Sensitive configuration values shall not be hardcoded.

Examples include:

- Connection strings.
- Authentication secrets.
- Cryptographic keys.

Sensitive information shall be protected using appropriate configuration mechanisms.

---

## 19.9 Dependency Rules

Application components shall obtain configuration through abstractions rather than reading configuration sources directly.

Configuration access shall remain centralized and consistent across the solution.

The Domain Layer shall remain completely independent from application configuration.

---

## 19.10 Architectural Objective

The configuration management strategy enables the application to operate consistently across multiple environments while keeping configuration concerns separate from business logic and implementation details.

# 20. Containerization Strategy

The application is designed to execute within a containerized environment.

Containerization provides a consistent runtime environment across development, testing, and deployment scenarios while simplifying application provisioning and execution.

The solution uses Docker for containerization and Docker Compose for local service orchestration.

---

## 20.1 Objectives

The containerization strategy pursues the following objectives:

- Ensure environment consistency.
- Simplify application deployment.
- Isolate application dependencies.
- Support reproducible development environments.
- Facilitate local execution.

Containerization shall not modify application behavior.

---

## 20.2 Container Architecture

The solution is composed of multiple cooperating containers.

The primary containers include:

- Inventory API
- SQL Server Database

Additional containers may be incorporated in the future without affecting the application architecture.

---

## 20.3 Service Orchestration

Docker Compose is responsible for orchestrating all application services required for local execution.

Docker Compose coordinates:

- Service startup.
- Network communication.
- Service dependencies.
- Runtime configuration.
- Persistent storage configuration.

Service orchestration remains external to the application code.

---

## 20.4 Application Container

The application container hosts the ASP.NET Core Web API.

Its responsibilities include:

- Hosting the REST API.
- Processing HTTP requests.
- Executing business use cases.
- Communicating with the SQL Server container.

The application container shall remain stateless.

---

## 20.5 Database Container

The database container hosts the Microsoft SQL Server instance required by the application.

The database container is responsible for:

- Persisting application data.
- Processing database requests.
- Supporting transactional operations.

Application components shall communicate with the database exclusively through the Persistence Layer.

---

## 20.6 Persistent Storage

Persistent application data shall remain independent from the lifecycle of individual containers.

The containerization strategy shall provide persistent storage for the SQL Server database to preserve data across container restarts and recreations.

Application containers shall not depend on local file system state.

---

## 20.7 Database Initialization

The solution shall support automated database initialization for development environments.

Database initialization may include:

- Database creation.
- Schema creation.
- Initial reference data.
- Development seed data.

The initialization strategy shall be repeatable and consistent across development environments.

---

## 20.8 Runtime Configuration

Container-specific configuration shall be supplied externally through supported configuration mechanisms.

Typical runtime configuration includes:

- Connection strings.
- Authentication settings.
- Environment variables.
- Service endpoints.

Container images shall remain independent from environment-specific values.

---

## 20.9 Networking

Containers communicate through the network configured by Docker Compose.

Application components shall communicate using service names defined by the orchestration configuration rather than environment-specific network addresses.

---

## 20.10 Dependency Rules

The containerization strategy shall not introduce architectural dependencies between application layers.

Container boundaries are considered deployment concerns and shall remain independent from the internal architecture of the application.

---

## 20.11 Architectural Objective

The containerization strategy provides a reproducible and portable execution environment while preserving the architectural separation between presentation, application, domain, infrastructure, and persistence concerns.

Containerization supports consistent execution without affecting the business behavior of the application.

# 21. Deployment Considerations

This architecture is designed to support consistent deployments across multiple environments.

Deployment activities shall preserve the architectural principles and functional behavior defined by the approved specifications.

Deployment-specific concerns remain independent from business logic.

---

## 21.1 Objectives

The deployment strategy pursues the following objectives:

- Ensure consistent deployments.
- Support multiple execution environments.
- Minimize deployment complexity.
- Preserve application reliability.
- Simplify operational maintenance.

Deployment mechanisms shall not require changes to the application source code.

---

## 21.2 Deployment Environments

The application may be deployed to multiple environments.

Typical environments include:

- Development.
- Testing.
- Production.

Each environment may provide its own configuration while preserving identical application behavior.

---

## 21.3 Deployment Package

The deployable unit consists of the containerized application and its required supporting services.

The deployment package includes:

- Inventory API container.
- SQL Server container.
- Docker Compose configuration.
- Runtime configuration.

Deployment artifacts shall remain version controlled together with the application source code.

---

## 21.4 Configuration Management

Environment-specific configuration shall be supplied during deployment.

Configuration includes:

- Database connection settings.
- Authentication settings.
- Environment variables.
- Application settings.

No environment-specific values shall be embedded within the application binaries.

---

## 21.5 Database Deployment

Database deployment shall remain synchronized with the application version.

Schema changes shall be applied using the approved database migration strategy.

Deployment procedures shall preserve database consistency.

---

## 21.6 Application Availability

Application startup shall verify that all required dependencies are available before accepting requests.

Critical failures during startup shall prevent the application from entering a partially operational state.

---

## 21.7 Rollback Considerations

Deployment procedures should support recovery from unsuccessful deployments.

Application binaries, database schema, and runtime configuration shall remain versioned to facilitate rollback when required.

---

## 21.8 Monitoring

After deployment, the application should remain observable through the logging and observability mechanisms defined by this architecture.

Operational monitoring supports maintenance and incident diagnosis.

---

## 21.9 Architectural Objective

The deployment strategy enables reliable and repeatable application delivery while preserving architectural consistency, operational stability, and compliance with the approved specifications.

# 22. Architectural Constraints

This section defines the architectural constraints that govern the implementation of the Inventory Management System.

These constraints preserve consistency, maintainability, and compliance with the approved specifications.

No implementation decision shall violate these constraints without an approved Architectural Decision Record (ADR).

---

## 22.1 General Constraints

The implementation shall comply with the following architectural principles:

- Clean Architecture.
- SOLID principles.
- Clean Code practices.
- Separation of Concerns.
- Dependency Inversion.

Architectural consistency takes precedence over implementation convenience.

---

## 22.2 Technology Constraints

The implementation shall use the technologies established by the approved specifications.

These include:

- .NET 8.
- ASP.NET Core Web API.
- SQL Server.
- Docker.
- Docker Compose.
- Entity Framework.
- Dapper.
- JWT Bearer authentication.

Technology substitutions are outside the scope of this architecture.

---

## 22.3 Layering Constraints

The dependency rules defined by this architecture shall be strictly enforced.

In particular:

- The Domain Layer shall not depend on any other application layer.
- The Application Layer shall not depend on Infrastructure or Persistence implementations.
- The API Layer shall not contain business rules.
- Persistence shall remain isolated from presentation concerns.

Layer boundaries shall not be bypassed.

---

## 22.4 Business Rule Constraints

Business rules shall exist exclusively within the Domain Layer.

Business rules shall not be duplicated in:

- Controllers.
- Infrastructure components.
- Persistence implementations.
- Database access components.

The Domain Layer remains the authoritative source of business behavior.

---

## 22.5 Persistence Constraints

Database access shall occur exclusively through the Persistence Layer.

Read operations shall be implemented using Entity Framework.

Write operations shall be implemented using Dapper.

Direct database access from other layers is prohibited.

---

## 22.6 Security Constraints

Authentication shall follow the approved JWT Bearer authentication security model.

Protected resources shall require successful authentication before application processing begins.

Security implementation details shall remain outside the Domain Layer.

---

## 22.7 Testing Constraints

The solution shall include automated tests as defined by the approved testing strategy.

Architectural rules shall remain verifiable through Architecture Tests.

Business behavior shall remain verifiable through Unit Tests.

---

## 22.8 Configuration Constraints

Configuration values shall remain external to the application source code.

Sensitive information shall never be hardcoded.

Environment-specific configuration shall remain independent from business logic.

---

## 22.9 Containerization Constraints

The application shall remain executable within the approved Docker-based architecture.

Containerization shall not introduce dependencies between application layers.

Persistent application data shall remain independent from container lifecycle.

---

## 22.10 Architectural Compliance

All implementation decisions shall remain consistent with the following approved documents:

- Project Vision.
- Functional Specification.
- Domain Model.
- API Contract.
- Architecture Document.
- Technical Assessment Specification.

Whenever a conflict exists, the approved specifications shall take precedence over implementation preferences.

---

## 22.11 Architectural Objective

These constraints establish the non-negotiable architectural rules that preserve consistency throughout the implementation lifecycle.

Compliance with these constraints ensures that future development remains aligned with the approved solution architecture.

# 23. Traceability Matrix

This matrix demonstrates the relationship between the approved specification documents and the architectural decisions described in this document.

The purpose of this matrix is to ensure that every architectural decision can be traced back to an approved source and that no architectural element introduces new functional behavior.

| Architectural Topic | Project Vision | Functional Specification | Domain Model | API Contract | Technical Assessment |
|---------------------|:--------------:|:------------------------:|:------------:|:------------:|:--------------------:|
| Solution Objectives | ✓ | | | | |
| Clean Architecture | | | | | ✓ |
| Layered Architecture | | | | | ✓ |
| Project Structure | | | | | ✓ |
| REST API | | ✓ | | ✓ | ✓ |
| Domain Entities | | | ✓ | | |
| Business Rules | | ✓ | ✓ | | |
| Repository Contracts | | | ✓ | | |
| CQRS | | | | | ✓ |
| MediatR | | | | | ✓ |
| Entity Framework (Read Operations) | | | | | ✓ |
| Dapper (Write Operations) | | | | | ✓ |
| SQL Server | | | | | ✓ |
| JWT Bearer authentication | | | | ✓ | ✓ |
| Validation Strategy | | ✓ | ✓ | ✓ | |
| Testing Strategy | | | | | ✓ |
| Dependency Injection | | | | | ✓ |
| Logging Strategy | | | | | ✓ |
| Configuration Management | | | | | ✓ |
| Docker | | | | | ✓ |
| Docker Compose | | | | | ✓ |
| Deployment Strategy | | | | | ✓ |

---

## 23.1 Traceability Principles

Every architectural decision documented in this specification shall be traceable to at least one approved source document.

No architectural component shall introduce:

- New functional requirements.
- New business rules.
- New domain concepts.
- New API behavior.

Architecture defines implementation structure only.

---

## 23.2 Specification Dependency

The following document hierarchy governs the implementation of the solution:

1. Technical Assessment Specification
2. Project Vision
3. Functional Specification
4. Domain Model
5. API Contract
6. Architecture

Lower-level documents shall elaborate higher-level decisions without modifying their intent.

---

## 23.3 Architectural Consistency

If a conflict is identified between this document and an approved specification, the approved specification shall take precedence.

Architectural corrections shall preserve consistency across all specification documents.

---

## 23.4 Change Control

Changes affecting the architecture shall be evaluated to determine whether they also impact:

- Project Vision.
- Functional Specification.
- Domain Model.
- API Contract.
- Technical Assessment Specification.

Any inconsistency shall be resolved before implementation begins.

---

## 23.5 Architectural Objective

The Traceability Matrix ensures that the architecture remains fully aligned with the approved specifications and the technical assessment.

This traceability supports maintainability, auditability, and consistency throughout the implementation lifecycle.