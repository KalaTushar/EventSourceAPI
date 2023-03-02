using Microsoft.EntityFrameworkCore;
using RetailService.Models;
using RetailService.DAL;
using RetailService.ViewModels;
using RetailService.MQ;

namespace RetailService.Services
{
    public class RetailServices : IRetailServices
    {
        private readonly RetailContext _context;
        private readonly IRabbitMQManager _rabbitMQManager;
        public RetailServices(RetailContext context, IRabbitMQManager rabbitMQManager)
        {
            _context = context;
            _rabbitMQManager = rabbitMQManager;
        }
        public async Task<IEnumerable<Retail>> GetAll()
        {
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                MessageType = Types.retail,
                Method = TypeOfMethod.get,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-c-routing");
            return await _context.Retails.ToListAsync();
        }
        public async Task<Retail> GetById(int id)
        {
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                MessageType = Types.retail,
                Method = TypeOfMethod.get,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-c-routing");
            if(!await _context.Retails.AnyAsync(x => x.RetailId == id))
            {
                throw new Exception("Not Found");
            }
            return await _context.Retails.FirstOrDefaultAsync(x => x.RetailId == id);
        }
        public async Task<RetailViewModel> CreateAsync(CreateViewModel product)
        {
            var p = ToEntity(product);

            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                MessageType = Types.retail,
                Method = TypeOfMethod.post,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-c-routing");
            return new RetailViewModel
            {
                RetailId = p.RetailId,
                Detail = p.Detail,
                NumberOfProduct = p.NumberOfProduct,
                TotalPrice = p.TotalPrice,
            };
        }
        private Retail ToEntity(CreateViewModel p)
        {
            return new Retail
            {
                Detail = p.Detail,
                NumberOfProduct = p.NumberOfProduct,
                TotalPrice = p.TotalPrice,
            };
        }
    }
}
