# Microservices

一个简单的微服务系统示例，展示微服务架构的核心特性。

## 系统架构

本项目包含三个微服务：

1. **API Gateway（API 网关）** - 端口 5000
   - 所有客户端请求的统一入口
   - 路由请求到相应的微服务
   - 提供服务健康检查

2. **User Service（用户服务）** - 端口 5001
   - 管理用户信息
   - 提供用户的 CRUD 操作

3. **Product Service（产品服务）** - 端口 5002
   - 管理产品信息
   - 提供产品的 CRUD 操作

## 微服务核心特性

### 1. 服务独立性
- 每个服务都是独立的应用程序
- 可以独立开发、部署和扩展
- 使用各自的端口运行

### 2. API Gateway 模式
- 统一的入口点
- 请求路由
- 服务聚合

### 3. 服务间通信
- HTTP/REST API
- 异步消息传递（可扩展）

### 4. 健康检查
- 每个服务提供 `/health` 端点
- API Gateway 聚合所有服务的健康状态

### 5. 容器化
- 每个服务都有独立的 Dockerfile
- 使用 Docker Compose 进行编排

## 技术栈

- **.NET 9.0** - 应用框架
- **ASP.NET Core** - Web API
- **Docker** - 容器化
- **Docker Compose** - 服务编排

## 快速开始

### 前置要求

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)（可选，用于容器化部署）

### 本地运行（不使用 Docker）

1. **启动 User Service**
```bash
cd src/UserService/UserService
dotnet run
```

2. **启动 Product Service**（新终端窗口）
```bash
cd src/ProductService/ProductService
dotnet run
```

3. **启动 API Gateway**（新终端窗口）
```bash
cd src/ApiGateway/ApiGateway
dotnet run
```

### 使用 Docker Compose 运行

```bash
docker-compose up --build
```

## API 端点

### API Gateway (http://localhost:5000)

#### 健康检查
```bash
GET http://localhost:5000/health
```

#### 用户相关
```bash
# 获取所有用户
GET http://localhost:5000/api/users

# 获取特定用户
GET http://localhost:5000/api/users/{id}
```

#### 产品相关
```bash
# 获取所有产品
GET http://localhost:5000/api/products

# 获取特定产品
GET http://localhost:5000/api/products/{id}
```

### 直接访问服务（用于测试）

#### User Service (http://localhost:5001)
```bash
GET http://localhost:5001/health
GET http://localhost:5001/api/users
GET http://localhost:5001/api/users/{id}
POST http://localhost:5001/api/users
```

#### Product Service (http://localhost:5002)
```bash
GET http://localhost:5002/health
GET http://localhost:5002/api/products
GET http://localhost:5002/api/products/{id}
POST http://localhost:5002/api/products
```

## 测试示例

### 使用 curl

```bash
# 通过 API Gateway 获取所有用户
curl http://localhost:5000/api/users

# 通过 API Gateway 获取所有产品
curl http://localhost:5000/api/products

# 检查系统健康状态
curl http://localhost:5000/health
```

### 使用 PowerShell

```powershell
# 获取所有用户
Invoke-RestMethod -Uri http://localhost:5000/api/users

# 获取所有产品
Invoke-RestMethod -Uri http://localhost:5000/api/products

# 检查健康状态
Invoke-RestMethod -Uri http://localhost:5000/health
```

## 项目结构

```
Microservices/
├── src/
│   ├── ApiGateway/          # API 网关服务
│   │   └── ApiGateway/
│   │       ├── Program.cs
│   │       ├── Dockerfile
│   │       └── ...
│   ├── UserService/         # 用户服务
│   │   └── UserService/
│   │       ├── Program.cs
│   │       ├── Dockerfile
│   │       └── ...
│   └── ProductService/      # 产品服务
│       └── ProductService/
│           ├── Program.cs
│           ├── Dockerfile
│           └── ...
├── docker-compose.yml       # Docker Compose 配置
└── README.md
```

## 扩展建议

1. **服务发现** - 添加 Consul 或 Eureka
2. **配置管理** - 使用配置中心（如 Consul、Azure App Configuration）
3. **日志聚合** - 集成 ELK Stack 或 Seq
4. **分布式追踪** - 添加 OpenTelemetry
5. **API 文档** - 集成 Swagger/OpenAPI
6. **认证授权** - 实现 JWT 或 OAuth2
7. **消息队列** - 添加 RabbitMQ 或 Kafka 用于异步通信
8. **数据库** - 为每个服务添加独立的数据库
9. **缓存** - 添加 Redis
10. **负载均衡** - 使用 Nginx 或 Kubernetes Ingress

## 许可证

MIT