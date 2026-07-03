# Developer Evaluation Project

`READ CAREFULLY`

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Getting Started

The solution is a .NET 8 backend located in [`template/backend`](/template/backend) and uses PostgreSQL for persistence.

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL 13 (a `docker-compose` with a database service is included)
- Entity Framework Core tools, to apply the migrations: `dotnet tool install --global dotnet-ef`

### Running the API
1. Start the database (from the repository root):
   ```bash
   docker compose -f template/backend/docker-compose.yml up -d ambev.developerevaluation.database
   ```
   Any PostgreSQL 13 reachable at `localhost:5432` with database `developer_evaluation`, user `developer` and password `ev@luAt10n` also works. The connection string lives in [`appsettings.json`](/template/backend/src/Ambev.DeveloperEvaluation.WebApi/appsettings.json).
2. Apply the migrations (run from the WebApi project folder so the connection string is picked up):
   ```bash
   cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
   dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM
   ```
3. Run the API:
   ```bash
   dotnet run --project template/backend/src/Ambev.DeveloperEvaluation.WebApi
   ```
   The console prints the URL; open `/swagger` there to explore the endpoints.

### Running the tests
```bash
dotnet test template/backend/Ambev.DeveloperEvaluation.sln
```

### Configuration

| Setting | Environment variable | Default |
| --- | --- | --- |
| Database connection | `ConnectionStrings__DefaultConnection` | see `appsettings.json` |
| Default page size | `Pagination__DefaultPageSize` | `10` |
| Maximum page size | `Pagination__MaxPageSize` | `50` |

### Sales API

The Sales API provides the full CRUD, sale and item cancellation, and pagination, ordering and filtering on the list endpoint. See [Sales API](/.doc/sales-api.md) for the endpoints and payloads; a Postman collection is available at [`/.doc/sales.postman_collection.json`](/.doc/sales.postman_collection.json).

### What was implemented
- Sales CRUD (create, get, list, update, delete) plus cancel sale and cancel item.
- Quantity-based discount rules (4–9 items: 10%, 10–20 items: 20%, above 20: not allowed) centralized in a single domain policy.
- External Identities with denormalization for customer, branch and product.
- Domain events (`SaleCreated`, `SaleModified`, `SaleCancelled`, `ItemCancelled`) written to the application log.
- Pagination, ordering and filtering on the list endpoint, with the page size limits taken from configuration.
- Unit tests for the discount rules, the sale behavior and the application handlers.

### What I would do with more time
- Integration tests exercising the endpoints against a real database.
- Richer filtering (partial string matches) as described in the general API conventions.
- Publish the domain events to a real message broker (for example Rebus) instead of the application log.

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)
