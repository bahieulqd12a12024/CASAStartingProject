# Example Features - Product Store Ideas

This document lists beginner-friendly features you can add after the basic Product Store works.

The current backend flow is:

```
ProductsController -> IProductService/ProductService -> AppDbContext -> Products table
```

When adding backend behavior, prefer putting product logic in `ProductService` and keeping `ProductsController` focused on HTTP routes and status codes.

---

## Feature 1: Product Search

Add a route that searches products by name or description.

### Backend Idea

Add this method to `IProductService`:

```csharp
List<ProductResponseDto> Search(string query);
```

Add this method to `ProductService`:

```csharp
public List<ProductResponseDto> Search(string query)
{
    query = query.ToLower();

    return _context.Products
        .Where(product =>
            product.Name.ToLower().Contains(query) ||
            product.Description.ToLower().Contains(query))
        .Select(product => ToResponseDto(product))
        .ToList();
}
```

Add this action to `ProductsController`:

```csharp
[HttpGet("search")]
public IActionResult Search([FromQuery] string query = "")
{
    var products = _productService.Search(query);
    return Ok(products);
}
```

### API

```http
GET /api/products/search?query=keyboard
```

---

## Feature 2: Price Filtering

Add a route that returns products within a price range.

### Backend Idea

Add this method to `IProductService`:

```csharp
List<ProductResponseDto> GetByPriceRange(decimal? minPrice, decimal? maxPrice);
```

Add this method to `ProductService`:

```csharp
public List<ProductResponseDto> GetByPriceRange(decimal? minPrice, decimal? maxPrice)
{
    var query = _context.Products.AsQueryable();

    if (minPrice.HasValue)
        query = query.Where(product => product.Price >= minPrice.Value);

    if (maxPrice.HasValue)
        query = query.Where(product => product.Price <= maxPrice.Value);

    return query
        .Select(product => ToResponseDto(product))
        .ToList();
}
```

Add this action to `ProductsController`:

```csharp
[HttpGet("price-range")]
public IActionResult GetByPriceRange([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
{
    var products = _productService.GetByPriceRange(minPrice, maxPrice);
    return Ok(products);
}
```

### API

```http
GET /api/products/price-range?minPrice=10&maxPrice=100
```

---

## Feature 3: Product Validation

Add validation attributes to DTOs so the API rejects incomplete or invalid product data.

### DTO Example

```csharp
using System.ComponentModel.DataAnnotations;

namespace DotNetApi.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }
    }
}
```

Do the same for `UpdateProductDto`.

Because `ProductsController` has `[ApiController]`, ASP.NET Core automatically returns `400 Bad Request` when validation fails.

---

## Feature 4: Product Categories

After you understand one table, add a second table:

```
Categories (one)
  -> Products (many)
```

Suggested backend files:
- `Models/Category.cs`
- `DTOs/CategoryResponseDto.cs`
- `Services/ICategoryService.cs`
- `Services/CategoryService.cs`
- `Controllers/CategoriesController.cs`

Suggested product changes:
- Add `CategoryId` to `Product`
- Add `Category` navigation property to `Product`
- Add a migration with `dotnet ef migrations add AddCategories`
- Apply it with `dotnet ef database update`

---

## Feature 5: Frontend Polish

Useful React improvements:
- Add search input that calls `/api/products/search`.
- Add min/max price inputs for `/api/products/price-range`.
- Add an edit form that calls `PUT /api/products/{id}`.
- Show validation errors returned by the API.
- Add loading and empty states for product lists.

---

[Back to Setup Guide](../SETUP_GUIDE.md)
