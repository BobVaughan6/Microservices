using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers;

/// <summary>
/// 健康检查控制器
/// </summary>
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// 健康检查端点
    /// </summary>
    /// <returns>服务健康状态</returns>
    [HttpGet]
    public IActionResult Check()
    {
        return Ok(new { status = "healthy", service = "ProductService" });
    }
}
