# React + .NET 8 + PostgreSQL - Beginner-Friendly Full-Stack Template

A **simple, easy-to-learn** template for building full-stack applications with React frontend, .NET 8 backend, and PostgreSQL database.

> **Designed for learning!** Each step is explained clearly with minimal complexity.

---

## 🎯 What You'll Build

A simple product store application:
- ✅ Add new products (name, description, price)
- ✅ View all products in a table
- ✅ Delete products
- ✅ See changes in real-time

**By the end, you'll understand:**
- How APIs work
- How databases store data
- How React talks to a backend
- How to build a full-stack application

---

## 📚 Documentation

Start here - **choose your learning style:**

### 🚀 Fast Track (30 minutes)
Perfect if you want to see it working immediately:
- [QUICK_START.md](./docs/QUICK_START.md) - Copy-paste and run

### 📖 Step-by-Step (Recommended for Learning)
Perfect if you want to understand each part:

1. **[DATABASE_SETUP.md](./docs/DATABASE_SETUP.md)** (5 min)
   - Set up PostgreSQL
   - Learn basic SQL
   - Understand databases

2. **[BACKEND_SETUP.md](./docs/BACKEND_SETUP.md)** (10 min)
   - Build .NET 8 API
   - Create models and controllers
   - Understand REST APIs

3. **[FRONTEND_SETUP.md](./docs/FRONTEND_SETUP.md)** (10 min)
   - Build React app
   - Connect to API
   - Understand React hooks

4. **[API_DOCUMENTATION.md](./docs/API_DOCUMENTATION.md)** (Reference)
   - All endpoints explained
   - How to test them

5. **[DATABASE_SCHEMA.md](./docs/DATABASE_SCHEMA.md)** (Reference)
   - Database structure
   - SQL queries
   - How data is organized

---

## 🏗️ Project Structure

Simple and clean:

```
project/
├── backend/
│   └── DotNetApi/              # .NET 8 API
│       ├── Models/              # Data structure
│       ├── Controllers/         # API endpoints  
│       ├── Data/                # Database connection
│       └── Program.cs           # Main configuration
│
├── frontend/
│   └── react-app/              # React UI
│       ├── src/
│       │   ├── components/      # UI components
│       │   ├── services/        # API calls
│       │   ├── hooks/           # React logic
│       │   └── App.js           # Main app
│       └── .env.local           # Configuration
│
└── docs/
    ├── QUICK_START.md
    ├── BACKEND_SETUP.md
    ├── FRONTEND_SETUP.md
    ├── DATABASE_SETUP.md
    ├── API_DOCUMENTATION.md
    └── DATABASE_SCHEMA.md
```

---

## 🔑 What's Different (Beginner-Friendly Version)

| Feature | Beginner Version | Advanced Version |
|---------|------------------|------------------|
| **Database Model** | 1 table (Products) | Multiple related tables |
| **Complexity** | Simple CRUD | Complex relationships |
| **Explanation** | Very detailed | Brief |
| **Code Comments** | Extensive | Minimal |
| **Learning Curve** | Gentle | Steep |

---

## 📊 Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Frontend** | React | 18.0+ |
| **Backend** | ASP.NET Core | 8.0 |
| **Database** | PostgreSQL | 13.0+ |
| **ORM** | Entity Framework | 8.0 |
| **HTTP Client** | Axios | 1.4+ |

---

## ⚡ Quick Start

Get running in **30 minutes**:

```bash
# Terminal 1: Database
psql -U postgres
CREATE DATABASE dotnet_db;
\q

# Terminal 2: Backend
cd backend/DotNetApi
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
# (Copy files from BACKEND_SETUP.md)
dotnet run

# Terminal 3: Frontend
cd frontend/react-app
npm install axios
# (Copy files from FRONTEND_SETUP.md)
npm start
```

Then open: `http://localhost:3000`

---

## 📖 Learning Path

**Complete for beginners:**

1. **Understand Databases** (15 min)
   - Read DATABASE_SETUP.md
   - Run some SQL queries
   - See how data is stored

2. **Understand APIs** (20 min)
   - Read BACKEND_SETUP.md
   - Understand REST (GET, POST, PUT, DELETE)
   - Test endpoints in Swagger

3. **Understand React** (20 min)
   - Read FRONTEND_SETUP.md
   - Learn hooks (useState, useEffect)
   - See components in action

4. **Build Something** (30 min)
   - Follow QUICK_START.md
   - Create your first full-stack app
   - Test everything works

5. **Extend It** (ongoing)
   - Add features from EXAMPLE_FEATURES.md
   - Experiment with code
   - Break things and fix them

---

## 🎓 Concepts You'll Learn

### Database Concepts
- Tables and rows
- Primary keys
- SQL queries
- Data types
- Relationships

### API Concepts
- HTTP methods (GET, POST, PUT, DELETE)
- Request and response
- Status codes
- JSON format
- CORS

### React Concepts
- Components
- Hooks (useState, useEffect)
- Props
- State management
- Reusable functions

### Full-Stack Concepts
- Frontend ↔ Backend communication
- Database normalization
- Error handling
- Testing
- Debugging

---

## 🚀 APIs Explained Simply

Your app has these endpoints:

```
GET    /api/products       → Get all products
GET    /api/products/1     → Get product #1
POST   /api/products       → Create new product
PUT    /api/products/1     → Update product #1
DELETE /api/products/1     → Delete product #1
```

Each one does something different:
- **GET**: Retrieve data
- **POST**: Send new data
- **PUT**: Change existing data
- **DELETE**: Remove data

---

## 🧪 Testing

### Visual (Easiest)
1. Backend: `https://localhost:7000/swagger`
2. React: `http://localhost:3000`
3. Manual test by filling forms

### Command Line
```bash
# Get all products
curl https://localhost:7000/api/products

# Create product
curl -X POST https://localhost:7000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Laptop","price":1299.99}'
```

### Database Check
```bash
psql -U postgres -d dotnet_db
SELECT * FROM "Products";
\q
```

---

## 🐛 Troubleshooting

### "Cannot connect to database"
```bash
# Start PostgreSQL
brew services start postgresql  # macOS
sudo systemctl start postgresql # Linux

# Verify
pg_isready -h localhost
```

### "Cannot connect to backend"
```bash
# Check it's running
curl http://localhost:7000/swagger

# Check firewall isn't blocking port 7000
```

### "React shows error"
1. Check browser console (F12)
2. Check backend is running
3. Check `.env.local` is correct
4. Restart React dev server

---

## 📚 Resources

### Official Docs
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### Tools
- [Swagger UI](https://swagger.io/) - Test API
- [pgAdmin](https://www.pgadmin.org/) - Database GUI
- [Postman](https://www.postman.com/) - API testing
- [VS Code](https://code.visualstudio.com/) - Editor

---

## 🚦 Getting Started NOW

Choose one:

### Option A: I want to see it work immediately
→ Follow [QUICK_START.md](./docs/QUICK_START.md) (30 min)

### Option B: I want to understand everything
→ Follow step-by-step docs (1-2 hours)

### Option C: I want to learn specific parts
→ Jump to section:
- Database: [DATABASE_SETUP.md](./docs/DATABASE_SETUP.md)
- Backend: [BACKEND_SETUP.md](./docs/BACKEND_SETUP.md)
- Frontend: [FRONTEND_SETUP.md](./docs/FRONTEND_SETUP.md)

---

## 💡 Key Principles

This template follows these principles:

✅ **Simple** - No unnecessary complexity
✅ **Explained** - Every step is commented
✅ **Beginner-friendly** - Assumes no prior experience
✅ **Practical** - Learning by doing
✅ **Extensible** - Easy to add features

---

## 🎯 Learning Outcomes

After working through this template, you'll be able to:

- ✅ Set up PostgreSQL database
- ✅ Create .NET API endpoints
- ✅ Write basic SQL queries
- ✅ Build React components
- ✅ Connect frontend to backend
- ✅ Understand CRUD operations
- ✅ Debug full-stack applications
- ✅ Deploy a full-stack app

---

## 📝 Files to Create

**Backend (.NET):**
- Models/Product.cs
- Data/AppDbContext.cs
- Controllers/ProductsController.cs
- Program.cs
- appsettings.json

**Frontend (React):**
- services/api.js
- hooks/useProducts.js
- components/ProductList.js
- components/ProductForm.js
- App.js

All provided in documentation!

---

## 🤝 Tips for Success

1. **Read carefully** - Documentation explains WHY, not just WHAT
2. **Type code** - Don't copy-paste, type it to learn
3. **Experiment** - Change things and see what breaks
4. **Use browser DevTools** - F12 to see network requests
5. **Check backend logs** - See what's happening
6. **Ask questions** - Each doc has examples

---

## 🎓 Next After This

Once you understand this template:

1. **Add authentication** - Login/logout
2. **Add more models** - Multiple tables
3. **Add relationships** - Connect tables
4. **Add validation** - Check data
5. **Add error handling** - Handle failures
6. **Deploy** - Put on internet

See [EXAMPLE_FEATURES.md](./docs/EXAMPLE_FEATURES.md) for ideas.

---

## 📞 Quick Reference

### Start Everything
```bash
# Terminal 1
psql -U postgres
CREATE DATABASE dotnet_db;
\q

# Terminal 2
cd backend/DotNetApi && dotnet run

# Terminal 3
cd frontend/react-app && npm start
```

### Check Everything
```bash
# Database
psql -U postgres -d dotnet_db -c "SELECT count(*) FROM \"Products\";"

# Backend
curl https://localhost:7000/swagger

# Frontend
http://localhost:3000
```

---

## 🏆 Success Checklist

- [ ] PostgreSQL installed and running
- [ ] Database `dotnet_db` created
- [ ] Backend runs on port 7000
- [ ] Frontend runs on port 3000
- [ ] Can add product in UI
- [ ] Product appears in table
- [ ] Can delete product
- [ ] Product is removed from table
- [ ] Can see data in database

---

**Ready?** Start with [QUICK_START.md](./docs/QUICK_START.md) or [DATABASE_SETUP.md](./docs/DATABASE_SETUP.md)

**Questions?** Check the relevant documentation file.

**Stuck?** See troubleshooting section.

---

## 📄 License

This template is provided for educational and commercial use.

---

**Made for learning. Happy coding! 🚀**
# CASAStartingProject
