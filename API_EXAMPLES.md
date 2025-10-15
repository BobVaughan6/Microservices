# API Request Examples

This file contains example HTTP requests for testing the microservices system.

## API Gateway Endpoints (Port 5000)

### Health Check

```http
GET http://localhost:5000/health
```

### User Endpoints

#### Get All Users
```http
GET http://localhost:5000/api/users
```

Example Response:
```json
[
  {
    "id": 1,
    "name": "Alice",
    "email": "alice@example.com"
  },
  {
    "id": 2,
    "name": "Bob",
    "email": "bob@example.com"
  },
  {
    "id": 3,
    "name": "Charlie",
    "email": "charlie@example.com"
  }
]
```

#### Get User by ID
```http
GET http://localhost:5000/api/users/1
```

Example Response:
```json
{
  "id": 1,
  "name": "Alice",
  "email": "alice@example.com"
}
```

### Product Endpoints

#### Get All Products
```http
GET http://localhost:5000/api/products
```

Example Response:
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "High-performance laptop",
    "price": 999.99
  },
  {
    "id": 2,
    "name": "Mouse",
    "description": "Wireless mouse",
    "price": 29.99
  },
  {
    "id": 3,
    "name": "Keyboard",
    "description": "Mechanical keyboard",
    "price": 89.99
  }
]
```

#### Get Product by ID
```http
GET http://localhost:5000/api/products/2
```

Example Response:
```json
{
  "id": 2,
  "name": "Mouse",
  "description": "Wireless mouse",
  "price": 29.99
}
```

---

## Direct Service Access (For Testing)

### User Service (Port 5001)

#### Health Check
```http
GET http://localhost:5001/health
```

#### Get All Users
```http
GET http://localhost:5001/api/users
```

#### Get User by ID
```http
GET http://localhost:5001/api/users/1
```

#### Create New User
```http
POST http://localhost:5001/api/users
Content-Type: application/json

{
  "id": 0,
  "name": "David",
  "email": "david@example.com"
}
```

---

### Product Service (Port 5002)

#### Health Check
```http
GET http://localhost:5002/health
```

#### Get All Products
```http
GET http://localhost:5002/api/products
```

#### Get Product by ID
```http
GET http://localhost:5002/api/products/1
```

#### Create New Product
```http
POST http://localhost:5002/api/products
Content-Type: application/json

{
  "id": 0,
  "name": "Monitor",
  "description": "4K Ultra HD Monitor",
  "price": 499.99
}
```

---

## Using curl

### Basic GET Request
```bash
curl http://localhost:5000/api/users
```

### With Pretty Print (using jq)
```bash
curl -s http://localhost:5000/api/users | jq
```

### With Python JSON Tool
```bash
curl -s http://localhost:5000/api/users | python3 -m json.tool
```

### POST Request
```bash
curl -X POST http://localhost:5001/api/users \
  -H "Content-Type: application/json" \
  -d '{"id":0,"name":"Eve","email":"eve@example.com"}'
```

---

## Using PowerShell

### GET Request
```powershell
Invoke-RestMethod -Uri http://localhost:5000/api/users
```

### POST Request
```powershell
$user = @{
    id = 0
    name = "Eve"
    email = "eve@example.com"
} | ConvertTo-Json

Invoke-RestMethod -Uri http://localhost:5001/api/users `
    -Method Post `
    -Body $user `
    -ContentType "application/json"
```

---

## Testing Scenarios

### Scenario 1: Verify System Health
1. Check API Gateway health: `GET http://localhost:5000/health`
2. Check User Service health: `GET http://localhost:5001/health`
3. Check Product Service health: `GET http://localhost:5002/health`

### Scenario 2: API Gateway Routing
1. Request users through Gateway: `GET http://localhost:5000/api/users`
2. Request products through Gateway: `GET http://localhost:5000/api/products`
3. Verify responses are correct

### Scenario 3: Service Independence
1. Access User Service directly: `GET http://localhost:5001/api/users`
2. Access Product Service directly: `GET http://localhost:5002/api/products`
3. Verify both work independently

### Scenario 4: Error Handling
1. Request non-existent user: `GET http://localhost:5000/api/users/999`
2. Request non-existent product: `GET http://localhost:5000/api/products/999`
3. Verify 404 responses

### Scenario 5: Create Resources
1. Create new user: `POST http://localhost:5001/api/users` with JSON body
2. Retrieve the new user: `GET http://localhost:5001/api/users`
3. Create new product: `POST http://localhost:5002/api/products` with JSON body
4. Retrieve the new product: `GET http://localhost:5002/api/products`
