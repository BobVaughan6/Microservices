using Microsoft.AspNetCore.Mvc;
using UserService.Controllers;
using UserService.Models;
using Xunit;

namespace UserService.Tests;

/// <summary>
/// 用户控制器单元测试
/// </summary>
public class UsersControllerTests
{
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController();
    }

    [Fact]
    public void GetAllUsers_ReturnsOkResult()
    {
        // Act
        var result = _controller.GetAllUsers();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetAllUsers_ReturnsListOfUsers()
    {
        // Act
        var result = _controller.GetAllUsers() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(result.Value);
        Assert.NotEmpty(users);
    }

    [Fact]
    public void GetUserById_ExistingId_ReturnsOkResult()
    {
        // Arrange
        int existingId = 1;

        // Act
        var result = _controller.GetUserById(existingId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetUserById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        int nonExistingId = 999;

        // Act
        var result = _controller.GetUserById(nonExistingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CreateUser_ValidUser_ReturnsCreatedResult()
    {
        // Arrange
        var newUser = new User(0, "David", "david@example.com");

        // Act
        var result = _controller.CreateUser(newUser);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public void CreateUser_ValidUser_ReturnsCreatedUser()
    {
        // Arrange
        var newUser = new User(0, "David", "david@example.com");

        // Act
        var result = _controller.CreateUser(newUser) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        var createdUser = Assert.IsType<User>(result.Value);
        Assert.Equal("David", createdUser.Name);
        Assert.Equal("david@example.com", createdUser.Email);
        Assert.True(createdUser.Id > 0);
    }
}
