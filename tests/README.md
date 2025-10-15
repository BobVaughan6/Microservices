# Microservices Tests

This directory contains unit tests for all microservices in the project.

## Test Projects

### 1. UserService.Tests
Tests for the User Service, including:
- **UsersControllerTests**: Tests for user CRUD operations
- **HealthControllerTests**: Tests for health check endpoint

### 2. ProductService.Tests
Tests for the Product Service, including:
- **ProductsControllerTests**: Tests for product CRUD operations
- **HealthControllerTests**: Tests for health check endpoint

### 3. ApiGateway.Tests
Tests for the API Gateway, including:
- **UsersControllerTests**: Tests for user service routing
- **ProductsControllerTests**: Tests for product service routing
- **HealthControllerTests**: Tests for aggregated health checks

## Running Tests

### Run all tests
```bash
# From the repository root
cd /path/to/Microservices
dotnet test

# Or run tests for all projects in the tests directory
cd tests
dotnet test
```

### Run tests for a specific service
```bash
# User Service tests
cd tests/UserService.Tests
dotnet test

# Product Service tests
cd tests/ProductService.Tests
dotnet test

# API Gateway tests
cd tests/ApiGateway.Tests
dotnet test
```

### Run tests with detailed output
```bash
dotnet test --verbosity detailed
```

### Run tests with code coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Framework

- **xUnit**: Test framework
- **Microsoft.AspNetCore.Mvc.Testing**: For integration testing ASP.NET Core applications
- **Moq**: Mocking framework for API Gateway tests (to mock HttpClient)

## Test Statistics

- **Total Tests**: 24
- **UserService.Tests**: 8 tests
- **ProductService.Tests**: 8 tests
- **ApiGateway.Tests**: 8 tests

All tests are passing ✅

## Writing New Tests

When adding new features, please follow these guidelines:

1. **Name tests descriptively**: Use the pattern `MethodName_Scenario_ExpectedBehavior`
2. **Follow AAA pattern**: Arrange, Act, Assert
3. **Test one thing at a time**: Each test should verify a single behavior
4. **Use meaningful assertions**: Make it clear what is being verified
5. **Keep tests independent**: Tests should not depend on each other

## Example Test

```csharp
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
```

## CI/CD Integration

These tests should be run as part of your CI/CD pipeline to ensure code quality:

```yaml
# Example GitHub Actions workflow
- name: Run tests
  run: dotnet test --no-build --verbosity normal
```

## Future Improvements

- [ ] Add integration tests that test services end-to-end
- [ ] Add code coverage reporting
- [ ] Add performance tests
- [ ] Add load tests for API Gateway
- [ ] Add contract tests between services
