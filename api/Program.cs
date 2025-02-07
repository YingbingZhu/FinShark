using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Swagger/OpenAPI services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson( options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


// Add DbContext for MySQL
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41)) // Match your MySQL version
    );
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Map a simple GET endpoint for testing
app.MapGet("/", () => "Hello, World!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      // Serves the OpenAPI specification
    app.UseSwaggerUI();    // Enables the Swagger UI
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
