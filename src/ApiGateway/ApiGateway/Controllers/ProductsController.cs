using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

/// <summary>
/// 产品网关控制器 - 转发产品相关请求到产品服务
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _productServiceUrl;

    public ProductsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _productServiceUrl = _configuration["ServiceUrls:ProductService"] ?? "http://localhost:5002";
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_productServiceUrl}/api/products");
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    /// <summary>
    /// 根据 ID 获取产品
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_productServiceUrl}/api/products/{id}");
        
        if (!response.IsSuccessStatusCode)
            return NotFound();
            
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }
}
