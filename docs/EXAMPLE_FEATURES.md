# Example Features - Implementation Guide

This document provides additional features and examples you can implement in your React + .NET application.

---

## Feature 1: Employee Search and Filter

### Backend Enhancement: Add Search Endpoint

Update `Controllers/EmployeesController.cs`:

```csharp
// GET: api/employees/search?query=john&departmentId=1
[HttpGet("search")]
public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees(
    [FromQuery] string query = "",
    [FromQuery] int? departmentId = null)
{
    var result = _context.Employees.Include(e => e.Department).AsQueryable();

    if (!string.IsNullOrEmpty(query))
    {
        result = result.Where(e =>
            e.FirstName.ToLower().Contains(query.ToLower()) ||
            e.LastName.ToLower().Contains(query.ToLower()) ||
            e.Email.ToLower().Contains(query.ToLower()));
    }

    if (departmentId.HasValue)
    {
        result = result.Where(e => e.DepartmentId == departmentId.Value);
    }

    return await result.ToListAsync();
}
```

### Frontend Component: Search Form

Create `src/components/EmployeeSearch.js`:

```javascript
import React, { useState, useCallback } from 'react';
import { employeeService } from '../services/api';
import './EmployeeSearch.css';

export const EmployeeSearch = ({ onResults }) => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = useCallback(async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      const response = await employeeService.search(query);
      setResults(response.data);
      onResults(response.data);
    } catch (error) {
      console.error('Search failed:', error);
    } finally {
      setLoading(false);
    }
  }, [query, onResults]);

  return (
    <form className="search-form" onSubmit={handleSearch}>
      <input
        type="text"
        placeholder="Search employees by name or email..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
      <button type="submit" disabled={loading}>
        {loading ? 'Searching...' : 'Search'}
      </button>
    </form>
  );
};
```

---

## Feature 2: Pagination

### Backend Enhancement: Add Pagination

Update `Controllers/EmployeesController.cs`:

```csharp
// GET: api/employees/paginated?pageNumber=1&pageSize=10
[HttpGet("paginated")]
public async Task<ActionResult<PagedResult<Employee>>> GetEmployeesPaginated(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
{
    if (pageNumber < 1 || pageSize < 1)
        return BadRequest("Page number and size must be greater than 0");

    var totalCount = await _context.Employees.CountAsync();
    var employees = await _context.Employees
        .Include(e => e.Department)
        .OrderBy(e => e.EmployeeId)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var result = new PagedResult<Employee>
    {
        Items = employees,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalCount = totalCount,
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
    };

    return Ok(result);
}
```

Create `Models/PagedResult.cs`:

```csharp
namespace DotNetApi.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
```

### Frontend Hook: usePagination

Create `src/hooks/usePagination.js`:

```javascript
import { useState, useEffect } from 'react';
import { employeeService } from '../services/api';

export const usePagination = () => {
  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchPage();
  }, [pageNumber, pageSize]);

  const fetchPage = async () => {
    try {
      setLoading(true);
      const response = await employeeService.getPaginated(pageNumber, pageSize);
      setData(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return {
    data,
    pageNumber,
    pageSize,
    totalPages,
    loading,
    error,
    setPageNumber,
    setPageSize,
  };
};
```

---

## Feature 3: Salary Statistics Dashboard

### Backend Endpoint: Get Statistics

Create `Controllers/StatisticsController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetApi.Data;
using DotNetApi.Models;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("salary-by-department")]
        public async Task<ActionResult<SalaryStatistics>> GetSalaryByDepartment()
        {
            var stats = await _context.Departments
                .Include(d => d.Employees)
                .Select(d => new DepartmentSalaryStats
                {
                    DepartmentName = d.Name,
                    EmployeeCount = d.Employees.Count,
                    AverageSalary = d.Employees.Any() 
                        ? d.Employees.Average(e => e.Salary) 
                        : 0,
                    MinSalary = d.Employees.Any() 
                        ? d.Employees.Min(e => e.Salary) 
                        : 0,
                    MaxSalary = d.Employees.Any() 
                        ? d.Employees.Max(e => e.Salary) 
                        : 0
                })
                .ToListAsync();

            return Ok(stats);
        }

        [HttpGet("total-payroll")]
        public async Task<ActionResult<TotalPayrollStats>> GetTotalPayroll()
        {
            var stats = new TotalPayrollStats
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalPayroll = await _context.Employees.SumAsync(e => e.Salary),
                AverageSalary = await _context.Employees.AverageAsync(e => e.Salary),
                HighestSalary = await _context.Employees.MaxAsync(e => e.Salary),
                LowestSalary = await _context.Employees.MinAsync(e => e.Salary)
            };

            return Ok(stats);
        }
    }
}
```

Create `Models/StatisticsModels.cs`:

```csharp
namespace DotNetApi.Models
{
    public class DepartmentSalaryStats
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public double AverageSalary { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }

    public class TotalPayrollStats
    {
        public int TotalEmployees { get; set; }
        public decimal TotalPayroll { get; set; }
        public double AverageSalary { get; set; }
        public decimal HighestSalary { get; set; }
        public decimal LowestSalary { get; set; }
    }
}
```

### Frontend Dashboard Component

Create `src/components/SalaryDashboard.js`:

```javascript
import React, { useState, useEffect } from 'react';
import api from '../services/api';
import './SalaryDashboard.css';

export const SalaryDashboard = () => {
  const [totalStats, setTotalStats] = useState(null);
  const [deptStats, setDeptStats] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchStatistics();
  }, []);

  const fetchStatistics = async () => {
    try {
      setLoading(true);
      const [totalResponse, deptResponse] = await Promise.all([
        api.get('/statistics/total-payroll'),
        api.get('/statistics/salary-by-department'),
      ]);

      setTotalStats(totalResponse.data);
      setDeptStats(deptResponse.data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Loading statistics...</div>;
  if (error) return <div className="error">Error: {error}</div>;
  if (!totalStats) return <div>No data available</div>;

  return (
    <div className="dashboard">
      <h2>Salary Dashboard</h2>
      
      <div className="summary-cards">
        <div className="card">
          <h3>Total Employees</h3>
          <p className="value">{totalStats.totalEmployees}</p>
        </div>
        <div className="card">
          <h3>Total Payroll</h3>
          <p className="value">${totalStats.totalPayroll.toLocaleString()}</p>
        </div>
        <div className="card">
          <h3>Average Salary</h3>
          <p className="value">${totalStats.averageSalary.toLocaleString(undefined, { maximumFractionDigits: 0 })}</p>
        </div>
        <div className="card">
          <h3>Highest Salary</h3>
          <p className="value">${totalStats.highestSalary.toLocaleString()}</p>
        </div>
      </div>

      <div className="department-stats">
        <h3>Salary by Department</h3>
        <table>
          <thead>
            <tr>
              <th>Department</th>
              <th>Employees</th>
              <th>Average Salary</th>
              <th>Min Salary</th>
              <th>Max Salary</th>
            </tr>
          </thead>
          <tbody>
            {deptStats.map((dept, idx) => (
              <tr key={idx}>
                <td>{dept.departmentName}</td>
                <td>{dept.employeeCount}</td>
                <td>${dept.averageSalary.toLocaleString(undefined, { maximumFractionDigits: 0 })}</td>
                <td>${dept.minSalary.toLocaleString()}</td>
                <td>${dept.maxSalary.toLocaleString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};
```

Create `src/components/SalaryDashboard.css`:

```css
.dashboard {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.dashboard h2 {
  color: #333;
  margin-bottom: 30px;
}

.summary-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
  margin-bottom: 40px;
}

.card {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.card h3 {
  margin: 0 0 10px 0;
  font-size: 14px;
  opacity: 0.9;
}

.card .value {
  margin: 0;
  font-size: 28px;
  font-weight: bold;
}

.department-stats {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.department-stats table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.department-stats th {
  background-color: #667eea;
  color: white;
  padding: 12px;
  text-align: left;
  font-weight: bold;
}

.department-stats td {
  padding: 12px;
  border-bottom: 1px solid #ddd;
}

.department-stats tr:hover {
  background-color: #f5f5f5;
}
```

---

## Feature 4: Export to CSV

### Frontend Component: Export Button

Create `src/utils/csvExport.js`:

```javascript
export const exportToCSV = (data, filename) => {
  const headers = Object.keys(data[0]);
  const csvContent = [
    headers.join(','),
    ...data.map(row =>
      headers.map(header => {
        const value = row[header];
        // Escape quotes and wrap in quotes if contains comma
        return typeof value === 'string' && value.includes(',')
          ? `"${value.replace(/"/g, '""')}"`
          : value;
      }).join(',')
    ),
  ].join('\n');

  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement('a');
  const url = URL.createObjectURL(blob);

  link.setAttribute('href', url);
  link.setAttribute('download', `${filename}.csv`);
  link.style.visibility = 'hidden';

  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};
```

Usage in component:

```javascript
import { exportToCSV } from '../utils/csvExport';

// In your component:
const handleExport = () => {
  exportToCSV(employees, 'employees');
};

return (
  <button onClick={handleExport}>Export to CSV</button>
);
```

---

## Feature 5: Input Validation

### Frontend Validation Hook

Create `src/hooks/useFormValidation.js`:

```javascript
import { useState } from 'react';

export const useFormValidation = (initialValues, onSubmit) => {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});

  const validate = () => {
    const newErrors = {};

    if (!values.firstName?.trim()) newErrors.firstName = 'First name is required';
    if (!values.lastName?.trim()) newErrors.lastName = 'Last name is required';
    if (!values.email?.trim()) newErrors.email = 'Email is required';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(values.email))
      newErrors.email = 'Email is invalid';
    if (!values.hireDate) newErrors.hireDate = 'Hire date is required';
    if (!values.salary) newErrors.salary = 'Salary is required';
    else if (values.salary <= 0) newErrors.salary = 'Salary must be positive';
    if (!values.departmentId) newErrors.departmentId = 'Department is required';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
  };

  const handleBlur = (e) => {
    const { name } = e.target;
    setTouched(prev => ({ ...prev, [name]: true }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (validate()) {
      await onSubmit(values);
    }
  };

  return {
    values,
    errors,
    touched,
    handleChange,
    handleBlur,
    handleSubmit,
  };
};
```

---

## Feature 6: Error Handling and Retry Logic

### Custom Hook: useAsync

Create `src/hooks/useAsync.js`:

```javascript
import { useState, useCallback } from 'react';

export const useAsync = (asyncFunction, immediate = true) => {
  const [status, setStatus] = useState('idle');
  const [value, setValue] = useState(null);
  const [error, setError] = useState(null);

  const execute = useCallback(
    async (...args) => {
      setStatus('pending');
      setValue(null);
      setError(null);
      try {
        const response = await asyncFunction(...args);
        setValue(response);
        setStatus('success');
        return response;
      } catch (err) {
        setError(err);
        setStatus('error');
        throw err;
      }
    },
    [asyncFunction]
  );

  const retry = useCallback(() => execute(), [execute]);

  return { execute, retry, status, value, error };
};
```

---

## Feature 7: Authentication/Authorization

### Simple JWT Example - Backend

Add to `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Add authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]);
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add authorization
app.UseAuthentication();
app.UseAuthorization();
```

Add to `appsettings.json`:

```json
{
  "Jwt": {
    "Secret": "your-super-secret-key-min-32-characters-long",
    "Issuer": "your-app",
    "Audience": "your-app-users"
  }
}
```

---

## Next Steps

1. Implement search and filter functionality
2. Add pagination for large datasets
3. Create statistics and reporting dashboards
4. Add data export capabilities
5. Implement proper error handling
6. Add authentication and authorization
7. Optimize performance with caching
8. Add comprehensive logging

---

[Back to Setup Guide](../SETUP_GUIDE.md)
