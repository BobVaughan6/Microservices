// =============================================================================
// API Gateway (API 网关)
// =============================================================================
// 作用：作为微服务架构的统一入口点，负责请求路由、负载均衡、认证等功能
// 核心概念：
//   1. 单一入口点：所有客户端请求通过网关访问后端服务
//   2. 请求路由：根据URL路径将请求转发到相应的微服务
//   3. 服务聚合：可以聚合多个微服务的响应返回给客户端
//   4. 横切关注点：统一处理认证、日志、限流、监控等
// 
// 未来扩展方向：
//   - 添加认证授权（JWT、OAuth2）
//   - 实现请求限流和熔断器模式（Polly）
//   - 添加缓存层（Redis）
//   - 实现负载均衡
//   - 添加API版本控制
//   - 集成服务发现（Consul、Eureka）
//   - 添加请求/响应日志
//   - 实现请求转换和响应聚合
// =============================================================================

// ========== 命名空间引用 ==========
// 引入 Scalar UI 所需的命名空间，用于提供交互式 API 文档界面
using Scalar.AspNetCore;

// 创建 Web 应用程序构建器，用于配置服务和中间件
var builder = WebApplication.CreateBuilder(args);

// ========== 服务注册 (Dependency Injection) ==========
// 将服务添加到依赖注入容器中，供整个应用程序使用

// AddOpenApi(): 启用 OpenAPI 支持，用于生成 API 文档 (OpenAPI Specification)
// OpenAPI 是一个标准的 RESTful API 规范，用于描述 API 的结构
// 生成的文档可以被各种工具使用（Swagger UI、Scalar、Postman 等）
builder.Services.AddOpenApi();

// AddHttpClient(): 注册 IHttpClientFactory，用于创建和管理 HttpClient 实例
// 好处：避免 Socket 耗尽问题，自动管理连接池，支持 Polly 集成
// 未来扩展：可以配置超时、重试策略、熔断器等
builder.Services.AddHttpClient();

// ========== 构建应用程序 ==========
var app = builder.Build();

// ========== 配置 HTTP 请求管道 (Middleware Pipeline) ==========
// 中间件按照添加的顺序执行，处理每个 HTTP 请求

// ========== API 文档和交互式 UI 配置 ==========
// 在开发环境中启用 OpenAPI 端点和 Scalar UI
// Scalar 是一个现代化、美观的 API 文档和测试工具，相比 Swagger UI 更加优雅
// 
// 功能特性：
//   1. 美观的现代化 UI 设计
//   2. 交互式 API 测试（可直接在浏览器中调用 API）
//   3. 支持深色模式
//   4. 快速搜索和导航
//   5. 代码生成（多种语言的客户端代码示例）
//   6. 实时响应预览
//   7. 请求历史记录
// 
// 访问地址：
//   - OpenAPI 规范文档: http://localhost:5000/openapi/v1.json
//   - Scalar UI 界面: http://localhost:5000/scalar/v1
// 
// 使用场景：
//   1. API 开发和调试：实时测试 API 端点
//   2. API 文档：自动生成的交互式文档
//   3. 团队协作：分享 API 文档给前端开发者
//   4. API 设计审查：可视化 API 结构
// 
// 生产环境建议：
//   - 考虑是否需要在生产环境暴露 API 文档
//   - 如果需要，可以添加认证保护
//   - 或者使用环境变量控制是否启用
if (app.Environment.IsDevelopment())
{
    // 暴露 OpenAPI 规范端点：/openapi/v1.json
    // 该端点返回 API 的完整 OpenAPI 规范（JSON 格式）
    app.MapOpenApi();
    
    // 添加 Scalar UI 中间件
    // 提供一个交互式的 Web 界面来浏览和测试 API
    // 参数说明：
    //   - options.WithTitle(): 设置文档标题
    //   - options.WithTheme(): 设置 UI 主题（ScalarTheme.Default, Mars, Moon, Purple, BluePlanet, Saturn, Kepler, DeepSpace）
    //   - options.WithDefaultHttpClient(): 设置默认的 HTTP 客户端（ScalarTarget.CSharp, JavaScript, Python 等）
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("API Gateway - 微服务网关")  // 设置页面标题
            .WithTheme(ScalarTheme.Mars)         // 使用默认主题（可选：Mars, Moon, Purple 等）
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient); // 默认显示 C# HttpClient 代码示例
    });
}

// 从依赖注入容器获取 HttpClientFactory
// 用于创建 HttpClient 实例来调用后端微服务
var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();

// ========== 后端服务 URL 配置 ==========
// 生产环境建议：
//   1. 将这些 URL 移到配置文件（appsettings.json）
//   2. 使用环境变量进行配置
//   3. 集成服务发现（Consul/Eureka）自动获取服务地址
//   4. 使用 Docker 网络或 Kubernetes 服务名称
var userServiceUrl = "http://localhost:5001";       // 用户服务地址
var productServiceUrl = "http://localhost:5002";    // 产品服务地址

// ========== 健康检查端点 (Health Check Endpoint) ==========
// 作用：监控系统整体健康状态，检查所有依赖的后端服务是否正常运行
// 返回值：
//   - healthy: 所有服务都正常
//   - degraded: 部分服务不可用，但网关仍可提供部分功能
// 
// 使用场景：
//   1. 负载均衡器用来判断是否将流量路由到该实例
//   2. 监控系统（Prometheus、Grafana）采集健康状态
//   3. Kubernetes 存活探针和就绪探针
// 
// 未来扩展：
//   - 添加数据库连接检查
//   - 添加缓存服务检查（Redis）
//   - 添加消息队列检查（RabbitMQ、Kafka）
//   - 返回更详细的健康信息（响应时间、错误率等）
//   - 实现不同级别的健康检查（浅检查/深检查）
app.MapGet("/health", async () =>
{
    // 创建 HTTP 客户端用于调用后端服务的健康检查端点
    var client = httpClientFactory.CreateClient();
    
    // 并行检查各个后端服务的健康状态
    var userHealth = await CheckServiceHealth(client, userServiceUrl);
    var productHealth = await CheckServiceHealth(client, productServiceUrl);
    
    // 返回聚合的健康状态
    return new
    {
        // 只有所有服务都健康时，整体状态才是 healthy
        status = userHealth && productHealth ? "healthy" : "degraded",
        service = "ApiGateway",
        services = new
        {
            userService = userHealth ? "healthy" : "unhealthy",
            productService = productHealth ? "healthy" : "unhealthy"
        }
    };
})
.WithName("HealthCheck"); // 为端点命名，便于日志记录和监控

// ========== 用户服务路由 (User Service Routes) ==========
// API Gateway 的核心功能：请求路由和转发
// 
// 路由模式：反向代理模式
//   1. 客户端请求 API Gateway
//   2. API Gateway 转发请求到后端服务
//   3. 后端服务处理并返回响应
//   4. API Gateway 将响应返回给客户端
// 
// 未来扩展：
//   - 实现请求/响应转换（数据格式转换、字段映射）
//   - 添加请求验证和参数校验
//   - 实现响应缓存减少后端压力
//   - 添加请求聚合（组合多个服务的响应）
//   - 实现断路器模式（Circuit Breaker）防止级联故障
//   - 添加重试机制处理临时故障
//   - 实现超时控制
//   - 添加请求日志和监控指标

// 获取所有用户
// 端点：GET /api/users
// 功能：将请求转发到用户服务，获取用户列表
app.MapGet("/api/users", async () =>
{
    var client = httpClientFactory.CreateClient();
    // 转发 GET 请求到用户服务
    var response = await client.GetAsync($"{userServiceUrl}/api/users");
    // 读取响应内容并返回给客户端
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetUsers");

// 根据 ID 获取特定用户
// 端点：GET /api/users/{id}
// 参数：id - 用户唯一标识符
// 功能：将请求转发到用户服务，获取指定用户信息
// 错误处理：如果用户不存在，返回 404 Not Found
app.MapGet("/api/users/{id}", async (int id) =>
{
    var client = httpClientFactory.CreateClient();
    // 转发 GET 请求到用户服务，包含用户 ID
    var response = await client.GetAsync($"{userServiceUrl}/api/users/{id}");
    
    // 处理错误响应：如果后端服务返回非成功状态码，网关也返回相应的错误
    if (!response.IsSuccessStatusCode)
        return Results.NotFound();
        
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetUserById");

// ========== 产品服务路由 (Product Service Routes) ==========
// 类似用户服务路由，提供产品相关的 API 转发功能

// 获取所有产品
// 端点：GET /api/products
// 功能：将请求转发到产品服务，获取产品列表
app.MapGet("/api/products", async () =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"{productServiceUrl}/api/products");
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetProducts");

// 根据 ID 获取特定产品
// 端点：GET /api/products/{id}
// 参数：id - 产品唯一标识符
// 功能：将请求转发到产品服务，获取指定产品信息
app.MapGet("/api/products/{id}", async (int id) =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"{productServiceUrl}/api/products/{id}");
    if (!response.IsSuccessStatusCode)
        return Results.NotFound();
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetProductById");

// ========== 启动应用程序 ==========
// 开始监听 HTTP 请求
// 默认监听端口在 launchSettings.json 或 ASPNETCORE_URLS 环境变量中配置
app.Run();

// ========== 辅助方法 ==========

/// <summary>
/// 检查指定微服务的健康状态
/// </summary>
/// <param name="client">HTTP 客户端</param>
/// <param name="serviceUrl">服务的基础 URL</param>
/// <returns>如果服务健康返回 true，否则返回 false</returns>
/// 
/// 工作原理：
///   1. 向服务的 /health 端点发送 GET 请求
///   2. 如果返回成功状态码（200-299），认为服务健康
///   3. 如果请求失败或超时，认为服务不健康
/// 
/// 未来改进：
///   - 添加超时配置
///   - 实现重试逻辑
///   - 缓存健康检查结果，避免频繁检查
///   - 返回更详细的健康信息（响应时间等）
async Task<bool> CheckServiceHealth(HttpClient client, string serviceUrl)
{
    try
    {
        // 调用服务的健康检查端点
        var response = await client.GetAsync($"{serviceUrl}/health");
        // 检查是否返回成功状态码（2xx）
        return response.IsSuccessStatusCode;
    }
    catch
    {
        // 任何异常（网络错误、超时等）都认为服务不健康
        return false;
    }
}
