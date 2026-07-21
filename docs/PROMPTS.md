# AI Prompt Engineering Log

## Overview

This document summarizes the representative prompts used during the Specification-Driven Development (SDD) process followed for this technical assessment.

Rather than documenting every interaction with the AI assistant, this document captures the prompt patterns that guided the evolution of the project's specification baseline.

Throughout the project, AI was used as a **requirements analysis**, **specification review**, and **architectural documentation** assistant. It was intentionally **not used as an autonomous code generator** during the specification phase.

Final decisions, approvals, and implementation choices always remained the responsibility of the developer.

---

# AI-Assisted Development Workflow

The project followed the workflow below.

| Phase | Objective | Primary Artifact | AI Contribution |
|---------|-----------|-----------------|-----------------|
| Project Vision | Define business scope | 01_ProjectVision.md | Reviewed scope, identified ambiguities and missing decisions |
| Functional Specification | Define system behavior | 02_FunctionalSpecification.md | Expanded approved requirements into functional behavior |
| Domain Model | Define business concepts | 03_DomainModel.md | Identified entities, relationships and business rules |
| API Contract | Define external interface | 04_APIContract.md | Proposed REST resources and API behavior |
| Architecture | Define implementation strategy | 05_Architecture.md | Organized the solution using Clean Architecture and CQRS |
| Architecture Decision Records | Document architectural rationale | 06_ArchitectureDecisionRecords.md | Produced ADRs describing accepted implementation decisions |
| Final Governance Review | Validate specification consistency | Approved SDD Baseline | Verified cross-document consistency and traceability |

---

# Prompt Engineering Strategy

Every interaction followed a repeatable prompting strategy.

The process consistently applied the following principles:

1. Establish the current approved document as the authoritative source.
2. Clearly define the AI's responsibility before each task.
3. Explicitly prohibit implementation while producing specifications.
4. Preserve traceability between every specification artifact.
5. Validate consistency before approving each document.
6. Promote iterative refinement rather than one-shot generation.
7. Complete the specification baseline before implementing software.

This structured prompting approach helped maintain clear separation between:

- Business requirements
- Functional behavior
- Domain concepts
- API definition
- Architecture
- Architectural decisions
- Software implementation

---

# Representative Prompt 1 — Project Vision Review

## Objective

Validate the business scope before any design work begins.

## Representative Prompt

> I am implementing a technical assessment using a Specification-Driven Development (SDD) approach.
>
> The attached Project Vision document is the approved project scope.
>
> Do not generate any code.
>
> Review the document, identify ambiguities, inconsistencies, or missing functional decisions.

## AI Contribution

- Reviewed the approved scope
- Identified ambiguous requirements
- Suggested missing business decisions
- Avoided implementation details

## Human Validation

All suggested improvements were manually reviewed before updating the Project Vision.

## Result

**01_ProjectVision.md**

---

# Representative Prompt 2 — Functional Specification

## Objective

Transform the approved business vision into functional requirements.

## Representative Prompt

> Consider the approved Project Vision as the single source of truth.
>
> Generate a Functional Specification derived exclusively from that document.
>
> Do not introduce new requirements.

## AI Contribution

- Structured functional requirements
- Expanded business behavior
- Preserved traceability
- Avoided architectural assumptions

## Human Validation

Requirements were reviewed and refined before approval.

## Result

**02_FunctionalSpecification.md**

---

# Representative Prompt 3 — Domain Modeling

## Objective

Create the conceptual domain model.

## Representative Prompt

> Using the approved Functional Specification, identify entities, relationships, aggregates, and business rules.
>
> Do not generate implementation classes, ORM models, or database schemas.

## AI Contribution

- Proposed domain entities
- Identified relationships
- Suggested aggregate boundaries
- Distinguished business concepts from persistence concerns

## Human Validation

Entity boundaries and relationships were manually validated.

## Result

**03_DomainModel.md**

---

# Representative Prompt 4 — API Contract

## Objective

Specify the external behavior of the REST API.

## Representative Prompt

> Generate an API Contract derived exclusively from the approved Functional Specification and Domain Model.
>
> Do not make implementation assumptions.

## AI Contribution

- Designed REST resources
- Proposed request and response models
- Suggested status codes
- Defined validation behavior

## Human Validation

Endpoints and payloads were reviewed before approval.

## Result

**04_APIContract.md**

---

# Representative Prompt 5 — Architecture

## Objective

Describe how the approved specifications would be implemented.

## Representative Prompt

> The Project Vision, Functional Specification, Domain Model, and API Contract are approved.
>
> Generate the Architecture document describing how the approved behavior will be implemented.
>
> Preserve traceability to every approved artifact.

## AI Contribution

- Organized the solution using Clean Architecture
- Defined CQRS strategy
- Proposed dependency structure
- Documented validation, persistence, security, testing, and deployment strategies

## Human Validation

Architectural decisions were refined to remain implementation-focused.

## Result

**05_Architecture.md**

---

# Representative Prompt 6 — Architecture Decision Records

## Objective

Document the rationale behind architectural decisions.

## Representative Prompt

> Generate Architecture Decision Records based on the approved Architecture.
>
> ADRs must explain implementation decisions without introducing new functional behavior.

## AI Contribution

- Produced structured ADRs
- Evaluated alternative approaches
- Documented trade-offs
- Preserved separation between specification and implementation

## Human Validation

Every ADR was reviewed before approval.

## Result

**06_ArchitectureDecisionRecords.md**

---

# Representative Prompt 7 — Final Consistency Review

## Objective

Validate the complete Specification-Driven Development baseline.

## Representative Prompt

> Review the complete specification baseline.
>
> Identify inconsistencies, missing traceability, duplicated responsibilities, or architectural conflicts.

## AI Contribution

- Reviewed every specification artifact
- Verified cross-document consistency
- Confirmed traceability
- Identified governance improvements

## Human Validation

The complete baseline was manually reviewed and formally approved before implementation.

## Result

**Approved Specification-Driven Development Baseline**

---

# Lessons Learned

The most valuable use of AI during this project was not code generation.

Instead, AI served as a structured engineering assistant that supported:

- Requirements refinement
- Specification review
- Domain analysis
- API design
- Architectural documentation
- Consistency verification
- Architectural governance

The iterative prompting strategy reduced ambiguity before implementation, improved documentation quality, strengthened traceability, and helped establish a complete Specification-Driven Development baseline.

The software implementation that followed was therefore based on a stable, reviewed, and versioned specification rather than evolving requirements.