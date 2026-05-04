# API Documentation - Complete Reference

All endpoints are at: `http://localhost:7000/api`

---

## Product Endpoints

### 1. Get All Products

**Request:**
```
GET /api/products
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "High-performance laptop",
    "price": 1299.99
  },
  {
    "id": 2,
    "name": "Mouse",
    "description": "Wireless mouse",
    "price": 29.99
  }
]
```

**Status Codes:**
- `200` - Success, products returned
- `500` - Server error

---

### 2. Get One Product

**Request:**
```
GET /api/products/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 1299.99
}
```

**Status Codes:**
- `200` - Success, product found
- `404` - Not found (product doesn't exist)
- `500` - Server error

---

### 3. Create Product

**Request:**
```
POST /api/products
Content-Type: application/json

{
  "name": "Keyboard",
  "description": "Mechanical keyboard",
  "price": 149.99
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "name": "Keyboard",
  "description": "Mechanical keyboard",
  "price": 149.99
}
```

**Status Codes:**
- `201` - Success, product created
- `400` - Bad request (invalid data)
- `500` - Server error

---

### 4. Update Product

**Request:**
```
PUT /api/products/1
Content-Type: application/json

{
  "id": 1,
  "name": "Gaming Laptop",
  "description": "Updated description",
  "price": 1499.99
}
```

**Response (204 No Content):**
```
(empty response)
```

**Status Codes:**
- `204` - Success, product updated
- `400` - Bad request (ID mismatch)
- `404` - Not found (product doesn't exist)
- `500` - Server error

---

### 5. Delete Product

**Request:**
```
DELETE /api/products/1
```

**Response (204 No Content):**
```
(empty response)
```

**Status Codes:**
- `204` - Success, product deleted
- `404` - Not found (product doesn't exist)
- `500` - Server error

---

## Common Status Codes

| Code | Meaning | Description |
|------|---------|-------------|
| `200` | OK | Request successful |
| `201` | Created | New resource created |
| `204` | No Content | Success, no data in response |
| `400` | Bad Request | Invalid request data |
| `404` | Not Found | Resource doesn't exist |
| `500` | Server Error | Backend error |

---

## Product Data Format

| Field | Type | Required | Description | Example |
|-------|------|----------|-------------|---------|
| `id` | integer | Auto | Unique identifier | `1` |
| `name` | string | Yes | Product name | `"Laptop"` |
| `description` | string | No | Product details | `"Gaming computer"` |
| `price` | decimal | Yes | Product cost | `1299.99` |

---

## Testing Endpoints

### Using Swagger (Easiest)

1. Start backend: `dotnet run`
2. Open: `https://localhost:7000/swagger`
3. Click endpoint
4. Click "Try it out"
5. Click "Execute"

---

### Using cURL (Command Line)

**Get all products:**
```bash
curl https://localhost:7000/api/products
```

**Get one product:**
```bash
curl https://localhost:7000/api/products/1
```

**Create product:**
```bash
curl -X POST https://localhost:7000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Monitor",
    "description": "27-inch 4K",
    "price": 399.99
  }'
```

**Update product:**
```bash
curl -X PUT https://localhost:7000/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "Gaming Laptop",
    "description": "Updated",
    "price": 1500.00
  }'
```

**Delete product:**
```bash
curl -X DELETE https://localhost:7000/api/products/1
```

---

### Using JavaScript/React

```javascript
// Imports
import axios from 'axios';

const API = 'http://localhost:7000/api';

// Get all products
const getProducts = async () => {
  const response = await axios.get(`${API}/products`);
  console.log(response.data); // Array of products
};

// Get one product
const getProduct = async (id) => {
  const response = await axios.get(`${API}/products/${id}`);
  console.log(response.data); // One product
};

// Create product
const createProduct = async (product) => {
  const response = await axios.post(`${API}/products`, {
    name: product.name,
    description: product.description,
    price: parseFloat(product.price)
  });
  console.log(response.data); // The created product with ID
};

// Update product
const updateProduct = async (id, product) => {
  await axios.put(`${API}/products/${id}`, {
    id: id,
    name: product.name,
    description: product.description,
    price: parseFloat(product.price)
  });
  console.log('Updated!');
};

// Delete product
const deleteProduct = async (id) => {
  await axios.delete(`${API}/products/${id}`);
  console.log('Deleted!');
};
```

---

## Example Workflow

### Scenario: Add a Product

**Step 1: User fills form**
```
Name: Gaming Keyboard
Description: RGB LED mechanical keyboard
Price: 149.99
```

**Step 2: React sends POST request**
```
POST /api/products
{
  "name": "Gaming Keyboard",
  "description": "RGB LED mechanical keyboard",
  "price": 149.99
}
```

**Step 3: Backend receives and saves**
- Validates data
- Adds to database
- Gets ID from database (auto-generated)

**Step 4: Backend responds**
```
201 Created
{
  "id": 5,
  "name": "Gaming Keyboard",
  "description": "RGB LED mechanical keyboard",
  "price": 149.99
}
```

**Step 5: React displays**
- Adds product to the table
- Shows ID, Name, Description, Price
- Offers Delete button

---

## Error Examples

### Missing Required Field
**Request:**
```json
{
  "description": "Some product",
  "price": 99.99
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Name is required"
}
```

---

### Product Not Found
**Request:**
```
GET /api/products/999
```

**Response (404 Not Found):**
```
(empty response)
```

---

### Invalid Price
**Request:**
```json
{
  "name": "Product",
  "price": "not-a-number"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Price must be a number"
}
```

---

## API Usage Rules

1. **Always use POST for creating**
2. **Always use PUT for updating**
3. **Always use DELETE for removing**
4. **Send JSON data** with correct headers
5. **Include ID in PUT requests** (must match URL)
6. **Check response status** - 200/201 = success

---

## Performance Tips

1. **Get all at once** instead of one-by-one
2. **Use caching** if you fetch same data repeatedly
3. **Batch operations** when possible
4. **Don't pollute** with unnecessary data

---

## Security Notes

- ✓ Always validate input on backend
- ✓ Check data types and lengths
- ✓ Handle errors gracefully
- ✓ Don't expose database errors to users
- ✓ Use HTTPS in production

---

**Additional Help**: [Database Schema](./DATABASE_SCHEMA.md) | [Frontend Setup](./FRONTEND_SETUP.md)

## Base URL
```
http://localhost:7000/api
```

## All endpoints require Content-Type: application/json

---

## Employee Endpoints

### 1. Get All Employees
**GET** `/api/employees`

**Response:**
```json
[
  {
    "employeeId": 1,
    "firstName": "John",
    "lastName": "Smith",
    "email": "john.smith@company.com",
    "phone": "555-0101",
    "hireDate": "2023-01-15T00:00:00",
    "salary": 85000,
    "departmentId": 1,
    "department": {
      "departmentId": 1,
      "name": "Engineering",
      "description": "Software development team",
      "employees": []
    }
  }
]
```

**Status Codes:**
- **200**: OK - Employees retrieved successfully
- **500**: Server error

---

### 2. Get Employee by ID
**GET** `/api/employees/{id}`

**Path Parameters:**
- `id` (integer): Employee ID

**Response:**
```json
{
  "employeeId": 1,
  "firstName": "John",
  "lastName": "Smith",
  "email": "john.smith@company.com",
  "phone": "555-0101",
  "hireDate": "2023-01-15T00:00:00",
  "salary": 85000,
  "departmentId": 1,
  "department": {
    "departmentId": 1,
    "name": "Engineering",
    "description": "Software development team"
  }
}
```

**Status Codes:**
- **200**: OK - Employee found
- **404**: Not Found - Employee doesn't exist
- **500**: Server error

---

### 3. Create Employee
**POST** `/api/employees`

**Request Body:**
```json
{
  "firstName": "Jane",
  "lastName": "Doe",
  "email": "jane.doe@company.com",
  "phone": "555-0106",
  "hireDate": "2024-01-10",
  "salary": 95000,
  "departmentId": 1
}
```

**Response:**
```json
{
  "employeeId": 6,
  "firstName": "Jane",
  "lastName": "Doe",
  "email": "jane.doe@company.com",
  "phone": "555-0106",
  "hireDate": "2024-01-10T00:00:00",
  "salary": 95000,
  "departmentId": 1
}
```

**Status Codes:**
- **201**: Created - Employee created successfully
- **400**: Bad Request - Invalid data
- **500**: Server error

---

### 4. Update Employee
**PUT** `/api/employees/{id}`

**Path Parameters:**
- `id` (integer): Employee ID

**Request Body:**
```json
{
  "employeeId": 1,
  "firstName": "John",
  "lastName": "Smith",
  "email": "john.smith@newcompany.com",
  "phone": "555-0101",
  "hireDate": "2023-01-15T00:00:00",
  "salary": 90000,
  "departmentId": 2
}
```

**Response:**
- No content (status 204)

**Status Codes:**
- **204**: No Content - Employee updated successfully
- **400**: Bad Request - ID mismatch
- **404**: Not Found - Employee doesn't exist
- **500**: Server error

---

### 5. Delete Employee
**DELETE** `/api/employees/{id}`

**Path Parameters:**
- `id` (integer): Employee ID

**Response:**
- No content (status 204)

**Status Codes:**
- **204**: No Content - Employee deleted successfully
- **404**: Not Found - Employee doesn't exist
- **500**: Server error

---

## Department Endpoints

### 1. Get All Departments
**GET** `/api/departments`

**Response:**
```json
[
  {
    "departmentId": 1,
    "name": "Engineering",
    "description": "Software development team",
    "employees": [
      {
        "employeeId": 1,
        "firstName": "John",
        "lastName": "Smith",
        "email": "john.smith@company.com",
        "phone": "555-0101",
        "hireDate": "2023-01-15T00:00:00",
        "salary": 85000,
        "departmentId": 1
      }
    ]
  }
]
```

**Status Codes:**
- **200**: OK - Departments retrieved successfully
- **500**: Server error

---

### 2. Get Department by ID
**GET** `/api/departments/{id}`

**Path Parameters:**
- `id` (integer): Department ID

**Response:**
```json
{
  "departmentId": 1,
  "name": "Engineering",
  "description": "Software development team",
  "employees": [...]
}
```

**Status Codes:**
- **200**: OK - Department found
- **404**: Not Found - Department doesn't exist
- **500**: Server error

---

### 3. Create Department
**POST** `/api/departments`

**Request Body:**
```json
{
  "name": "Finance",
  "description": "Financial planning and analysis"
}
```

**Response:**
```json
{
  "departmentId": 4,
  "name": "Finance",
  "description": "Financial planning and analysis"
}
```

**Status Codes:**
- **201**: Created - Department created successfully
- **400**: Bad Request - Invalid data
- **500**: Server error

---

### 4. Update Department
**PUT** `/api/departments/{id}`

**Path Parameters:**
- `id` (integer): Department ID

**Request Body:**
```json
{
  "departmentId": 1,
  "name": "Engineering",
  "description": "Software and hardware development"
}
```

**Response:**
- No content (status 204)

**Status Codes:**
- **204**: No Content - Department updated successfully
- **400**: Bad Request - ID mismatch
- **404**: Not Found - Department doesn't exist
- **500**: Server error

---

### 5. Delete Department
**DELETE** `/api/departments/{id}`

**Path Parameters:**
- `id` (integer): Department ID

**Response:**
- No content (status 204)

**Status Codes:**
- **204**: No Content - Department deleted successfully
- **404**: Not Found - Department doesn't exist
- **500**: Server error

---

## Common Response Codes

| Code | Meaning | Description |
|------|---------|-------------|
| 200 | OK | Successful GET request |
| 201 | Created | Successful POST request |
| 204 | No Content | Successful PUT/DELETE request |
| 400 | Bad Request | Invalid request data |
| 404 | Not Found | Resource not found |
| 500 | Server Error | Internal server error |

---

## Data Types

### Employee
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| employeeId | integer | - | Auto-generated ID |
| firstName | string | Yes | Employee first name (max 100 chars) |
| lastName | string | Yes | Employee last name (max 100 chars) |
| email | string | Yes | Employee email address |
| phone | string | No | Employee phone number |
| hireDate | date | Yes | Date employee was hired (format: YYYY-MM-DD) |
| salary | decimal | Yes | Annual salary (2 decimal places) |
| departmentId | integer | Yes | ID of employee's department |
| department | object | - | Department details (read-only, returned in GET requests) |

### Department
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| departmentId | integer | - | Auto-generated ID |
| name | string | Yes | Department name |
| description | string | No | Department description |
| employees | array | - | List of employees in department (read-only, returned in GET requests) |

---

## Error Response Format

Errors are returned with appropriate HTTP status codes and may include error messages:

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "0HMVI99V12345:00000001"
}
```

---

## Testing the API

### Using cURL
```bash
# Get all employees
curl http://localhost:7000/api/employees

# Get employee by ID
curl http://localhost:7000/api/employees/1

# Create employee
curl -X POST http://localhost:7000/api/employees \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Jane",
    "lastName": "Doe",
    "email": "jane@company.com",
    "phone": "555-0106",
    "hireDate": "2024-01-10",
    "salary": 95000,
    "departmentId": 1
  }'

# Update employee
curl -X PUT http://localhost:7000/api/employees/1 \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": 1,
    "firstName": "John",
    "lastName": "Smith",
    "email": "john@company.com",
    "phone": "555-0101",
    "hireDate": "2023-01-15",
    "salary": 90000,
    "departmentId": 1
  }'

# Delete employee
curl -X DELETE http://localhost:7000/api/employees/1
```

### Using Swagger UI
1. Start the .NET backend: `dotnet run`
2. Visit: `https://localhost:7000/swagger`
3. Use the interactive UI to test all endpoints

### Using JavaScript/Fetch
```javascript
// Get all employees
const response = await fetch('http://localhost:7000/api/employees');
const employees = await response.json();

// Create employee
const newEmployee = {
  firstName: 'Jane',
  lastName: 'Doe',
  email: 'jane@company.com',
  phone: '555-0106',
  hireDate: '2024-01-10',
  salary: 95000,
  departmentId: 1
};

const createResponse = await fetch('http://localhost:7000/api/employees', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(newEmployee)
});

const createdEmployee = await createResponse.json();
```

---

[Back to Setup Guide](../SETUP_GUIDE.md)
