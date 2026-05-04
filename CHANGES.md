# Summary of Current Project Guidance

## Overview

This project is now a beginner-friendly Product Store built with:
- React frontend
- .NET 8 Web API backend
- PostgreSQL database
- Entity Framework Core
- DTOs and a small service layer

The guidance now focuses only on the current Product-based app.

---

## Current Backend Shape

The backend uses a clean beginner-friendly flow:

```
React
  -> JSON request
ProductsController
  -> CreateProductDto / UpdateProductDto
IProductService / ProductService
  -> Product entity
AppDbContext
  -> PostgreSQL Products table
```

### Backend Files

| Area | Files | Purpose |
|------|-------|---------|
| Model | `Models/Product.cs` | Database entity mapped by EF Core |
| DTOs | `CreateProductDto`, `UpdateProductDto`, `ProductResponseDto` | API request/response shapes |
| Service | `IProductService`, `ProductService` | Product logic and DTO mapping |
| Data | `AppDbContext.cs` | PostgreSQL/EF Core database access |
| Controller | `ProductsController.cs` | HTTP endpoints and status codes |
| Config | `Program.cs`, `appsettings.json` | Dependency injection, CORS, Swagger, database connection |

---

## Current Database

The current schema is intentionally simple:

```sql
Products
```

The `Products` table stores:
- `Id`
- `Name`
- `Description`
- `Price`

There are no relationship tables in the current beginner version. More tables can be added later after the basic CRUD flow is comfortable.

---

## DTO and Service Layer

### DTOs

DTO means Data Transfer Object. These classes control the JSON that enters and leaves the API.

| DTO | Used For | Includes `Id`? |
|-----|----------|----------------|
| `CreateProductDto` | Creating products | No |
| `UpdateProductDto` | Updating products | No |
| `ProductResponseDto` | Returning products | Yes |

### Service Layer

`ProductsController` no longer talks directly to `AppDbContext`.

Instead:
1. The controller receives an HTTP request.
2. The controller calls `IProductService`.
3. `ProductService` reads/writes products with `AppDbContext`.
4. `ProductService` maps `Product` entities to `ProductResponseDto`.
5. The controller returns the correct HTTP response.

This keeps the controller easier to read and makes the API safer because database entities are not accepted directly from the client.

---

## Current API Endpoints

| Method | Endpoint | Purpose | Request DTO | Response |
|--------|----------|---------|-------------|----------|
| GET | `/api/products` | Get all products | None | Product list |
| GET | `/api/products/{id}` | Get one product | None | One product or `404` |
| POST | `/api/products` | Create product | `CreateProductDto` | Created product |
| PUT | `/api/products/{id}` | Update product | `UpdateProductDto` | Updated product |
| DELETE | `/api/products/{id}` | Delete product | None | `204 No Content` |

Important: `POST` and `PUT` request bodies do not include `id`. The `id` comes from the URL for updates and from PostgreSQL for creates.

---

## Documentation Updated

| File | Current Focus |
|------|---------------|
| `README.md` | Project overview and learning path |
| `SETUP_GUIDE.md` | Full setup path and current structure |
| `docs/QUICK_START.md` | Fast setup for the Product Store |
| `docs/BACKEND_SETUP.md` | Product model, DTOs, service, controller, EF Core |
| `docs/API_DOCUMENTATION.md` | Product endpoints and DTO request/response examples |
| `docs/DATABASE_SCHEMA.md` | Current `Products` table only |
| `docs/EXAMPLE_FEATURES.md` | Product-focused extensions |

---

## Learning Path

Recommended order:
1. Create the PostgreSQL database.
2. Create the `Product` model.
3. Create DTOs for API input/output.
4. Create `AppDbContext`.
5. Create `IProductService` and `ProductService`.
6. Create `ProductsController`.
7. Run EF Core migrations.
8. Connect the React frontend.
9. Test add/view/update/delete product flows.

---

## Next Steps

After the current version is working, good beginner extensions are:
- Add validation attributes to DTOs.
- Add product search.
- Add price filtering.
- Add an edit form in React.
- Add a `Category` model and a relationship to `Product`.

See [EXAMPLE_FEATURES.md](./docs/EXAMPLE_FEATURES.md) for current Product-focused ideas.
