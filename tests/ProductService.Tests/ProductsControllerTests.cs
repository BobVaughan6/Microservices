using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;

namespace ProductService.Tests;

/// <summary>
/// 产品控制器单元测试
/// </summary>
public class ProductsControllerTests
{
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new ProductsController();
    }

    [Fact]
    public void GetAllProducts_ReturnsOkResult()
    {
        // Act
        var result = _controller.GetAllProducts();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetAllProducts_ReturnsListOfProducts()
    {
        // Act
        var result = _controller.GetAllProducts() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(result.Value);
        Assert.NotEmpty(products);
    }

    [Fact]
    public void GetProductById_ExistingId_ReturnsOkResult()
    {
        // Arrange
        int existingId = 1;

        // Act
        var result = _controller.GetProductById(existingId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetProductById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        int nonExistingId = 999;

        // Act
        var result = _controller.GetProductById(nonExistingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CreateProduct_ValidProduct_ReturnsCreatedResult()
    {
        // Arrange
        var newProduct = new Product(0, "Monitor", "4K Display", 299.99m);

        // Act
        var result = _controller.CreateProduct(newProduct);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public void CreateProduct_ValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var newProduct = new Product(0, "Monitor", "4K Display", 299.99m);

        // Act
        var result = _controller.CreateProduct(newProduct) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        var createdProduct = Assert.IsType<Product>(result.Value);
        Assert.Equal("Monitor", createdProduct.Name);
        Assert.Equal("4K Display", createdProduct.Description);
        Assert.Equal(299.99m, createdProduct.Price);
        Assert.True(createdProduct.Id > 0);
    }
}
