using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Extension;
using PersonalAssistant.Models.AccountManager;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalAssistant.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockInitializationsController : Controller
    {
        private readonly ILogger<StockInitializationsController> _logger;
        private readonly ApplicationDbContext _context;

        public StockInitializationsController(ILogger<StockInitializationsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            return Ok(_context.StockInitialization.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var stockInitialization = await _context.StockInitialization.FindAsync(id);
            if (stockInitialization == null)
                return NotFound();
            if (userID != stockInitialization.OwnerID)
                return Unauthorized();
            return Ok(stockInitialization);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StockInitialization stockInitialization)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            stockInitialization.OwnerID = userID;
            stockInitialization.ID = null;
            _context.Add(stockInitialization);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = stockInitialization.ID }, stockInitialization);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]StockInitialization stockInitialization)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            stockInitialization.OwnerID = userID;
            var oldStockInitialization = await _context.StockInitialization.FindAsync(id);
            _context.Entry(oldStockInitialization).State = EntityState.Detached;
            if (oldStockInitialization == null)
            {
                stockInitialization.ID = null;
                _context.Add(stockInitialization);
            }
            else
            {
                if (userID != oldStockInitialization.OwnerID)
                    return Unauthorized();
                if (id != oldStockInitialization.ID)
                    return BadRequest();
                _context.Update(stockInitialization);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stockInitialization = await _context.StockInitialization.FindAsync(id);
            if (stockInitialization == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != stockInitialization.OwnerID)
                return Unauthorized();
            _context.StockInitialization.Remove(stockInitialization);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
    }
}
