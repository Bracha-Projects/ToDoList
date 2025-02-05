using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ToDoDB");

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.32-mysql")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin() // Allow any origin
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors("AllowAllOrigins"); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.MapGet("/tasks", async (ToDoDbContext dbContext) =>
{
    var items = await dbContext.Items.ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/tasks/{id}", async (int id, ToDoDbContext dbContext) =>
{
    var item = await dbContext.Items.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});
    
app.MapPost("/tasks", async (Item item, ToDoDbContext dbContext) =>
{
    dbContext.Items.Add(item);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item);
});

app.MapPut("/tasks/{id}", async (int id, Item updatedItem, ToDoDbContext dbContext) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.IsComplete = updatedItem.IsComplete;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (int id, ToDoDbContext dbContext) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    dbContext.Items.Remove(item);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});
app.MapGet('/',()=>"AuthServer API is running");
app.Run();
