using Microsoft.EntityFrameworkCore;
using LoggingService.DAL;
using LoggingService.Models;
using LoggingService.Services;

namespace LoggingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHostedService<RabbitMQConsumerService>();
            builder.Services.AddSingleton<LoggingContext>();
            builder.Services.AddControllers();
            builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQConfig"));
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }
}