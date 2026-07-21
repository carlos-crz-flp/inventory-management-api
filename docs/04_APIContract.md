# 04_APIContract.md

**Version:** 1.0  
**Status:** Approved

---

# Revision History

| Version | Date | Status | Description |
|---------|------|--------|-------------|
| 1.0 | YYYY-MM-DD | Approved | Initial approved version. |

---

# Based On

This document is based on the following approved specifications:

- **01_ProjectVision.md v0.2**
- **02_FunctionalSpecification.md v1.2**
- **03_DomainModel.md v1.0**

These documents constitute the authoritative specification baseline for this API Contract.

---

# Table of Contents

1. Purpose
2. Scope
3. Design Principles
4. API Overview
5. Authentication
6. Security Considerations
7. Common Conventions
8. Resource Model
9. Resource Relationships
10. Resource Lifecycle
11. Endpoint Specifications
12. Common Data Types
13. Request Schemas
14. Response Schemas

---

# 1. Purpose

This document defines the public HTTP interface exposed by the Inventory Management System.

Its purpose is to specify the externally observable behavior of the API independently of any implementation technology, architectural style, persistence mechanism, or deployment model.

The API Contract establishes:

- The resources exposed by the system.
- Supported HTTP operations.
- Request and response representations.
- Resource relationships.
- Standard HTTP semantics.
- Error response format.
- Authentication requirements.
- Common conventions applicable to all endpoints.

This document serves as the authoritative contract between API providers and API consumers.

Unless revised through an approved specification process, the API Contract shall remain stable regardless of implementation changes.

---

# 2. Scope

This document specifies only the externally observable behavior of the REST API.

It defines:

- Public REST resources.
- Resource identifiers.
- HTTP methods.
- URI conventions.
- Request schemas.
- Response schemas.
- Standard HTTP status codes.
- Authentication requirements.
- Error response format.
- Resource lifecycle behavior.

This document does **not** define:

- Business requirements.
- Business rules.
- Domain Model design.
- Internal architecture.
- Application services.
- Persistence mechanisms.
- Database schema.
- Programming language.
- Frameworks.
- Infrastructure.
- Deployment strategy.

Those concerns are specified by other approved project documents.

---

# 3. Design Principles

The API Contract follows these principles.

## 3.1 Resource-Oriented Design

The API exposes business resources rather than implementation details.

Every URI identifies a resource or a resource collection.

---

## 3.2 Stateless Communication

Each request shall contain all information necessary for the server to process it.

The server shall not rely on conversational session state between requests.

---

## 3.3 Uniform Interface

Resources shall be manipulated using standard HTTP methods and semantics.

The meaning of each HTTP method shall remain consistent throughout the API.

---

## 3.4 Consistent Resource Representation

Each resource shall expose a consistent JSON representation regardless of the endpoint through which it is retrieved.

---

## 3.5 Predictable Behavior

Equivalent operations shall produce consistent observable results.

Clients shall not be required to understand internal implementation details.

---

## 3.6 Implementation Independence

This API Contract defines observable behavior only.

It intentionally avoids prescribing:

- Architectural patterns.
- Programming languages.
- Frameworks.
- Persistence technologies.
- Internal object models.
- Application workflows.

---

## 3.7 Backward Compatibility

Breaking changes shall only be introduced through a new API version.

Minor revisions shall preserve compatibility whenever reasonably possible.

---

# 4. API Overview

The Inventory Management System exposes REST resources that allow authenticated clients to manage Categories, Products, and Inventory Movements.

The API uses JSON for request and response payloads and follows standard HTTP semantics.

Inventory is represented as the observable Current Stock of a Product.

Current Stock cannot be modified directly.

Inventory changes occur exclusively through Inventory Movement operations.

---

## 4.1 Base URI

The API is exposed under the following versioned base path:

```text
/v1
```

The deployment host is environment-specific and outside the scope of this document.

---

## 4.2 Media Type

Unless otherwise specified, requests and responses shall use:

```http
Content-Type: application/json
Accept: application/json
```

---

## 4.3 Character Encoding

JSON payloads shall use UTF-8 encoding.

---

## 4.4 Resource Identification

Each resource is uniquely identified within its own resource type.

Resource identifiers are opaque values assigned by the API.

Clients shall not infer business meaning from resource identifiers.

---

# 5. Authentication

All endpoints, unless explicitly stated otherwise, require successful authentication.

Authentication is performed through the Authentication Resource, which issues a JSON Web Token (JWT).

Clients shall include the issued token in the Authorization header of every authenticated request.

```http
Authorization: Bearer <access-token>
```

Successful authentication establishes no server-side session state.

Authentication failures shall result in the appropriate HTTP status code as defined in this document.

---

# 6. Security Considerations

## 6.1 Transport Security

The API is intended to be accessed over HTTPS.

Requests received through unsecured transport are outside the scope of this specification.

---

## 6.2 Authentication

Authenticated endpoints require a valid Bearer token.

Requests without valid authentication credentials shall be rejected.

---

## 6.3 Authorization

Authentication does not imply authorization.

Authenticated users may only perform operations permitted by the system.

Authorization rules are defined by the approved Functional Specification.

---

## 6.4 Sensitive Information

The API shall not expose confidential information beyond what is explicitly defined by this contract.

Sensitive implementation details shall never appear in successful or error responses.

---

## 6.5 Input Validation

Every request shall be validated before processing.

Invalid requests shall produce standardized error responses as defined by this document.

---

# 7. Common Conventions

## 7.1 URI Naming

Resource names shall:

- Use plural nouns.
- Use lowercase characters.
- Use hyphen-separated words when necessary.
- Avoid verbs in resource paths.

Examples:

```text
/categories
/products
/products/{id}/movements
```

The following are intentionally avoided:

```text
/createProduct
/updateCategory
/deleteProduct
```

---

## 7.2 HTTP Methods

The API uses standard HTTP semantics.

| Method | Purpose |
|---------|---------|
| GET | Retrieve resources. |
| POST | Create resources or business records. |
| PUT | Replace the mutable representation of a resource. |
| DELETE | Remove a resource. |

PUT replaces the mutable state exposed by the API.

Read-only and system-managed properties remain unaffected.

DELETE provides idempotent observable behavior from the client's perspective.

---

## 7.3 HTTP Status Codes

The API uses standard HTTP status codes to communicate request outcomes.

Typical success codes include:

- 200 OK
- 201 Created
- 204 No Content

Typical client error codes include:

- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 409 Conflict

Unexpected server failures shall return:

- 500 Internal Server Error

---

## 7.4 Pagination

Collection resources may support pagination.

When supported, pagination behavior shall be explicitly documented by the corresponding endpoint.

Pagination metadata shall be returned using the standard PaginationMetadata representation defined in this document.

---

## 7.5 Sorting

Collection resources may support sorting.

Supported sort fields, if any, shall be documented by the corresponding endpoint.

---

## 7.6 Filtering

Collection resources may support filtering.

Supported filtering parameters, if any, shall be documented by the corresponding endpoint.

---

## 7.7 Null Handling

Consumers shall not distinguish between omitted optional properties and explicit null values unless otherwise documented by a specific resource.

---

## 7.8 Error Representation

Unless otherwise specified, non-success responses shall use the standardized Problem Details representation defined by RFC 7807.

Implementation-specific extension members may be included provided they remain compliant with RFC 7807.

Clients shall ignore unrecognized extension members unless otherwise documented.

# 8. Resource Model

This section defines the REST resources exposed by the API.

Resources are derived from the approved Domain Model and represent the externally observable business capabilities of the system.

This section specifies only the public HTTP interface and does not describe internal domain objects, persistence structures, or implementation details.

---

## 8.1 Authentication Resource

### Purpose

Provides the authentication interface used to obtain an access token.

### Description

The Authentication Resource accepts user credentials and returns an authentication token that enables access to protected resources.

It represents an interaction endpoint rather than a persistent business resource.

### Exposed Operations

- Authenticate a user.

---

## 8.2 Category Resource

### Purpose

Represents a business classification used to organize Products.

### Description

A Category is an independently managed resource.

Categories may be created, retrieved, updated, and deleted, subject to the business constraints defined by the approved Functional Specification.

### Exposed Operations

- Create Category
- Retrieve Categories
- Retrieve Category
- Update Category
- Delete Category

---

## 8.3 Product Resource

### Purpose

Represents an inventory item managed by the system.

### Description

A Product contains descriptive business information together with its observable Current Stock.

Current Stock is a read-only property exposed by the API.

Clients cannot modify Current Stock directly.

Inventory changes occur exclusively through Inventory Movement operations.

### Exposed Operations

- Create Product
- Retrieve Products
- Retrieve Product
- Update Product
- Delete Product

---

## 8.4 Inventory Movement Resource

### Purpose

Represents a recorded inventory operation affecting a Product.

### Description

Inventory Movements record changes to Product inventory through either Entry or Exit operations.

Each Inventory Movement is immutable once recorded.

Inventory Movements cannot be updated or deleted.

### Exposed Operations

- Register Inventory Entry
- Register Inventory Exit
- Retrieve Product Movement History

---

# 9. Resource Relationships

This section describes the navigational relationships exposed by the API.

These relationships define how resources are accessed through the REST interface.

They do not imply ownership, persistence structures, or aggregate boundaries.

---

## 9.1 Category → Product

A Category groups one or more Products.

Products reference the Category to which they belong.

Navigation is provided through Product representations.

The API does not expose nested Category resources.

---

## 9.2 Product → Inventory Movement

A Product is associated with zero or more Inventory Movements.

Inventory Movements represent the complete observable history of inventory changes for a Product.

Movement history is accessed through the Product resource.

---

## 9.3 Authentication → Protected Resources

Successful authentication allows clients to access protected API resources.

Authentication itself does not establish a persistent server-side session.

---

# 10. Resource Lifecycle

This section summarizes the lifecycle behavior of every exposed resource.

---

## 10.1 Lifecycle Summary

| Resource | POST | GET | PUT | DELETE | Specialized Operations |
|----------|------|-----|-----|--------|------------------------|
| Authentication | Authenticate | — | — | — | Issue Access Token |
| Category | Create | Retrieve | Update | Delete | — |
| Product | Create | Retrieve | Update | Delete | Retrieve Current Stock |
| Inventory Movement | Register | Retrieve History | — | — | Entry / Exit |

---

## 10.2 Authentication Lifecycle

Authentication requests produce an access token.

Authentication does not create a persistent business resource.

Authentication requests are independent of one another.

---

## 10.3 Category Lifecycle

Categories may be:

- Created.
- Retrieved.
- Updated.
- Deleted.

Deletion behavior is governed by the approved Functional Specification.

---

## 10.4 Product Lifecycle

Products may be:

- Created.
- Retrieved.
- Updated.
- Deleted.

Current Stock is not part of the mutable Product state.

Changes to inventory are performed exclusively through Inventory Movement operations.

---

## 10.5 Inventory Movement Lifecycle

Inventory Movements may be:

- Recorded.
- Retrieved as part of Product movement history.

Inventory Movements:

- are immutable;
- cannot be updated;
- cannot be deleted.

Each recorded movement contributes to the observable Current Stock of the associated Product.

---

## 10.6 Business Constraints

The following observable constraints apply to the exposed resources:

- Current Stock is a derived observable value.
- Current Stock cannot be modified directly.
- Inventory Movements are append-only records.
- Inventory Movements are immutable.
- Product inventory changes occur exclusively through Entry and Exit operations.
- Resource identifiers are immutable once assigned.

---

## 10.7 Resource Identifiers

Every resource is uniquely identified within its own resource type.

Resource identifiers:

- are assigned by the API;
- are immutable;
- have no business meaning;
- shall be treated as opaque values by clients.

Clients shall never generate resource identifiers.

Resource identifiers remain stable throughout the lifetime of the resource.

# 11. Endpoint Specifications

This section defines the public operations exposed by each API resource.

Every endpoint follows a uniform specification template consisting of:

- Purpose
- HTTP Method
- URI
- Authentication
- Request Headers
- Path Parameters
- Query Parameters
- Request Body
- Successful Responses
- Error Responses
- Observable Business Constraints
- Example Request
- Example Response

Unless otherwise specified, all request and response bodies use the JSON media type.

---

# 11.1 Authentication Resource

## Authenticate User

### Purpose

Authenticates a user and issues an access token for subsequent requests.

### HTTP Method

```http
POST
```

### URI

```text
/authentication
```

### Authentication

Not required.

### Request Headers

| Header | Required | Description |
|---------|----------|-------------|
| Content-Type: application/json | Yes | Request media type. |
| Accept: application/json | Yes | Expected response media type. |

### Path Parameters

None.

### Query Parameters

None.

### Request Body

Authentication Request

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Authentication successful. |

### Error Responses

| Status | Description |
|---------|-------------|
| 400 Bad Request | Invalid request format. |
| 401 Unauthorized | Invalid credentials. |
| 500 Internal Server Error | Unexpected server error. |

### Observable Business Constraints

- Successful authentication issues an access token.
- Successful authentication does not establish server-side session state.
- Clients shall include the issued token in subsequent authenticated requests.

### Example Request

```http
POST /v1/authentication HTTP/1.1
Content-Type: application/json
Accept: application/json

{
  "username": "administrator",
  "password": "********"
}
```

### Example Response

```http
HTTP/1.1 200 OK
Content-Type: application/json

{
  "accessToken": "<jwt>",
  "tokenType": "Bearer",
  "expiresIn": 3600
}
```

---

# 11.2 Category Resource

## Create Category

### Purpose

Creates a new Category.

### HTTP Method

```http
POST
```

### URI

```text
/categories
```

### Authentication

Required.

### Request Headers

| Header | Required | Description |
|---------|----------|-------------|
| Authorization | Yes | Bearer access token. |
| Content-Type | Yes | application/json |
| Accept | Yes | application/json |

### Path Parameters

None.

### Query Parameters

None.

### Request Body

Create Category Request

### Successful Responses

| Status | Description |
|---------|-------------|
| 201 Created | Category created successfully. |

The response shall include a `Location` header identifying the created resource.

### Error Responses

| Status | Description |
|---------|-------------|
| 400 Bad Request | Validation failed. |
| 401 Unauthorized | Authentication required. |
| 403 Forbidden | Operation not permitted. |
| 409 Conflict | Category already exists. |
| 500 Internal Server Error | Unexpected server error. |

### Observable Business Constraints

- Category names shall be unique.
- Resource identifiers are assigned by the API.
- Clients shall not provide resource identifiers.

### Example Request

```http
POST /v1/categories HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "categoryName": "Electronics"
}
```

### Example Response

```http
HTTP/1.1 201 Created
Location: /v1/categories/8c0f0cb7

{
  "id": "8c0f0cb7",
  "categoryName": "Electronics"
}
```

---

## Retrieve Categories

### Purpose

Retrieves the Category collection.

### HTTP Method

```http
GET
```

### URI

```text
/categories
```

### Authentication

Required.

### Request Headers

| Header | Required | Description |
|---------|----------|-------------|
| Authorization | Yes | Bearer access token. |

### Path Parameters

None.

### Query Parameters

Pagination, filtering, and sorting parameters are supported only when explicitly documented by this endpoint version.

### Request Body

None.

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Collection returned successfully. |

### Error Responses

| Status | Description |
|---------|-------------|
| 401 Unauthorized | Authentication required. |
| 500 Internal Server Error | Unexpected server error. |

### Observable Business Constraints

- The collection represents the observable Category resources.
- Pagination metadata is included only when pagination is supported.

### Example Request

```http
GET /v1/categories HTTP/1.1
Authorization: Bearer <token>
```

### Example Response

```http
HTTP/1.1 200 OK

{
  "items": [
    {
      "id": "8c0f0cb7",
      "categoryName": "Electronics"
    }
  ]
}
```

---

## Retrieve Category

### Purpose

Retrieves a single Category.

### HTTP Method

```http
GET
```

### URI

```text
/categories/{id}
```

### Authentication

Required.

### Request Headers

| Header | Required | Description |
|---------|----------|-------------|
| Authorization | Yes | Bearer access token. |

### Path Parameters

| Name | Description |
|------|-------------|
| id | Category identifier. |

### Query Parameters

None.

### Request Body

None.

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Category returned successfully. |

### Error Responses

| Status | Description |
|---------|-------------|
| 401 Unauthorized | Authentication required. |
| 404 Not Found | Category does not exist. |
| 500 Internal Server Error | Unexpected server error. |

### Observable Business Constraints

- Resource identifiers are immutable.
- Clients shall treat identifiers as opaque values.

### Example Request

```http
GET /v1/categories/8c0f0cb7 HTTP/1.1
Authorization: Bearer <token>
```

### Example Response

```http
HTTP/1.1 200 OK

{
  "id": "8c0f0cb7",
  "categoryName": "Electronics"
}
```

## 11.3 Product Resource

### Overview

The Product Resource exposes operations for managing Products and observing their current inventory state.

Current Stock is part of the Product representation and is maintained exclusively through Inventory Movement operations.

---

## Create Product

### Purpose

Creates a new Product.

### HTTP Method

```http
POST
```

### URI

```text
/products
```

### Request Body

Create Product Request

### Successful Responses

| Status | Description |
|---------|-------------|
| 201 Created | Product created successfully. |

The response shall include a `Location` header identifying the created resource.

### Error Responses

| Status | Description |
|---------|-------------|
| 404 Not Found | Referenced Category does not exist. |
| 409 Conflict | A Product with the same SKU already exists. |

### Observable Business Constraints

- SKU shall be unique.
- The referenced Category shall exist.
- Resource identifiers are assigned by the API.

### Example Request

```http
POST /v1/products

{
  "sku": "LAP-1001",
  "productName": "Wireless Mouse",
  "categoryId": "8c0f0cb7"
}
```

### Example Response

```http
HTTP/1.1 201 Created
Location: /v1/products/b51e8d24

{
  "id": "b51e8d24",
  "sku": "LAP-1001",
  "productName": "Wireless Mouse",
  "categoryId": "8c0f0cb7",
  "currentStock": 0
}
```

---

## Retrieve Products

### Purpose

Retrieves the Product collection.

### HTTP Method

GET

### URI

```text
/products
```

### Query Parameters

Pagination, filtering, and sorting are supported only when explicitly documented by this endpoint version.

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Product collection returned successfully. |

### Observable Business Constraints

- Current Stock is included in every Product representation.
- Pagination metadata is returned only when pagination is supported.

### Example Request

```http
GET /v1/products
```

### Example Response

```json
{
  "items": [
    {
      "id": "b51e8d24",
      "sku": "LAP-1001",
      "productName": "Wireless Mouse",
      "categoryId": "8c0f0cb7",
      "currentStock": 145
    }
  ]
}
```

---

## Retrieve Product

### Purpose

Retrieves a single Product.

### HTTP Method

GET

### URI

```text
/products/{id}
```

### Path Parameters

| Name | Description |
|------|-------------|
| id | Product identifier. |

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Product returned successfully. |
| 404 Not Found | Product does not exist. |

### Observable Business Constraints

- Current Stock is read-only.
- Clients cannot modify Current Stock directly.

### Example Request

```http
GET /v1/products/b51e8d24
```

### Example Response

```json
{
  "id": "b51e8d24",
  "sku": "LAP-1001",
  "productName": "Wireless Mouse",
  "categoryId": "8c0f0cb7",
  "currentStock": 145
}
```

## Update Product

### Purpose

Updates the mutable representation of an existing Product.

### HTTP Method

```http
PUT
```

### URI

```text
/products/{id}
```

### Path Parameters

| Name | Description |
|------|-------------|
| id | Product identifier. |

### Request Body

Update Product Request

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Product updated successfully. |

### Error Responses

| Status | Description |
|---------|-------------|
| 404 Not Found | Product does not exist. |

### Observable Business Constraints

- The referenced Category shall exist.
- SKU is immutable.
- Current Stock is read-only and shall not be modified by this operation.
- PUT replaces the mutable representation of the Product exposed by the API.

### Example Request

```http
PUT /v1/products/b51e8d24

{
  "productName": "Wireless Mouse Pro",
  "categoryId": "8c0f0cb7"
}
```

### Example Response

```json
{
  "id": "b51e8d24",
  "sku": "LAP-1001",
  "productName": "Wireless Mouse Pro",
  "categoryId": "8c0f0cb7",
  "currentStock": 145
}
```

---

## Delete Product

### Purpose

Deletes an existing Product.

### HTTP Method

```http
DELETE
```

### URI

```text
/products/{id}
```

### Path Parameters

| Name | Description |
|------|-------------|
| id | Product identifier. |

### Request Body

None.

### Successful Responses

| Status | Description |
|---------|-------------|
| 204 No Content | Product deleted successfully. |

### Error Responses

| Status | Description |
|---------|-------------|
| 404 Not Found | Product does not exist. |

### Observable Business Constraints

- DELETE provides idempotent observable behavior.
- Deletion behavior is governed by the approved Functional Specification.

### Example Request

```http
DELETE /v1/products/b51e8d24
```

### Example Response

```http
HTTP/1.1 204 No Content
```

---

## 11.4 Inventory Movement Resource

### Overview

The Inventory Movement Resource records inventory changes affecting a Product.

Inventory Movements are immutable and append-only.

They represent the complete observable history of inventory changes for a Product.

---

## Register Inventory Entry

### Purpose

Registers an inventory entry for a Product.

### HTTP Method

```http
POST
```

### URI

```text
/products/{id}/entries
```

### Path Parameters

| Name | Description |
|------|-------------|
| id | Product identifier. |

### Request Body

Inventory Entry Request

### Successful Responses

| Status | Description |
|---------|-------------|
| 201 Created | Inventory entry recorded successfully. |

The response shall include a `Location` header identifying the newly created Inventory Movement resource.

### Error Responses

| Status | Description |
|---------|-------------|
| 404 Not Found | Product does not exist. |

### Observable Business Constraints

- Quantity shall be greater than zero.
- Inventory Movements are immutable.
- Recording an Entry updates the observable Current Stock of the Product.

### Example Request

```http
POST /v1/products/b51e8d24/entries

{
  "quantity": 25
}
```

### Example Response

```http
HTTP/1.1 201 Created
Location: /v1/products/b51e8d24/movements

{
  "movementId": "f9249d0a",
  "productId": "b51e8d24",
  "movementType": "Entry",
  "quantity": 25,
  "timestamp": "2026-07-20T15:30:45Z"
}
```

---

## Register Inventory Exit

### Purpose

Registers an inventory exit for a Product.

### HTTP Method

```http
POST
```

### URI

```text
/products/{id}/exits
```

### Path Parameters

| Name | Description |
|------|-------------|
| id | Product identifier. |

### Request Body

Inventory Exit Request

### Successful Responses

| Status | Description |
|---------|-------------|
| 201 Created | Inventory exit recorded successfully. |

The response shall include a `Location` header identifying the newly created Inventory Movement resource.

### Error Responses

| Status | Description |
|---------|-------------|
| 404 Not Found | Product does not exist. |
| 409 Conflict | Inventory exit cannot be completed. |

### Observable Business Constraints

- Quantity shall be greater than zero.
- Inventory Movements are immutable.
- Recording an Exit updates the observable Current Stock of the Product.

### Example Request

```http
POST /v1/products/b51e8d24/exits

{
  "quantity": 5
}
```

### Example Response

```http
HTTP/1.1 201 Created
Location: /v1/products/b51e8d24/movements

{
  "movementId": "f9249d0a",
  "productId": "b51e8d24",
  "movementType": "Exit",
  "quantity": 5,
  "timestamp": "2026-07-20T15:35:12Z"
}
```

---

## Retrieve Product Movement History

### Purpose

Retrieves the complete inventory movement history of a Product.

### HTTP Method

```http
GET
```

### URI

```text
/products/{id}/movements
```

### Path Parameters

| Name | Description |
|------|-------------|
| id | Product identifier. |

### Query Parameters

Pagination, filtering, and sorting are supported only when explicitly documented by this endpoint version.

### Successful Responses

| Status | Description |
|---------|-------------|
| 200 OK | Inventory movement history returned successfully. |

### Error Responses

| Status | Description |
|---------|-------------|
| 404 Not Found | Product does not exist. |

### Observable Business Constraints

- Inventory Movements are returned in chronological order (oldest to newest), unless otherwise specified by a future API version.
- Movement history is append-only.
- Movement records are immutable.

### Example Request

```http
GET /v1/products/b51e8d24/movements
```

### Example Response

```json
{
  "items": [
    {
      "movementId": "f9249d0a",
      "productId": "b51e8d24",
      "movementType": "Entry",
      "quantity": 25,
      "timestamp": "2026-07-20T15:30:45Z"
    },
    {
      "movementId": "5bc13a92",
      "productId": "b51e8d24",
      "movementType": "Exit",
      "quantity": 5,
      "timestamp": "2026-07-20T15:35:12Z"
    }
  ]
}
```

# 12. Common Data Types

This section defines reusable data representations used throughout the API Contract.

These data types describe the external JSON contract only.

They promote consistency across request and response payloads without implying any internal implementation, domain object, persistence model, or architectural decision.

---

## 12.1 Identifier

### Purpose

Represents the unique identifier of a REST resource.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| id | String | Yes | Unique identifier assigned by the API. |

### Validation Rules

- Assigned by the API.
- Unique within its resource type.
- Immutable.
- Clients shall treat identifiers as opaque values.
- Clients shall neither generate nor infer business meaning from identifiers.

### Example

```json
{
  "id": "8c0f0cb7"
}
```

---

## 12.2 SKU

### Purpose

Represents the business identifier assigned to a Product.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| sku | String | Yes | Business identifier of the Product. |

### Validation Rules

- Required during Product creation.
- Unique among Products.
- Immutable after creation.

### Example

```json
{
  "sku": "LAP-1001"
}
```

---

## 12.3 CategoryName

### Purpose

Represents the display name of a Category.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| categoryName | String | Yes | Human-readable Category name. |

### Validation Rules

- Required.
- Shall not be empty.
- Unique among Categories.

### Example

```json
{
  "categoryName": "Electronics"
}
```

---

## 12.4 ProductName

### Purpose

Represents the display name of a Product.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| productName | String | Yes | Human-readable Product name. |

### Validation Rules

- Required.
- Shall not be empty.

### Example

```json
{
  "productName": "Wireless Mouse"
}
```

---

## 12.5 Quantity

### Purpose

Represents the positive magnitude associated with an Inventory Movement.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| quantity | Number | Yes | Positive quantity associated with the operation. |

### Validation Rules

- Greater than zero.
- Always represents a positive magnitude.
- Whether the quantity increases or decreases Current Stock depends on the invoked endpoint.

### Example

```json
{
  "quantity": 25
}
```

---

## 12.6 CurrentStock

### Purpose

Represents the observable inventory currently available for a Product.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| currentStock | Number | Yes | Current observable inventory available for the Product. |

### Validation Rules

- Read-only.
- Returned only as part of Product representations.
- Never supplied by clients.
- Cannot be modified directly through the API.

### Example

```json
{
  "currentStock": 145
}
```

---

## 12.7 Timestamp

### Purpose

Represents a date and time exchanged through the API.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| timestamp | String | Yes | ISO 8601 timestamp including timezone information. |

### Validation Rules

- ISO 8601 compliant.
- Includes timezone information.

### Example

```json
{
  "timestamp": "2026-07-20T15:30:45Z"
}
```

---

## 12.8 PaginationMetadata

### Purpose

Provides pagination information for collection responses.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| page | Integer | Yes | Current page number. |
| pageSize | Integer | Yes | Number of returned items. |
| totalItems | Integer | Yes | Total matching items. |
| totalPages | Integer | Yes | Total available pages. |

### Validation Rules

- Returned only by paginated responses.
- Values shall be non-negative.

### Example

```json
{
  "page": 1,
  "pageSize": 20,
  "totalItems": 145,
  "totalPages": 8
}
```

---

## 12.9 ProblemDetails

### Purpose

Represents standardized error information according to RFC 7807.

### Representation

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| type | String | Yes | Problem type identifier. |
| title | String | Yes | Short summary. |
| status | Integer | Yes | HTTP status code. |
| detail | String | No | Human-readable explanation. |
| instance | String | No | URI identifying the occurrence. |

### Validation Rules

- Returned for non-success responses unless otherwise specified.
- Complies with RFC 7807.
- Additional extension members may be included provided they remain RFC 7807 compliant.
- Clients shall ignore unrecognized extension members unless otherwise documented.

### Example

```json
{
  "type": "https://example.com/problems/resource-not-found",
  "title": "Resource Not Found",
  "status": 404,
  "detail": "The requested Product does not exist.",
  "instance": "/v1/products/b51e8d24"
}
```

---

# 13. Request Schemas

Every request schema follows the same specification template.

---

## 13.1 Authentication Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| username | String | Yes | User identifier. |
| password | String | Yes | User password. |

### Example

```json
{
  "username": "administrator",
  "password": "********"
}
```

---

## 13.2 Create Category Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| categoryName | String | Yes | Name of the Category. |

### Example

```json
{
  "categoryName": "Electronics"
}
```

---

## 13.3 Update Category Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| categoryName | String | Yes | Updated Category name. |

### Example

```json
{
  "categoryName": "Office Supplies"
}
```

---

## 13.4 Create Product Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| sku | String | Yes | Product SKU. |
| productName | String | Yes | Product name. |
| categoryId | String | Yes | Associated Category identifier. |

### Example

```json
{
  "sku": "LAP-1001",
  "productName": "Wireless Mouse",
  "categoryId": "8c0f0cb7"
}
```

---

## 13.5 Update Product Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| productName | String | Yes | Updated Product name. |
| categoryId | String | Yes | Associated Category identifier. |

### Example

```json
{
  "productName": "Wireless Mouse Pro",
  "categoryId": "8c0f0cb7"
}
```

---

## 13.6 Inventory Entry Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| quantity | Number | Yes | Quantity entering inventory. |

### Example

```json
{
  "quantity": 25
}
```

---

## 13.7 Inventory Exit Request

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| quantity | Number | Yes | Quantity leaving inventory. |

### Example

```json
{
  "quantity": 5
}
```

---

# 14. Response Schemas

---

## 14.1 Authentication Response

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| accessToken | String | Yes | Issued JWT access token. |
| tokenType | String | Yes | Authentication scheme. |
| expiresIn | Integer | Yes | Token lifetime in seconds. |

### Example

```json
{
  "accessToken": "<jwt>",
  "tokenType": "Bearer",
  "expiresIn": 3600
}
```

---

## 14.2 Category Response

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| id | String | Yes | Category identifier. |
| categoryName | String | Yes | Category name. |

### Example

```json
{
  "id": "8c0f0cb7",
  "categoryName": "Electronics"
}
```

---

## 14.3 Product Response

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| id | String | Yes | Product identifier. |
| sku | String | Yes | Product SKU. |
| productName | String | Yes | Product name. |
| categoryId | String | Yes | Associated Category identifier. |
| currentStock | Number | Yes | Current observable inventory. |

### Example

```json
{
  "id": "b51e8d24",
  "sku": "LAP-1001",
  "productName": "Wireless Mouse",
  "categoryId": "8c0f0cb7",
  "currentStock": 145
}
```

---

## 14.4 Inventory Movement Response

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| movementId | String | Yes | Inventory Movement identifier. |
| productId | String | Yes | Associated Product identifier. |
| movementType | String | Yes | Entry or Exit. |
| quantity | Number | Yes | Movement quantity. |
| timestamp | String | Yes | Date and time of the movement. |

### Example

```json
{
  "movementId": "f9249d0a",
  "productId": "b51e8d24",
  "movementType": "Entry",
  "quantity": 25,
  "timestamp": "2026-07-20T15:30:45Z"
}
```

---

## 14.5 Collection Response

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| items | Array | Yes | Collection of resources. |
| pagination | PaginationMetadata | No | Pagination information when applicable. |

### Example

```json
{
  "items": [
    {
      "id": "8c0f0cb7",
      "categoryName": "Electronics"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalItems": 1,
    "totalPages": 1
  }
}
```