using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(connectionString);
});

 

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
var scope = app.Services.CreateScope();
 
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
    db.Database.Migrate();
    DbInitializer.Initialize(db);
    }
    catch(Exception ex)
    {
        logger.LogError(ex, "Faild to migrate the data");

    }
 
 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

