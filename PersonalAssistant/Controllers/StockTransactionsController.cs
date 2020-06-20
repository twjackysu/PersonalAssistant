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
    public class StockTransactionsController : Controller
    {
        private readonly ILogger<StockTransactionsController> _logger;
        private readonly ApplicationDbContext _context;

        public StockTransactionsController(ILogger<StockTransactionsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            return Ok(await _context.StockTransaction.Where(x => x.OwnerID == userID)
                .Select(x => new StockTransaction
                {
                    ID = x.ID,
                    Account = x.Account,
                    Amount = x.Amount,
                    EffectiveDate = x.EffectiveDate,
                    Fees = x.Fees,
                    Price = x.Price,
                    StockCode = x.StockCode,
                    Type = x.Type
                })//remove user sid to reduce json size
                .AsNoTracking().ToArrayAsync());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var stockTransaction = await _context.StockTransaction.FindAsync(id);
            if (stockTransaction == null)
            {
                return NotFound();
            }
            if (userID != stockTransaction.OwnerID)
                return Unauthorized();
            return Ok(stockTransaction);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StockTransaction stockTransaction)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            stockTransaction.OwnerID = userID;
            stockTransaction.ID = null;
            _context.Add(stockTransaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = stockTransaction.ID }, stockTransaction);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]StockTransaction stockTransaction)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            stockTransaction.OwnerID = userID;
            var oldStockTransaction = await _context.StockTransaction.FindAsync(id);
            _context.Entry(oldStockTransaction).State = EntityState.Detached;
            if (oldStockTransaction == null)
            {
                stockTransaction.ID = null;
                _context.Add(stockTransaction);
            }
            else
            {
                if (userID != oldStockTransaction.OwnerID)
                    return Unauthorized();
                if (id != oldStockTransaction.ID)
                    return BadRequest();
                _context.Update(stockTransaction);
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
            var stockTransaction = await _context.StockTransaction.FindAsync(id);
            if (stockTransaction == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != stockTransaction.OwnerID)
                return Unauthorized();
            _context.StockTransaction.Remove(stockTransaction);

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
