using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    public class ExpendituresController : Controller
    {
        private readonly ILogger<ExpendituresController> _logger;
        private readonly ApplicationDbContext _context;

        public ExpendituresController(ILogger<ExpendituresController> logger, ApplicationDbContext context)
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
            return Ok(_context.Expenditure.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var expenditure = await _context.Expenditure.FindAsync(id);
            if (expenditure == null)
                return NotFound();
            if (userID != expenditure.OwnerID)
                return Unauthorized();
            return Ok(expenditure);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Expenditure expenditure)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            expenditure.OwnerID = userID;
            expenditure.ID = null;
            _context.Add(expenditure);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = expenditure.ID }, expenditure);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Expenditure expenditure)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            expenditure.OwnerID = userID;
            var oldExpenditure = await _context.Expenditure.FindAsync(id);
            _context.Entry(oldExpenditure).State = EntityState.Detached;
            if (oldExpenditure == null)
            {
                expenditure.ID = null;
                _context.Add(expenditure);
            }
            else
            {
                if (userID != oldExpenditure.OwnerID)
                    return Unauthorized();
                if (id != oldExpenditure.ID)
                    return BadRequest();
                _context.Update(expenditure);
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
            var expenditure = await _context.Expenditure.FindAsync(id);
            if (expenditure == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != expenditure.OwnerID)
                return Unauthorized();
            _context.Expenditure.Remove(expenditure);

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
