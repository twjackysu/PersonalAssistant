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
    public class AccountInitializationsController : Controller
    {
        private readonly ILogger<AccountInitializationsController> _logger;
        private readonly ApplicationDbContext _context;

        public AccountInitializationsController(ILogger<AccountInitializationsController> logger, ApplicationDbContext context)
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
            return Ok(_context.AccountInitialization.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var accountInitialization = await _context.AccountInitialization.FindAsync(id);
            if (accountInitialization == null)
                return NotFound();
            if (userID != accountInitialization.OwnerID)
                return Unauthorized();
            return Ok(accountInitialization);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AccountInitialization accountInitialization)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            accountInitialization.OwnerID = userID;
            accountInitialization.ID = null;
            _context.Add(accountInitialization);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = accountInitialization.ID }, accountInitialization);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]AccountInitialization accountInitialization)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            accountInitialization.OwnerID = userID;
            var oldAccountInitialization = await _context.AccountInitialization.FindAsync(id);
            _context.Entry(oldAccountInitialization).State = EntityState.Detached;
            if (oldAccountInitialization == null)
            {
                accountInitialization.ID = null;
                _context.Add(accountInitialization);
            }
            else
            {
                if (userID != oldAccountInitialization.OwnerID)
                    return Unauthorized();
                if (id != oldAccountInitialization.ID)
                    return BadRequest();
                _context.Update(accountInitialization);
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
            var accountInitialization = await _context.AccountInitialization.FindAsync(id);
            if (accountInitialization == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != accountInitialization.OwnerID)
                return Unauthorized();
            _context.AccountInitialization.Remove(accountInitialization);

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
