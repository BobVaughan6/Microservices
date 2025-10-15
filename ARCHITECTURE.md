# Microservices Architecture Diagram

```
                                    ┌─────────────────────┐
                                    │   Client / Browser  │
                                    └──────────┬──────────┘
                                               │
                                               │ HTTP Requests
                                               │
                                               ▼
                               ┌───────────────────────────────┐
                               │      API Gateway              │
                               │   (Port 5000)                 │
                               │                               │
                               │  - Request Routing            │
                               │  - Service Aggregation        │
                               │  - Health Check Aggregation   │
                               └───────────┬───────────────────┘
                                          │
                   ┌──────────────────────┴───────────────────────┐
                   │                                               │
                   │                                               │
                   ▼                                               ▼
     ┌─────────────────────────┐                    ┌─────────────────────────┐
     │   User Service          │                    │   Product Service       │
     │   (Port 5001)           │                    │   (Port 5002)           │
     │                         │                    │                         │
     │  Endpoints:             │                    │  Endpoints:             │
     │  - GET /health          │                    │  - GET /health          │
     │  - GET /api/users       │                    │  - GET /api/products    │
     │  - GET /api/users/{id}  │                    │  - GET /api/products/{id}│
     │  - POST /api/users      │                    │  - POST /api/products   │
     │                         │                    │                         │
     │  Data:                  │                    │  Data:                  │
     │  - In-Memory User List  │                    │  - In-Memory Product    │
     │                         │                    │    List                 │
     └─────────────────────────┘                    └─────────────────────────┘
```

## Communication Flow

### 1. Client → API Gateway → User Service
```
Client Request: GET http://localhost:5000/api/users
      ↓
API Gateway routes to: http://localhost:5001/api/users
      ↓
User Service processes and returns user data
      ↓
API Gateway returns response to client
```

### 2. Client → API Gateway → Product Service
```
Client Request: GET http://localhost:5000/api/products
      ↓
API Gateway routes to: http://localhost:5002/api/products
      ↓
Product Service processes and returns product data
      ↓
API Gateway returns response to client
```

### 3. Health Check Aggregation
```
Client Request: GET http://localhost:5000/health
      ↓
API Gateway checks: http://localhost:5001/health
API Gateway checks: http://localhost:5002/health
      ↓
API Gateway aggregates all health statuses
      ↓
Returns overall system health to client
```

## Key Microservices Characteristics Demonstrated

1. **Service Independence**: Each service runs independently on its own port
2. **Single Responsibility**: Each service handles one domain (Users or Products)
3. **API Gateway Pattern**: Centralized entry point for all client requests
4. **Service Discovery**: Services are accessible at known endpoints
5. **Health Monitoring**: Each service provides health status
6. **Loose Coupling**: Services communicate via HTTP/REST APIs
7. **Scalability**: Each service can be scaled independently
8. **Containerization**: Each service has its own Dockerfile
9. **Orchestration**: Docker Compose manages all services together
