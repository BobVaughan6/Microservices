using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

/// <summary>
/// 健康检查控制器 - 聚合所有后端服务的健康状态
/// </summary>
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public HealthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    /// <summary>
    /// 健康检查端点 - 检查所有后端服务的健康状态
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Check()
    {
        var client = _httpClientFactory.CreateClient();
        
        // 获取服务 URL 配置
        var userServiceUrl = _configuration["ServiceUrls:UserService"] ?? "http://localhost:5001";
        var productServiceUrl = _configuration["ServiceUrls:ProductService"] ?? "http://localhost:5002";
        
        // 并行检查各个后端服务的健康状态
        var userHealth = await CheckServiceHealth(client, userServiceUrl);
        var productHealth = await CheckServiceHealth(client, productServiceUrl);
        
        // 返回聚合的健康状态
        return Ok(new
        {
            status = userHealth && productHealth ? "健康" : "降级",
            service = "ApiGateway",
            services = new
            {
                userService = userHealth ? "健康" : "不健康",
                productService = productHealth ? "健康" : "不健康"
            }
        });
    }

    private static async Task<bool> CheckServiceHealth(HttpClient client, string serviceUrl)
    {
        try
        {
            var response = await client.GetAsync($"{serviceUrl}/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
