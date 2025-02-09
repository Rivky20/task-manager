using TodoApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ToDoListDB");

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB") ?? 
                     Environment.GetEnvironmentVariable("ToDoDB"),
                     new MySqlServerVersion(new Version(8, 0, 0))));
                     
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseCors();

app.MapGet("/", (ToDoDbContext db) => "Server is running!");

app.MapGet("/items", async (ToDoDbContext db) => {
    return await db.Items.ToListAsync();
});

app.MapGet("/items/{id}", async (int id, ToDoDbContext db) => {
    var item = await db.Items.FindAsync(id);
    if (item == null) {
        return Results.NotFound();
    }
    return Results.Ok(item);
});


app.MapPost("/items",async (ToDoDbContext db, Item item) => {
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}",async (int id, Item updatedItem, ToDoDbContext db) => {
    var item = await db.Items.FindAsync(id);
    if (item == null) {
        return Results.NotFound();
    }
    item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;
   
    await db.SaveChangesAsync();
    return Results.Ok(item); 
});

app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) => {
    var item = await db.Items.FindAsync(id);
    if (item == null) {
        return Results.NotFound(); 
    }

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok("item deleted successfully");
});

app.Run();