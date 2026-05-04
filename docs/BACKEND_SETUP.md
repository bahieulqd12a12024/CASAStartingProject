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
- **1 Controller**: Handle GET, POST, PUT, DELETE
- **1 Database**: PostgreSQL stores the data

Think of it like: Database ↔ Backend API ↔ Frontend

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
        public string Name { get; set; }         // Product name
        public string Description { get; set; } // Product details
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

## Step 6: Configure Program.cs (The Brain)

Open file: `Program.cs`

Replace all content with:

```csharp
using Microsoft.EntityFrameworkCore;
using DotNetApi.Data;

// Builder = Sets up the API
var builder = WebApplicationBuilder.CreateBuilder(args);

// Add services (things the API needs)
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", b =>
    {
        // Allow React to talk to this API
        b.WithOrigins("http://localhost:3000")
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

// Add Swagger (nice documentation UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to PostgreSQL database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Build the app
var app = builder.Build();

// Create database tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Runs migrations (creates tables)
}

// Use CORS (allow React to connect)
app.UseCors("AllowReact");

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**What each part does:**
- `AddControllers()` - Enables API routes
- `AddCors()` - Allows React (on port 3000) to connect
- `AddDbContext()` - Connects to PostgreSQL
- `db.Database.Migrate()` - Creates database tables
- `UseSwagger()` - Enables interactive API documentation

---

## Step 7: Create Migrations (Database Setup)

Migrations = Instructions for creating database tables

```bash
# Create a migration (instructions for first database setup)
dotnet ef migrations add InitialCreate

# What to expect:
# - Creates a "Migrations" folder
# - A file like "20240115_InitialCreate.cs" is created
# - This file has the SQL to create the Products table
```

---

## Step 8: Create the API Controller

Create file: `Controllers/ProductsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetApi.Data;
using DotNetApi.Models;

namespace DotNetApi.Controllers
{
    // This controller handles all /api/products requests
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // _context = our database connection
        private readonly AppDbContext _context;

        // Constructor - gets database from dependency injection
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/products
        // Gets ALL products from database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Fetch all products
            var products = await _context.Products.ToListAsync();
            return Ok(products); // Return 200 OK with products
        }

        // GET /api/products/5
        // Gets ONE product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Find product with this ID
            var product = await _context.Products.FindAsync(id);

            // If not found, return 404 Not Found
            if (product == null)
            {
                return NotFound();
            }

            // Return 200 OK with the product
            return Ok(product);
        }

        // POST /api/products
        // Creates a NEW product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            // Add product to database
            _context.Products.Add(product);

            // Save changes to database
            await _context.SaveChangesAsync();

            // Return 201 Created with the new product
            return CreatedAtAction(nameof(GetProduct), 
                new { id = product.Id }, product);
        }

        // PUT /api/products/5
        // Updates an EXISTING product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            // Check if ID matches
            if (id != product.Id)
            {
                return BadRequest(); // Return 400 Bad Request
            }

            // Mark product as modified
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                // Save changes to database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If error, check if product exists
                if (!await ProductExists(id))
                {
                    return NotFound(); // Not found
                }
                throw; // Re-throw other errors
            }

            // Return 204 No Content (success, no body needed)
            return NoContent();
        }

        // DELETE /api/products/5
        // Deletes a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Find the product
            var product = await _context.Products.FindAsync(id);

            // If not found, return 404
            if (product == null)
            {
                return NotFound();
            }

            // Remove from database
            _context.Products.Remove(product);

            // Save changes
            await _context.SaveChangesAsync();

            // Return 204 No Content
            return NoContent();
        }

        // Helper method: Check if product exists
        private async Task<bool> ProductExists(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
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

## Step 9: Setup the Database

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

## Step 10: Run the API

```bash
# Inside DotNetApi folder
dotnet run

# You'll see:
# "Now listening on: https://localhost:7000"
# "Now listening on: http://localhost:5000"
```

**The API is now running!**

---

## Step 11: Test Your API

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
    "id": 1,
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
4. Controller queries database: SELECT * FROM Products WHERE Id = 1;
   ↓
5. Database returns the product
   ↓
6. Controller returns JSON: { "id": 1, "name": "Laptop", ... }
   ↓
7. React receives JSON and displays it
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
- Check Program.cs has `.WithOrigins("http://localhost:3000")`

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
✅ Built a REST API controller
✅ Connected to PostgreSQL
✅ Tested all CRUD operations

**Next Step:** [Frontend Setup (React)](./FRONTEND_SETUP.md)

---

## Key Concepts

**Model** = What we store (Product with Name, Price, etc.)
**Controller** = How we access it (GET, POST, PUT, DELETE)
**DbContext** = Connection to database
**Migration** = Instructions for database changes
**REST API** = Standard way to communicate over HTTP
