using Microsoft.AspNetCore.HttpOverrides;

using System.Net;
using System.Text;


namespace Microsoft.Extensions.DependencyInjection;


public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureForwardedHeadersOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.KnownNetworks.Clear();
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.ForwardedForHeaderName = "X-Original-Forwarded-For";
            options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:10.0.0.0"), 104));
        });

        return builder;
    }

    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
        string[] allowOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyMethod().WithHeaders("Accept", "Content-Type", "Origin", "Authorization", "Referer", "User-Agent");

                    if (allowOrigins?.Length > 0)
                    {
                        builder.WithOrigins(allowOrigins);
                    }
                    else
                    {
                        builder.AllowAnyOrigin();
                    }
                });
        });
        return builder;
    }
}
