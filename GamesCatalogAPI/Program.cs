using GamesCatalogAPI.Models;

using MessagePipe;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.AddHealthChecks(connectionString);
// Connect to PostgreSQL Database
builder.Services.AddDbContextFactory<GamesCatalogDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddMessagePipe(options => options.InstanceLifetime = InstanceLifetime.Scoped);
builder.Services.AddControllers();

builder.ConfigureForwardedHeadersOptions();
// Add Cors for cross site access
builder.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(x => x.FullName);
            Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList()
                .ForEach(xmlFilePath => c.IncludeXmlComments(xmlFilePath));
            c.EnableAnnotations();
            c.SchemaFilter<SwaggerSchemaFilter>();
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseForwardedHeaders();
app.UseCors();

//  этот момент спорный в стандартном шаблоне ASP.NET
//app.UseHttpsRedirection();
app.MapExceptions();
app.MapControllers();

app.Run();
