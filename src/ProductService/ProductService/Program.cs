// =============================================================================
// Product Service (产品微服务)
// =============================================================================
// 作用：专门负责产品数据的管理，展示微服务的单一职责原则
// 
// 核心设计原则：
//   1. 单一职责：只处理产品相关的业务逻辑
//   2. 独立部署：可以独立于用户服务进行开发和部署
//   3. 数据独立：拥有自己的数据存储（遵循数据库分离原则）
//   4. 松耦合：通过 API 与其他服务通信，不直接依赖其他服务
//   5. 业务边界清晰：产品的增删改查都在此服务内完成
// 
// 微服务优势体现：
//   - 技术栈独立：可以选择不同于其他服务的技术
//   - 独立扩展：根据产品服务的负载独立扩展实例
//   - 故障隔离：产品服务故障不会影响用户服务
//   - 独立部署：可以独立更新产品功能而不影响其他服务
// 
// 未来扩展方向：
//   - 产品分类和标签管理
//   - 产品库存管理
//   - 产品图片和媒体管理
//   - 产品评价和评分系统
//   - 产品搜索引擎（Elasticsearch）
//   - 产品推荐算法
//   - 价格历史追踪
//   - 产品变体管理（尺寸、颜色等）
//   - 产品供应商关系
//   - 与订单服务集成
// =============================================================================

// 创建 Web 应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// ========== 服务注册 ==========
// AddOpenApi(): 启用 OpenAPI 支持，用于 API 文档和测试
builder.Services.AddOpenApi();

var app = builder.Build();

// ========== 配置 HTTP 请求管道 ==========
if (app.Environment.IsDevelopment())
{
    // 开发环境暴露 OpenAPI 端点
    app.MapOpenApi();
}

// ========== 数据存储 ==========
// 当前实现：内存存储（List<Product>）
// 
// 为什么使用内存存储？
//   - 简化演示：无需配置数据库
//   - 快速启动：减少学习曲线
//   - 突出重点：专注于微服务架构而非数据库配置
// 
// 内存存储的限制：
//   - 数据不持久：重启后数据丢失
//   - 无法扩展：多实例间数据不同步
//   - 并发问题：无锁机制，可能有竞态条件
//   - 容量限制：受内存大小限制
// 
// 生产环境数据库选择建议：
//   1. PostgreSQL：强大的关系型数据库，支持 JSON
//   2. MongoDB：文档型数据库，适合产品这种半结构化数据
//   3. MySQL：流行的关系型数据库
//   4. SQL Server：企业级数据库
// 
// 配合使用：
//   - Redis：缓存热门产品数据
//   - Elasticsearch：全文搜索和产品推荐
var products = new List<Product>
{
    new Product(1, "Laptop", "High-performance laptop", 999.99m),
    new Product(2, "Mouse", "Wireless mouse", 29.99m),
    new Product(3, "Keyboard", "Mechanical keyboard", 89.99m)
};

// ========== API 端点定义 (RESTful API Endpoints) ==========
// RESTful 设计原则：
//   1. 资源导向：URL 代表资源（/api/products）
//   2. HTTP 方法语义：GET（读取）、POST（创建）、PUT（更新）、DELETE（删除）
//   3. 无状态：每个请求包含所有必要信息
//   4. 统一接口：标准化的 HTTP 状态码和方法

// ===== 健康检查端点 =====
// 端点：GET /health
// 作用：服务健康监控，供 API Gateway、负载均衡器、K8s 使用
// 
// 在容器环境中的重要性：
//   - Docker：health check 配置
//   - Kubernetes：liveness probe 和 readiness probe
//   - 负载均衡器：决定是否将流量路由到此实例
//   - 监控系统：采集服务可用性指标
app.MapGet("/health", () => new { status = "healthy", service = "ProductService" })
    .WithName("HealthCheck");

// ===== 获取所有产品 =====
// 端点：GET /api/products
// 功能：返回产品目录中的所有产品
// 返回：200 OK + 产品数组
// 
// 查询示例：
//   curl http://localhost:5002/api/products
// 
// 未来功能扩展：
//   - 分页：GET /api/products?page=1&size=20
//   - 过滤：GET /api/products?category=electronics&minPrice=100
//   - 排序：GET /api/products?sort=price&order=desc
//   - 搜索：GET /api/products?q=laptop
//   - 字段选择：GET /api/products?fields=id,name,price
//   - 缓存：添加 ETag 或 Cache-Control 头
app.MapGet("/api/products", () => Results.Ok(products))
    .WithName("GetAllProducts");

// ===== 根据 ID 获取产品 =====
// 端点：GET /api/products/{id}
// 参数：id (int) - 产品唯一标识符
// 返回：
//   - 200 OK + 产品对象：产品存在
//   - 404 Not Found：产品不存在
// 
// 查询示例：
//   curl http://localhost:5002/api/products/1
// 
// 错误处理：
//   - 使用适当的 HTTP 状态码（404 表示资源不存在）
//   - 清晰的错误响应
// 
// 性能优化建议：
//   - 添加缓存：热门产品可以缓存到 Redis
//   - 数据库索引：在 ID 字段建立主键索引
app.MapGet("/api/products/{id}", (int id) =>
{
    // LINQ 查询：FirstOrDefault 返回第一个匹配或 null
    var product = products.FirstOrDefault(p => p.Id == id);
    // 三元表达式：根据查询结果返回不同的 HTTP 响应
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById");

// ===== 创建新产品 =====
// 端点：POST /api/products
// 请求体：Product 对象（JSON 格式，Id 会被系统重新分配）
// 返回：201 Created + 新产品对象 + Location 头
// 
// HTTP 201 Created 语义：
//   - 表示成功创建了新资源
//   - Location 头指向新资源的 URI
//   - 响应体包含完整的新资源
// 
// 请求示例：
//   curl -X POST http://localhost:5002/api/products \
//     -H "Content-Type: application/json" \
//     -d '{
//       "id": 0,
//       "name": "Monitor",
//       "description": "4K Display",
//       "price": 299.99
//     }'
// 
// 数据验证建议：
//   - 检查必填字段（Name、Price）
//   - 验证数据类型和格式
//   - 验证价格范围（Price > 0）
//   - 检查名称长度限制
//   - 防止 SQL 注入（使用参数化查询）
//   - 防止 XSS 攻击（清理输入）
// 
// 未来改进：
//   - 使用数据注解（[Required]、[Range]）进行验证
//   - 返回详细的验证错误信息
//   - 实现幂等性（避免重复创建）
//   - 添加创建时间戳
//   - 记录操作日志
//   - 发送事件通知（新产品创建事件）
app.MapPost("/api/products", (Product product) =>
{
    // 生成新 ID：找到当前最大 ID 并加 1
    // 注意：这种方式在并发环境下不安全，数据库通常使用自增 ID 或 GUID
    var newProduct = product with { Id = products.Max(p => p.Id) + 1 };
    
    // 添加到产品列表
    products.Add(newProduct);
    
    // 返回 201 Created 响应
    // RFC 7231: Location 头应包含新创建资源的 URI
    return Results.Created($"/api/products/{newProduct.Id}", newProduct);
})
.WithName("CreateProduct");

// ========== 启动应用程序 ==========
// 开始监听 HTTP 请求
app.Run();

// ========== 数据模型 (Domain Model) ==========

/// <summary>
/// 产品实体
/// 使用 C# Record 类型定义领域模型
/// </summary>
/// 
/// Record 优势：
///   1. 不可变性：避免意外修改，线程安全
///   2. 值语义：根据内容比较而非引用
///   3. 简洁：自动生成样板代码
///   4. With 表达式：方便创建修改后的副本
/// 
/// 属性说明：
///   - Id (int): 产品唯一标识符，数据库主键
///   - Name (string): 产品名称，应该必填且有长度限制
///   - Description (string): 产品描述，详细说明产品特性
///   - Price (decimal): 产品价格，使用 decimal 类型保证精度
/// 
/// 为什么使用 decimal 而不是 double？
///   - decimal: 128 位，适合货币计算，无浮点误差
///   - double: 64 位，浮点数，有精度问题，不适合金额
///   例如：0.1 + 0.2 用 double 可能不等于 0.3
/// 
/// 未来扩展属性：
///   - CategoryId: 产品分类
///   - SKU: 库存单位编码
///   - Stock: 库存数量
///   - ImageUrl: 产品图片链接
///   - Tags: 产品标签数组
///   - IsActive: 是否上架
///   - CreatedAt: 创建时间
///   - UpdatedAt: 更新时间
///   - Rating: 平均评分
///   - ReviewCount: 评价数量
record Product(int Id, string Name, string Description, decimal Price);

