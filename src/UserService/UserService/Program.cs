var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// In-memory user storage
var users = new List<User>
{
    new User(1, "Alice", "alice@example.com"),
    new User(2, "Bob", "bob@example.com"),
    new User(3, "Charlie", "charlie@example.com")
};

// Health check endpoint
app.MapGet("/health", () => new { status = "healthy", service = "UserService" })
    .WithName("HealthCheck");

// Get all users
app.MapGet("/api/users", () => Results.Ok(users))
    .WithName("GetAllUsers");

// Get user by ID
app.MapGet("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUserById");

// Create a new user
app.MapPost("/api/users", (User user) =>
{
    var newUser = user with { Id = users.Max(u => u.Id) + 1 };
    users.Add(newUser);
    return Results.Created($"/api/users/{newUser.Id}", newUser);
})
.WithName("CreateUser");

app.Run();

record User(int Id, string Name, string Email);
