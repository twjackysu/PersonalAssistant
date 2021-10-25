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
    public class InternalTransfersController : Controller
    {
        private readonly ILogger<InternalTransfersController> _logger;
        private readonly ApplicationDbContext _context;

        public InternalTransfersController(ILogger<InternalTransfersController> logger, ApplicationDbContext context)
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
            return Ok(_context.InternalTransfer.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var internalTransfer = await _context.InternalTransfer.FindAsync(id);
            if (internalTransfer == null)
                return NotFound();
            if (userID != internalTransfer.OwnerID)
                return Unauthorized();
            return Ok(internalTransfer);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InternalTransfer internalTransfer)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            internalTransfer.OwnerID = userID;
            internalTransfer.ID = null;
            _context.Add(internalTransfer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = internalTransfer.ID }, internalTransfer);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]InternalTransfer internalTransfer)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            internalTransfer.OwnerID = userID;
            var oldInternalTransfer = await _context.Income.FindAsync(id);
            _context.Entry(oldInternalTransfer).State = EntityState.Detached;
            if (oldInternalTransfer == null)
            {
                internalTransfer.ID = null;
                _context.Add(internalTransfer);
            }
            else
            {
                if (userID != oldInternalTransfer.OwnerID)
                    return Unauthorized();
                if (id != oldInternalTransfer.ID)
                    return BadRequest();
                _context.Update(internalTransfer);
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
            var internalTransfer = await _context.InternalTransfer.FindAsync(id);
            if (internalTransfer == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != internalTransfer.OwnerID)
                return Unauthorized();
            _context.InternalTransfer.Remove(internalTransfer);

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
