# Scalar UI - API 文档使用指南

## 什么是 Scalar UI？

Scalar 是一个现代化、美观的 API 文档和测试工具，为开发者提供交互式的 API 探索体验。相比传统的 Swagger UI，Scalar 提供了更加优雅的界面设计和更强大的功能。

## 功能特性

### 🎨 美观的用户界面
- 现代化的设计风格
- 支持浅色/深色主题
- 响应式布局，适配各种屏幕尺寸
- 流畅的动画和交互效果

### 🚀 强大的功能
1. **交互式 API 测试** - 直接在浏览器中调用 API，无需 Postman
2. **代码生成** - 自动生成多种编程语言的客户端代码示例
3. **快速搜索** - 快速查找和导航 API 端点
4. **请求历史** - 保存和查看之前的 API 调用
5. **实时响应预览** - 即时查看 API 响应结果
6. **认证支持** - 支持 Bearer Token、API Key 等多种认证方式

## 如何使用

### 1. 启动 API Gateway

确保 API Gateway 正在开发环境中运行：

```bash
cd src/ApiGateway/ApiGateway
dotnet run
```

或者使用项目根目录的启动脚本：

```bash
# Windows
start-all.bat

# Linux/Mac
./start-all.sh
```

### 2. 访问 Scalar UI

在浏览器中打开以下地址：

**Scalar UI 界面：** http://localhost:5000/scalar/v1

**OpenAPI 规范文档：** http://localhost:5000/openapi/v1.json

### 3. 探索和测试 API

#### 浏览 API 端点
- 左侧面板显示所有可用的 API 端点
- 点击任意端点查看详细信息
- 查看请求参数、响应格式、状态码等

#### 测试 API
1. 选择要测试的 API 端点
2. 填写必要的参数（路径参数、查询参数、请求体等）
3. 点击 "Send Request" 按钮
4. 查看响应结果（状态码、响应头、响应体）

#### 生成代码示例
1. 在 API 端点详情页面
2. 查看 "Code Examples" 部分
3. 选择编程语言（C#、JavaScript、Python、cURL 等）
4. 复制生成的代码到你的项目中使用

## 可用的 API 端点

### 健康检查
- **GET /health** - 检查 API Gateway 和所有后端服务的健康状态

### 用户服务
- **GET /api/users** - 获取所有用户列表
- **GET /api/users/{id}** - 根据 ID 获取特定用户

### 产品服务
- **GET /api/products** - 获取所有产品列表
- **GET /api/products/{id}** - 根据 ID 获取特定产品

## 主题配置

Scalar 支持多种主题，可以在 `Program.cs` 中配置：

```csharp
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("API Gateway - 微服务网关")
        .WithTheme(ScalarTheme.Default) // 可选主题
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});
```

### 可用主题
- `ScalarTheme.Default` - 默认主题
- `ScalarTheme.Mars` - 火星红色主题
- `ScalarTheme.Moon` - 月光蓝色主题
- `ScalarTheme.Purple` - 紫色主题
- `ScalarTheme.BluePlanet` - 蓝色星球主题
- `ScalarTheme.Saturn` - 土星主题
- `ScalarTheme.Kepler` - 开普勒主题
- `ScalarTheme.DeepSpace` - 深空主题

## 代码示例

### C# HttpClient 示例

```csharp
using System.Net.Http;
using System.Threading.Tasks;

var client = new HttpClient();
var response = await client.GetAsync("http://localhost:5000/api/users");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);
```

### JavaScript Fetch 示例

```javascript
fetch('http://localhost:5000/api/users')
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### Python Requests 示例

```python
import requests

response = requests.get('http://localhost:5000/api/users')
data = response.json()
print(data)
```

### cURL 示例

```bash
curl -X GET "http://localhost:5000/api/users" -H "accept: application/json"
```

## 生产环境注意事项

### 安全性
- 在生产环境中，考虑是否需要暴露 API 文档
- 如果需要暴露，建议添加认证保护
- 可以使用环境变量控制是否启用 Scalar UI

### 配置示例

```csharp
// 只在开发和测试环境启用
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// 或者使用配置控制
if (builder.Configuration.GetValue<bool>("EnableApiDocumentation"))
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
```

## 故障排查

### Scalar UI 无法访问
1. 确保 API Gateway 正在运行
2. 检查是否在开发环境中运行（`ASPNETCORE_ENVIRONMENT=Development`）
3. 验证端口号是否正确（默认 5000）
4. 查看控制台输出是否有错误信息

### API 调用失败
1. 检查后端服务（UserService、ProductService）是否正在运行
2. 验证服务 URL 配置是否正确
3. 检查防火墙或安全软件是否阻止了连接
4. 查看浏览器控制台的网络请求

### OpenAPI 文档不完整
1. 确保 API 端点使用了 `.WithName()` 方法
2. 添加更多的 OpenAPI 元数据（使用属性或 `.WithOpenApi()` 方法）
3. 检查是否有编译警告或错误

## 扩展阅读

- [Scalar 官方文档](https://github.com/scalar/scalar)
- [OpenAPI 规范](https://swagger.io/specification/)
- [ASP.NET Core OpenAPI 支持](https://learn.microsoft.com/aspnet/core/fundamentals/openapi)

## 总结

Scalar UI 为您的 API Gateway 提供了专业、现代化的文档界面，让 API 开发和测试变得更加高效和愉悦。通过交互式的方式探索 API，开发者可以更快地理解和使用您的微服务。

Happy Coding! 🚀
