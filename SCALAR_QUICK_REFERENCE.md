# Scalar UI 快速参考

## 🚀 快速启动

```bash
# 1. 启动所有服务
cd f:\Test\Microservices
start-all.bat

# 2. 访问 Scalar UI
浏览器打开: http://localhost:5000/scalar/v1
```

## 🎯 关键端点

| 端点 | 地址 | 说明 |
|------|------|------|
| Scalar UI | http://localhost:5000/scalar/v1 | 交互式 API 文档界面 |
| OpenAPI 规范 | http://localhost:5000/openapi/v1.json | API 规范文档（JSON） |
| 健康检查 | http://localhost:5000/health | 系统健康状态 |
| 用户列表 | http://localhost:5000/api/users | 获取所有用户 |
| 产品列表 | http://localhost:5000/api/products | 获取所有产品 |

## 🎨 可用主题

在 `Program.cs` 中修改 `.WithTheme()` 参数：

```csharp
ScalarTheme.Default    // 默认主题
ScalarTheme.Mars       // 火星红色
ScalarTheme.Moon       // 月光蓝色
ScalarTheme.Purple     // 紫色
ScalarTheme.BluePlanet // 蓝色星球
ScalarTheme.Saturn     // 土星
ScalarTheme.Kepler     // 开普勒
ScalarTheme.DeepSpace  // 深空
```

## 🔧 配置选项

```csharp
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("自定义标题")           // 设置页面标题
        .WithTheme(ScalarTheme.Default)    // 设置主题
        .WithDefaultHttpClient(             // 设置默认代码示例语言
            ScalarTarget.CSharp,            // 目标语言
            ScalarClient.HttpClient         // HTTP 客户端类型
        );
});
```

### 支持的代码目标（ScalarTarget）
- `ScalarTarget.CSharp`
- `ScalarTarget.JavaScript`
- `ScalarTarget.Python`
- `ScalarTarget.Shell` (cURL)
- `ScalarTarget.PHP`
- `ScalarTarget.Go`
- `ScalarTarget.Ruby`

## 📝 使用步骤

### 1. 浏览 API
- 左侧面板显示所有端点
- 点击端点查看详情
- 查看请求/响应格式

### 2. 测试 API
1. 选择端点
2. 填写参数
3. 点击 "Send Request"
4. 查看响应结果

### 3. 生成代码
1. 查看 "Code Examples" 部分
2. 选择编程语言
3. 复制代码到项目

## 🛠️ 故障排查

### Scalar UI 无法访问
```bash
# 检查服务是否运行
netstat -ano | findstr :5000

# 重启服务
cd src\ApiGateway\ApiGateway
dotnet run
```

### API 调用失败
```bash
# 检查后端服务
# UserService 应该在端口 5001
# ProductService 应该在端口 5002

# 测试健康检查
curl http://localhost:5000/health
```

## 📦 已安装的包

```xml
<PackageReference Include="Scalar.AspNetCore" Version="2.9.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.9" />
```

## 🎓 学习资源

- 📖 [完整文档](./SCALAR_UI.md)
- 📋 [更新日志](./CHANGELOG_SCALAR.md)
- 🌐 [Scalar 官网](https://github.com/scalar/scalar)

## ⚡ 常用代码片段

### C# 调用示例
```csharp
var client = new HttpClient();
var response = await client.GetAsync("http://localhost:5000/api/users");
var users = await response.Content.ReadAsStringAsync();
```

### JavaScript 调用示例
```javascript
fetch('http://localhost:5000/api/users')
  .then(res => res.json())
  .then(data => console.log(data));
```

### Python 调用示例
```python
import requests
response = requests.get('http://localhost:5000/api/users')
users = response.json()
```

### cURL 调用示例
```bash
curl -X GET "http://localhost:5000/api/users"
```

## 💡 提示

- 💡 使用快捷键 `/` 快速搜索 API
- 💡 点击主题图标切换深色/浅色模式
- 💡 使用 "Try It" 功能直接测试 API
- 💡 查看 "Models" 部分了解数据结构
- 💡 响应示例会自动生成，无需手动编写

---

**需要帮助？** 查看完整文档 [SCALAR_UI.md](./SCALAR_UI.md)
