# =============================================================================
# Dockerfile - API Gateway
# =============================================================================
# 作用：定义如何构建 API Gateway 服务的 Docker 镜像
# 
# Docker 多阶段构建（Multi-stage Build）：
#   - 使用多个 FROM 指令创建多个构建阶段
#   - 每个阶段可以使用不同的基础镜像
#   - 后续阶段可以从前面阶段复制文件
#   - 最终镜像只包含最后一个阶段的内容
# 
# 多阶段构建的优势：
#   1. 减小镜像体积：最终镜像不包含编译工具和中间文件
#   2. 提高安全性：生产镜像只包含运行时依赖
#   3. 优化构建速度：利用 Docker 层缓存
#   4. 分离关注点：构建和运行使用不同的环境
# 
# 本 Dockerfile 包含 4 个阶段：
#   1. base: 运行时基础镜像
#   2. build: 编译和构建阶段
#   3. publish: 发布阶段（生成可部署文件）
#   4. final: 最终运行镜像
# =============================================================================

# =============================================================================
# 阶段 1: Base - 运行时基础镜像
# =============================================================================
# 使用官方 ASP.NET Core 运行时镜像
# mcr.microsoft.com: Microsoft Container Registry（微软官方容器仓库）
# aspnet:9.0: ASP.NET Core 9.0 运行时，只包含运行应用所需的最小依赖
# 
# 为什么使用 aspnet 而不是 sdk？
#   - aspnet: 只有运行时，镜像小（约 200MB）
#   - sdk: 包含完整开发工具，镜像大（约 700MB）
#   - 生产环境只需要运行时，不需要编译工具
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# 设置工作目录
# WORKDIR: 为后续的 RUN、CMD、COPY 等指令设置工作目录
# 如果目录不存在，Docker 会自动创建
WORKDIR /app

# 暴露端口
# EXPOSE: 声明容器运行时监听的端口（文档性质）
# 注意：EXPOSE 不会实际发布端口，需要在 docker run 时使用 -p 参数
# 或在 docker-compose.yml 中配置 ports
EXPOSE 5000

# =============================================================================
# 阶段 2: Build - 构建阶段
# =============================================================================
# 使用 .NET SDK 镜像进行编译
# sdk:9.0: 包含编译器、NuGet、工具链等完整开发工具
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# 复制项目文件并还原依赖
# COPY: 从构建上下文复制文件到镜像
# ["source", "destination"]: JSON 数组格式，支持包含空格的路径
# 
# 为什么先只复制 .csproj 文件？
#   - 利用 Docker 层缓存优化构建速度
#   - 依赖很少变化，代码经常变化
#   - 如果 .csproj 未变化，Docker 会使用缓存的 restore 结果
#   - 避免每次代码修改都重新下载 NuGet 包
COPY ["ApiGateway.csproj", "./"]

# 还原 NuGet 包依赖
# dotnet restore: 下载项目所需的所有 NuGet 包
# 这一层会被 Docker 缓存，除非 .csproj 文件改变
RUN dotnet restore "ApiGateway.csproj"

# 复制所有源代码
# COPY . .: 复制构建上下文中的所有文件到当前工作目录
# 第一个 . 表示构建上下文（docker-compose.yml 中的 context）
# 第二个 . 表示容器内的当前目录（/src）
COPY . .

# 编译项目
# dotnet build: 编译 .NET 项目
# -c Release: 使用 Release 配置（优化性能，移除调试信息）
# -o /app/build: 输出目录
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build

# =============================================================================
# 阶段 3: Publish - 发布阶段
# =============================================================================
# 从 build 阶段继承
# FROM build: 继续使用 build 阶段的环境和文件
FROM build AS publish

# 发布应用
# dotnet publish: 创建可部署的应用程序包
#   - 包含所有运行时依赖
#   - 优化和裁剪不必要的程序集
#   - 生成可以直接运行的文件
# -c Release: Release 配置
# -o /app/publish: 输出到 /app/publish 目录
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

# =============================================================================
# 阶段 4: Final - 最终运行镜像
# =============================================================================
# 从 base 阶段（运行时镜像）构建最终镜像
# 优势：最终镜像不包含 SDK 和源代码，体积小，安全性高
FROM base AS final

WORKDIR /app

# 从 publish 阶段复制编译好的文件
# COPY --from=publish: 从指定阶段复制文件
# /app/publish: publish 阶段的输出目录
# .: 当前工作目录（/app）
# 
# 最终镜像只包含：
#   - ASP.NET Core 运行时
#   - 编译后的应用程序 DLL
#   - 运行时依赖
# 不包含：
#   - .NET SDK
#   - 源代码
#   - 中间编译文件
COPY --from=publish /app/publish .

# 容器启动命令
# ENTRYPOINT: 配置容器启动时执行的命令
# ["executable", "param1", "param2"]: exec 格式（推荐）
#   - 直接执行，不通过 shell
#   - 可以正确处理信号（SIGTERM、SIGINT）
#   - 支持优雅关闭
# 
# dotnet ApiGateway.dll: 运行编译好的应用程序
# 
# ENTRYPOINT vs CMD:
#   - ENTRYPOINT: 定义容器的主进程，不易被覆盖
#   - CMD: 提供默认参数，可以在 docker run 时覆盖
ENTRYPOINT ["dotnet", "ApiGateway.dll"]

# =============================================================================
# 使用说明
# =============================================================================
# 
# 1. 构建镜像：
#    docker build -t api-gateway .
# 
# 2. 运行容器：
#    docker run -p 5000:5000 api-gateway
# 
# 3. 查看日志：
#    docker logs <container-id>
# 
# 4. 进入容器：
#    docker exec -it <container-id> /bin/bash
# 
# =============================================================================
# 
# 优化建议：
#   1. 添加 .dockerignore 文件，排除不必要的文件
#   2. 使用 Alpine 基础镜像进一步减小体积
#   3. 配置健康检查（HEALTHCHECK）
#   4. 添加非 root 用户运行应用（安全性）
#   5. 配置环境变量默认值
#   6. 添加标签（LABEL）记录元数据
# 
# =============================================================================

