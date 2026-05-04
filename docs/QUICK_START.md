# Quick Start Guide - 30 Minutes

Get your React + .NET 8 + PostgreSQL application running fast!

---

## What You'll Build

A simple product store where you can:
- ✓ Add new products
- ✓ View all products in a table
- ✓ Delete products

**Tech Stack:**
- **Backend**: .NET 8 API
- **Frontend**: React 18
- **Database**: PostgreSQL

---

## Prerequisites (5 minutes)

Make sure you have:

```bash
# Check Node.js
node --version          # Should be 18+

# Check npm
npm --version          # Should be 8+

# Check .NET
dotnet --version       # Should be 8.0+

# Check PostgreSQL
psql --version         # Should be 13+
```

If anything is missing, install it first.

---

## Timeline

- **5 min**: Database setup
- **8 min**: Backend setup
- **8 min**: Frontend setup
- **5 min**: Testing
- **4 min**: Troubleshooting (if needed)

---

## Step 1: Database (5 min)

### Start PostgreSQL
```bash
# macOS
brew services start postgresql

# Or Linux
sudo systemctl start postgresql
```

### Create Database
```bash
# Connect to PostgreSQL
psql -U postgres

# Inside psql, run these commands:
CREATE DATABASE dotnet_db;
\q
```

**Done!** Database is ready.

---

## Step 2: Backend (.NET) (8 min)

### Create Project
```bash
cd backend
dotnet new webapi -n DotNetApi -f net8.0
cd DotNetApi
```

### Add Packages
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Create Files

**Create: `Models/Product.cs`**
```csharp
namespace DotNetApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
```

**Create: `Data/AppDbContext.cs`**
```csharp
using Microsoft.EntityFrameworkCore;
using DotNetApi.Models;

namespace DotNetApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
```

### Update Configuration

**Update: `appsettings.json`**
```json
{
  "Logging": {"LogLevel": {"Default": "Information"}},
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=dotnet_db;User Id=postgres;Password=yourpassword;"
  },
  "AllowedHosts": "*"
}
```

Replace `yourpassword` with your PostgreSQL password!

### Update Program.cs

**Replace entire `Program.cs`:**
```csharp
using Microsoft.EntityFrameworkCore;
using DotNetApi.Data;
using DotNetApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

var app = builder.Build();

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

### Add DTOs and Service Layer

Create these backend files too:
- `DTOs/CreateProductDto.cs`
- `DTOs/UpdateProductDto.cs`
- `DTOs/ProductResponseDto.cs`
- `Services/IProductService.cs`
- `Services/ProductService.cs`

See [BACKEND_SETUP.md](./BACKEND_SETUP.md) Steps 6 and 7 for the full code. These files keep the controller clean and prevent the API from accepting or returning database entities directly.

### Create Controller

**Create: `Controllers/ProductsController.cs`**

See [BACKEND_SETUP.md](./BACKEND_SETUP.md) Step 8 for full code.

(It's long, copy it from there)

### Create Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Start Backend
```bash
dotnet run
# Should say: "Now listening on: https://localhost:7000"
```

**Done!** Backend running on port 7000.

---

## Step 3: Frontend (React) (8 min)

### Create App
```bash
cd frontend
npx create-react-app react-app
cd react-app
npm install axios
```

### Configure Environment

**Create: `.env.local`**
```
REACT_APP_API_BASE_URL=http://localhost:7000/api
```

### Create Files

**Create: `src/services/api.js`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 4 for code.

**Create: `src/hooks/useProducts.js`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 5 for code.

**Create: `src/components/ProductList.js`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 6 for code.

**Create: `src/components/ProductList.css`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 8 for code.

**Create: `src/components/ProductForm.js`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 7 for code.

**Create: `src/components/ProductForm.css`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 8 for code.

### Update App Files

**Replace: `src/App.js`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 9 for code.

**Replace: `src/App.css`**

See [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) Step 10 for code.

### Start Frontend
```bash
npm start
# Opens browser at http://localhost:3000
```

**Done!** Frontend running on port 3000.

---

## Step 4: Test Everything (5 min)

### Check Each Part Works

1. **Backend running?**
   ```bash
   curl https://localhost:7000/swagger
   # Should open page
   ```

2. **React running?**
   ```bash
   Browser at http://localhost:3000
   # Should see "Product Store" header
   ```

3. **Database connected?**
   ```bash
   psql -U postgres -d dotnet_db
   SELECT * FROM "Products";
   \q
   ```

### Test Full Flow

1. Open React app (http://localhost:3000)
2. Fill in form:
   - Name: "Laptop"
   - Description: "Gaming laptop"
   - Price: "1299.99"
3. Click "Add Product"
4. See it appear in the table!

---

## Troubleshooting

### Backend won't start
```bash
# Check .NET is installed
dotnet --version

# Check package versions
dotnet list package

# Restore packages
dotnet restore

# Try again
dotnet run
```

### React shows "Cannot GET"
- Ensure backend is running on port 7000
- Check `.env.local` has correct API URL
- Restart React dev server

### Database connection error
```bash
# Check PostgreSQL running
pg_isready -h localhost

# Start it
brew services start postgresql

# Verify database exists
psql -U postgres -l | grep dotnet_db
```

### "Port already in use" error
```bash
# Find what's using port 7000
lsof -i :7000

# Kill it (replace PID)
kill -9 <PID>
```

---

## Files Created

```
DotNetApi/
├── Models/Product.cs
├── DTOs/
│   ├── CreateProductDto.cs
│   ├── UpdateProductDto.cs
│   └── ProductResponseDto.cs
├── Services/
│   ├── IProductService.cs
│   └── ProductService.cs
├── Data/AppDbContext.cs
├── Controllers/ProductsController.cs
├── Program.cs
├── appsettings.json
└── Migrations/

react-app/
├── src/
│   ├── services/api.js
│   ├── hooks/useProducts.js
│   ├── components/
│   │   ├── ProductList.js
│   │   ├── ProductList.css
│   │   ├── ProductForm.js
│   │   └── ProductForm.css
│   ├── App.js
│   └── App.css
└── .env.local
```

---

## Running It All

**Terminal 1 (Backend):**
```bash
cd backend/DotNetApi
dotnet run
```

**Terminal 2 (Frontend):**
```bash
cd frontend/react-app
npm start
```

**Terminal 3 (Optional - Database):**
```bash
psql -U postgres -d dotnet_db
```

---

## API Endpoints

| Method | URL | Action |
|--------|-----|--------|
| GET | /api/products | Get all |
| GET | /api/products/1 | Get one |
| POST | /api/products | Create |
| PUT | /api/products/1 | Update |
| DELETE | /api/products/1 | Delete |

See [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) for details.

---

## Next Steps

1. ✅ Run the basic app
2. 📖 Read [BACKEND_SETUP.md](./BACKEND_SETUP.md) to understand how it works
3. 📖 Read [FRONTEND_SETUP.md](./FRONTEND_SETUP.md) to understand React
4. 🎯 Try [EXAMPLE_FEATURES.md](./EXAMPLE_FEATURES.md) to add more features

---

## Key Commands

```bash
# Backend
dotnet run                          # Start API
dotnet ef migrations add Name       # Create migration
dotnet ef database update           # Apply migrations
dotnet watch run                    # Auto-reload on changes

# Frontend
npm start                           # Start dev server
npm build                           # Build for production
npm test                            # Run tests

# Database
psql -U postgres -d dotnet_db       # Connect
SELECT * FROM "Products";           # View data
pg_dump -U postgres -d dotnet_db    # Backup
```

---

## Summary

✅ PostgreSQL database set up
✅ .NET backend API running
✅ React frontend running
✅ All three communicating
✅ Can add/view/delete products

**You're ready to learn!**

---

**Next**: [Full Tutorials](../SETUP_GUIDE.md)

## Helpful Links

- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [React Documentation](https://react.dev/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Axios Documentation](https://axios-http.com/docs/intro)
- [Create React App](https://create-react-app.dev/)

---

[Back to Setup Guide](../SETUP_GUIDE.md)
