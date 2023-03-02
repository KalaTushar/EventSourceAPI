using PaymentService.MQ;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Services;
using PaymentService.ViewModels;
using PaymentService.Models;
namespace PaymentService.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentService;
        private readonly IRabbitMQManager _rabbitMQManager;
        
        public PaymentController(IPaymentServices paymentService, IRabbitMQManager rabbitMQManager)
        {
            _paymentService = paymentService;
            _rabbitMQManager = rabbitMQManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> Get()
        {
            return Ok(await _paymentService.GetAllAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Payment>>> Get(int id)
        {
            return Ok(await _paymentService.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateViewModel book)
        {
            return Ok(await _paymentService.CreateAsync(book));
        }
    }
}