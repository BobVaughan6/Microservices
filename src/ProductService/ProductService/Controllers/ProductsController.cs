using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers;

/// <summary>
/// 产品控制器 - 处理产品相关的 HTTP 请求
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // 内存存储（演示用）
    private static readonly List<Product> Products = new()
    {
        new Product(1, "Laptop", "High-performance laptop", 999.99m),
        new Product(2, "Mouse", "Wireless mouse", 29.99m),
        new Product(3, "Keyboard", "Mechanical keyboard", 89.99m)
    };

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns>产品列表</returns>
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        return Ok(Products);
    }

    /// <summary>
    /// 根据 ID 获取产品
    /// </summary>
    /// <param name="id">产品 ID</param>
    /// <returns>产品对象或 404</returns>
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        return product is not null ? Ok(product) : NotFound();
    }

    /// <summary>
    /// 创建新产品
    /// </summary>
    /// <param name="product">产品对象</param>
    /// <returns>创建的产品对象</returns>
    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        // 自动生成新 ID
        var newProduct = product with { Id = Products.Max(p => p.Id) + 1 };
        Products.Add(newProduct);
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }
}
