# Database Setup - Neon.tech PostgreSQL (Beginner-Friendly)

This guide teaches you how to set up a cloud PostgreSQL database using Neon.tech.

## Prerequisites

- **Neon.tech account**: Free cloud PostgreSQL service
- **psql** (optional): Command-line tool for database access

**No local PostgreSQL installation needed!**

---

## What We're Building

A simple database with:
- **1 Database**: `dotnet_db` (stores everything)
- **1 Table**: `Products` (stores product data)
- **3 Columns**: Id, Name, Price, Description

---

## Step 1: Create Neon.tech Account

1. **Go to**: https://neon.tech/
2. **Sign up** with GitHub, Google, or email (free)
3. **Create your first project**:
   - Project name: `my-dotnet-app`
   - Region: Choose closest to you (e.g., `us-east-1`)
   - PostgreSQL version: `15` (latest stable)

**Free tier includes:**
- 512 MB storage
- 100 hours compute time/month
- Perfect for learning!

---

## Step 2: Get Your Connection String

After creating your project:

1. **Go to Dashboard** → **Connection Details**
2. **Copy the connection string** (it looks like this):
   ```
   postgresql://username:password@hostname/database?sslmode=require
   ```

**Example:**
```
postgresql://john.doe:abc123def456@ep-cool-mode-123456.us-east-1.aws.neon.tech/neondb?sslmode=require
```

---

## Step 3: Update Your .NET Backend

### Update appsettings.json

Replace the SQLite connection string with your Neon connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=ep-cool-mode-123456.us-east-1.aws.neon.tech;Port=5432;Database=neondb;User Id=john.doe;Password=abc123def456;SSL Mode=Require;Trust Server Certificate=true;"
  }
}
```

### Update DotNetApi.csproj

Change back to PostgreSQL package:

```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
```

### Update Program.cs

Change the database provider back to PostgreSQL:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## Step 4: Understanding the Connection String

Your Neon connection string contains:

**Example:**
```
Server=ep-cool-mode-123456.us-east-1.aws.neon.tech;Port=5432;Database=neondb;User Id=john.doe;Password=abc123def456;SSL Mode=Require;Trust Server Certificate=true;
```

**Each part means:**
- `Server=ep-cool-mode-...` - Your unique Neon database URL
- `Port=5432` - PostgreSQL standard port
- `Database=neondb` - Your database name (created automatically)
- `User Id=john.doe` - Your Neon username
- `Password=abc123def456` - Your Neon password
- `SSL Mode=Require` - Required for cloud databases (security)
- `Trust Server Certificate=true` - Accepts Neon's SSL certificate

---

## Step 5: The Products Table

When you run `dotnet ef database update`, Entity Framework Core automatically creates this table in your Neon database:

```sql
CREATE TABLE "Products" (
    "Id" SERIAL PRIMARY KEY,           -- Unique ID, auto-generated
    "Name" VARCHAR(255) NOT NULL,      -- Product name (required)
    "Description" TEXT,                -- Product details
    "Price" NUMERIC(18, 2) NOT NULL    -- Product cost
);
```

**What each column does:**
- `Id` (PRIMARY KEY) - Unique number for each product (1, 2, 3...)
- `Name` - What you call the product (max 255 characters)
- `Description` - Details about the product
- `Price` - How much it costs (with 2 decimal places)

---

## Step 6: View Your Table

### Option 1: Neon Web Interface (Easiest)

1. **Go to your Neon Dashboard**
2. **Click "SQL Editor"** in the left sidebar
3. **Run this query**:
   ```sql
   SELECT * FROM "Products";
   ```
4. **See your data** in a nice table format

### Option 2: Command Line (psql)

If you have psql installed locally:

```bash
# Connect using your Neon connection string
psql "postgresql://john.doe:abc123def456@ep-cool-mode-123456.us-east-1.aws.neon.tech/neondb?sslmode=require"

# List all tables
\dt

# View table structure
\d "Products"

# View all products
SELECT * FROM "Products";

# Exit
\q
```

---

## Step 7: Add Sample Data (Optional)

### Using Neon SQL Editor:

Go to **Dashboard** → **SQL Editor** and run:

```sql
INSERT INTO "Products" ("Name", "Description", "Price") VALUES
  ('Laptop', 'High-performance gaming laptop', 1299.99),
  ('Mouse', 'Wireless USB mouse', 29.99),
  ('Keyboard', 'Mechanical keyboard', 149.99),
  ('Monitor', '27-inch 4K display', 399.99);
```

### Using Command Line:

```bash
# Connect to your database
psql "postgresql://john.doe:abc123def456@ep-cool-mode-123456.us-east-1.aws.neon.tech/neondb?sslmode=require"

# Add products
INSERT INTO "Products" ("Name", "Description", "Price") VALUES
  ('Laptop', 'High-performance gaming laptop', 1299.99),
  ('Mouse', 'Wireless USB mouse', 29.99),
  ('Keyboard', 'Mechanical keyboard', 149.99),
  ('Monitor', '27-inch 4K display', 399.99);

# View what you added
SELECT * FROM "Products";

# Exit
\q
```

---

## Step 8: Common Database Commands

### View Data (Using Neon SQL Editor)

```sql
-- See all products
SELECT * FROM "Products";

-- See product names and prices only
SELECT "Name", "Price" FROM "Products";

-- See products under $100
SELECT * FROM "Products" WHERE "Price" < 100;

-- Count how many products
SELECT COUNT(*) FROM "Products";
```

### Modify Data

```sql
-- Update a product price
UPDATE "Products" SET "Price" = 1499.99 WHERE "Id" = 1;

-- Delete a product
DELETE FROM "Products" WHERE "Id" = 4;

-- Delete all products (careful!)
DELETE FROM "Products";
```

### Advanced Queries

```sql
-- Find expensive products
SELECT * FROM "Products" WHERE "Price" > 500;

-- Search by name
SELECT * FROM "Products" WHERE "Name" ILIKE '%laptop%';

-- Get average price
SELECT AVG("Price") as average_price FROM "Products";

-- Group by price ranges
SELECT
  CASE
    WHEN "Price" < 50 THEN 'Under $50'
    WHEN "Price" < 200 THEN '$50-$199'
    ELSE 'Over $200'
  END as price_range,
  COUNT(*) as product_count
FROM "Products"
GROUP BY price_range;
```

---

## Step 9: Troubleshooting

### "Connection timeout" or "SSL connection error"
- **Check your connection string** - make sure it's copied exactly from Neon
- **Verify SSL settings** - Neon requires `SSL Mode=Require`
- **Check your firewall** - make sure port 5432 is not blocked

### "Authentication failed"
- **Double-check username/password** from Neon dashboard
- **Make sure you're using the right database name** (usually `neondb`)

### "Table doesn't exist"
- **Run your .NET app first** - Entity Framework creates tables automatically
- **Check database name** in connection string

### "Quota exceeded"
- **Free tier limits**: 512MB storage, 100 compute hours/month
- **Upgrade plan** or clean up old data

### Can't connect from command line?
- **Install psql** if needed: `brew install postgresql` (just the client)
- **Use full connection string** with quotes

---

## Step 10: Neon Web Interface Features

Neon provides an excellent web interface:

### SQL Editor
- **Write and run queries** directly in browser
- **Syntax highlighting** and auto-complete
- **Query history** and saved queries
- **Export results** to CSV/JSON

### Dashboard
- **Real-time monitoring** of database usage
- **Compute time tracking** (free tier limit)
- **Storage usage** graphs
- **Connection details** and management

### Database Browser
- **Visual table viewer** - see your data without SQL
- **Table structure** - view columns and types
- **Import/Export** data easily

### Branching (Advanced Feature)
- **Create database branches** like Git branches
- **Test changes safely** without affecting production
- **Instant provisioning** - new database in seconds

---

## Understanding the Flow

```
Neon.tech PostgreSQL (Cloud Database)
        ↑
        │ Secure SSL Connection
        │ (Internet)
        ↓
    DotNetApi (.NET Backend)
        ↑
        │ API Calls
        │
    React Frontend (Your UI)


When you add a product in React:
1. React sends: POST /api/products with {name, price...}
2. Backend connects to Neon via SSL
3. Neon PostgreSQL stores it in Products table
4. Backend returns the new product
5. React displays it in the table
```

---

## Database Connection Test

To verify everything works:

### Using Neon SQL Editor:
```sql
SELECT COUNT(*) as product_count FROM "Products";
```

**Should return:**
```
product_count
--------------
0
```

### Using .NET Backend:
```bash
cd backend/DotNetApi
dotnet run
```

Visit `http://localhost:5106/swagger` and test the API!

---

## Key Concepts

| Term | Meaning |
|------|---------|
| **Database** | Container for all data (neondb) |
| **Table** | Like a spreadsheet with rows and columns (Products) |
| **Column** | A property/field (Name, Price, Id) |
| **Row** | One record (one product) |
| **Primary Key** | Unique ID for each row (Id) |
| **PostgreSQL** | The database system (hosted by Neon) |
| **SSL** | Secure connection encryption (required for cloud) |
| **Connection Pooling** | Neon's automatic connection management |

---

## Neon.tech Advantages

✅ **No local setup** - works immediately
✅ **Always available** - managed by Neon
✅ **Auto-scaling** - handles traffic spikes
✅ **Free tier** - perfect for learning
✅ **Modern features** - branching, monitoring
✅ **Secure** - SSL encryption built-in
✅ **Fast** - Global CDN-like performance

---

## Summary

✅ Created Neon.tech account
✅ Set up cloud PostgreSQL database
✅ Configured .NET backend connection
✅ Understand connection strings
✅ Know how to view/add data
✅ Learned SQL commands
✅ Can troubleshoot connection issues

---

**Next Step**: [Backend Setup](./BACKEND_SETUP.md)

---

## Quick Reference

**Neon Dashboard**: https://console.neon.tech/
**Free Tier Limits**: 512MB storage, 100 compute hours/month
**Connection Format**: `postgresql://user:pass@host/db?sslmode=require`
**.NET Format**: `Server=host;Port=5432;Database=db;User Id=user;Password=pass;SSL Mode=Require;Trust Server Certificate=true;`

---

**Need Help?**
- Neon Documentation: https://neon.tech/docs/
- Community Forum: https://community.neon.tech/
