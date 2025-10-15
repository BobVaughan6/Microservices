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

📊 **详细架构图和通信流程请查看 [ARCHITECTURE.md](ARCHITECTURE.md)**

## 微服务核心特性

本项目演示了以下微服务架构的核心特性：

### 1. 服务独立性 (Service Independence)
- ✅ 每个服务都是独立的应用程序
- ✅ 可以独立开发、部署和扩展
- ✅ 使用各自的端口运行（5000, 5001, 5002）

### 2. API Gateway 模式 (API Gateway Pattern)
- ✅ 统一的入口点（Port 5000）
- ✅ 请求路由到适当的后端服务
- ✅ 服务健康状态聚合

### 3. 服务间通信 (Inter-Service Communication)
- ✅ HTTP/REST API 通信
- ✅ 标准化的接口定义
- ✅ JSON 数据交换格式

### 4. 健康检查 (Health Checks)
- ✅ 每个服务提供 `/health` 端点
- ✅ API Gateway 聚合所有服务的健康状态
- ✅ 便于监控和故障诊断

### 5. 容器化 (Containerization)
- ✅ 每个服务都有独立的 Dockerfile
- ✅ 使用 Docker Compose 进行编排
- ✅ 一键启动整个系统

### 6. 单一职责原则 (Single Responsibility)
- ✅ User Service 专注于用户管理
- ✅ Product Service 专注于产品管理
- ✅ API Gateway 专注于路由和聚合

### 7. 松耦合 (Loose Coupling)
- ✅ 服务之间通过标准 HTTP API 通信
- ✅ 服务可以独立修改和部署
- ✅ 每个服务维护自己的数据

### 8. 可扩展性 (Scalability)
- ✅ 每个服务可以独立扩展
- ✅ 支持水平扩展
- ✅ 使用 Docker 便于部署多个实例

## 技术栈

- **.NET 9.0** - 应用框架
- **ASP.NET Core** - Web API
- **Docker** - 容器化
- **Docker Compose** - 服务编排

## 快速开始

### 前置要求

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)（可选，用于容器化部署）

### 方法 1: 使用启动脚本（推荐）

**Linux/MacOS:**
```bash
./start-all.sh
```

**Windows:**
```bash
start-all.bat
```

### 方法 2: 手动启动服务

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

### 方法 3: 使用 Docker Compose

```bash
docker-compose up --build
```

## 📚 API 文档（Scalar UI）

本项目集成了 **Scalar UI**，提供现代化的交互式 API 文档界面。

### 访问方式

启动服务后，在浏览器中打开：

**🎯 Scalar UI 界面:** http://localhost:5000/scalar/v1

**📄 OpenAPI 规范:** http://localhost:5000/openapi/v1.json

### 功能特性

- ✅ **美观的现代化 UI** - 比传统 Swagger UI 更优雅
- ✅ **交互式测试** - 直接在浏览器中调用 API
- ✅ **多语言代码示例** - 自动生成 C#、JavaScript、Python、cURL 等代码
- ✅ **深色模式支持** - 舒适的阅读体验
- ✅ **快速搜索** - 快速查找 API 端点
- ✅ **实时响应预览** - 即时查看 API 返回结果

### 使用步骤

1. 启动所有服务（使用 `start-all.bat` 或 `start-all.sh`）
2. 访问 http://localhost:5000/scalar/v1
3. 浏览左侧的 API 端点列表
4. 点击任意端点查看详情和测试

📖 **详细的 Scalar UI 使用指南请查看 [SCALAR_UI.md](SCALAR_UI.md)**

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

📖 **更多详细的 API 请求示例和测试场景请查看 [API_EXAMPLES.md](API_EXAMPLES.md)**

## 测试示例

### 使用测试脚本（推荐）

```bash
# 确保所有服务都在运行，然后执行
./test-services.sh
```

### 手动测试 - 使用 curl

```bash
# 通过 API Gateway 获取所有用户
curl http://localhost:5000/api/users

# 通过 API Gateway 获取所有产品
curl http://localhost:5000/api/products

# 检查系统健康状态
curl http://localhost:5000/health
```

### 手动测试 - 使用 PowerShell

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
├── start-all.sh             # Linux/MacOS 启动脚本
├── start-all.bat            # Windows 启动脚本
├── test-services.sh         # 测试脚本
└── README.md
```

## 扩展建议

1. **服务发现** - 添加 Consul 或 Eureka
2. **配置管理** - 使用配置中心（如 Consul、Azure App Configuration）
3. **日志聚合** - 集成 ELK Stack 或 Seq
4. **分布式追踪** - 添加 OpenTelemetry
5. ~~**API 文档** - 集成 Swagger/OpenAPI~~ ✅ **已完成 - 使用 Scalar UI**
6. **认证授权** - 实现 JWT 或 OAuth2
7. **消息队列** - 添加 RabbitMQ 或 Kafka 用于异步通信
8. **数据库** - 为每个服务添加独立的数据库
9. **缓存** - 添加 Redis
10. **负载均衡** - 使用 Nginx 或 Kubernetes Ingress

## 相关文档

- 📖 [系统架构](ARCHITECTURE.md) - 详细的架构图和通信流程
- 📖 [API 示例](API_EXAMPLES.md) - 详细的 API 请求示例和测试场景
- 📖 [Scalar UI 使用指南](SCALAR_UI.md) - API 文档界面使用说明
- 📖 [Scalar 快速参考](SCALAR_QUICK_REFERENCE.md) - 快速参考卡片
- 📋 [Scalar 更新日志](CHANGELOG_SCALAR.md) - Scalar UI 集成更新记录

## 许可证

MIT