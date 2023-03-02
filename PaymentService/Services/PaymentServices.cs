using PaymentService.ViewModels;
using PaymentService.DAL;
using PaymentService.Models;
using PaymentService.MQ;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly PaymentContext _context;
        private readonly IRabbitMQManager _rabbitMQManager;
        public PaymentServices(PaymentContext context, IRabbitMQManager rabbitMQManager)
        {
            _context = context;
            _rabbitMQManager = rabbitMQManager;
        }
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Method = TypeOfMethod.get,
                MessageType = Types.payment,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-c-routing");
            return await _context.Payments.ToListAsync();
        }
        public async Task<Payment> GetByIdAsync(int id)
        {
            if(!await _context.Payments.AnyAsync(x => x.PaymentId == id))
            {
                throw new Exception("Not Found Id");
            }
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Method = TypeOfMethod.get,
                MessageType = Types.payment,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-c-routing");
            return await _context.Payments.FirstOrDefaultAsync(x=> x.PaymentId==id);
        } 
        public async Task<Payment> CreateAsync(CreateViewModel payment)
        {
            var p = ToEntity(payment);

            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Method=TypeOfMethod.post,
                MessageType = Types.payment,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-c-routing");
            return p;
        }
        private Payment ToEntity(CreateViewModel book)
        {
            return new Payment
            {
                PayerName=book.PayerName,
                TotalPrice=book.TotalPrice,
            };
        }
    }
}
