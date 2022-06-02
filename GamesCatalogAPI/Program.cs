using FluentValidation.AspNetCore;

using GamesCatalogAPI.Models;
using GamesCatalogAPI.Swagger;

using MessagePipe;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry();

//builder.WebHost.ConfigureKestrel((context, options) =>
//{
//    options.ListenAnyIP(5001, listenOptions =>
//    {
//        listenOptions.Protocols = //HttpProtocols.Http1AndHttp2AndHttp3;
//        listenOptions.UseHttps();
//    });
//});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.AddHealthChecks(connectionString);
// Connect to PostgreSQL Database
builder.Services.AddDbContextFactory<GamesCatalogDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddMessagePipe(options => options.InstanceLifetime = InstanceLifetime.Scoped);
builder.Services.AddControllers()
        // Adds fluent validators to Asp.net
        .AddFluentValidation(c =>
        {
            // Validate child properties and root collection elements
            c.ImplicitlyValidateChildProperties = true;
            c.ImplicitlyValidateRootCollectionElements = true;
            c.RegisterValidatorsFromAssemblyContaining<GamesCatalogDbContext>();
            // Optionally set validator factory if you have problems with scope resolve inside validators.
            //c.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
        });
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.ConfigureForwardedHeadersOptions();
// Add Cors for cross site access
builder.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersionedApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(x => x.FullName);
    // integrate xml comments
    Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList()
        .ForEach(xmlFilePath => c.IncludeXmlComments(xmlFilePath));
    c.EnableAnnotations();
    // add a custom operation filter which sets default values
    c.OperationFilter<SwaggerDefaultValues>();
    c.SchemaFilter<SwaggerSchemaFilter>();
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

// Adds FluentValidationRules staff to Swagger. (Minimal configuration)
builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

//app.UseSentryTracing();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // build a swagger endpoint for each discovered API version
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}
app.UseForwardedHeaders();
app.UseCors();

//  этот момент спорный в стандартном шаблоне ASP.NET
//app.UseHttpsRedirection();
app.MapExceptions();
app.MapControllers();

app.Run();
