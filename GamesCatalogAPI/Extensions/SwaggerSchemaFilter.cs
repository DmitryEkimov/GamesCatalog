using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Reflection;
using System.Text.Json.Serialization;

namespace Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Swagger schema filter class.
/// </summary>
public class SwaggerSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Apply.
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null)
        {
            return;
        }

        var excludedProperties =
            context?.Type.GetProperties().Where(
                t => t.GetCustomAttribute<JsonIgnoreAttribute>() != null);

        if (excludedProperties == null)
            return;

        foreach (var excludedProperty in excludedProperties)
        {
            var propertyToRemove =
                schema.Properties.Keys.SingleOrDefault(
                    x => String.Equals(x, excludedProperty.Name, StringComparison.CurrentCultureIgnoreCase));

            if (propertyToRemove == null)
                continue;

            schema.Properties.Remove(propertyToRemove);
        }

        var namesProperties =
            context?.Type.GetProperties().Where(
                t => t.GetCustomAttribute<JsonPropertyNameAttribute>() != null).Select(p => (p, p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name));

        foreach (var nameProperty in namesProperties)
        {
            if (string.Equals(nameProperty.Name, nameProperty.p.Name, StringComparison.CurrentCultureIgnoreCase))
                continue;

            var property =
                schema.Properties.Keys.SingleOrDefault(
                    x => String.Equals(x, nameProperty.p.Name, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                continue;

            var s = schema.Properties[property];
            schema.Properties.Remove(property);
            schema.Properties.Add(nameProperty.Name, s);
        }

    }
}
