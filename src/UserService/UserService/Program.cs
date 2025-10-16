// =============================================================================
// User Service (用户微服务)
// =============================================================================
// 作用：专门负责用户数据的管理，遵循微服务的单一职责原则
// 
// 架构变更：从 Minimal API 迁移到 Controller 模式
// 优势：
//   1. 更好的代码组织：控制器将相关端点分组
//   2. 易于测试：控制器可以轻松进行单元测试
//   3. 符合传统 MVC 模式：团队熟悉的开发模式
//   4. 更好的可维护性：随着端点增加，代码更清晰
// =============================================================================

// 创建 Web 应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// ========== 服务注册 ==========
// 添加控制器支持
builder.Services.AddControllers();

// 启用 OpenAPI/Swagger 支持
builder.Services.AddOpenApi();

var app = builder.Build();

// ========== 配置 HTTP 请求管道 ==========
if (app.Environment.IsDevelopment())
{
    // 在开发环境暴露 OpenAPI 规范端点
    app.MapOpenApi();
}

// 映射控制器路由
app.MapControllers();

// ========== 启动应用程序 ==========
app.Run();

