using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using LoggingService.DAL;
using LoggingService.Models;

namespace LoggingService.Services
{
    public class RabbitMQConsumerService : BackgroundService, IDisposable
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly LoggingContext _context;

        public RabbitMQConsumerService(ILoggerFactory loggerFactory, IOptions<RabbitMQConfig> options, LoggingContext context)
        {
            _logger = loggerFactory.CreateLogger<RabbitMQConsumerService>();
            _rabbitMQConfig = options.Value;
            _context = context;
            InitializeMessageQueue();
        }

        private void InitializeMessageQueue()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                VirtualHost = _rabbitMQConfig.VirtualHost,
                Port = _rabbitMQConfig.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("ms-exchange", ExchangeType.Topic);
            _channel.QueueDeclare("ms-queue", false, false, false, null);
            _channel.QueueBind("ms-queue", "ms-exchange", "ms-c-routing", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += (sender, args) =>
            {
                _logger.LogInformation($"Message queue connection shutting down...");
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, args) =>
            {
                var messageString = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonConvert.DeserializeObject<Message>(messageString);
                _logger.LogInformation($"Message received: {Environment.NewLine}{message.Id}{Environment.NewLine}{message.MessageType}{Environment.NewLine}{message.Method}");
                if (message.MessageType == Types.retail)
                {
                    string s = "";
                    if (message.Method == TypeOfMethod.get)
                    {
                        s = "Get Method";
                    }
                    else
                    {
                        s = "Post Method";
                    }
                    var p = new Log
                    {
                        LogName = "Retail",
                        LogType = s,
                        LogCreated = DateTime.Now,
                    };
                    await _context.AddAsync(p);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    string s = "";
                    if (message.Method == TypeOfMethod.get)
                    {
                        s = "Get Method";
                    }
                    else
                    {
                        s = "Post Method";
                    }
                    var p = new Log
                    {
                        LogName = "Payment",
                        LogType = s,
                        LogCreated = DateTime.Now,
                    };
                    await _context.AddAsync(p);
                    await _context.SaveChangesAsync();
                }
                _channel.BasicAck(args.DeliveryTag, false);
            };

            _channel.BasicConsume("ms-queue", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
