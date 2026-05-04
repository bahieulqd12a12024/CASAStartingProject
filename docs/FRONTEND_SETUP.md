# Frontend Setup - React with Create React App (Simplified)

This guide shows the easiest frontend setup for your React + .NET + PostgreSQL app.

## Prerequisites

- Node.js 18+
- npm 8+
- Backend running on `http://localhost:7000`

Verify installation:
```bash
node --version
npm --version
```

---

## Step 1: Create the React app

If you have not already created the app, run:
```bash
cd frontend
npx create-react-app react-app
```

If `react-app` already exists, skip this step.

---

## Step 2: Install Axios

From the app folder:
```bash
cd frontend/react-app
npm install axios
```

---

## Step 3: Point React to the backend

Create `.env.local` in `frontend/react-app` with:
```
REACT_APP_API_BASE_URL=http://localhost:7000/api
```

If your backend uses HTTPS locally, use `https://localhost:7000/api`.

---

## Step 4: Create the API service

Create `src/services/api.js` with:
```javascript
import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
});

export const productService = {
  getAll: () => api.get('/products'),
  create: (product) => api.post('/products', product),
  delete: (id) => api.delete(`/products/${id}`),
};
```

---

## Step 5: Replace `src/App.js`

Replace `src/App.js` with this single-page app:

```javascript
import { useEffect, useState } from 'react';
import { productService } from './services/api';
import './App.css';

function App() {
  const [products, setProducts] = useState([]);
  const [form, setForm] = useState({ name: '', description: '', price: '' });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    setLoading(true);
    setError('');
    try {
      const response = await productService.getAll();
      setProducts(response.data);
    } catch {
      setError('Could not load products.');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!form.name || !form.price) {
      setError('Name and price are required.');
      return;
    }

    try {
      await productService.create({
        name: form.name,
        description: form.description,
        price: parseFloat(form.price),
      });
      setForm({ name: '', description: '', price: '' });
      fetchProducts();
    } catch {
      setError('Could not add product.');
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this product?')) return;
    try {
      await productService.delete(id);
      setProducts((prev) => prev.filter((product) => product.id !== id));
    } catch {
      setError('Could not delete product.');
    }
  };

  return (
    <div className="App">
      <header className="app-header">
        <h1>Product Store</h1>
        <p>React + .NET 8 + PostgreSQL</p>
      </header>

      <main>
        <section className="form-card">
          <h2>Add Product</h2>
          {error && <div className="error">{error}</div>}
          <form onSubmit={handleSubmit}>
            <label>
              Name
              <input
                name="name"
                value={form.name}
                onChange={handleChange}
                placeholder="Laptop"
                required
              />
            </label>

            <label>
              Description
              <textarea
                name="description"
                value={form.description}
                onChange={handleChange}
                placeholder="Optional description"
              />
            </label>

            <label>
              Price
              <input
                name="price"
                type="number"
                step="0.01"
                value={form.price}
                onChange={handleChange}
                placeholder="1299.99"
                required
              />
            </label>

            <button type="submit">Add Product</button>
          </form>
        </section>

        <section className="product-list">
          <h2>Products</h2>
          {loading ? (
            <p>Loading products...</p>
          ) : products.length === 0 ? (
            <p>No products yet.</p>
          ) : (
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                  <th>Description</th>
                  <th>Price</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {products.map((product) => (
                  <tr key={product.id}>
                    <td>{product.id}</td>
                    <td>{product.name}</td>
                    <td>{product.description}</td>
                    <td>${product.price.toFixed(2)}</td>
                    <td>
                      <button onClick={() => handleDelete(product.id)}>
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;
```

---

## Step 6: Replace `src/App.css`

Use this simple styling:

```css
.App {
  min-height: 100vh;
  background-color: #f5f5f5;
  font-family: Arial, sans-serif;
}

.app-header {
  background: #007bff;
  color: white;
  padding: 30px 20px;
  text-align: center;
}

.app-header h1,
.app-header p {
  margin: 0;
}

main {
  max-width: 900px;
  margin: 20px auto;
  padding: 0 20px;
}

.form-card,
.product-list {
  background: white;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  margin-bottom: 20px;
}

label {
  display: block;
  margin-bottom: 12px;
  font-weight: 600;
}

input,
textarea {
  width: 100%;
  padding: 10px;
  margin-top: 6px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 16px;
}

button {
  background: #007bff;
  color: white;
  border: none;
  padding: 12px 18px;
  border-radius: 4px;
  cursor: pointer;
  margin-top: 10px;
}

button:hover {
  background: #0056b3;
}

.error {
  color: #b00020;
  margin-bottom: 16px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  background: #007bff;
  color: white;
  padding: 12px;
  text-align: left;
}

tbody td {
  padding: 12px;
  border-top: 1px solid #eee;
}

tbody tr:hover {
  background: #f5f5f5;
}
```

---

## Step 7: Start the frontend

From `frontend/react-app`:
```bash
npm start
```

Open `http://localhost:3000`.

---

## Compatibility notes

- This frontend uses the backend endpoints `GET /api/products`, `POST /api/products`, and `DELETE /api/products/{id}`.
- Make sure your backend is running on port `7000`.
- If the backend is using HTTPS locally, update `.env.local`.

---

## Minimal file structure

```
react-app/
├── src/
│   ├── App.js
│   ├── App.css
│   └── services/
│       └── api.js
├── .env.local
├── package.json
└── public/
```

## Troubleshooting

- If React will not start, run `npm install` again.
- If requests fail, check that the backend is running and `.env.local` has the correct URL.
- If you see CORS errors, ensure the backend allows `http://localhost:3000`.
