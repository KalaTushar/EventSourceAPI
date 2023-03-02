using Microsoft.AspNetCore.Mvc;
using RetailService.MQ;
using RetailService.Services;
using RetailService.ViewModels;
using RetailService.Models;
namespace RetailService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailController : ControllerBase
    {
        private readonly IRetailServices _retailService;
        private readonly IRabbitMQManager _rabbitMQManager;

        public RetailController(IRetailServices bookService, IRabbitMQManager rabbitMQManager)
        {
            _retailService = bookService;
            _rabbitMQManager = rabbitMQManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Retail>>> GetAll()
        {
            return Ok(await _retailService.GetAll());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Retail>> GetById(int id)
        {
            return Ok(await _retailService.GetById(id));
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateViewModel product)
        {
            return Ok(await _retailService.CreateAsync(product));
        }
    }
}