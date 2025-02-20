using Microsoft.Extensions.DependencyInjection;

namespace Services;

public static class DI
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMessagesService, MessagesService>();
    }
}