# 06_ArchitectureDecisionRecords.md

**Version:** 1.0

**Status:** Approved

---

# Revision History

| Version | Date | Description |
| ------- | ---------- | --------------------------- |
| 1.0 | 2026-07-20 | Initial approved version. |

---

# Based On

This document derives its architectural decisions exclusively from the following approved documents:

- 01_ProjectVision.md v0.2
- 02_FunctionalSpecification.md v1.2
- 03_DomainModel.md v1.0
- 04_APIContract.md v1.0
- 05_Architecture.md v1.0
- Technical Assessment Specification

No functional requirements, business rules, domain concepts, API contracts, or externally observable behaviors are introduced or modified by this document.

This document records only the architectural decisions that govern the implementation of the approved specifications.

---

# Table of Contents

1. Purpose
2. Scope
3. ADR Methodology
4. Architecture Decision Records
   - ADR-001 – Adopt Clean Architecture
   - ADR-002 – Organize the Solution into Independent Projects
   - ADR-003 – Adopt CQRS
   - ADR-004 – Use MediatR for Request Dispatching
   - ADR-005 – Separate Read and Write Persistence Technologies
   - ADR-006 – Use Entity Framework for Read Operations
   - ADR-007 – Use Dapper for Write Operations
   - ADR-008 – Use SQL Server as the Relational Database
   - ADR-009 – Adopt JWT Bearer Authentication
   - ADR-010 – Use FluentValidation for Request Validation
   - ADR-011 – Adopt Dependency Injection
   - ADR-012 – Use Docker for Containerization
   - ADR-013 – Use Docker Compose for Local Orchestration
   - ADR-014 – Use Swagger / OpenAPI for API Documentation
   - ADR-015 – Enforce Layer Dependency Rules Through Architecture Tests
5. Decision Traceability Matrix
6. ADR Governance

---

# 1. Purpose

This document records the architectural decisions adopted during the design of the Inventory Management System.

Its purpose is to preserve the rationale behind the selected implementation approaches, document the principal alternatives that were evaluated, and describe the architectural consequences of each accepted decision.

The Architecture Decision Records (ADRs) contained in this document complement the approved Architecture Specification by explaining **why** specific implementation strategies were selected.

This document does not define or modify:

- Functional Requirements.
- Business Rules.
- Domain Model.
- REST API Contract.
- Externally observable system behavior.

Those concerns remain exclusively defined by the approved specification documents.

---

# 2. Scope

This document records architectural decisions related to:

- Architectural style.
- Solution organization.
- Application structure.
- Persistence strategy.
- Security implementation.
- Validation implementation.
- Dependency management.
- Containerization.
- API documentation.
- Architectural governance.

Each decision is derived from the approved Architecture Specification and the Technical Assessment Specification.

No Architecture Decision Record introduces new functional behavior or alters any approved specification.

Whenever a future architectural decision affects implementation without modifying approved behavior, it should be documented through an additional ADR rather than by modifying the approved specification baseline.

---

# 3. ADR Methodology

This document follows the Architecture Decision Record (ADR) approach to document significant architectural decisions.

Each ADR captures a single decision together with its architectural context and rationale.

Every Architecture Decision Record follows a consistent structure consisting of:

- Status
- Context
- Decision
- Alternatives Considered
- Consequences
- Specification Impact
- Related Documents

The objective of this methodology is to ensure that architectural decisions remain:

- Traceable.
- Justified.
- Consistent.
- Reviewable.
- Independent from functional specifications.

Architectural decisions document implementation strategy only.

They do not redefine approved business behavior or externally observable API behavior.

---

# 4. Architecture Decision Records

---

# ADR-001 — Adopt Clean Architecture

## Status

**Accepted**

---

## Context

The approved specifications require a solution that is maintainable, testable, extensible, and independent from specific frameworks and infrastructure technologies.

The architecture must preserve a clear separation between business rules and implementation concerns while allowing infrastructure technologies to evolve without affecting the approved Domain Model.

A consistent architectural style is therefore required to organize the solution and govern dependencies between components.

---

## Decision

The solution **adopts Clean Architecture** as the primary architectural style.

The implementation is organized into independent architectural layers with explicit dependency rules.

Dependencies always point toward the Domain Layer.

Business rules remain independent from:

- Presentation technologies.
- Persistence technologies.
- Frameworks.
- Infrastructure implementations.

The architectural layers and their responsibilities are defined in the approved Architecture Specification.

---

## Alternatives Considered

### Traditional Layered Architecture

Provides separation between presentation, business logic, and persistence.

Rejected because business rules typically become coupled to infrastructure concerns as the application evolves.

### N-Layer Architecture with Shared Dependencies

Simplifies project organization.

Rejected because unrestricted dependencies reduce maintainability and increase coupling between layers.

### Vertical Slice Architecture

Provides strong feature isolation.

Rejected because the Technical Assessment Specification explicitly establishes Clean Architecture as the preferred organizational model for the solution.

---

## Consequences

### Positive Consequences

- Strong separation of concerns.
- High maintainability.
- Improved testability.
- Infrastructure independence.
- Long-term architectural stability.
- Explicit dependency direction.

### Negative Consequences

- Increased number of projects.
- Additional abstraction compared to simpler architectures.
- Greater initial implementation effort.

### Trade-offs

The additional structural complexity is accepted because it improves maintainability and supports long-term evolution of the solution.

---

## Specification Impact

This ADR records an accepted architectural decision.

It introduces no modifications to the approved:

- Project Vision.
- Functional Specification.
- Domain Model.
- API Contract.

It defines only the architectural organization used to implement the approved specifications.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Architectural Principles, High-Level Architecture, Layer Responsibilities)

---

# ADR-002 — Organize the Solution into Independent Projects

## Status

**Accepted**

---

## Context

The selected architectural style requires clear physical separation between architectural responsibilities.

A single project containing all application code would make dependency enforcement difficult and increase coupling between business logic and infrastructure.

The solution structure should reinforce the architectural boundaries defined by the approved Architecture.

---

## Decision

The solution **is organized into independent projects** corresponding to the principal architectural layers.

The approved solution structure consists of:

- Inventory.Api
- Inventory.Application
- Inventory.Domain
- Inventory.Infrastructure
- Inventory.Persistence
- Inventory.SharedKernel

Dedicated test projects are maintained separately from production code.

Project references enforce the dependency direction established by the approved Architecture.

---

## Alternatives Considered

### Single Project Solution

Provides the simplest project structure.

Rejected because architectural boundaries cannot be effectively enforced through project references.

### Three-Project Layered Solution

Separates presentation, business logic, and persistence.

Rejected because it provides insufficient isolation for the architectural responsibilities defined by Clean Architecture.

### Feature-Based Projects

Organizes projects by functional area.

Rejected because the Technical Assessment Specification prioritizes architectural layering over feature-oriented physical organization.

---

## Consequences

### Positive Consequences

- Clear architectural boundaries.
- Strong dependency enforcement.
- Improved maintainability.
- Independent project evolution.
- Simplified testing.

### Negative Consequences

- Increased solution complexity.
- Additional project configuration.
- More explicit dependency management.

### Trade-offs

The additional project structure is accepted to preserve long-term architectural integrity.

---

## Specification Impact

This ADR records an accepted architectural decision.

It establishes only the physical organization of the implementation.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Solution Structure, Layer Responsibilities)

---

# ADR-003 — Adopt CQRS

## Status

**Accepted**

---

## Context

The approved API Contract defines operations that either modify application state or retrieve information.

The Technical Assessment Specification recommends applying Command Query Responsibility Segregation (CQRS) to separate these responsibilities and simplify the application layer.

The architecture should distinguish write operations from read operations without altering the externally observable API behavior.

---

## Decision

The Application Layer **adopts Command Query Responsibility Segregation (CQRS).**

Application use cases are implemented as either:

- A **Command**, representing an operation that modifies system state.
- A **Query**, representing an operation that retrieves information without modifying system state.

Each request is handled independently through the Application Layer.

The adoption of CQRS is an internal architectural decision and does not affect the REST API defined by the approved API Contract.

---

## Alternatives Considered

### Traditional CRUD Application Services

Implements read and write operations through the same service classes.

Rejected because it tends to concentrate responsibilities within large service classes and reduces separation between commands and queries.

### Generic Repository with Shared Services

Provides a simple implementation model.

Rejected because it does not clearly distinguish between state-changing operations and read-only operations.

### Event Sourcing

Provides complete event history and reconstruction capabilities.

Rejected because the approved specifications do not require event sourcing and its additional complexity is not justified for the current project scope.

---

## Consequences

### Positive Consequences

- Clear separation between commands and queries.
- Improved maintainability.
- Simpler application handlers.
- Better alignment with the approved Architecture.
- Independent optimization of read and write operations.

### Negative Consequences

- Increased number of application classes.
- Additional organizational complexity.
- More explicit request handling infrastructure.

### Trade-offs

The additional implementation structure is accepted because it improves clarity, scalability, and adherence to the architectural principles established by the project.

---

## Specification Impact

This ADR records an accepted architectural decision.

CQRS is an internal implementation strategy used to realize the approved behavior without affecting the externally observable API.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Application Layer Design, Request Processing Flow)

---

# ADR-004 — Use MediatR for Request Dispatching

## Status

**Accepted**

---

## Context

The approved Architecture separates application use cases into individual Commands and Queries following the CQRS pattern.

A mechanism is required to dispatch application requests while keeping the Presentation Layer independent from the implementation details of the Application Layer.

The selected approach should promote loose coupling and simplify request handling without modifying the externally observable behavior defined by the approved API Contract.

---

## Decision

The solution **uses MediatR** to dispatch Commands and Queries within the Application Layer.

Controllers communicate exclusively with MediatR, which delegates each request to its corresponding handler.

The Presentation Layer remains unaware of handler implementations, preserving the architectural boundaries established by the approved Architecture.

---

## Alternatives Considered

### Direct Service Invocation

Controllers invoke application services directly.

Rejected because it tightly couples controllers to service implementations and reduces flexibility.

### Custom Request Dispatcher

A proprietary request dispatching mechanism.

Rejected because MediatR provides a mature implementation with broad adoption and supports the architectural goals of the project.

### Command Bus Framework

A dedicated command bus solution.

Rejected because the project scope does not require capabilities beyond those provided by MediatR.

---

## Consequences

### Positive Consequences

- Loose coupling between controllers and application handlers.
- Simplified request routing.
- Improved maintainability.
- Better separation of concerns.
- Consistent implementation of CQRS.

### Negative Consequences

- Additional framework dependency.
- Developers require familiarity with MediatR.

### Trade-offs

The additional dependency is accepted because it significantly simplifies request dispatching while preserving architectural boundaries.

---

## Specification Impact

This ADR records an accepted implementation decision.

It introduces no modifications to the approved specifications or externally observable API behavior.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Application Layer Design, Request Processing Flow)

---

# ADR-005 — Separate Read and Write Persistence Technologies

## Status

**Accepted**

---

## Context

The approved Architecture adopts CQRS, allowing read and write operations to evolve independently.

The project requires a persistence strategy that balances development productivity with efficient execution while remaining consistent with the approved Technical Assessment Specification.

The selected persistence approach is an implementation decision specific to this project and is not intended as a universal recommendation for all systems.

---

## Decision

The solution **uses separate persistence technologies** for read and write operations.

- Entity Framework Core is used for read operations.
- Dapper is used for write operations.

This separation supports the architectural organization established by CQRS while preserving the behavior defined by the approved specifications.

---

## Alternatives Considered

### Entity Framework Core Only

A single ORM is used for both reads and writes.

Rejected because the approved Architecture intentionally separates persistence responsibilities to align with CQRS.

### Dapper Only

A lightweight data access approach is used for all persistence operations.

Rejected because the project benefits from Entity Framework's mapping capabilities for read scenarios.

### Custom Data Access Framework

A proprietary persistence solution.

Rejected because it would introduce unnecessary implementation complexity.

---

## Consequences

### Positive Consequences

- Independent optimization of read and write operations.
- Clear persistence responsibilities.
- Improved maintainability.
- Alignment with the approved Architecture.

### Negative Consequences

- Two persistence technologies must be maintained.
- Developers require familiarity with both frameworks.

### Trade-offs

The additional implementation complexity is accepted because it supports the architectural goals established for this specific project.

---

## Specification Impact

This ADR records an accepted implementation decision.

It does not modify the approved specifications or externally observable behavior.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Persistence Strategy)

---

# ADR-006 — Use Entity Framework for Read Operations

## Status

**Accepted**

---

## Context

Read operations frequently require object mapping and efficient retrieval of relational data.

The approved persistence strategy separates read and write concerns while maintaining consistency with the approved Architecture.

---

## Decision

The solution **uses Entity Framework Core** for read operations.

Entity Framework provides object-relational mapping capabilities that simplify query implementation while supporting the approved Domain Model.

Read operations remain isolated from write operations according to the approved persistence strategy.

---

## Alternatives Considered

### Dapper for Read Operations

Lightweight SQL-based queries.

Rejected because Entity Framework simplifies object mapping for the project's read requirements.

### Manual ADO.NET

Direct database access.

Rejected because it increases implementation complexity without providing additional architectural benefits.

### Custom ORM

A proprietary mapping framework.

Rejected because Entity Framework provides mature functionality appropriate for the project.

---

## Consequences

### Positive Consequences

- Simplified object mapping.
- Improved developer productivity.
- Mature ORM capabilities.
- Better maintainability.

### Negative Consequences

- Additional abstraction layer.
- Potential performance overhead for some query scenarios.

### Trade-offs

The selected approach prioritizes maintainability and development productivity for read operations.

---

## Specification Impact

This ADR records an accepted implementation decision.

It introduces no modifications to the approved specifications.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Persistence Strategy)

---

# ADR-007 — Use Dapper for Write Operations

## Status

**Accepted**

---

## Context

Write operations require efficient execution of transactional commands while remaining consistent with the CQRS persistence strategy established by the approved Architecture.

The selected implementation should provide explicit control over database commands without affecting the approved system behavior.

---

## Decision

The solution **uses Dapper** for write operations.

Dapper executes transactional commands directly against the relational database while preserving the architectural separation between read and write responsibilities.

The use of Dapper is an implementation decision that supports the approved persistence strategy.

---

## Alternatives Considered

### Entity Framework Core for Write Operations

Entity Framework manages write operations.

Rejected because the approved Architecture intentionally separates persistence responsibilities.

### Manual ADO.NET

Database commands are implemented directly through ADO.NET.

Rejected because Dapper provides a lightweight abstraction while preserving explicit SQL execution.

### Stored Procedure-Only Strategy

All write operations are implemented exclusively through stored procedures.

Rejected because the approved Architecture does not require such a restriction.

---

## Consequences

### Positive Consequences

- Lightweight data access.
- Explicit SQL execution.
- Efficient transactional operations.
- Alignment with the approved persistence strategy.

### Negative Consequences

- Manual SQL maintenance.
- Developers require familiarity with Dapper.

### Trade-offs

The additional responsibility of maintaining SQL statements is accepted because it provides efficient and predictable write operations.

---

## Specification Impact

This ADR records an accepted implementation decision.

It introduces no changes to the approved specifications or externally observable behavior.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Persistence Strategy)

---

# ADR-008 — Use SQL Server as the Relational Database

## Status

**Accepted**

---

## Context

The approved Technical Assessment Specification identifies Microsoft SQL Server as the relational database platform for the Inventory Management System.

The selected persistence platform must support transactional consistency, relational integrity, mature tooling, and seamless integration with the technologies adopted by the approved Architecture.

The database technology is an implementation decision and does not affect the externally observable behavior defined by the approved specifications.

---

## Decision

The solution **uses Microsoft SQL Server** as its relational database management system.

SQL Server stores the persistent data required by the approved Domain Model and supports the persistence strategy defined by the approved Architecture.

The database platform is selected as part of the project's implementation approach and remains transparent to API consumers.

---

## Alternatives Considered

### PostgreSQL

Provides a mature, standards-compliant relational database platform.

Rejected because the Technical Assessment Specification selected SQL Server as the implementation platform for this project.

---

### MySQL

Widely adopted and suitable for many transactional systems.

Rejected because selecting a different database platform would introduce unnecessary divergence from the approved technical baseline.

---

### NoSQL Database

Provides flexible schema evolution and horizontal scalability.

Rejected because the approved Domain Model is strongly relational and requires transactional consistency between aggregates.

---

## Consequences

### Positive Consequences

- Mature relational database platform.
- Strong transactional support.
- Robust tooling and administration.
- Tight integration with the selected persistence technologies.
- Broad industry adoption and long-term support.

### Negative Consequences

- Vendor-specific platform.
- Future migration to another relational database would require additional implementation effort.

### Trade-offs

Adopting SQL Server aligns the implementation with the approved technical baseline while providing a mature and stable relational platform.

---

## Specification Impact

This ADR records an accepted implementation decision.

It introduces no modifications to the approved:

- Project Vision
- Functional Specification
- Domain Model
- API Contract

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Persistence Strategy)

---

# ADR-009 — Adopt JWT Bearer Authentication

## Status

**Accepted**

---

## Context

The approved API Contract specifies JWT Bearer authentication for protecting secured endpoints.

The implementation requires an authentication mechanism that validates bearer tokens while remaining transparent to clients beyond the behavior already defined by the API Contract.

The architectural decision concerns the implementation of authentication, not the definition of externally observable authentication behavior.

---

## Decision

The solution **adopts JWT Bearer authentication** for securing protected API endpoints.

Authentication is implemented using bearer tokens as defined by the approved API Contract.

The implementation validates JWT tokens before protected operations are executed while preserving the authentication behavior already specified by the approved API Contract.

---

## Alternatives Considered

### Session-Based Authentication

Maintains authenticated user state on the server.

Rejected because the approved API Contract defines stateless bearer token authentication.

---

### Cookie-Based Authentication

Provides browser-oriented authentication using cookies.

Rejected because the solution exposes a REST API intended to operate through bearer tokens.

---

### API Key Authentication

Provides simple request authentication.

Rejected because it does not provide the identity and claims model established by the approved API Contract.

---

## Consequences

### Positive Consequences

- Stateless authentication.
- Well-established industry practice.
- Suitable for REST APIs.
- Supports secure authorization workflows.
- Consistent with the approved API Contract.

### Negative Consequences

- Token lifecycle management requires appropriate implementation.
- Authentication depends on correct token validation.

### Trade-offs

JWT Bearer authentication provides a balance between security, interoperability, and implementation simplicity while remaining consistent with the approved specifications.

---

## Specification Impact

This ADR records the architectural implementation of the authentication approach already established by the approved API Contract.

It introduces no new authentication behavior or security requirements.

---

## Related Documents

- 04_APIContract.md
- 05_Architecture.md (Security Architecture)
- Technical Assessment Specification

---

# ADR-010 — Use FluentValidation for Request Validation

## Status

**Accepted**

---

## Context

The approved Architecture separates application validation responsibilities from controllers and domain entities.

A validation mechanism is required to verify application requests before business logic is executed while maintaining a clean separation of concerns.

---

## Decision

The solution **uses FluentValidation** to validate application requests within the Application Layer.

Validation rules are implemented independently of controllers and are executed before the corresponding application handler processes the request.

This implementation supports the validation strategy described by the approved Architecture without altering the business validation rules defined by the approved specifications.

---

## Alternatives Considered

### Controller-Based Validation

Validation logic implemented directly within controllers.

Rejected because it couples validation to the Presentation Layer and reduces reuse.

---

### Manual Validation Inside Handlers

Validation performed explicitly within each handler.

Rejected because it duplicates validation logic and reduces consistency across requests.

---

### Custom Validation Framework

A proprietary validation mechanism.

Rejected because FluentValidation provides mature functionality that satisfies the project's implementation requirements.

---

## Consequences

### Positive Consequences

- Separation of validation concerns.
- Consistent request validation.
- Improved maintainability.
- Reduced duplication.
- Better testability.

### Negative Consequences

- Additional framework dependency.
- Developers require familiarity with FluentValidation.

### Trade-offs

The additional dependency is accepted because it simplifies validation while preserving a clean application architecture.

---

## Specification Impact

This ADR records an accepted implementation decision.

Business validation rules continue to be defined exclusively by the approved specifications.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Validation Strategy)

---

# ADR-011 — Adopt Dependency Injection

## Status

**Accepted**

---

## Context

The approved Architecture emphasizes loose coupling between architectural layers and components.

A mechanism is required to manage object creation and dependency resolution while preserving the dependency direction established by Clean Architecture.

---

## Decision

The solution **adopts Dependency Injection (DI)** as the mechanism for resolving application dependencies.

Dependencies are provided through constructor injection, allowing components to depend on abstractions rather than concrete implementations.

The dependency injection container is configured within the application's composition root, keeping dependency management separate from business logic.

---

## Alternatives Considered

### Manual Object Construction

Objects are instantiated directly throughout the application.

Rejected because it tightly couples components and complicates testing.

---

### Service Locator Pattern

Components resolve dependencies dynamically.

Rejected because it hides dependencies and reduces code transparency.

---

### Static Singleton Implementations

Shared services are exposed through static instances.

Rejected because they increase coupling and make testing more difficult.

---

## Consequences

### Positive Consequences

- Loose coupling.
- Improved testability.
- Clear dependency management.
- Better maintainability.
- Alignment with the Dependency Inversion Principle.

### Negative Consequences

- Initial configuration effort.
- Developers must understand dependency injection concepts.

### Trade-offs

The additional configuration required by Dependency Injection is accepted because it significantly improves maintainability and supports the architectural goals established for the project.

---

## Specification Impact

This ADR records an accepted implementation decision.

It does not introduce new functional behavior or modify any approved specification.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Dependency Injection)

---

# ADR-012 — Use Docker for Containerization

## Status

**Accepted**

---

## Context

The approved Architecture identifies containerization as the preferred mechanism for providing a consistent execution environment throughout development and deployment.

A standardized packaging approach reduces environmental differences and improves deployment reproducibility without affecting the behavior defined by the approved specifications.

---

## Decision

The solution **uses Docker** to package the application and its runtime dependencies into portable containers.

Containerization provides a consistent execution environment across supported deployment targets while remaining transparent to API consumers.

The use of Docker is an implementation decision specific to this project and is independent of the system's functional behavior.

---

## Alternatives Considered

### Native Host Deployment

Applications execute directly on the operating system.

Rejected because environmental differences can introduce inconsistencies across development, testing, and deployment environments.

---

### Virtual Machines

Applications execute inside full virtual machines.

Rejected because virtual machines introduce greater resource overhead than required for the current project.

---

### Alternative Container Technologies

Other container platforms provide similar capabilities.

Rejected because Docker was selected by the approved Technical Assessment Specification as the project's containerization technology.

---

## Consequences

### Positive Consequences

- Consistent runtime environment.
- Simplified deployment.
- Improved portability.
- Reproducible application packaging.
- Alignment with modern deployment practices.

### Negative Consequences

- Additional tooling required for development.
- Developers require familiarity with container concepts.

### Trade-offs

The additional tooling is accepted because containerization significantly improves environment consistency and deployment reliability.

---

## Specification Impact

This ADR records an accepted implementation decision.

It introduces no changes to the approved specifications or externally observable behavior.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Containerization Strategy)

---

# ADR-013 — Use Docker Compose for Local Orchestration

## Status

**Accepted**

---

## Context

The solution consists of multiple components that must execute together during local development.

Coordinating these components manually increases configuration effort and reduces consistency between development environments.

---

## Decision

The solution **uses Docker Compose** to orchestrate the application's local development environment.

Docker Compose coordinates the execution of the application and its supporting services through a declarative configuration.

This orchestration approach is intended solely for development and local integration scenarios and does not prescribe a production deployment strategy.

---

## Alternatives Considered

### Manual Service Startup

Each component is started independently.

Rejected because manual coordination is repetitive and prone to configuration inconsistencies.

---

### Custom Startup Scripts

Local environments are initialized through operating system scripts.

Rejected because scripts are typically platform-dependent and more difficult to maintain.

---

### Container Orchestration Platforms

Platforms such as Kubernetes provide advanced orchestration capabilities.

Rejected because the current project scope does not require production-grade orchestration for local development.

---

## Consequences

### Positive Consequences

- Simplified local environment setup.
- Consistent developer experience.
- Reduced manual configuration.
- Improved reproducibility.

### Negative Consequences

- Additional Docker Compose configuration.
- Developers require familiarity with Docker Compose.

### Trade-offs

Docker Compose provides sufficient orchestration capabilities for the project's current scope while maintaining implementation simplicity.

---

## Specification Impact

This ADR records an accepted implementation decision.

It does not modify the approved specifications or define deployment requirements beyond the approved Architecture.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Containerization Strategy)

---

# ADR-014 — Use Swagger / OpenAPI for API Documentation

## Status

**Accepted**

---

## Context

The approved Architecture identifies API documentation as an important capability for development, testing, and integration.

A standardized documentation mechanism improves discoverability of API endpoints while remaining derived entirely from the approved API Contract.

---

## Decision

The solution **uses Swagger/OpenAPI** to generate interactive API documentation.

The generated documentation reflects the implementation of the approved API Contract and serves as a development and integration aid.

Swagger documentation is derived from the implemented API and does not constitute the authoritative specification of system behavior.

---

## Alternatives Considered

### Manually Maintained Documentation

API documentation maintained independently.

Rejected because manually maintained documentation is more susceptible to becoming inconsistent with the implementation.

---

### No Interactive Documentation

API consumers rely exclusively on external documentation.

Rejected because interactive documentation improves developer productivity and simplifies API exploration.

---

### Alternative Documentation Frameworks

Other documentation generators provide similar functionality.

Rejected because Swagger/OpenAPI is the technology identified in the approved technical baseline.

---

## Consequences

### Positive Consequences

- Interactive API exploration.
- Improved developer experience.
- Reduced documentation maintenance effort.
- Better support for testing and integration.

### Negative Consequences

- Additional package dependency.
- Documentation quality depends upon implementation accuracy.

### Trade-offs

Swagger/OpenAPI improves API usability while remaining consistent with the approved API Contract.

---

## Specification Impact

This ADR records an accepted implementation decision.

The approved **04_APIContract.md** remains the authoritative specification of the public API.

Swagger documentation represents the implemented API and must remain consistent with the approved API Contract.

---

## Related Documents

- 04_APIContract.md
- Technical Assessment Specification
- 05_Architecture.md (API Documentation)

---

# ADR-015 — Enforce Layer Dependency Rules Through Architecture Tests

## Status

**Accepted**

---

## Context

The approved Architecture establishes explicit dependency rules between architectural layers.

As the solution evolves, there is a risk that implementation changes could unintentionally violate these dependency rules.

An automated verification mechanism improves architectural consistency throughout the project's lifecycle.

---

## Decision

The solution **uses architecture tests** to verify compliance with the dependency rules defined by the approved Architecture.

These tests validate architectural constraints such as permitted project references and dependency direction.

Architecture tests verify implementation structure only and do not replace functional or integration testing.

---

## Alternatives Considered

### Manual Code Reviews

Dependency rules are verified during peer review.

Rejected because manual verification is subject to human error and may not consistently identify architectural violations.

---

### Documentation-Only Governance

Developers rely solely on architectural documentation.

Rejected because documentation alone cannot automatically detect implementation drift.

---

### No Automated Architectural Verification

Architectural compliance depends exclusively on developer discipline.

Rejected because long-term architectural consistency benefits from automated verification.

---

## Consequences

### Positive Consequences

- Continuous architectural validation.
- Early detection of dependency violations.
- Improved maintainability.
- Better preservation of the approved Architecture.

### Negative Consequences

- Additional test maintenance.
- Increased build execution time.

### Trade-offs

The modest increase in testing effort is accepted because it helps preserve architectural integrity throughout the evolution of the solution.

---

## Specification Impact

This ADR records an accepted implementation decision.

It introduces no functional behavior and does not modify any approved specification.

---

## Related Documents

- Technical Assessment Specification
- 05_Architecture.md (Testing Strategy, Architectural Constraints)

---

# 5. Decision Traceability Matrix

| ADR | Architectural Decision | Primary Reference |
|------|-------------------------|-------------------|
| ADR-001 | Adopt Clean Architecture | 05_Architecture.md – Architectural Principles |
| ADR-002 | Organize the Solution into Independent Projects | 05_Architecture.md – Solution Structure |
| ADR-003 | Adopt CQRS | 05_Architecture.md – Application Layer Design |
| ADR-004 | Use MediatR for Request Dispatching | 05_Architecture.md – Request Processing Flow |
| ADR-005 | Separate Read and Write Persistence Technologies | 05_Architecture.md – Persistence Strategy |
| ADR-006 | Use Entity Framework for Read Operations | 05_Architecture.md – Persistence Strategy |
| ADR-007 | Use Dapper for Write Operations | 05_Architecture.md – Persistence Strategy |
| ADR-008 | Use SQL Server as the Relational Database | Technical Assessment Specification / 05_Architecture.md |
| ADR-009 | Adopt JWT Bearer Authentication | 04_APIContract.md / 05_Architecture.md |
| ADR-010 | Use FluentValidation for Request Validation | 05_Architecture.md – Validation Strategy |
| ADR-011 | Adopt Dependency Injection | 05_Architecture.md – Dependency Injection |
| ADR-012 | Use Docker for Containerization | 05_Architecture.md – Containerization Strategy |
| ADR-013 | Use Docker Compose for Local Orchestration | 05_Architecture.md – Containerization Strategy |
| ADR-014 | Use Swagger / OpenAPI for API Documentation | 04_APIContract.md / 05_Architecture.md |
| ADR-015 | Enforce Layer Dependency Rules Through Architecture Tests | 05_Architecture.md – Testing Strategy |

---

# 6. ADR Governance

This document records the accepted architectural decisions that support the implementation of the approved specification baseline.

The following governance principles apply:

1. Architecture Decision Records document **accepted architectural decisions** and their rationale.

2. Architecture Decision Records do not introduce functional requirements, business rules, domain concepts, API resources, or externally observable behavior.

3. The approved specification baseline remains authoritative:

   - 01_ProjectVision.md
   - 02_FunctionalSpecification.md
   - 03_DomainModel.md
   - 04_APIContract.md

4. The approved Architecture describes how the specification baseline is realized.

5. This document records why architectural approaches were selected; it does not redefine or extend the approved Architecture.

6. Future architectural decisions should be documented through additional Architecture Decision Records rather than by modifying existing approved ADRs, unless an approved governance process explicitly requires revision.

7. Any future architectural decision that changes externally observable behavior must first be reflected in the appropriate specification artifact before being documented as an architectural decision.

8. Architectural decisions should maintain complete traceability to the approved specification baseline and the approved Architecture.

---

**End of Document**