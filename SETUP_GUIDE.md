# React + .NET 8 + PostgreSQL - Beginner-Friendly Setup Guide

A **simplified** template for learning full-stack development, step-by-step.

## 📚 Documentation Structure

This guide includes comprehensive documentation designed for **easy learning**:

### 🚀 Start Here (Pick One)

**Option A: Fast Track (30 minutes)**
- Just want to see it work? → [QUICK_START.md](./docs/QUICK_START.md)
- Includes copy-paste code and simple instructions

**Option B: Learn Step-by-Step (90 minutes)**
- Want to understand everything? → Follow docs in order

### 📖 Detailed Setup Guides (In Order)

1. **[DATABASE_SETUP.md](./docs/DATABASE_SETUP.md)** - PostgreSQL Setup (5 min)
   - Install and start PostgreSQL
   - Create database
   - Learn basic SQL
   - Understand databases

2. **[BACKEND_SETUP.md](./docs/BACKEND_SETUP.md)** - .NET 8 API (10 min)
   - Create .NET project
   - One simple model: Product
   - DTOs for create, update, and response data
   - Product service layer between controller and database
   - Build REST API (GET, POST, PUT, DELETE)
   - Connect to PostgreSQL
   - Test with Swagger UI

3. **[FRONTEND_SETUP.md](./docs/FRONTEND_SETUP.md)** - React App (10 min)
   - Create React app
   - Build components
   - Call API from React
   - See data in real-time

4. **[API_DOCUMENTATION.md](./docs/API_DOCUMENTATION.md)** - Reference
   - All endpoints explained
   - Example requests and responses
   - How to test

5. **[DATABASE_SCHEMA.md](./docs/DATABASE_SCHEMA.md)** - Reference
   - Table structure
   - SQL queries
   - Database concepts

---

## 🎯 What You're Building

A **Product Store** with:
- ✅ Database to store products
- ✅ Backend API to manage products
- ✅ React UI to add/view/delete products
- ✅ Everything connected and working

**By the end, you'll understand:**
- How databases work
- How APIs work
- How React talks to a backend
- How to build a complete full-stack app

---

## 🏗️ Project Structure

```
web-app/
├── backend/
│   └── DotNetApi/
│       ├── Models/
│       │   └── Product.cs              # Database Entity
│       ├── DTOs/
│       │   ├── CreateProductDto.cs     # POST Request Shape
│       │   ├── UpdateProductDto.cs     # PUT Request Shape
│       │   └── ProductResponseDto.cs   # API Response Shape
│       ├── Services/
│       │   ├── IProductService.cs      # Service Contract
│       │   └── ProductService.cs       # Product Logic + Mapping
│       ├── Data/
│       │   └── AppDbContext.cs         # Database Connection
│       ├── Controllers/
│       │   └── ProductsController.cs   # HTTP Endpoints
│       ├── Migrations/                 # Database Changes
│       ├── Program.cs                  # Configuration
│       ├── appsettings.json           # Connection String
│       └── DotNetApi.csproj
│
├── frontend/
│   └── react-app/
│       ├── public/
│       ├── src/
│       │   ├── components/
│       │   │   ├── ProductList.js     # Display Products
│       │   │   ├── ProductList.css
│       │   │   ├── ProductForm.js     # Add Products
│       │   │   ├── ProductForm.css
│       │   │   └── index.js
│       │   ├── services/
│       │   │   └── api.js             # API Client
│       │   ├── hooks/
│       │   │   └── useProducts.js     # Manage Products
│       │   ├── App.js
│       │   ├── App.css
│       │   └── index.js
│       ├── .env.local
│       ├── package.json
│       └── public/
│
└── docs/
    ├── QUICK_START.md
    ├── DATABASE_SETUP.md
    ├── BACKEND_SETUP.md
    ├── FRONTEND_SETUP.md
    ├── API_DOCUMENTATION.md
    └── DATABASE_SCHEMA.md
```

---

## 🔑 Key Differences (Simplified for Learning)

**Why we simplified:**

| Old Version | New Version | Why? |
|------------|------------|------|
| Multiple related models | 1 model (`Product`) | Easier to understand the first CRUD flow |
| Controller talks directly to DbContext | Controller uses ProductService | Cleaner beginner-friendly layering |
| API accepts database entities | API accepts DTOs | Safer request/response boundaries |
| Complex relationships | Single table | Focus on fundamentals |
| Long code examples | Shorter, simpler code | Less overwhelming |
| Brief explanations | Detailed explanations | Learn the WHY |

**Result:** You learn faster, understand better, can build confidently.

---

## 📊 Technology Stack

| Part | Technology | Purpose |
|------|-----------|---------|
| **Database** | PostgreSQL 13+ | Store product data |
| **Backend** | .NET 8 | Create REST API |
| **Backend ORM** | Entity Framework Core | Talk to database |
| **Frontend** | React 18 | User interface |
| **Frontend HTTP** | Axios | Call API from React |

---

## ⚡ Quick Commands

### Database
```bash
# Create database
psql -U postgres
CREATE DATABASE dotnet_db;
\q

# View data
psql -U postgres -d dotnet_db
SELECT * FROM "Products";
\q
```

### Backend
```bash
cd backend/DotNetApi

# Add packages
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Create database tables
dotnet ef database update

# Run API
dotnet run
# Opens at: https://localhost:7000
```

### Frontend
```bash
cd frontend/react-app

# Install dependencies
npm install

# Start dev server
npm start
# Opens at: http://localhost:3000
```

---

## 📋 Learning Steps

**Recommended approach:**

1. **Read DATABASE_SETUP.md** (10 min)
   - Understand what a database is
   - Create PostgreSQL database
   - Learn basic SQL commands

2. **Read BACKEND_SETUP.md** (20 min)
   - Understand what an API is
   - Create first .NET project
   - Build API endpoints
   - Test in Swagger

3. **Read FRONTEND_SETUP.md** (20 min)
   - Understand React components
   - Create UI components
   - Call API from React
   - See everything work together

4. **Test Everything** (10 min)
   - Add product in UI
   - See it appear in table
   - Check database
   - Delete product

5. **Extend** (optional)
   - Add features from EXAMPLE_FEATURES.md
   - Experiment with code
   - Build confidence

---

## 🚀 Running Everything

**Terminal 1: Database**
```bash
psql -U postgres
CREATE DATABASE dotnet_db;
\q
```

**Terminal 2: Backend**
```bash
cd backend/DotNetApi
dotnet run
```

**Terminal 3: Frontend**
```bash
cd frontend/react-app
npm start
```

Then open: `http://localhost:3000`

---

## 📖 Examples

### Adding a Product (Via UI)
1. Fill form: Name, Description, Price
2. Click "Add Product"
3. See it in the table
4. Product saved to database

### Checking Database
```bash
psql -U postgres -d dotnet_db
SELECT * FROM "Products";
```

### Testing API
```bash
# Get all products
curl https://localhost:7000/api/products

# Add product
curl -X POST https://localhost:7000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Laptop","description":"Gaming","price":1299.99}'
```

---

## 🎓 Concepts You'll Learn

### Database
- What is a table?
- What is a row?
- What is a column?
- Primary keys
- Simple SQL queries

### API  
- REST (GET, POST, PUT, DELETE)
- Endpoints
- Request/Response
- Status codes
- CORS

### React
- Components
- Hooks (useState, useEffect)
- Props
- State management
- API calls

### Full-Stack
- Frontend ↔ Backend communication
- How data flows
- Debugging
- Testing

---

## 🐛 Common Issues

### "Cannot connect to database"
→ See [DATABASE_SETUP.md](./docs/DATABASE_SETUP.md) Troubleshooting

### "API won't start"
→ See [BACKEND_SETUP.md](./docs/BACKEND_SETUP.md) Troubleshooting

### "React shows errors"
→ See [FRONTEND_SETUP.md](./docs/FRONTEND_SETUP.md) Troubleshooting

---

## ✅ Success Indicators

✓ Database created and connected
✓ Backend API running on port 7000
✓ Frontend running on port 3000
✓ Can add products via UI
✓ Products appear in table
✓ Can delete products
✓ Products removed from table and database

---

## 📚 Next Steps

Once comfortable with this template:

1. **Add validation** - Check input
2. **Add error handling** - Handle failures
3. **Add more models** - Multiple tables
4. **Add relationships** - Connect tables
5. **Add authentication** - Login/logout
6. **Deploy** - Put on internet

See [EXAMPLE_FEATURES.md](./docs/EXAMPLE_FEATURES.md) for examples.

---

## 🎯 Quick Navigation

- **Start Getting Running**: [QUICK_START.md](./docs/QUICK_START.md)
- **Database Questions**: [DATABASE_SETUP.md](./docs/DATABASE_SETUP.md)
- **Backend Questions**: [BACKEND_SETUP.md](./docs/BACKEND_SETUP.md)
- **Frontend Questions**: [FRONTEND_SETUP.md](./docs/FRONTEND_SETUP.md)
- **API Testing**: [API_DOCUMENTATION.md](./docs/API_DOCUMENTATION.md)
- **Data Structure**: [DATABASE_SCHEMA.md](./docs/DATABASE_SCHEMA.md)

---

**Ready?** Pick an option above and start learning!

**Have questions?** Each documentation file has detailed explanations and examples.
