using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GCServer;

public class GCServerSettings
{
    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;

    public string[]? AllowedEmails { get; set; }
}

public static class GCServerServiceCollectionExtensions
{
    public static IServiceCollection AddGCServerConfig(this IServiceCollection services, IConfiguration config)
    {
        return services.Configure<GCServerSettings>(config.GetSection(nameof(GCServerSettings)));
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGCServerConfig(builder.Configuration);

        var gcServerConfig = builder.Configuration.GetSection(nameof(GCServerSettings));
        builder.Services.AddOptions<GCServerSettings>().Bind(gcServerConfig);

        var config = gcServerConfig.Get<GCServerSettings>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = config.Authority;
                options.Audience = config.Audience;
            });

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
