# Backend Setup - .NET 8 with Entity Framework Core (Beginner-Friendly)

This guide teaches you step-by-step, building from simple to more complex.

## Prerequisites

- **.NET 8 SDK**: Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/)
- **PostgreSQL**: Install PostgreSQL 13+
- **VS Code** (recommended for learning)

Verify installation:
```bash
dotnet --version          # Should show 8.0.x
psql --version           # Should show 13.x or higher
```

---

## What We're Building

A simple REST API with:
- **1 Model**: `Product` (name, description, price)
- **3 DTOs**: Safe request/response shapes for products
- **1 Service Layer**: Product business/data logic
- **1 Controller**: Handles GET, POST, PUT, DELETE by calling the service
- **1 Database**: PostgreSQL stores the data

Think of it like: PostgreSQL ↔ EF Core DbContext ↔ ProductService ↔ ProductsController ↔ React

---

## Step 1: Create a New .NET Project

```bash
# 1. Go to your backend folder
cd backend

# 2. Create a new Web API project
dotnet new webapi -n DotNetApi -f net8.0

# 3. Enter the project folder
cd DotNetApi

# 4. Check the files created
ls -la
# You'll see: Program.cs, appsettings.json, Controllers/, etc.
```

**What just happened:**
- Created a folder called `DotNetApi`
- Generated starter files for a .NET API
- This is your project structure ready to go

---

## Step 2: Add Database Packages

```bash
# Inside DotNetApi folder, add PostgreSQL support
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Three packages for:
# 1. PostgreSQL connection
# 2. Database design tools
# 3. Migration commands (ef)
```

**What just happened:**
- Added tools to connect to PostgreSQL
- Added tools to manage database changes

---

## Step 3: Create Your First Model

Create a new folder: `Models/`

Then create file: `Models/Product.cs`

```csharp
namespace DotNetApi.Models
{
    // This class represents a Product in our database
    public class Product
    {
        // Properties (columns in database)
        public int Id { get; set; }              // Primary key
        public string Name { get; set; } = string.Empty;        // Product name
        public string Description { get; set; } = string.Empty; // Product details
        public decimal Price { get; set; }      // Product cost
    }
}
```

**What this means:**
- `Id` - Unique identifier for each product (auto-generated)
- `Name` - Product name (like "Laptop", "Mouse", etc.)
- `Description` - Details about the product
- `Price` - Cost of the product

---

## Step 4: Create Database Connection

Create a new folder: `Data/`

Then create file: `Data/AppDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using DotNetApi.Models;

namespace DotNetApi.Data
{
    // AppDbContext = Connection to our database
    // It knows about all our models (Product, etc.)
    public class AppDbContext : DbContext
    {
        // Constructor - required by Entity Framework
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        // DbSet = A table in the database
        // Products table will store all our product data
        public DbSet<Product> Products { get; set; }
    }
}
```

**What this does:**
- Creates a connection to the database
- Tells the system: "We have a Products table"
- Makes it easy to read/write product data

---

## Step 5: Configure the Connection String

Open file: `appsettings.json`

Replace it with:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=dotnet_db;User Id=postgres;Password=yourpassword;"
  },
  "AllowedHosts": "*"
}
```

**What this means:**
- `Server=localhost` - Database is on your computer
- `Database=dotnet_db` - Database name (you'll create this)
- `User Id=postgres` - Default PostgreSQL user
- `Password=yourpassword` - Your PostgreSQL password (from setup)

> **Warning**: Change `yourpassword` to match your PostgreSQL password!

---

## Step 6: Create DTOs

DTO means **Data Transfer Object**. DTOs describe the data that enters and leaves the API. The database model stays in `Models/Product.cs`; the API request/response shapes live in `DTOs/`.

Create a new folder: `DTOs/`

Create file: `DTOs/CreateProductDto.cs`

```csharp
namespace DotNetApi.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
```

Create file: `DTOs/UpdateProductDto.cs`

```csharp
namespace DotNetApi.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
```

Create file: `DTOs/ProductResponseDto.cs`

```csharp
namespace DotNetApi.DTOs
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
```

**Why this helps:**
- Create/update requests do not send `Id`; PostgreSQL generates it.
- Controllers no longer expose the database entity directly.
- Later, you can add fields to `Product` without automatically exposing them through the API.

---

## Step 7: Create the Product Service

Services hold the work that used to live directly inside the controller. The controller stays focused on HTTP responses, while the service handles database access and DTO mapping.

Create a new folder: `Services/`

Create file: `Services/IProductService.cs`

```csharp
using DotNetApi.DTOs;

namespace DotNetApi.Services
{
    public interface IProductService
    {
        List<ProductResponseDto> GetAll();
        ProductResponseDto? GetById(int id);
        ProductResponseDto Create(CreateProductDto dto);
        ProductResponseDto? Update(int id, UpdateProductDto dto);
        bool Delete(int id);
    }
}
```

Create file: `Services/ProductService.cs`

```csharp
using DotNetApi.Data;
using DotNetApi.DTOs;
using DotNetApi.Models;

namespace DotNetApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public List<ProductResponseDto> GetAll()
        {
            return _context.Products
                .Select(product => ToResponseDto(product))
                .ToList();
        }

        public ProductResponseDto? GetById(int id)
        {
            var product = _context.Products.Find(id);
            return product == null ? null : ToResponseDto(product);
        }

        public ProductResponseDto Create(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return ToResponseDto(product);
        }

        public ProductResponseDto? Update(int id, UpdateProductDto dto)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;

            _context.SaveChanges();

            return ToResponseDto(product);
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }

        private static ProductResponseDto ToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }
    }
}
```

---

## Step 8: Configure Program.cs (The Brain)

Open file: `Program.cs`

Replace all content with:

```csharp
using Microsoft.EntityFrameworkCore;
using DotNetApi.Data;
using DotNetApi.Services;

// Builder = Sets up the API
var builder = WebApplication.CreateBuilder(args);

// Add services (things the API needs)
builder.Services.AddControllers();

// Connect to PostgreSQL database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register the product service for dependency injection
builder.Services.AddScoped<IProductService, ProductService>();

// Add Swagger (nice documentation UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

// Build the app
var app = builder.Build();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**What each part does:**
- `AddControllers()` - Enables API routes
- `AddDbContext()` - Connects to PostgreSQL
- `AddScoped<IProductService, ProductService>()` - Lets controllers ask for the product service
- `AddCors()` - Allows the React app to connect
- `UseSwagger()` - Enables interactive API documentation

---

## Step 9: Create Migrations (Database Setup)

Migrations = Instructions for creating database tables

```bash
# Create a migration (instructions for first database setup)
dotnet ef migrations add InitialCreate
dotnet ef database update

# What to expect:
# - Creates a "Migrations" folder
# - A file like "20240115_InitialCreate.cs" is created
# - This file has the SQL to create the Products table
```

---

## Step 10: Create the API Controller

Create file: `Controllers/ProductsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using DotNetApi.DTOs;
using DotNetApi.Services;

namespace DotNetApi.Controllers
{
    // This controller handles all /api/products requests
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        // GET /api/products/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST /api/products
        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto dto)
        {
            if (dto == null)
                return BadRequest();

            var product = _productService.Create(dto);

            return CreatedAtAction(nameof(GetById),
                new { id = product.Id }, product);
        }

        // PUT /api/products/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (dto == null)
                return BadRequest();

            var product = _productService.Update(id, dto);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // DELETE /api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _productService.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
```

**Endpoints explained:**
- `GET /api/products` - Get all products
- `GET /api/products/5` - Get product with ID 5
- `POST /api/products` - Create new product
- `PUT /api/products/5` - Update product ID 5
- `DELETE /api/products/5` - Delete product ID 5

---

## Step 11: Setup the Database

First, create the database in PostgreSQL:

```bash
# Connect to PostgreSQL
psql -U postgres

# Inside psql, create the database:
CREATE DATABASE dotnet_db;

# Exit
\q
```

---

## Step 12: Run the API

```bash
# Inside DotNetApi folder
dotnet run

# You'll see:
# "Now listening on: https://localhost:7000"
# "Now listening on: http://localhost:5000"
```

**The API is now running!**

---

## Step 13: Test Your API

### Option 1: Swagger UI (Easiest)

Open browser:
```
https://localhost:7000/swagger
```

- Click on `/api/products` endpoints
- Click "Try it out"
- Click "Execute"

### Option 2: Using cURL (Command Line)

```bash
# Get all products
curl https://localhost:7000/api/products

# Create a product
curl -X POST https://localhost:7000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop",
    "description": "Gaming Laptop",
    "price": 1200.00
  }'

# Get product by ID
curl https://localhost:7000/api/products/1

# Update product
curl -X PUT https://localhost:7000/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Gaming Laptop",
    "description": "High-end gaming",
    "price": 1500.00
  }'

# Delete product
curl -X DELETE https://localhost:7000/api/products/1
```

### Option 3: Check Database

```bash
psql -U postgres -d dotnet_db

SELECT * FROM "Products";

\q
```

---

## Understanding the Flow

```
1. User clicks button in React
   ↓
2. React sends: GET /api/products/1
   ↓
3. ProductsController receives request
   ↓
4. Controller asks ProductService for the product
   ↓
5. ProductService queries the database through AppDbContext
   ↓
6. ProductService maps Product to ProductResponseDto
   ↓
7. Controller returns JSON: { "id": 1, "name": "Laptop", ... }
   ↓
8. React receives JSON and displays it
```

---

## Common Errors & Fixes

### Error: "Connection to database failed"
```bash
# Check if PostgreSQL is running
pg_isready -h localhost

# If not, start it (macOS):
brew services start postgresql
```

### Error: "Migrations not applied"
```bash
# Apply migrations
dotnet ef database update
```

### Error: "CORS error" (React can't connect)
- Check React is on port 3000
- Check Program.cs has `app.UseCors("AllowAll")`

### Error: "Port 7000 already in use"
```bash
# Find what's using port 7000
lsof -i :7000

# Kill the process
kill -9 <PID>
```

---

## Summary: What You Learned

✅ Created a .NET project
✅ Set up Entity Framework Core
✅ Created a database model
✅ Added DTOs for request/response data
✅ Added a service layer between controller and database
✅ Built a REST API controller
✅ Connected to PostgreSQL
✅ Tested all CRUD operations

**Next Step:** [Frontend Setup (React)](./FRONTEND_SETUP.md)

---

## Key Concepts

**Model** = What we store (Product with Name, Price, etc.)
**DTO** = What the API accepts or returns
**Service** = Business/data logic between controller and database
**Controller** = HTTP endpoint layer (GET, POST, PUT, DELETE)
**DbContext** = Connection to database
**Migration** = Instructions for database changes
**REST API** = Standard way to communicate over HTTP
