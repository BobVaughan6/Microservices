using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using Xunit;

namespace ProductService.Tests;

/// <summary>
/// 健康检查控制器单元测试
/// </summary>
public class HealthControllerTests
{
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _controller = new HealthController();
    }

    [Fact]
    public void Check_ReturnsOkResult()
    {
        // Act
        var result = _controller.Check();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Check_ReturnsHealthyStatus()
    {
        // Act
        var result = _controller.Check() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result.Value;
        Assert.NotNull(value);
        
        // 使用反射检查匿名对象的属性
        var statusProperty = value.GetType().GetProperty("status");
        Assert.NotNull(statusProperty);
        Assert.Equal("healthy", statusProperty.GetValue(value));
        
        var serviceProperty = value.GetType().GetProperty("service");
        Assert.NotNull(serviceProperty);
        Assert.Equal("ProductService", serviceProperty.GetValue(value));
    }
}
