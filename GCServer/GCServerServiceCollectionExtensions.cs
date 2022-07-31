namespace GCServer;

public static class GCServerServiceCollectionExtensions
{
    public static IServiceCollection AddGCServerConfig(this IServiceCollection services, IConfiguration config)
    {
        return services.Configure<GCServerSettings>(config.GetSection(nameof(GCServerSettings)));
    }
}
