using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GCServer;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }


    public void ConfigureServices(IServiceCollection services)
    {

        services.AddGCServerConfig(Configuration);

        var gcServerConfig = Configuration.GetSection(nameof(GCServerSettings));
        services.AddOptions<GCServerSettings>().Bind(gcServerConfig);

        var config = gcServerConfig.Get<GCServerSettings>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = config.Authority;
                options.Audience = config.Audience;
            });

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(x => x.MapControllers());
    }
}
