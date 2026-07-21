# Functional Specification

**Project:** Inventory Management REST API

**Version:** 1.2

**Status:** Approved

**Based on:** Approved Project Vision v0.2

---

# Revision History

| Version | Date | Description |
|----------|------------|---------------------------------------------------------------|
| 1.0 | 2026-07-20 | Initial Functional Specification. |
| 1.1 | 2026-07-20 | Improved traceability, glossary, priorities, business rule mapping, document organization and error specification after architecture review. |
| 1.2 | 2026-07-20 | Approved version. Minor editorial refinements applied during the final architecture review. |

---

# 1. Purpose

This Functional Specification defines the business behavior of the Inventory Management REST API and serves as the implementation contract for the development phase.

Its purpose is to translate the approved Project Vision into a complete, traceable and implementation-independent description of the functional capabilities expected from the system.

This document intentionally focuses on **what** the system shall accomplish from a business perspective rather than **how** it will be implemented.

The specification establishes:

- Functional capabilities.
- Business rules governing system behavior.
- User interactions through use cases.
- Validation requirements.
- Acceptance criteria.
- Functional traceability.

Technical implementation decisions remain the responsibility of the Technical Specification and Architecture documents.

---

# 2. Scope

Version 1 of the Inventory Management REST API includes the following business capabilities:

- Authentication.
- Category management.
- Product management.
- Inventory movement registration.
- Inventory information retrieval.
- Inventory movement history retrieval.

The following capabilities are explicitly outside the scope of Version 1:

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

No functionality outside the approved Project Vision shall be considered part of this specification.

> **Architectural Note**
>
> OpenAPI documentation, Dockerized deployment, SQL Server persistence, API versioning and other technical concerns are intentionally documented within the Architecture and Non-Functional Specifications rather than the Functional Scope because they do not represent business capabilities.

---

# 3. Definitions

The following domain terms are used throughout this specification.

| Term | Definition |
|------|------------|
| **Category** | A logical grouping used to organize products. A category may contain zero or more products. |
| **Product** | An inventory item managed by the system. Every product belongs to exactly one category and is uniquely identified by its SKU. |
| **Inventory Movement** | A business transaction that modifies the available stock of a product. Every movement is classified as either an Entry or an Exit and represents the authoritative inventory history. |
| **CurrentStock** | The current available quantity of a product after applying every successful inventory movement. It is maintained as a denormalized value to optimize inventory queries. |
| **Entry** | An inventory movement that increases the available stock of a product. |
| **Exit** | An inventory movement that decreases the available stock of a product. An Exit shall never produce negative inventory. |
| **JWT Bearer Token** | Security token issued after successful authentication. The token grants access to protected API resources until it expires. |

These definitions are derived from the approved Project Vision and are intended to ensure consistent terminology throughout the project.

---

# 4. Actors

## 4.1 Authenticated User

### Description

An Authenticated User is any user successfully authenticated through the application's OAuth2 authentication mechanism using JWT Bearer tokens.

### Responsibilities

The Authenticated User is responsible for:

- Managing categories.
- Managing products.
- Registering inventory entries.
- Registering inventory exits.
- Retrieving inventory information.
- Retrieving inventory movement history.

### Permissions

The Authenticated User may access every functional capability defined within the scope of Version 1.

No additional application roles, permissions or authorization levels are defined by this specification.

---

# 5. Business Rules

| ID | Business Rule |
|----|---------------|
| **BR-001** | Only authenticated users may access protected API functionality. |
| **BR-002** | Every product shall belong to exactly one category. |
| **BR-003** | Category names shall be unique. |
| **BR-004** | Product SKUs shall be unique. |
| **BR-005** | Categories may exist without products. |
| **BR-006** | Categories associated with one or more products cannot be deleted. |
| **BR-007** | Products with inventory movement history cannot be deleted. |
| **BR-008** | Every inventory movement shall be classified as either Entry or Exit. |
| **BR-009** | Inventory shall never become negative. |
| **BR-010** | Every successful inventory movement shall update the product's CurrentStock. |
| **BR-011** | InventoryMovement represents the authoritative history of inventory changes. |
| **BR-012** | CurrentStock shall be maintained as a denormalized value to optimize inventory retrieval. |

The above business rules are normative and apply across all functional requirements and use cases unless explicitly stated otherwise.

---

# 6. Functional Requirements

---

## FR-001 Authentication

### Business Capability

The system shall authenticate users before granting access to protected business capabilities, ensuring that only authorized users can interact with inventory management functionality.

### Supported Operations

- Authenticate using valid credentials.
- Issue a JWT Bearer access token.
- Validate authentication before executing protected operations.

### Priority

**High**

### Traceability

**Implements**

- UC-01 Authenticate User

**Business Rules**

- BR-001

---

## FR-002 Category Management

### Business Capability

The system shall enable authenticated users to organize inventory by maintaining a catalog of product categories that serves as the organizational structure for products.

The system shall preserve the integrity of category information while enforcing the business constraints defined for category lifecycle management.

### Supported Operations

- Create category.
- Retrieve category.
- Retrieve category collection.
- Update category.
- Delete category.

### Priority

**High**

### Traceability

**Implements**

- UC-02 Create Category
- UC-03 Update Category
- UC-04 Delete Category

**Business Rules**

- BR-001
- BR-003
- BR-005
- BR-006

## FR-003 Product Management

### Business Capability

The system shall enable authenticated users to maintain the inventory catalog by managing products that belong to categories and uniquely represent inventory items.

The system shall preserve product integrity throughout its lifecycle while enforcing uniqueness and category association rules.

### Supported Operations

- Create product.
- Retrieve product.
- Retrieve product collection.
- Update product.
- Delete product.

### Priority

**High**

### Traceability

**Implements**

- UC-05 Create Product
- UC-06 Update Product
- UC-07 Delete Product

**Business Rules**

- BR-001
- BR-002
- BR-004
- BR-007

---

## FR-004 Inventory Entry Registration

### Business Capability

The system shall allow authenticated users to increase the available inventory of existing products by registering inventory entry movements.

Each successful inventory entry shall become part of the permanent inventory history and update the product's CurrentStock.

### Supported Operations

- Register inventory entry.
- Persist inventory movement.
- Update CurrentStock.

### Priority

**High**

### Traceability

**Implements**

- UC-08 Register Inventory Entry

**Business Rules**

- BR-001
- BR-008
- BR-010
- BR-011
- BR-012

---

## FR-005 Inventory Exit Registration

### Business Capability

The system shall allow authenticated users to decrease the available inventory of existing products by registering inventory exit movements while ensuring that inventory levels never become negative.

Each successful inventory exit shall become part of the permanent inventory history and update the product's CurrentStock.

### Supported Operations

- Register inventory exit.
- Validate available stock.
- Persist inventory movement.
- Update CurrentStock.

### Priority

**High**

### Traceability

**Implements**

- UC-09 Register Inventory Exit

**Business Rules**

- BR-001
- BR-008
- BR-009
- BR-010
- BR-011
- BR-012

---

## FR-006 Inventory Information Retrieval

### Business Capability

The system shall provide authenticated users with access to the current inventory status of products by exposing the latest available stock maintained by the system.

The information returned shall represent the current business state of inventory after all successful inventory movements have been applied.

### Supported Operations

- Retrieve current inventory information.
- Retrieve current stock for products.

### Priority

**Medium**

### Traceability

**Implements**

- UC-10 Retrieve Inventory

**Business Rules**

- BR-001
- BR-012

---

## FR-007 Inventory Movement History Retrieval

### Business Capability

The system shall provide authenticated users with access to the complete history of inventory movements in order to allow inspection of inventory changes over time.

The movement history shall represent the authoritative record of all successful inventory entries and exits.

### Supported Operations

- Retrieve inventory movement history.
- Retrieve movement history for products.

### Priority

**Medium**

### Traceability

**Implements**

- UC-11 Retrieve Inventory Movement History

**Business Rules**

- BR-001
- BR-011

---

## Functional Requirement Traceability Matrix

| Functional Requirement | Related Use Cases | Related Business Rules |
|-------------------------|------------------|------------------------|
| **FR-001 Authentication** | UC-01 Authenticate User | BR-001 |
| **FR-002 Category Management** | UC-02, UC-03, UC-04 | BR-001, BR-003, BR-005, BR-006 |
| **FR-003 Product Management** | UC-05, UC-06, UC-07 | BR-001, BR-002, BR-004, BR-007 |
| **FR-004 Inventory Entry Registration** | UC-08 | BR-001, BR-008, BR-010, BR-011, BR-012 |
| **FR-005 Inventory Exit Registration** | UC-09 | BR-001, BR-008, BR-009, BR-010, BR-011, BR-012 |
| **FR-006 Inventory Information Retrieval** | UC-10 | BR-001, BR-012 |
| **FR-007 Inventory Movement History Retrieval** | UC-11 | BR-001, BR-011 |

---

# 7. Use Cases

---

## UC-01 Authenticate User

### Goal

Authenticate a user and grant access to the protected API.

### Primary Actor

Authenticated User

### Preconditions

- A predefined administrator account exists.
- The user provides valid authentication credentials.

### Main Flow

1. The user submits authentication credentials.
2. The system validates the supplied credentials.
3. The system generates a JWT Bearer access token.
4. The system returns the generated token.
5. The user becomes authorized to invoke protected endpoints.

### Alternate Flows

#### UC-01-AF-01 Invalid Credentials

1. Credentials cannot be validated.
2. Authentication is rejected.

### Postconditions

- A valid JWT Bearer Token has been issued.
- The authenticated session may access protected resources until the token expires.

### Business Rules Applied

- BR-001

---

## UC-02 Create Category

### Goal

Create a new product category.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- The category name does not already exist.

### Main Flow

1. User submits category information.
2. System validates the submitted information.
3. System verifies category name uniqueness.
4. System creates the category.
5. System confirms successful creation.

### Alternate Flows

#### UC-02-AF-01 Duplicate Category Name

- The submitted category name already exists.
- The request is rejected.

### Postconditions

- A new category is available for product assignment.

### Business Rules Applied

- BR-001
- BR-003
- BR-005

## UC-03 Update Category

### Goal

Modify the information of an existing category.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- Category exists.

### Main Flow

1. User requests category modification.
2. System validates submitted information.
3. System verifies category name uniqueness.
4. System updates the category.
5. Updated information is persisted.

### Alternate Flows

#### UC-03-AF-01 Category Not Found

- The requested category does not exist.

#### UC-03-AF-02 Duplicate Category Name

- The updated category name already belongs to another category.

### Postconditions

- Category reflects the submitted changes.

### Business Rules Applied

- BR-001
- BR-003

---

## UC-04 Delete Category

### Goal

Remove a category that is no longer required.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- Category exists.
- Category has no associated products.

### Main Flow

1. User requests category deletion.
2. System verifies business constraints.
3. System deletes the category.
4. Deletion is confirmed.

### Alternate Flows

#### UC-04-AF-01 Category Contains Products

- Deletion is rejected because the category is associated with one or more products.

#### UC-04-AF-02 Category Not Found

- The requested category does not exist.

### Postconditions

- Category is permanently removed.

### Business Rules Applied

- BR-001
- BR-005
- BR-006

---

## UC-05 Create Product

### Goal

Create a new inventory product.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- The referenced category exists.
- Product SKU is unique.

### Main Flow

1. User submits product information.
2. System validates submitted information.
3. System validates category existence.
4. System validates SKU uniqueness.
5. System creates the product.
6. Product becomes available for inventory operations.

### Alternate Flows

#### UC-05-AF-01 Category Does Not Exist

- Product creation is rejected.

#### UC-05-AF-02 Duplicate SKU

- Product creation is rejected.

### Postconditions

- Product exists within the inventory catalog.

### Business Rules Applied

- BR-001
- BR-002
- BR-004

---

## UC-06 Update Product

### Goal

Modify an existing product.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- Product exists.

### Main Flow

1. User requests product modification.
2. System validates submitted information.
3. System verifies SKU uniqueness.
4. System updates the product.
5. Changes are persisted.

### Alternate Flows

#### UC-06-AF-01 Product Not Found

- The requested product does not exist.

#### UC-06-AF-02 Duplicate SKU

- The updated SKU already belongs to another product.

### Postconditions

- Product reflects the updated information.

### Business Rules Applied

- BR-001
- BR-002
- BR-004

---

## UC-07 Delete Product

### Goal

Delete a product that has never participated in inventory operations.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- Product exists.
- Product has no inventory movement history.

### Main Flow

1. User requests product deletion.
2. System verifies inventory movement history.
3. System deletes the product.
4. Deletion is confirmed.

### Alternate Flows

#### UC-07-AF-01 Product Has Inventory Movement History

- Deletion is rejected because the product has associated inventory movements.

#### UC-07-AF-02 Product Not Found

- The requested product does not exist.

### Postconditions

- Product is permanently removed.

### Business Rules Applied

- BR-001
- BR-007
- BR-011

---

## UC-08 Register Inventory Entry

### Goal

Increase the available stock of a product.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- Product exists.
- Submitted quantity is greater than zero.

### Main Flow

1. User submits an inventory entry.
2. System validates the movement.
3. System records the inventory movement.
4. System updates the product's CurrentStock.
5. Operation is confirmed.

### Alternate Flows

#### UC-08-AF-01 Product Not Found

- Operation is rejected because the requested product does not exist.

#### UC-08-AF-02 Invalid Quantity

- Operation is rejected because the submitted quantity is not greater than zero.

### Postconditions

- Inventory history includes the newly registered movement.
- Product CurrentStock has increased accordingly.

### Business Rules Applied

- BR-001
- BR-008
- BR-010
- BR-011
- BR-012

## UC-09 Register Inventory Exit

### Goal

Decrease the available stock of a product.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.
- Product exists.
- Available stock is sufficient.
- Submitted quantity is greater than zero.

### Main Flow

1. User submits an inventory exit.
2. System validates the movement.
3. System verifies that sufficient inventory is available.
4. System records the inventory movement.
5. System updates the product's CurrentStock.
6. Operation is confirmed.

### Alternate Flows

#### UC-09-AF-01 Insufficient Stock

- The requested inventory exit would produce negative inventory.
- The operation is rejected.

#### UC-09-AF-02 Product Not Found

- The requested product does not exist.
- The operation is rejected.

#### UC-09-AF-03 Invalid Quantity

- The submitted quantity is not greater than zero.
- The operation is rejected.

### Postconditions

- Inventory history includes the newly registered movement.
- Product CurrentStock has decreased accordingly.

### Business Rules Applied

- BR-001
- BR-008
- BR-009
- BR-010
- BR-011
- BR-012

---

## UC-10 Retrieve Inventory

### Goal

Retrieve the current inventory status of products.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.

### Main Flow

1. User requests inventory information.
2. System retrieves the latest CurrentStock values.
3. System returns the requested inventory information.

### Postconditions

- Current inventory information has been returned.

### Business Rules Applied

- BR-001
- BR-012

---

## UC-11 Retrieve Inventory Movement History

### Goal

Retrieve the complete inventory movement history.

### Primary Actor

Authenticated User

### Preconditions

- User is authenticated.

### Main Flow

1. User requests inventory movement history.
2. System retrieves the inventory movement records.
3. System returns the movement history.

### Postconditions

- Inventory movement history has been returned.

### Business Rules Applied

- BR-001
- BR-011

---

# 8. Validation Rules

Validation Rules define business validation requirements independently of transport-level validation (such as HTTP model binding or request serialization). They describe the conditions that must be satisfied before a business operation can be successfully executed.

---

## 8.1 Authentication

| ID | Validation Rule |
|----|-----------------|
| **VR-001** | Authentication credentials shall be provided. |
| **VR-002** | Credentials shall be valid before issuing a JWT Bearer Token. |
| **VR-003** | A valid JWT Bearer Token shall be presented when accessing protected resources. |

---

## 8.2 Categories

| ID | Validation Rule |
|----|-----------------|
| **VR-004** | Category name is required. |
| **VR-005** | Category name shall be unique. |

---

## 8.3 Products

| ID | Validation Rule |
|----|-----------------|
| **VR-006** | Product SKU is required. |
| **VR-007** | Product SKU shall be unique. |
| **VR-008** | Every product shall reference an existing category. |

---

## 8.4 Inventory Movements

| ID | Validation Rule |
|----|-----------------|
| **VR-009** | Product shall exist. |
| **VR-010** | Quantity shall be greater than zero. |
| **VR-011** | Movement type shall be either Entry or Exit. |
| **VR-012** | Inventory exits shall never produce negative inventory. |

---

# 9. Error Scenarios

Unless otherwise stated, all error responses shall conform to **RFC 7807 Problem Details**, as established by the approved Project Vision.

| Scenario | HTTP Status | Response |
|----------|-------------|----------|
| Missing authentication | **401 Unauthorized** | RFC 7807 Problem Details |
| Invalid authentication credentials | **401 Unauthorized** | RFC 7807 Problem Details |
| Invalid or expired JWT Bearer Token | **401 Unauthorized** | RFC 7807 Problem Details |
| Requested category not found | **404 Not Found** | RFC 7807 Problem Details |
| Requested product not found | **404 Not Found** | RFC 7807 Problem Details |
| Duplicate category name | **409 Conflict** | RFC 7807 Problem Details |
| Duplicate product SKU | **409 Conflict** | RFC 7807 Problem Details |
| Category contains associated products | **409 Conflict** | RFC 7807 Problem Details |
| Product has inventory movement history | **409 Conflict** | RFC 7807 Problem Details |
| Inventory exit would produce negative inventory | **409 Conflict** | RFC 7807 Problem Details |
| Invalid inventory movement quantity | **400 Bad Request** | RFC 7807 Problem Details |
| Invalid request payload | **400 Bad Request** | RFC 7807 Problem Details |
| Optimistic concurrency conflict | **409 Conflict** | RFC 7807 Problem Details |

# 10. Acceptance Criteria

## AC-001 Authentication

**Given** valid authentication credentials

**When** the user requests authentication

**Then** the system issues a valid JWT Bearer Token.

---

## AC-002 Authentication Failure

**Given** invalid authentication credentials

**When** authentication is requested

**Then** the request is rejected with **401 Unauthorized**.

---

## AC-003 Category Creation

**Given** an authenticated user

**When** a unique category is submitted

**Then** the category is successfully created.

---

## AC-004 Duplicate Category

**Given** an existing category name

**When** another category with the same name is submitted

**Then** the request is rejected with **409 Conflict**.

---

## AC-005 Product Creation

**Given** an existing category

**When** a valid product is submitted

**Then** the product is successfully created.

---

## AC-006 Duplicate SKU

**Given** an existing Product SKU

**When** another product is submitted using the same SKU

**Then** the request is rejected with **409 Conflict**.

---

## AC-007 Inventory Entry

**Given** an existing product

**When** a valid inventory entry is registered

**Then** the product's CurrentStock increases accordingly and the movement is recorded in the inventory history.

---

## AC-008 Inventory Exit

**Given** sufficient available inventory

**When** a valid inventory exit is registered

**Then** the product's CurrentStock decreases accordingly and the movement is recorded in the inventory history.

---

## AC-009 Negative Inventory Prevention

**Given** insufficient available inventory

**When** an inventory exit is requested

**Then** the operation is rejected with **409 Conflict** and CurrentStock remains unchanged.

---

## AC-010 Delete Category

**Given** a category associated with one or more products

**When** deletion is requested

**Then** the operation is rejected with **409 Conflict**.

---

## AC-011 Delete Product

**Given** a product with inventory movement history

**When** deletion is requested

**Then** the operation is rejected with **409 Conflict**.

---

## AC-012 Retrieve Inventory

**Given** an authenticated user

**When** inventory information is requested

**Then** the latest CurrentStock information is returned.

---

## AC-013 Retrieve Inventory Movement History

**Given** an authenticated user

**When** inventory movement history is requested

**Then** the complete inventory movement history is returned.

---

# 11. Assumptions

The following assumptions originate from the approved Project Vision and are considered normative for Version 1.

| ID | Assumption |
|----|------------|
| **A-001** | Authentication is performed using OAuth2 with JWT Bearer Tokens issued by the application. |
| **A-002** | A predefined administrator account exists for authentication purposes. |
| **A-003** | Every product belongs to exactly one category. |
| **A-004** | Category names are unique. |
| **A-005** | Product SKUs are unique. |
| **A-006** | Categories with associated products cannot be deleted. |
| **A-007** | Products with inventory movement history cannot be deleted. |
| **A-008** | Inventory movements are represented by a single entity classified as Entry or Exit. |
| **A-009** | CurrentStock is maintained as a denormalized value to optimize inventory queries. |
| **A-010** | InventoryMovement represents the authoritative inventory history. |

These assumptions are inherited directly from the approved Project Vision and therefore do not constitute additional functional requirements.

---

# 12. Non-Functional Requirements Affecting Functionality

Although implementation details are documented separately, the following non-functional requirements directly influence the observable behavior of the system.

| ID | Requirement |
|----|-------------|
| **NFR-001** | Protected endpoints shall require OAuth2 authentication using JWT Bearer Tokens. |
| **NFR-002** | API errors shall be returned using RFC 7807 Problem Details. |
| **NFR-003** | The API shall support versioning. |
| **NFR-004** | Optimistic concurrency control shall prevent conflicting inventory updates. |
| **NFR-005** | The API shall expose OpenAPI (Swagger) documentation describing the available endpoints. |
| **NFR-006** | The solution shall execute successfully through Docker Compose. |
| **NFR-007** | Data shall be persisted in SQL Server. |
| **NFR-008** | The solution shall include automated unit tests verifying functional behavior. |
| **NFR-009** | The solution architecture shall follow Clean Architecture principles and apply CQRS where appropriate while preserving the functional behavior defined in this specification. |

---

# 13. Open Questions

**None.**

The approved Project Vision v0.2 provides sufficient functional guidance to proceed with the Domain Model and Technical Specification without requiring additional business decisions.

All assumptions required by this specification have already been formalized within the approved Project Vision.

---

# Approval

**Document:** 02_FunctionalSpecification.md

**Version:** 1.2

**Status:** ✅ Approved

This document constitutes the approved functional implementation contract for Version 1 of the Inventory Management REST API.

Subsequent design and implementation artifacts—including the Domain Model, API Contract, Architecture, Technical Specification and implementation—shall remain consistent with this specification.

---

**End of Functional Specification v1.2**