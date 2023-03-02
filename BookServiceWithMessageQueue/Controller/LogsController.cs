using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoggingService.DAL;
using LoggingService.Models;

namespace LoggingService.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LoggingContext _context;
        public LogsController(LoggingContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetAll()
        {
            return Ok(await _context.Logs.Select(p => new Log
            {
                LogId = p.LogId,
                LogCreated = p.LogCreated,
                LogName = p.LogName,
                LogType = p.LogType,
            }).ToListAsync());
        }
    }
}
