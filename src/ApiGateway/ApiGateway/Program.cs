var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();

// Service URLs
var userServiceUrl = "http://localhost:5001";
var productServiceUrl = "http://localhost:5002";

// Health check endpoint
app.MapGet("/health", async () =>
{
    var client = httpClientFactory.CreateClient();
    var userHealth = await CheckServiceHealth(client, userServiceUrl);
    var productHealth = await CheckServiceHealth(client, productServiceUrl);
    
    return new
    {
        status = userHealth && productHealth ? "healthy" : "degraded",
        service = "ApiGateway",
        services = new
        {
            userService = userHealth ? "healthy" : "unhealthy",
            productService = productHealth ? "healthy" : "unhealthy"
        }
    };
})
.WithName("HealthCheck");

// User Service Routes
app.MapGet("/api/users", async () =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"{userServiceUrl}/api/users");
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetUsers");

app.MapGet("/api/users/{id}", async (int id) =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"{userServiceUrl}/api/users/{id}");
    if (!response.IsSuccessStatusCode)
        return Results.NotFound();
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetUserById");

// Product Service Routes
app.MapGet("/api/products", async () =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"{productServiceUrl}/api/products");
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetProducts");

app.MapGet("/api/products/{id}", async (int id) =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.GetAsync($"{productServiceUrl}/api/products/{id}");
    if (!response.IsSuccessStatusCode)
        return Results.NotFound();
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("GetProductById");

app.Run();

async Task<bool> CheckServiceHealth(HttpClient client, string serviceUrl)
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
