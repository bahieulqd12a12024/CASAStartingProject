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
        public string Name { get; set; }
        public string Description { get; set; }
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

var builder = WebApplicationBuilder.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", b =>
    {
        b.WithOrigins("http://localhost:3000")
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowReact");

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

**Next**: [Full Tutorials](../../SETUP_GUIDE.md)

## Prerequisites Check

Before starting, ensure you have installed:

```bash
# Check versions
node --version          # Should be 18+
npm --version           # Should be 8+
dotnet --version        # Should be 8.0+
psql --version          # Should be 13+
```

---

## Step 1: Setup Database (5 minutes)

### Start PostgreSQL
```bash
# macOS
brew services start postgresql

# Windows
net start postgresql

# Linux
sudo systemctl start postgresql
```

### Create Database
```bash
psql -U postgres

-- Inside psql:
CREATE DATABASE net_app_db;
CREATE USER app_user WITH PASSWORD 'secure_password_123';
ALTER ROLE app_user SET client_encoding TO 'utf8';
ALTER ROLE app_user SET default_transaction_isolation TO 'read committed';
GRANT ALL PRIVILEGES ON DATABASE net_app_db TO app_user;
\q
```

---

## Step 2: Setup Backend (10 minutes)

### Create .NET Project
```bash
cd backend
dotnet new webapi -n DotNetApi -f net8.0
cd DotNetApi
```

### Add NuGet Packages
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Copy Configuration Files

From the documentation:
- Copy `Program.cs` configuration
- Copy `appsettings.json` with your connection string
- Copy all files from `Models/`, `Data/`, and `Controllers/` directories

### Run Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Verify Backend Works
```bash
dotnet run
# Visit: https://localhost:7000/swagger
```

---

## Step 3: Setup Frontend (10 minutes)

### Create React App
```bash
cd frontend
npx create-react-app react-app
cd react-app
npm install axios react-router-dom
```

### Environment Configuration
Create `.env.local`:
```
REACT_APP_API_BASE_URL=http://localhost:7000/api
```

### Copy React Files

From the documentation:
- Copy `src/services/api.js`
- Copy `src/hooks/` directory
- Copy `src/components/` directory
- Copy updated `src/App.js` and `src/App.css`

### Start Frontend
```bash
npm start
# Opens at http://localhost:3000
```

---

## Step 4: Test the Application (5 minutes)

### Add Sample Employee
1. Open React app at `http://localhost:3000`
2. Fill in the "Add New Employee" form
3. Select a department
4. Click "Add Employee"

### Verify in Database
```bash
psql -U app_user -d net_app_db

SELECT * FROM "Employees";
SELECT * FROM "Departments";

\q
```

### Check API Directly
```bash
# Get all employees
curl http://localhost:7000/api/employees

# Get all departments
curl http://localhost:7000/api/departments
```

---

## Troubleshooting

### React App Won't Connect to Backend
- ✓ Check backend is running: `dotnet run`
- ✓ Verify CORS is configured in `Program.cs`
- ✓ Check `REACT_APP_API_BASE_URL` in `.env.local`
- ✓ Restart React dev server: Ctrl+C, then `npm start`

### Database Connection Failed
- ✓ PostgreSQL running? `psql --version`
- ✓ Connection string correct in `appsettings.json`?
- ✓ Database exists? `psql -U postgres -l`
- ✓ User has permissions? Run GRANT commands again

### Network Request Errors
```javascript
// Check network tab in browser DevTools (F12)
// Common issues:
// - CORS error: Update origin in Program.cs
// - 404 Not Found: Check API route names
// - 500 Server Error: Check .NET terminal for error messages
```

### Port Already in Use
```bash
# Port 3000 (React)
lsof -i :3000
kill -9 <PID>

# Port 7000 (.NET)
lsof -i :7000
kill -9 <PID>

# Port 5432 (PostgreSQL)
lsof -i :5432
kill -9 <PID>
```

---

## Project Structure After Setup

```
project-root/
├── backend/
│   └── DotNetApi/
│       ├── Controllers/
│       │   ├── EmployeesController.cs
│       │   └── DepartmentsController.cs
│       ├── Models/
│       │   ├── Employee.cs
│       │   └── Department.cs
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Migrations/
│       ├── Program.cs
│       ├── appsettings.json
│       └── DotNetApi.csproj
│
├── frontend/
│   └── react-app/
│       ├── public/
│       ├── src/
│       │   ├── components/
│       │   │   ├── EmployeeList.js
│       │   │   ├── EmployeeForm.js
│       │   │   └── *.css
│       │   ├── services/
│       │   │   └── api.js
│       │   ├── hooks/
│       │   │   ├── useEmployees.js
│       │   │   └── useDepartments.js
│       │   ├── App.js
│       │   ├── App.css
│       │   └── index.js
│       ├── .env.local
│       ├── package.json
│       └── package-lock.json
│
└── docs/
    ├── BACKEND_SETUP.md
    ├── FRONTEND_SETUP.md
    ├── DATABASE_SETUP.md
    ├── API_DOCUMENTATION.md
    ├── DATABASE_SCHEMA.md
    └── EXAMPLE_FEATURES.md
```

---

## Running the Full Application

### Terminal 1: Start Backend
```bash
cd backend/DotNetApi
dotnet run
# Runs on https://localhost:7000
```

### Terminal 2: Start Frontend
```bash
cd frontend/react-app
npm start
# Opens browser at http://localhost:3000
```

### Terminal 3: Optional - Monitor Database
```bash
psql -U app_user -d net_app_db
```

---

## Key Endpoints

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/employees` | Get all employees |
| GET | `/api/employees/{id}` | Get employee by ID |
| POST | `/api/employees` | Create new employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee |
| GET | `/api/departments` | Get all departments |
| GET | `/api/departments/{id}` | Get department by ID |
| POST | `/api/departments` | Create department |
| PUT | `/api/departments/{id}` | Update department |
| DELETE | `/api/departments/{id}` | Delete department |

---

## Next Steps

1. **Add Features**: See [EXAMPLE_FEATURES.md](./EXAMPLE_FEATURES.md)
   - Search and filtering
   - Pagination
   - Salary statistics
   - CSV export
   - Authentication

2. **Improve Performance**
   - Add caching
   - Implement query optimization
   - Use async/await throughout

3. **Deploy**
   - Azure App Service (backend)
   - Azure Static Web Apps (frontend)
   - Azure Database for PostgreSQL

4. **Testing**
   - Unit tests (xUnit for .NET, Jest for React)
   - Integration tests
   - E2E tests

---

## Useful Commands

### Backend (.NET)
```bash
# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Scaffold from database
dotnet ef dbcontext scaffold

# Watch mode (auto-rebuild)
dotnet watch run
```

### Frontend (React)
```bash
# Install dependencies
npm install

# Start dev server
npm start

# Build for production
npm run build

# Run tests
npm test

# Eject configuration
npm run eject
```

### Database (PostgreSQL)
```bash
# Backup database
pg_dump -U app_user -d net_app_db > backup.sql

# Restore database
psql -U app_user -d net_app_db < backup.sql

# Connect to database
psql -U app_user -d net_app_db

# Drop database
dropdb -U app_user net_app_db

# Create database
createdb -U app_user net_app_db
```

---

## Helpful Links

- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [React Documentation](https://react.dev/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Axios Documentation](https://axios-http.com/docs/intro)
- [Create React App](https://create-react-app.dev/)

---

[Back to Setup Guide](../SETUP_GUIDE.md)
