var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// In-memory product storage
var products = new List<Product>
{
    new Product(1, "Laptop", "High-performance laptop", 999.99m),
    new Product(2, "Mouse", "Wireless mouse", 29.99m),
    new Product(3, "Keyboard", "Mechanical keyboard", 89.99m)
};

// Health check endpoint
app.MapGet("/health", () => new { status = "healthy", service = "ProductService" })
    .WithName("HealthCheck");

// Get all products
app.MapGet("/api/products", () => Results.Ok(products))
    .WithName("GetAllProducts");

// Get product by ID
app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById");

// Create a new product
app.MapPost("/api/products", (Product product) =>
{
    var newProduct = product with { Id = products.Max(p => p.Id) + 1 };
    products.Add(newProduct);
    return Results.Created($"/api/products/{newProduct.Id}", newProduct);
})
.WithName("CreateProduct");

app.Run();

record Product(int Id, string Name, string Description, decimal Price);
