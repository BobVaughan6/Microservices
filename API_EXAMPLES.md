# API 请求示例

本文件包含用于测试微服务系统的 HTTP 请求示例。

------

## API 网关端点（端口 5000）

### 健康检查

```
GET http://localhost:5000/health
```

------

### 用户接口（User Endpoints）

#### 获取所有用户

```
GET http://localhost:5000/api/users
```

示例响应：

```
[
  {
    "id": 1,
    "name": "Alice",
    "email": "alice@example.com"
  },
  {
    "id": 2,
    "name": "Bob",
    "email": "bob@example.com"
  },
  {
    "id": 3,
    "name": "Charlie",
    "email": "charlie@example.com"
  }
]
```

#### 根据 ID 获取用户

```
GET http://localhost:5000/api/users/1
```

示例响应：

```
{
  "id": 1,
  "name": "Alice",
  "email": "alice@example.com"
}
```

------

### 产品接口（Product Endpoints）

#### 获取所有产品

```
GET http://localhost:5000/api/products
```

示例响应：

```
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "高性能笔记本电脑",
    "price": 999.99
  },
  {
    "id": 2,
    "name": "Mouse",
    "description": "无线鼠标",
    "price": 29.99
  },
  {
    "id": 3,
    "name": "Keyboard",
    "description": "机械键盘",
    "price": 89.99
  }
]
```

#### 根据 ID 获取产品

```
GET http://localhost:5000/api/products/2
```

示例响应：

```
{
  "id": 2,
  "name": "Mouse",
  "description": "无线鼠标",
  "price": 29.99
}
```

------

## 直接访问服务（用于测试）

### 用户服务（端口 5001）

#### 健康检查

```
GET http://localhost:5001/health
```

#### 获取所有用户

```
GET http://localhost:5001/api/users
```

#### 根据 ID 获取用户

```
GET http://localhost:5001/api/users/1
```

#### 创建新用户

```
POST http://localhost:5001/api/users
Content-Type: application/json

{
  "id": 0,
  "name": "David",
  "email": "david@example.com"
}
```

------

### 产品服务（端口 5002）

#### 健康检查

```
GET http://localhost:5002/health
```

#### 获取所有产品

```
GET http://localhost:5002/api/products
```

#### 根据 ID 获取产品

```
GET http://localhost:5002/api/products/1
```

#### 创建新产品

```
POST http://localhost:5002/api/products
Content-Type: application/json

{
  "id": 0,
  "name": "Monitor",
  "description": "4K 超高清显示器",
  "price": 499.99
}
```

------

## 使用 curl 测试

### 基础 GET 请求

```
curl http://localhost:5000/api/users
```

### 美化输出（使用 jq）

```
curl -s http://localhost:5000/api/users | jq
```

### 使用 Python JSON 工具美化输出

```
curl -s http://localhost:5000/api/users | python3 -m json.tool
```

### POST 请求示例

```
curl -X POST http://localhost:5001/api/users \
  -H "Content-Type: application/json" \
  -d '{"id":0,"name":"Eve","email":"eve@example.com"}'
```

------

## 使用 PowerShell 测试

### GET 请求

```
Invoke-RestMethod -Uri http://localhost:5000/api/users
```

### POST 请求

```
$user = @{
    id = 0
    name = "Eve"
    email = "eve@example.com"
} | ConvertTo-Json

Invoke-RestMethod -Uri http://localhost:5001/api/users `
    -Method Post `
    -Body $user `
    -ContentType "application/json"
```

------

## 测试场景（Testing Scenarios）

### 场景 1：验证系统健康状态

1. 检查 API 网关健康状态：`GET http://localhost:5000/health`
2. 检查用户服务健康状态：`GET http://localhost:5001/health`
3. 检查产品服务健康状态：`GET http://localhost:5002/health`

------

### 场景 2：验证 API 网关路由

1. 通过网关请求用户列表：`GET http://localhost:5000/api/users`
2. 通过网关请求产品列表：`GET http://localhost:5000/api/products`
3. 验证返回结果是否正确

------

### 场景 3：验证服务独立性

1. 直接访问用户服务：`GET http://localhost:5001/api/users`
2. 直接访问产品服务：`GET http://localhost:5002/api/products`
3. 验证两者能独立运行

------

### 场景 4：错误处理

1. 请求不存在的用户：`GET http://localhost:5000/api/users/999`
2. 请求不存在的产品：`GET http://localhost:5000/api/products/999`
3. 验证返回 404 错误响应

------

### 场景 5：创建资源

1. 创建新用户：`POST http://localhost:5001/api/users`（带 JSON 请求体）
2. 获取新创建的用户：`GET http://localhost:5001/api/users`
3. 创建新产品：`POST http://localhost:5002/api/products`（带 JSON 请求体）
4. 获取新创建的产品：`GET http://localhost:5002/api/products`
