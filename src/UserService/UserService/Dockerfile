# =============================================================================
# Dockerfile - User Service
# =============================================================================
# 作用：定义如何构建 User Service 的 Docker 镜像
# 使用 Docker 多阶段构建优化镜像大小和构建效率
# 
# 详细说明请参考 ApiGateway 的 Dockerfile
# 本文件结构相同，只是服务名称和端口不同
# =============================================================================

# 阶段 1: 运行时基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001  # User Service 端口

# 阶段 2: 构建阶段 - 使用 SDK 编译应用
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 先复制项目文件，利用 Docker 层缓存优化
COPY ["UserService.csproj", "./"]
RUN dotnet restore "UserService.csproj"

# 复制源代码并编译
COPY . .
RUN dotnet build "UserService.csproj" -c Release -o /app/build

# 阶段 3: 发布阶段 - 生成可部署文件
FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

# 阶段 4: 最终镜像 - 只包含运行时和应用文件
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]

