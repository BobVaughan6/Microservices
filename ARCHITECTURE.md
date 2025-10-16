# 微服务架构图

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

1. ## 通信流程（Communication Flow）

   ### 1. 客户端 → API 网关 → 用户服务

   ```
   客户端请求: GET http://localhost:5000/api/users
         ↓
   API 网关路由到: http://localhost:5001/api/users
         ↓
   用户服务处理并返回用户数据
         ↓
   API 网关将响应返回给客户端
   ```

   ------

   ### 2. 客户端 → API 网关 → 产品服务

   ```
   客户端请求: GET http://localhost:5000/api/products
         ↓
   API 网关路由到: http://localhost:5002/api/products
         ↓
   产品服务处理并返回产品数据
         ↓
   API 网关将响应返回给客户端
   ```

   ------

   ### 3. 健康检查聚合（Health Check Aggregation）

   ```
   客户端请求: GET http://localhost:5000/health
         ↓
   API 网关检查: http://localhost:5001/health
   API 网关检查: http://localhost:5002/health
         ↓
   API 网关聚合所有健康状态
         ↓
   将整体系统健康状态返回给客户端
   ```

   ------

   ## 微服务关键特性（Key Microservices Characteristics）

   1. **服务独立性**：每个服务独立运行在自己的端口上
   2. **单一职责原则**：每个服务只负责一个领域（用户或产品）
   3. **API 网关模式**：所有客户端请求的统一入口
   4. **服务发现机制**：服务通过已知端点进行访问
   5. **健康监测**：每个服务都提供健康检查接口
   6. **低耦合通信**：服务之间通过 HTTP/REST API 进行交互
   7. **可扩展性**：每个服务可独立扩容部署
   8. **容器化**：每个服务拥有独立的 Dockerfile
   9. **编排管理**：通过 Docker Compose 统一管理多个服务
