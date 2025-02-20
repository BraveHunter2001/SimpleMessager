using DAL;
using Serilog;
using Services;
using SimpleMessagerApi.Hubs;

namespace SimpleMessager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", shared: true)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetValue<string>("ConnectionString")!;
            string clientUrl = builder.Configuration.GetValue<string>("ClientUrl")!;

            // Add services to the container.
            builder.Services.AddSerilog();
            builder.Services.AddDAL(connectionString);
            builder.Services.AddServices();

            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    o =>
                    {
                        o.WithOrigins(clientUrl)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(MyAllowSpecificOrigins);

            app.MapControllers();
            app.MapHub<MessagesHub>("/api/MessagesHub");

            app.Run();
        }
    }
}