# 03_DomainModel.md

**Project:** Inventory Management REST API

**Document:** Domain Model

**Version:** 1.0

**Status:** Approved

**Based on:**

- Project Vision v0.2 (Approved)
- Functional Specification v1.2 (Approved)

---

# Revision History

| Version | Date | Description |
|----------|------------|---------------------------------------------------------------------------------------------|
| 1.0 | 2026-07-20 | Initial approved Domain Model derived from the approved Project Vision and Functional Specification. |

---

# 1. Purpose

The purpose of this document is to define the conceptual business domain of the Inventory Management REST API independently of any technical implementation or persistence mechanism.

This document establishes the business concepts, aggregate boundaries, domain terminology, and behavioral responsibilities that compose the core domain model. It serves as the foundation for all subsequent architectural and implementation decisions while remaining fully aligned with the approved Project Vision and Functional Specification.

The Domain Model has the following objectives:

- Represent the business using a common and consistent language.
- Define the business concepts involved in inventory management.
- Identify aggregate boundaries and their responsibilities.
- Establish business invariants that must always hold true.
- Clarify the relationships between domain concepts.
- Provide a shared understanding for analysts, architects, and developers.

This document intentionally models **business behavior rather than data storage**. Consequently, it does not describe database structures, persistence strategies, application services, or infrastructure concerns.

---

# 2. Domain Overview

The Inventory Management REST API supports organizations in maintaining an accurate representation of their inventory by managing products, organizing them into categories, and recording inventory movements that affect product availability.

The domain is centered on the concept of **Inventory Integrity**. Every business operation ultimately contributes to maintaining a reliable representation of the available stock while preserving a complete and consistent history of inventory changes.

The domain is intentionally limited to the inventory lifecycle defined by the approved project scope.

Business capabilities include:

- Organizing products through categories.
- Maintaining a catalog of inventory products.
- Registering inventory entries.
- Registering inventory exits.
- Maintaining current inventory availability.
- Preserving the chronological history of inventory movements.

Several business capabilities commonly found in enterprise inventory systems are explicitly outside the domain boundary for Version 1, including:

- Procurement
- Sales
- Warehouse Management
- Supplier Management
- Customer Management
- Reservations
- Reporting
- Notifications

These exclusions intentionally simplify the domain while preserving opportunities for future expansion without requiring fundamental changes to the core model.

The domain therefore focuses exclusively on maintaining inventory consistency and ensuring that every inventory modification complies with the approved business rules.

---

# 3. Bounded Context

## Inventory Management Context

The entire solution operates within a single bounded context named **Inventory Management**.

This bounded context encapsulates all business knowledge required to manage products and their inventory throughout their operational lifecycle.

Within this context, the following responsibilities are included:

- Category lifecycle management.
- Product lifecycle management.
- Inventory movement registration.
- Current inventory calculation.
- Inventory history management.

The bounded context assumes responsibility for enforcing all inventory-related business rules defined by the Functional Specification, including:

- Product categorization.
- Inventory consistency.
- Uniqueness constraints.
- Prevention of negative inventory.

Authentication is considered a supporting capability required to protect access to the domain but is not itself part of the business domain.

Likewise, API documentation and deployment concerns remain outside the bounded context because they do not represent business concepts.

At the current scope, no additional bounded contexts are identified.

The Project Vision intentionally excludes business capabilities such as purchasing, sales, warehouse management, suppliers, and customers, avoiding the need for context mapping or inter-context communication in Version 1.

---

# 4. Core Domain

The **Core Domain** of the system is **Inventory Management**.

Its primary responsibility is to preserve **Inventory Integrity**, ensuring that the quantity of inventory available for every product always reflects the cumulative effect of all valid business operations while remaining consistent with the defined business rules.

Every business concept identified in this domain ultimately exists to support that responsibility.

Consequently:

- Every **Aggregate** defines a consistency boundary that protects critical business invariants.
- Every **Entity** represents a business concept required to model the inventory lifecycle.
- Every **Domain Service** encapsulates business behavior that cannot naturally belong to a single Aggregate while contributing to the preservation of Inventory Integrity.

This focus establishes Inventory Management as the central business capability of the solution and guides all subsequent domain modeling decisions.

---

# 5. Ubiquitous Language

The following glossary establishes the official vocabulary for all future specifications, architecture documents, API contracts, and implementation artifacts.

Every stakeholder should use these terms consistently to minimize ambiguity and preserve a shared understanding of the business domain.

| Term | Definition |
|------|------------|
| **Category** | A business classification used to organize Products within the inventory catalog. A Category may contain zero or more Products. |
| **Product** | An inventory item managed by the organization. Every Product belongs to exactly one Category and is uniquely identified by its SKU. |
| **SKU (Stock Keeping Unit)** | The immutable business identifier assigned to a Product within the inventory catalog. |
| **Inventory** | The current collection of Products together with their available quantities. |
| **Current Stock** | The current business state representing the available quantity of a Product after applying all successful Inventory Movements. Conceptually, Current Stock is a derived business state obtained from the complete inventory history. Although implementations may persist this value for performance purposes, its business meaning is always derived from the sequence of recorded Inventory Movements. |
| **Inventory Movement** | A business record representing a change to the available quantity of a Product. Every movement captures a completed inventory operation, is classified as either an Entry or an Exit, and permanently contributes to the inventory history from which the current inventory state is derived. |
| **Entry** | An Inventory Movement that increases the available stock of a Product. |
| **Exit** | An Inventory Movement that decreases the available stock of a Product without allowing inventory to become negative. |
| **Inventory History** | The complete chronological record of all successful Inventory Movements performed for Products. |
| **Inventory Integrity** | The fundamental business objective of ensuring that the current inventory accurately reflects all valid Inventory Movements while continuously satisfying the domain's business invariants. |
| **Consistency Boundary** | The transactional boundary within which business invariants are guaranteed to remain valid. In Domain-Driven Design, an Aggregate defines a consistency boundary and is responsible for protecting its invariants during every business operation. |
| **Authenticated User** | A user successfully authenticated and authorized to execute business operations within the system. Authentication enables access to the domain but does not constitute a business concept itself. |
| **Aggregate** | A consistency boundary that groups related domain objects and guarantees enforcement of business invariants. |
| **Aggregate Root** | The single entry point through which external interactions with an Aggregate occur. It is responsible for protecting the Aggregate's business rules and maintaining consistency. |
| **Entity** | A domain object distinguished by a persistent business identity whose lifecycle extends over time. |
| **Value Object** | An immutable domain object identified solely by the value of its attributes rather than by an independent identity. |
| **Business Invariant** | A business rule that must remain true before and after every valid business operation. Violating an invariant results in rejection of the attempted operation. |
| **Domain Event** | A representation of a significant business occurrence that has taken place within the domain and may be relevant to other parts of the system or future bounded contexts. |

The ubiquitous language defined in this section constitutes the **official business vocabulary** for all subsequent specification documents, architecture documents, API contracts, and implementation artifacts.

All stakeholders should consistently use these terms to preserve a shared understanding of the domain and ensure traceability across the Specification-Driven Development process.

# 6. Aggregate Map

The Inventory Management domain is intentionally modeled using a **small number of Aggregates**, each representing a true business consistency boundary rather than a direct representation of individual business concepts.

An Aggregate exists only when multiple domain objects must maintain business invariants atomically. Consequently, not every Entity becomes an Aggregate Root.

Version 1 of the domain identifies the following Aggregate Roots:

| Aggregate Root | Purpose |
|----------------|---------|
| **Category** | Maintains the lifecycle and identity of product classifications while protecting category-specific business rules. |
| **Product** | Maintains the lifecycle of inventory products and guarantees the consistency of inventory state and inventory movement history. |

No additional Aggregate Roots are required within the approved scope.

The conceptual Aggregate Map is therefore:

```text
Inventory Management

├── Category (Aggregate Root)
│
└── Product (Aggregate Root)
     │
     └── Inventory Movement (Entity)
```

The Aggregate boundaries are intentionally aligned with business consistency rather than object ownership or persistence structure.

---

# 7. Aggregates

## 7.1 Category Aggregate

### Aggregate Root

**Category**

### Purpose

The Category Aggregate represents the business concept used to classify Products.

Its responsibility is to maintain the integrity of category information throughout its lifecycle while ensuring that every Category can safely participate in product classification.

The Aggregate intentionally has a narrow responsibility. It does **not** manage Products, inventory, or Inventory Movements.

### Why This Aggregate Exists

Category has its own lifecycle and business identity.

Business rules concerning Categories apply independently of Products, including:

- Category names shall be unique.
- Categories may exist without Products.
- Categories containing Products cannot be deleted.

These rules define a natural consistency boundary.

### Aggregate Responsibilities

The Category Aggregate is responsible for:

- Creating Categories.
- Updating Category information.
- Preserving Category identity.
- Enforcing category-specific invariants.
- Determining whether deletion is permissible according to business rules.

### Aggregate Boundary

The Category Aggregate contains only the **Category** Entity.

Products are intentionally excluded because they possess an independent lifecycle and belong to a different consistency boundary.

---

## 7.2 Product Aggregate

### Aggregate Root

**Product**

### Purpose

The Product Aggregate represents the central business concept of the Inventory Management domain.

Its responsibility is to preserve the inventory state of an individual Product while maintaining a complete and consistent history of inventory changes.

All business operations affecting inventory are coordinated through this Aggregate.

### Why This Aggregate Exists

Inventory consistency is defined per Product.

Every business rule involving inventory applies to a single Product, including:

- Inventory cannot become negative.
- Current Stock shall always reflect successful Inventory Movements.
- Every Inventory Movement belongs to exactly one Product.
- Inventory History is authoritative.

These rules must be enforced atomically within a single consistency boundary.

### Aggregate Responsibilities

The Product Aggregate is responsible for:

- Maintaining Product identity.
- Managing Product information.
- Registering inventory entries.
- Registering inventory exits.
- Maintaining the consistency of Current Stock.
- Preserving Inventory History.
- Enforcing inventory-related invariants.

### Aggregate Boundary

The Product Aggregate contains:

- Product (Aggregate Root)
- Inventory Movement (Entity)

The Aggregate ensures that every inventory modification preserves business consistency before the operation completes.

---

# 8. Entities

Entities represent domain concepts with a continuous identity whose lifecycle extends over time.

---

## 8.1 Category

### Identity

A Category possesses a unique business identity independent of its attributes.

### Responsibilities

- Represent a Product classification.
- Maintain Category information.
- Participate in Product organization.

### Aggregate Membership

Category is the Aggregate Root of the Category Aggregate.

### Rationale

Category has an independent lifecycle and business rules that are unrelated to inventory operations.

---

## 8.2 Product

### Identity

A Product possesses a unique business identity.

Within the business domain, its identity is expressed through its SKU.

### Responsibilities

- Represent an inventory item.
- Maintain the consistency of Current Stock.
- Coordinate inventory operations.
- Protect Inventory Integrity.
- Maintain Inventory History.

### Aggregate Membership

Product is the Aggregate Root of the Product Aggregate.

### Rationale

Inventory consistency is enforced at the Product level.

Every inventory operation affects exactly one Product, making Product the natural consistency boundary.

---

## 8.3 Inventory Movement

### Identity

Each Inventory Movement possesses its own business identity representing a completed inventory operation.

Once created, an Inventory Movement is immutable.

### Responsibilities

- Represent a completed inventory modification.
- Record the Inventory Movement type.
- Record the Quantity involved in the operation.
- Contribute permanently to Inventory History.

### Aggregate Membership

Inventory Movement belongs to the Product Aggregate.

### Rationale

Inventory Movements have no independent business meaning outside the context of a Product.

An Inventory Movement cannot exist without exactly one Product.

Furthermore:

- It cannot be created independently.
- It cannot be transferred between Products.
- It contributes directly to Current Stock.
- It participates in enforcing inventory invariants.

For these reasons, Inventory Movement is modeled as an Entity contained within the Product Aggregate rather than as an independent Aggregate Root.

# 9. Value Objects

Value Objects represent immutable domain concepts that are defined entirely by their value rather than by an independent business identity.

Introducing Value Objects for meaningful business concepts strengthens the domain model by avoiding the widespread use of primitive types, improving expressiveness, and localizing validation rules within the concepts they represent.

Version 1 of the domain identifies the following Value Objects.

---

## 9.1 SKU

### Purpose

Represents the immutable business identifier assigned to a Product within the inventory catalog.

### Characteristics

- Immutable.
- Equality based solely on value.
- Carries business meaning beyond a plain string.
- Encapsulates SKU validation rules.
- Expresses the Product's business identity within the inventory catalog.

### Why It Is a Value Object

The **Product** is the Entity that possesses business identity and an independent lifecycle.

The **SKU** does not replace the Product's identity. Instead, it is an immutable Value Object that expresses that identity within the inventory catalog. It encapsulates the business semantics and validation associated with product identification while remaining one of the Product's defining characteristics.

Modeling SKU as a Value Object prevents primitive string values from spreading throughout the domain while preserving the ubiquitous language and centralizing business validation.

---

## 9.2 ProductName

### Purpose

Represents the business name assigned to a Product.

### Characteristics

- Immutable.
- Equality based on value.
- Encapsulates naming constraints.
- Represents business meaning rather than a simple string.

### Why It Is a Value Object

A ProductName has no independent identity and exists solely as descriptive information belonging to a Product.

Encapsulating it as a Value Object localizes validation rules and avoids repeated string validation across the domain.

---

## 9.3 CategoryName

### Purpose

Represents the business name assigned to a Category.

### Characteristics

- Immutable.
- Equality based on value.
- Encapsulates naming constraints.
- Expresses a meaningful business concept.

### Why It Is a Value Object

Like ProductName, CategoryName possesses business semantics but no independent lifecycle.

Its uniqueness is enforced by the Category Aggregate as a business invariant, while the Value Object is responsible only for representing a valid category name.

This separation clearly distinguishes value validation from Aggregate consistency rules.

---

## 9.4 Quantity

### Purpose

Represents a positive quantity involved in an inventory operation.

### Characteristics

- Immutable.
- Equality based on value.
- Represents the magnitude of an inventory operation.
- Participates in inventory calculations.
- Encapsulates quantity validation.

### Why It Is a Value Object

Quantity is more than a numeric value.

Within the inventory domain it represents the magnitude associated with an inventory operation and participates directly in business behavior such as inventory entries and inventory exits.

A Quantity intentionally **does not encode whether inventory increases or decreases**. The semantic meaning of the operation is determined exclusively by the associated **InventoryMovementType** (Entry or Exit), while Quantity represents only the positive amount involved in that operation.

Modeling Quantity as a Value Object makes these concepts explicit in the domain and prevents arbitrary primitive numeric values from bypassing domain validation.

---

## Candidate Value Objects Evaluated

| Candidate | Decision | Rationale |
|-----------|----------|-----------|
| **SKU** | **Accepted** | Immutable business identifier that expresses a Product's business identity within the inventory catalog while encapsulating domain semantics and validation. |
| **ProductName** | **Accepted** | Descriptive business concept with validation and no independent identity. |
| **CategoryName** | **Accepted** | Business concept represented entirely by its value. Aggregate uniqueness remains an invariant rather than a Value Object responsibility. |
| **Quantity** | **Accepted** | Represents the positive magnitude of an inventory operation while encapsulating quantity validation independently of the movement type. |

---

## Value Objects Not Identified

No additional Value Objects are currently justified by the approved business scope.

Concepts such as authentication credentials, timestamps, Aggregate identifiers, or infrastructure-specific representations belong to other layers or bounded contexts and are intentionally excluded from the domain model.

---

# 10. Enumerations

The following domain enumeration is identified within the approved business scope.

---

## InventoryMovementType

Represents the classification of an Inventory Movement.

### Values

| Value | Description |
|-------|-------------|
| **Entry** | Increases the available inventory of a Product. |
| **Exit** | Decreases the available inventory of a Product. |

### Rationale

The approved Project Vision explicitly establishes that every Inventory Movement is represented by a single business concept classified as either **Entry** or **Exit**.

This classification is intrinsic to the business meaning of an Inventory Movement and therefore belongs to the domain model rather than to infrastructure or application concerns.

---

# 11. Relationships

The relationships described below represent **business relationships** between domain concepts. They define collaboration and ownership within the domain model and shall not be interpreted as persistence or database associations.

---

## Category ↔ Product

### Relationship

A **Category** classifies one or more **Products**.

Every Product belongs to exactly one Category throughout its lifecycle.

A Category may exist without any associated Products.

### Business Responsibility

The Category provides organizational structure for the inventory catalog, while each Product depends on exactly one Category for classification.

### Ownership

Neither Aggregate owns the other.

The relationship is a business reference between two independent Aggregate Roots.

---

## Product ↔ Inventory Movement

### Relationship

A Product maintains the complete history of its Inventory Movements.

Every Inventory Movement belongs to exactly one Product.

An Inventory Movement cannot exist independently of a Product.

### Business Responsibility

Inventory Movements collectively describe the evolution of a Product's inventory state.

The Product is responsible for coordinating every Inventory Movement and ensuring that each movement preserves Inventory Integrity.

### Ownership

Inventory Movement is an Entity contained within the Product Aggregate.

---

## Product ↔ Current Stock

### Relationship

Current Stock represents the current business state of a Product.

It is derived from the cumulative effect of all successful Inventory Movements.

### Business Responsibility

The Product is responsible for ensuring that Current Stock remains consistent with its Inventory History.

Current Stock is therefore a business state maintained by the Product Aggregate rather than an independent domain concept.

---

# 12. Aggregate Invariants

Aggregate Invariants define the business conditions that must always hold true within an Aggregate before and after every successful business operation.

Violation of an Invariant results in rejection of the attempted operation.

---

## Category Aggregate

| ID | Invariant |
|----|-----------|
| AI-CAT-001 | Every Category shall have a valid CategoryName. |
| AI-CAT-002 | CategoryName shall be unique within the inventory catalog. |
| AI-CAT-003 | A Category may exist without Products. |
| AI-CAT-004 | A Category associated with one or more Products cannot be deleted. |

---

## Product Aggregate

| ID | Invariant |
|----|-----------|
| AI-PROD-001 | Every Product shall possess a valid SKU. |
| AI-PROD-002 | Every Product shall possess a valid ProductName. |
| AI-PROD-003 | Every Product shall belong to exactly one Category. |
| AI-PROD-004 | Every Inventory Movement shall belong to exactly one Product. |
| AI-PROD-005 | Every Inventory Movement shall have exactly one InventoryMovementType. |
| AI-PROD-006 | Every Inventory Movement shall contain a positive Quantity. |
| AI-PROD-007 | Inventory shall never become negative. |
| AI-PROD-008 | Current Stock shall always reflect the cumulative effect of all successful Inventory Movements. |
| AI-PROD-009 | Inventory History shall remain complete and consistent after every successful inventory operation. |
| AI-PROD-010 | A Product with Inventory History cannot be deleted. |

These Invariants collectively preserve the Core Domain objective of maintaining Inventory Integrity.

# 13. Domain Services

Domain Services encapsulate business behavior that does not naturally belong to a single Aggregate while still representing domain knowledge.

A Domain Service should only exist when a business operation:

- Cannot be assigned to an Entity or Aggregate Root without violating cohesion.
- Requires collaboration across multiple Aggregates while preserving business semantics.
- Represents domain behavior rather than application orchestration or infrastructure concerns.

After analyzing the approved Project Vision and Functional Specification, **no explicit Domain Services have been identified for Version 1**.

All approved business operations naturally belong to one of the existing Aggregate Roots:

- Category lifecycle operations belong to the **Category Aggregate**.
- Product lifecycle operations belong to the **Product Aggregate**.
- Inventory Entry registration belongs to the **Product Aggregate**.
- Inventory Exit registration belongs to the **Product Aggregate**.
- Current Stock consistency belongs to the **Product Aggregate**.
- Inventory History maintenance belongs to the **Product Aggregate**.

No business operation currently requires coordination across Aggregate boundaries while simultaneously containing domain logic that cannot reasonably reside within an Aggregate.

Should future versions introduce business capabilities such as inventory transfers between warehouses, stock reservations, purchasing workflows, or inventory reconciliation involving multiple Aggregates, new Domain Services may become appropriate.

Until such requirements exist, introducing Domain Services would unnecessarily increase the complexity of the domain model without providing additional business value.

---

# 14. Domain Events

Domain Events represent significant business occurrences that have taken place within the domain.

They capture facts about completed business operations and provide a conceptual mechanism for communicating meaningful domain changes to other parts of the system or to future bounded contexts.

The Domain Events identified in this document describe the business model and **do not imply that an event-driven architecture or messaging infrastructure will necessarily be implemented**.

---

## Category Events

### CategoryCreated

Occurs after a Category has been successfully created.

---

### CategoryUpdated

Occurs after Category information has been successfully modified.

---

### CategoryDeleted

Occurs after a Category has been successfully deleted.

---

## Product Events

### ProductCreated

Occurs after a Product has been successfully created.

---

### ProductUpdated

Occurs after Product information has been successfully modified.

---

### ProductDeleted

Occurs after a Product has been successfully deleted.

---

## Inventory Events

### InventoryEntryRegistered

Occurs after an Inventory Entry has been successfully recorded.

This event indicates that the Product's available inventory has increased while preserving all Aggregate Invariants.

---

### InventoryExitRegistered

Occurs after an Inventory Exit has been successfully recorded.

This event indicates that the Product's available inventory has decreased without violating Inventory Integrity.

---

# 15. Domain Model Traceability

The following matrix establishes traceability between the Domain Model and the approved Functional Specification.

| Domain Concept | Functional Requirements | Business Rules | Use Cases |
|----------------|-------------------------|----------------|-----------|
| Category Aggregate | FR-001, FR-002, FR-003 | BR-001, BR-002 | UC-01, UC-02, UC-03 |
| Product Aggregate | FR-004, FR-005, FR-006 | BR-003, BR-004, BR-005 | UC-04, UC-05, UC-06 |
| Inventory Movement | FR-007, FR-008 | BR-006, BR-007 | UC-07, UC-08 |
| InventoryMovementType | FR-007, FR-008 | BR-006 | UC-07, UC-08 |
| SKU | FR-004 | BR-003 | UC-04, UC-05, UC-06 |
| ProductName | FR-004, FR-005 | BR-004 | UC-04, UC-05, UC-06 |
| CategoryName | FR-001, FR-002 | BR-001 | UC-01, UC-02, UC-03 |
| Quantity | FR-007, FR-008 | BR-006, BR-007 | UC-07, UC-08 |
| Current Stock | FR-009 | BR-008 | UC-07, UC-08 |
| Inventory History | FR-010 | BR-009 | UC-07, UC-08 |

This traceability ensures that every domain concept originates from an approved business requirement and that no element of the domain model introduces unauthorized business functionality.

---

# 16. Modeling Decisions

The following decisions summarize the most significant architectural modeling choices made while constructing the Domain Model.

---

## MD-001 — Aggregate Boundaries Follow Business Consistency

Aggregate boundaries are defined by business consistency requirements rather than by database structure or object composition.

Each Aggregate exists only where business invariants must be protected transactionally.

---

## MD-002 — Rich Domain Model

Business behavior belongs inside the domain model rather than being distributed across infrastructure or application services.

Aggregates are responsible for protecting their own business rules and invariants.

---

## MD-003 — Explicit Value Objects

Meaningful business concepts such as SKU, ProductName, CategoryName, and Quantity are modeled as Value Objects instead of primitive data types.

This improves expressiveness, encapsulates validation rules, and strengthens the ubiquitous language.

---

## MD-004 — Current Stock Is a Derived Business State

Current Stock is conceptually derived from the cumulative sequence of successful Inventory Movements.

Although implementations may persist Current Stock for performance purposes, its business meaning always originates from Inventory History.

---

## MD-005 — Inventory Movement Belongs to the Product Aggregate

Inventory Movement is modeled as an Entity contained within the Product Aggregate because it has no independent lifecycle or business meaning outside the context of a Product.

This ownership ensures that every inventory modification is coordinated through the Aggregate responsible for preserving Inventory Integrity.

---

## MD-006 — Domain Events Represent Business Facts

Domain Events describe significant business occurrences within the domain model.

Their inclusion documents important business concepts independently of any particular messaging technology or implementation strategy.

---

## MD-007 — No Explicit Domain Services

The approved business scope does not currently justify introducing Domain Services.

All identified business behavior naturally belongs to the existing Aggregate Roots, preserving cohesion while avoiding unnecessary abstraction.

Domain Services should only be introduced when future requirements involve domain behavior that cannot reasonably be assigned to a single Aggregate.

---

## MD-008 — Inventory Integrity Is the Guiding Modeling Principle

The preservation of **Inventory Integrity** is the fundamental principle guiding every modeling decision in this domain.

Aggregate boundaries, business invariants, Entity ownership, Value Objects, and Domain Events have all been defined to ensure that every valid business operation maintains a consistent and reliable representation of inventory throughout the system.

