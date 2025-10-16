// =============================================================================
// API Gateway (API 网关)
// =============================================================================
// 作用：作为微服务架构的统一入口点，负责请求路由、负载均衡、认证等功能
// 
// 架构变更：从 Minimal API 迁移到 Controller 模式
// 优势：
//   1. 更好的代码组织：使用控制器分离路由逻辑
//   2. 配置驱动：服务 URL 在 appsettings.json 中配置
//   3. 易于扩展：添加新服务只需创建新控制器
//   4. 易于测试：控制器可以轻松进行单元测试
// =============================================================================

using Scalar.AspNetCore;

// 创建 Web 应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// ========== 服务注册 ==========
// 添加控制器支持
builder.Services.AddControllers();

// 启用 OpenAPI 支持
builder.Services.AddOpenApi();

// 注册 HttpClientFactory
builder.Services.AddHttpClient();

// 构建应用程序
var app = builder.Build();

// ========== 配置 HTTP 请求管道 ==========
if (app.Environment.IsDevelopment())
{
    // 暴露 OpenAPI 规范端点
    app.MapOpenApi();
    
    // 添加 Scalar UI
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("API Gateway - 微服务网关")  // 设置页面标题
            .WithTheme(ScalarTheme.Mars)         // 使用默认主题（可选：Mars, Moon, Purple 等）
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient); // 默认显示 C# HttpClient 代码示例
    });
}

// 映射控制器路由
app.MapControllers();

// ========== 启动应用程序 ==========
app.Run();
