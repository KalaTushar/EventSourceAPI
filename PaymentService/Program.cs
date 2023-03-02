using PaymentService.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaymentService.MQ;
using PaymentService.Services;
namespace PaymentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<PaymentContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
            });
            builder.Services.AddControllers();
            builder.Services.AddRabbitMQ(builder.Configuration);
            builder.Services.AddScoped<IPaymentServices, PaymentServices>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }
}