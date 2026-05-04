# Database Schema - Complete Reference

An explanation of the database structure and tables.

---

## Database Overview

Our database (`dotnet_db`) stores product information in a simple table.

**Why?** Makes learning easier. No complex relationships yet.

---

## Products Table

The main table where all product data is stored.

### Table Definition

```sql
CREATE TABLE "Products" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    "Price" NUMERIC(18, 2) NOT NULL
);
```

### Columns Explained

| Column | Type | Required | Description |
|--------|------|----------|-------------|
| `Id` | SERIAL | Auto | Unique identifier, auto-incremented |
| `Name` | VARCHAR(255) | Yes | Product name, max 255 characters |
| `Description` | TEXT | No | Optional product details |
| `Price` | NUMERIC(18,2) | Yes | Price with 2 decimal places |

---

## Column Details

### Id (Primary Key)
```sql
"Id" SERIAL PRIMARY KEY
```

- **Type**: SERIAL (auto-incrementing integer)
- **Meaning**: Starts at 1, increments automatically
- **Purpose**: Unique identifier for each product
- **Example**: 1, 2, 3, 4...

### Name (Required)
```sql
"Name" VARCHAR(255) NOT NULL
```

- **Type**: VARCHAR(255) (text up to 255 characters)
- **NOT NULL**: Must always have a value
- **Purpose**: What the product is called
- **Examples**: "Laptop", "Mouse", "USB Cable"

### Description (Optional)
```sql
"Description" TEXT
```

- **Type**: TEXT (unlimited text)
- **Optional**: Can be empty (NULL)
- **Purpose**: Additional details about the product
- **Examples**: "High-performance gaming laptop", "Wireless"

### Price (Required)
```sql
"Price" NUMERIC(18, 2) NOT NULL
```

- **Type**: NUMERIC(18,2) (decimal number)
- **18,2**: 18 total digits, 2 after decimal
- **NOT NULL**: Must always have a value
- **Purpose**: Cost of the product
- **Examples**: 1299.99, 29.99, 0.50

---

## Sample Data

When you create products:

```sql
INSERT INTO "Products" ("Name", "Description", "Price") VALUES
  ('Laptop', 'Gaming laptop with RTX 3080', 1299.99),
  ('Mouse', 'Wireless USB mouse', 29.99),
  ('Keyboard', 'Mechanical RGB keyboard', 149.99);
```

This creates:

| Id | Name | Description | Price |
|----|------|-------------|-------|
| 1 | Laptop | Gaming laptop with RTX 3080 | 1299.99 |
| 2 | Mouse | Wireless USB mouse | 29.99 |
| 3 | Keyboard | Mechanical RGB keyboard | 149.99 |

---

## Common SQL Queries

### Get all products
```sql
SELECT * FROM "Products";
```

**Result:** All rows and all columns

---

### Get specific columns
```sql
SELECT "Name", "Price" FROM "Products";
```

**Result:**
```
    Name    | Price
------------|--------
 Laptop    | 1299.99
 Mouse     |   29.99
 Keyboard  |  149.99
```

---

### Get products under $100
```sql
SELECT * FROM "Products" WHERE "Price" < 100;
```

**Result:** Only products cheaper than $100

---

### Get one product by ID
```sql
SELECT * FROM "Products" WHERE "Id" = 2;
```

**Result:**
```
 Id | Name  | Description | Price
----|-------|-------------|-------
  2 | Mouse | Wireless... |  29.99
```

---

### Search by name
```sql
SELECT * FROM "Products" WHERE "Name" LIKE '%Keyboard%';
```

**Result:** Products with "Keyboard" in the name

---

### Count all products
```sql
SELECT COUNT(*) FROM "Products";
```

**Result:**
```
 count
-------
     3
```

---

### Average price
```sql
SELECT AVG("Price") FROM "Products";
```

**Result:**
```
       avg
-----------
  493.32333...
```

---

### Sum of all prices
```sql
SELECT SUM("Price") FROM "Products";
```

**Result:**
```
    sum
-----------
  1479.97
```

---

### Find highest priced product
```sql
SELECT * FROM "Products" WHERE "Price" = (SELECT MAX("Price") FROM "Products");
```

**Result:** Most expensive product

---

## Adding Data

### From React UI
- Fill the form
- Click "Add Product"
- React sends to backend
- Backend inserts into database

### Manually in PostgreSQL
```sql
INSERT INTO "Products" ("Name", "Description", "Price") VALUES
  ('Monitor', '27-inch 4K display', 399.99);
```

---

## Updating Data

### From React UI
- Update form and send
- Backend updates the row

### Manually in PostgreSQL
```sql
UPDATE "Products" 
SET "Name" = 'Gaming Monitor', "Price" = 449.99
WHERE "Id" = 4;
```

---

## Deleting Data

### From React UI
- Click delete button
- Backend removes from database

### Manually in PostgreSQL
```sql
-- Delete one product
DELETE FROM "Products" WHERE "Id" = 4;

-- Delete all (careful!)
DELETE FROM "Products";
```

---

## Data Types Explained

| Type | Size | Example |
|------|------|---------|
| SERIAL | 4 bytes | 1, 2, 3, 4... |
| VARCHAR(255) | Variable | "Laptop keyboard" |
| TEXT | Variable | Long descriptions... |
| NUMERIC(18,2) | 10 bytes | 1234567890.12 |
| INTEGER | 4 bytes | 1000 |
| BOOLEAN | 1 byte | TRUE / FALSE |
| DATE | 4 bytes | 2024-01-15 |

---

## Indexes

Currently, the Products table has one index:

```sql
-- Automatically created with PRIMARY KEY
CREATE INDEX "PK_Products" ON "Products"("Id");
```

**What this does:** Makes searching by ID very fast

---

## Constraints

### PRIMARY KEY
```sql
PRIMARY KEY ("Id")
```
- Makes Id unique
- Ensures no duplicates
- Makes lookup fast

### NOT NULL
```sql
"Name" VARCHAR(255) NOT NULL
```
- Column must have a value
- Cannot be empty
- Must provide when inserting

---

## Database Relationships (Future)

Right now we have one table. Later you can add:

```
Products (one)
   ↕ (many-to-one)
Categories

Products (many)
   ↕ 
Orders (has many)
   ↕
Order Items (has many)
```

But we're keeping it simple for learning!

---

## Backup and Recovery

### Backup the table
```bash
pg_dump -U postgres -d dotnet_db -t "Products" > products_backup.sql
```

### Restore from backup
```bash
psql -U postgres -d dotnet_db < products_backup.sql
```

### Full database backup
```bash
pg_dump -U postgres -d dotnet_db > full_backup.sql
```

---

## Performance Notes

1. **Id is indexed** - Lookups by ID are fast
2. **VARCHAR(255)** - Chosen carefully for space efficiency
3. **NUMERIC(18,2)** - Better than float for money  
4. **TEXT for Description** - Allows long text

---

## Validation Rules

| Field | Rules |
|-------|-------|
| `Id` | Auto-generated, never modify |
| `Name` | Required, max 255 chars, not empty |
| `Description` | Optional, any text allowed |
| `Price` | Required, must be positive, 2 decimals |

---

## SQL Operations Mapped to API

| Operation | SQL | API Endpoint | Method |
|-----------|-----|--------------|--------|
| Read All | SELECT * | /api/products | GET |
| Read One | SELECT * WHERE Id=1 | /api/products/1 | GET |
| Create | INSERT INTO | /api/products | POST |
| Update | UPDATE SET WHERE Id=1 | /api/products/1 | PUT |
| Delete | DELETE WHERE Id=1 | /api/products/1 | DELETE |

---

## Key Concepts

| Term | Meaning |
|------|---------|
| **Table** | Like an Excel spreadsheet with rows and columns |
| **Column** | A property/field (Name, Price, etc.) |
| **Row** | One complete record (one product) |
| **Primary Key** | Unique identifier for each row |
| **Index** | Speeds up searching |
| **Schema** | Structure of tables and relationships |

---

## Summary

✅ Understand the Products table
✅ Know all columns and types
✅ Can write basic SQL queries
✅ Know how data flows from API → Database

---

**Related**: [API Documentation](./API_DOCUMENTATION.md) | [Database Setup](./DATABASE_SETUP.md)

## Overview

The database uses a relational design with two main entities: `Departments` and `Employees`. Each employee belongs to exactly one department, and each department can have multiple employees.

## Entity Relationship Diagram

```
┌─────────────┐         ┌──────────────┐
│ Departments │────────▶│  Employees   │
└─────────────┘    1:N  └──────────────┘
```

---

## Departments Table

Stores information about company departments.

### Structure

```sql
CREATE TABLE "Departments" (
    "DepartmentId" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    
    CONSTRAINT "PK_Departments" PRIMARY KEY ("DepartmentId")
);
```

### Columns

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `DepartmentId` | SERIAL | PRIMARY KEY | Unique identifier (auto-increment) |
| `Name` | VARCHAR(255) | NOT NULL | Department name |
| `Description` | TEXT | NULLABLE | Optional department description |

### Indexes

```sql
CREATE INDEX "IX_Departments_Name" ON "Departments"("Name");
```

### Sample Data

```sql
INSERT INTO "Departments" ("Name", "Description") VALUES
  ('Engineering', 'Software development team'),
  ('Sales', 'Sales and business development'),
  ('HR', 'Human resources department'),
  ('Finance', 'Financial planning and analysis'),
  ('Marketing', 'Marketing and business intelligence');
```

---

## Employees Table

Stores information about employees and their department assignments.

### Structure

```sql
CREATE TABLE "Employees" (
    "EmployeeId" SERIAL PRIMARY KEY,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "Phone" VARCHAR(20),
    "HireDate" TIMESTAMP NOT NULL,
    "Salary" NUMERIC(18, 2) NOT NULL,
    "DepartmentId" INTEGER NOT NULL,
    
    CONSTRAINT "PK_Employees" PRIMARY KEY ("EmployeeId"),
    CONSTRAINT "FK_Employees_Departments" FOREIGN KEY ("DepartmentId")
        REFERENCES "Departments"("DepartmentId") ON DELETE RESTRICT
);
```

### Columns

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `EmployeeId` | SERIAL | PRIMARY KEY | Unique identifier (auto-increment) |
| `FirstName` | VARCHAR(100) | NOT NULL | Employee first name |
| `LastName` | VARCHAR(100) | NOT NULL | Employee last name |
| `Email` | VARCHAR(255) | NOT NULL | Employee email address |
| `Phone` | VARCHAR(20) | NULLABLE | Employee phone number |
| `HireDate` | TIMESTAMP | NOT NULL | Date employee was hired |
| `Salary` | NUMERIC(18,2) | NOT NULL | Annual salary |
| `DepartmentId` | INTEGER | NOT NULL, FK | Reference to Department |

### Constraints

- **Primary Key**: `DepartmentId` (unique identifier)
- **Foreign Key**: `DepartmentId` references `Departments.DepartmentId`
  - **ON DELETE RESTRICT**: Cannot delete a department with employees
  - Ensures referential integrity

### Indexes

```sql
CREATE INDEX "IX_Employees_Email" ON "Employees"("Email");
CREATE INDEX "IX_Employees_DepartmentId" ON "Employees"("DepartmentId");
CREATE INDEX "IX_Employees_HireDate" ON "Employees"("HireDate");
```

### Sample Data

```sql
INSERT INTO "Employees" 
  ("FirstName", "LastName", "Email", "Phone", "HireDate", "Salary", "DepartmentId") 
VALUES
  ('John', 'Smith', 'john.smith@company.com', '555-0101', '2023-01-15', 85000, 1),
  ('Sarah', 'Johnson', 'sarah.johnson@company.com', '555-0102', '2023-02-20', 92000, 1),
  ('Mike', 'Williams', 'mike.williams@company.com', '555-0103', '2023-03-10', 78000, 2),
  ('Emily', 'Davis', 'emily.davis@company.com', '555-0104', '2023-04-05', 75000, 3),
  ('David', 'Brown', 'david.brown@company.com', '555-0105', '2023-05-12', 88000, 1),
  ('Lisa', 'Miller', 'lisa.miller@company.com', '555-0106', '2023-06-08', 82000, 4);
```

---

## Relationships

### One-to-Many: Department → Employees

Each department can have multiple employees. When querying departments, you can include related employees.

**Query Example:**
```sql
SELECT d."DepartmentId", d."Name", COUNT(e."EmployeeId") as "EmployeeCount"
FROM "Departments" d
LEFT JOIN "Employees" e ON d."DepartmentId" = e."DepartmentId"
GROUP BY d."DepartmentId", d."Name";
```

**Result:**
```
DepartmentId | Name        | EmployeeCount
1            | Engineering | 3
2            | Sales       | 1
3            | HR          | 1
```

### Foreign Key Constraint

The `ON DELETE RESTRICT` constraint prevents accidental deletion of departments that still have employees.

**Example: This will fail**
```sql
DELETE FROM "Departments" WHERE "DepartmentId" = 1;
-- Error: Cannot delete or update because other rows reference it
```

**Correct approach: Delete employees first**
```sql
DELETE FROM "Employees" WHERE "DepartmentId" = 1;
DELETE FROM "Departments" WHERE "DepartmentId" = 1;
```

---

## Common Queries

### Get All Employees with Department Names
```sql
SELECT 
  e."EmployeeId",
  e."FirstName",
  e."LastName",
  e."Email",
  d."Name" as "Department",
  e."Salary"
FROM "Employees" e
JOIN "Departments" d ON e."DepartmentId" = d."DepartmentId"
ORDER BY e."LastName";
```

### Get Employees by Department
```sql
SELECT 
  e."FirstName",
  e."LastName",
  e."Salary"
FROM "Employees" e
WHERE e."DepartmentId" = (SELECT "DepartmentId" FROM "Departments" WHERE "Name" = 'Engineering')
ORDER BY e."Salary" DESC;
```

### Get Salary Statistics by Department
```sql
SELECT 
  d."Name",
  COUNT(e."EmployeeId") as "EmployeeCount",
  AVG(e."Salary")::NUMERIC(10,2) as "AverageSalary",
  MIN(e."Salary") as "MinSalary",
  MAX(e."Salary") as "MaxSalary"
FROM "Departments" d
LEFT JOIN "Employees" e ON d."DepartmentId" = e."DepartmentId"
GROUP BY d."DepartmentId", d."Name"
ORDER BY "AverageSalary" DESC;
```

### Find Highest Paid Employees
```sql
SELECT 
  e."FirstName",
  e."LastName",
  e."Salary",
  d."Name" as "Department"
FROM "Employees" e
JOIN "Departments" d ON e."DepartmentId" = d."DepartmentId"
ORDER BY e."Salary" DESC
LIMIT 10;
```

### Get Recent Hires
```sql
SELECT 
  e."FirstName",
  e."LastName",
  e."Email",
  d."Name" as "Department",
  e."HireDate"
FROM "Employees" e
JOIN "Departments" d ON e."DepartmentId" = d."DepartmentId"
WHERE e."HireDate" >= NOW() - INTERVAL '90 days'
ORDER BY e."HireDate" DESC;
```

### Count Employees by Department
```sql
SELECT 
  d."Name",
  COUNT(e."EmployeeId") as "EmployeeCount"
FROM "Departments" d
LEFT JOIN "Employees" e ON d."DepartmentId" = e."DepartmentId"
GROUP BY d."DepartmentId", d."Name"
ORDER BY "EmployeeCount" DESC;
```

---

## Data Validation Rules

### Employees Table

| Field | Rules |
|-------|-------|
| `FirstName` | Required, max 100 characters |
| `LastName` | Required, max 100 characters |
| `Email` | Required, valid email format, unique recommended |
| `Phone` | Optional, max 20 characters |
| `HireDate` | Required, must be in the past or today |
| `Salary` | Required, must be positive, 2 decimal places |
| `DepartmentId` | Required, must reference existing department |

### Departments Table

| Field | Rules |
|-------|-------|
| `Name` | Required, max 255 characters, unique recommended |
| `Description` | Optional, no length limit |

---

## Migration History

### v1.0.0 - Initial Schema (InitialCreate)

- Created `Departments` table
- Created `Employees` table with FK relationship to `Departments`
- Added indexes for common queries

### To View Migration History

```bash
# In your .NET project directory
dotnet ef migrations list
```

### To Create a New Migration

```bash
# After modifying your models
dotnet ef migrations add YourMigrationName

# To update the database
dotnet ef database update
```

---

## Backup and Recovery

### Full Backup
```bash
pg_dump -U app_user -d net_app_db > backup_$(date +%Y%m%d_%H%M%S).sql
```

### Table-Specific Backup
```bash
pg_dump -U app_user -d net_app_db -t "Employees" > employees_backup.sql
```

### Restore from Backup
```bash
psql -U app_user -d net_app_db < backup_20240115_143022.sql
```

---

## Performance Considerations

1. **Indexes**: Created on frequently queried columns (Email, DepartmentId, HireDate)
2. **Foreign Keys**: Enforced referential integrity with ON DELETE RESTRICT
3. **NUMERIC precision**: Used NUMERIC(18,2) for salary to avoid floating-point precision issues
4. **VARCHAR limits**: Sized appropriately to reduce storage overhead
5. **JOIN optimization**: Foreign keys are indexed to optimize JOIN operations

---

[Back to Setup Guide](../SETUP_GUIDE.md)
