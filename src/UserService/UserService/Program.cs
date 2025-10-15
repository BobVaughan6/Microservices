// =============================================================================
// User Service (用户微服务)
// =============================================================================
// 作用：专门负责用户数据的管理，遵循微服务的单一职责原则
// 
// 核心设计原则：
//   1. 单一职责：只处理用户相关的业务逻辑
//   2. 独立部署：可以独立开发、测试、部署和扩展
//   3. 数据独立：拥有自己的数据存储（当前是内存，未来应使用独立数据库）
//   4. 松耦合：通过 RESTful API 与其他服务通信
//   5. 高内聚：所有用户相关功能集中在此服务
// 
// 当前实现：简化的内存存储版本
// 生产环境建议：
//   1. 使用独立数据库（PostgreSQL、MySQL、MongoDB）
//   2. 实现数据访问层（Repository 模式）
//   3. 添加业务逻辑层（Service 层）
//   4. 实现输入验证和错误处理
//   5. 添加日志记录
//   6. 实现认证和授权
//   7. 添加数据持久化
//   8. 实现分页、搜索、过滤功能
// 
// 未来扩展方向：
//   - 用户认证（密码哈希、JWT Token）
//   - 用户权限管理（RBAC）
//   - 用户资料扩展（头像、地址、电话等）
//   - 邮箱验证和密码重置
//   - 用户活动日志
//   - 集成第三方登录（OAuth2）
//   - 用户偏好设置
//   - 用户关系管理（好友、关注）
// =============================================================================

// 创建 Web 应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// ========== 服务注册 ==========
// AddOpenApi(): 启用 OpenAPI/Swagger 支持，方便 API 文档生成和测试
builder.Services.AddOpenApi();

var app = builder.Build();

// ========== 配置 HTTP 请求管道 ==========
if (app.Environment.IsDevelopment())
{
    // 在开发环境暴露 OpenAPI 规范端点
    app.MapOpenApi();
}

// ========== 数据存储 ==========
// 当前使用内存存储（List）用于演示
// 
// 注意：
//   - 重启服务后数据会丢失
//   - 不支持并发控制（多线程访问可能有问题）
//   - 无法扩展到多个实例（每个实例有独立的内存）
// 
// 生产环境替代方案：
//   1. 关系型数据库：PostgreSQL、MySQL、SQL Server
//      - 使用 Entity Framework Core ORM
//      - 支持事务、关系、约束
//   2. NoSQL 数据库：MongoDB、CosmosDB
//      - 适合文档型数据
//      - 易于扩展
//   3. 分布式缓存：Redis
//      - 配合数据库使用
//      - 提高读取性能
var users = new List<User>
{
    new User(1, "Alice", "alice@example.com"),
    new User(2, "Bob", "bob@example.com"),
    new User(3, "Charlie", "charlie@example.com")
};

// ========== API 端点定义 (API Endpoints) ==========
// 使用 ASP.NET Core Minimal API 风格
// 优点：简洁、高性能、易于理解
// 适合：简单的微服务、快速原型开发

// ===== 健康检查端点 =====
// 端点：GET /health
// 作用：让外部系统（负载均衡器、监控工具、容器编排器）检查服务状态
// 返回：简单的 JSON 对象表示服务健康状态
// 
// Kubernetes 使用示例：
//   livenessProbe:  # 存活探针
//     httpGet:
//       path: /health
//       port: 5001
//   readinessProbe: # 就绪探针
//     httpGet:
//       path: /health
//       port: 5001
app.MapGet("/health", () => new { status = "healthy", service = "UserService" })
    .WithName("HealthCheck"); // 端点命名，用于 OpenAPI 文档和日志

// ===== 获取所有用户 =====
// 端点：GET /api/users
// 功能：返回所有用户列表
// 返回：200 OK + 用户数组 JSON
// 
// 未来改进：
//   - 实现分页（?page=1&pageSize=10）
//   - 添加排序（?sortBy=name&order=asc）
//   - 添加搜索（?search=alice）
//   - 添加字段过滤（?fields=id,name）
//   - 实现缓存减少数据库查询
app.MapGet("/api/users", () => Results.Ok(users))
    .WithName("GetAllUsers");

// ===== 根据 ID 获取用户 =====
// 端点：GET /api/users/{id}
// 参数：id (int) - 用户的唯一标识符
// 返回：
//   - 200 OK + 用户对象：用户存在
//   - 404 Not Found：用户不存在
// 
// 查询方法：FirstOrDefault - LINQ 方法，返回第一个匹配项或 null
// 
// 未来改进：
//   - 添加请求日志记录
//   - 实现结果缓存
//   - 添加字段投影（只返回需要的字段）
//   - 返回更详细的错误信息
app.MapGet("/api/users/{id}", (int id) =>
{
    // 在用户列表中查找指定 ID 的用户
    var user = users.FirstOrDefault(u => u.Id == id);
    // 使用 C# 9.0 模式匹配语法返回结果
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUserById");

// ===== 创建新用户 =====
// 端点：POST /api/users
// 请求体：User 对象的 JSON（Id 字段会被忽略，系统自动分配）
// 返回：201 Created + 新用户对象 + Location 头（指向新资源的 URL）
// 
// RESTful 最佳实践：
//   - 使用 POST 创建资源
//   - 返回 201 状态码
//   - 在 Location 头中包含新资源的 URI
//   - 在响应体中返回完整的资源对象
// 
// 请求示例：
//   POST /api/users
//   Content-Type: application/json
//   {
//     "id": 0,
//     "name": "David",
//     "email": "david@example.com"
//   }
// 
// 未来改进：
//   - 添加数据验证（邮箱格式、必填字段等）
//   - 检查邮箱唯一性
//   - 密码加密（如果包含密码）
//   - 发送欢迎邮件
//   - 记录审计日志
//   - 返回详细的验证错误信息
//   - 使用 FluentValidation 进行复杂验证
app.MapPost("/api/users", (User user) =>
{
    // 自动生成新 ID：当前最大 ID + 1
    // 注意：生产环境应使用数据库的自增 ID 或 UUID
    var newUser = user with { Id = users.Max(u => u.Id) + 1 };
    
    // 添加到用户列表
    users.Add(newUser);
    
    // 返回 201 Created 响应
    // 第一个参数：新资源的 URI
    // 第二个参数：创建的资源对象
    return Results.Created($"/api/users/{newUser.Id}", newUser);
})
.WithName("CreateUser");

// ========== 启动应用程序 ==========
app.Run();

// ========== 数据模型 (Data Model) ==========

/// <summary>
/// 用户实体
/// 使用 C# 9.0 Record 类型定义不可变数据对象
/// </summary>
/// 
/// Record 特点：
///   1. 不可变（Immutable）：创建后不能修改，保证数据一致性
///   2. 值相等性：基于属性值比较，而不是引用
///   3. 简洁语法：自动生成构造函数、ToString、Equals、GetHashCode
///   4. With 表达式：可以创建修改了部分属性的新副本
/// 
/// 属性说明：
///   - Id: 用户唯一标识符（主键）
///   - Name: 用户姓名
///   - Email: 用户邮箱地址
/// 
/// 未来扩展：
///   - 添加密码（PasswordHash）
///   - 添加创建时间（CreatedAt）
///   - 添加更新时间（UpdatedAt）
///   - 添加用户状态（Active、Suspended、Deleted）
///   - 添加用户角色（Role）
///   - 添加更多个人信息（电话、地址、生日等）
///   - 添加验证属性（EmailConfirmed）
record User(int Id, string Name, string Email);

