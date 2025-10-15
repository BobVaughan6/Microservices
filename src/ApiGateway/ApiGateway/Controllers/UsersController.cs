using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

/// <summary>
/// 用户网关控制器 - 转发用户相关请求到用户服务
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _userServiceUrl;

    public UsersController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _userServiceUrl = _configuration["ServiceUrls:UserService"] ?? "http://localhost:5001";
    }

    /// <summary>
    /// 获取所有用户
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_userServiceUrl}/api/users");
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    /// <summary>
    /// 根据 ID 获取用户
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_userServiceUrl}/api/users/{id}");
        
        if (!response.IsSuccessStatusCode)
            return NotFound();
            
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }
}
