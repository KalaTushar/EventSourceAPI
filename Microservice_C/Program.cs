using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RetailService.Services;
using RetailService.DAL;
using RetailService.MQ;

namespace RetailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<RetailContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
            });
            builder.Services.AddControllers();
            builder.Services.AddRabbitMQ(builder.Configuration);
            builder.Services.AddScoped<IRetailServices, RetailServices>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }
}