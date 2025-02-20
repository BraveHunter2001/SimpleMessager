using Microsoft.Extensions.DependencyInjection;

namespace DAL;

public static class DI
{
    public static void AddDAL(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<StupidContext>();
        services.AddNpgsqlDataSource(connectionString);
        services.AddScoped<IMessagesRepository, MessagesRepository>();
    }
}