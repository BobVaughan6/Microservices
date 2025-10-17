using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers;

/// <summary>
/// 用户控制器 - 处理用户相关的 HTTP 请求
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // 内存存储（演示用）
    // 注意：在实际应用中应使用依赖注入的服务层和数据库
    private static readonly List<User> Users = new()
    {
        new User(1, "张三", "alice@example.com"),
        new User(2, "李四", "bob@example.com"),
        new User(3, "王五", "charlie@example.com")
    };

    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <returns>用户列表</returns>
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(Users);
    }

    /// <summary>
    /// 根据 ID 获取用户
    /// </summary>
    /// <param name="id">用户 ID</param>
    /// <returns>用户对象或 404</returns>
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        return user is not null ? Ok(user) : NotFound();
    }

    /// <summary>
    /// 创建新用户
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>创建的用户对象</returns>
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        // 自动生成新 ID
        var newUser = user with { Id = Users.Max(u => u.Id) + 1 };
        Users.Add(newUser);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }
}
