# Project Vision

**Project:** Inventory Management REST API

**Version:** 0.2

**Status:** Approved

---

# Purpose

The purpose of this project is to develop a RESTful API that allows authenticated users to manage products, categories, and inventory movements through a secure, scalable, and maintainable architecture.

The solution is intended to demonstrate software engineering best practices, including Clean Architecture, SOLID principles, CQRS where appropriate, automated testing, and containerized deployment.

---

# Business Problem

Organizations require an inventory management system capable of maintaining an accurate record of products and inventory movements.

The system must provide reliable inventory tracking while ensuring data consistency, maintainability, scalability, and extensibility.

---

# Functional Objectives

The system shall allow authenticated users to:

- Manage categories.
- Manage products.
- Register inventory entries.
- Register inventory exits.
- Retrieve current inventory information.
- Retrieve inventory movement history.

---

# Technical Objectives

The solution shall:

- Expose a RESTful API.
- Persist data in SQL Server.
- Run through Docker Compose.
- Protect endpoints using OAuth2 authentication with JWT Bearer tokens.
- Provide OpenAPI (Swagger) documentation.
- Include automated unit tests.
- Follow Clean Architecture principles.
- Apply SOLID principles.
- Apply CQRS where appropriate.

---

# Scope

Version 1 of the system includes:

- Category management.
- Product management.
- Inventory movement registration.
- Inventory queries.
- Authentication.
- API documentation.
- Dockerized deployment.

---

# Out of Scope

The following features are intentionally excluded:

- Purchase orders.
- Sales.
- Suppliers.
- Customers.
- Multi-warehouse support.
- Inventory reservations.
- Reporting.
- Notifications.
- Audit trail.
- User and role administration.

---

# Constraints

The implementation must satisfy the following constraints:

- .NET 8 or newer.
- SQL Server.
- Docker.
- Environment variables for configuration.
- English-only source code.
- Public source repository.
- Clean Code conventions.
- Entity Framework and Dapper according to the technical assessment requirements.
- Unit testing.
- Standardized API errors using RFC 7807 Problem Details.
- API versioning.
- Optimistic concurrency control.

---

# Assumptions

The system assumes:

- Only authenticated users may access the API.
- Authentication is performed through OAuth2 using JWT Bearer tokens issued by the application.
- A predefined administrator account is available for authentication.
- Every product belongs to exactly one category.
- Category names are unique.
- Product SKUs are unique.
- Inventory movements modify the available stock.
- Inventory cannot become negative.
- Categories with associated products cannot be deleted.
- Products with inventory movement history cannot be deleted.
- Inventory movements are represented by a single entity with a movement type (Entry or Exit).
- CurrentStock is maintained as a denormalized value to optimize read performance.
- InventoryMovement represents the source of truth for inventory history.

---

# Success Criteria

The project will be considered successful when:

- All functional requirements are implemented.
- The solution can be executed through Docker Compose.
- The API is fully documented using Swagger.
- Unit tests execute successfully.
- The architecture is modular, maintainable, and aligned with the defined specifications.

---

# Revision History

| Version | Date | Description |
|----------|------------|--------------------------------------------|
| 0.1 | 2026-07-20 | Initial draft |
| 0.2 | 2026-07-20 | Updated after AI architectural review and approved |