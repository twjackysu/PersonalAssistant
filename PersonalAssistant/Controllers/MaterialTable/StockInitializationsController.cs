using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Extension;
using PersonalAssistant.Models.AccountManager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalAssistant.Controllers.MaterialTable
{
    [Authorize]
    [ApiController]
    [Route("api/MaterialTable/[controller]")]
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
        public async Task<IActionResult> Get()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();

            return Ok(new Models.FrontEndDataObject.MaterialTable
            {
                title = typeof(StockInitialization).Name,
                columns = typeof(StockInitialization).GetMaterialTableColumns(null),
                data = await _context.StockInitialization.Where(x => x.OwnerID == userID)
                .Select(x => new {
                    x.ID,
                    x.StockCode,
                    EffectiveDate = x.EffectiveDate.ToString("yyyy-MM-dd"),
                    x.Amount
                })
                .AsNoTracking().ToArrayAsync()
            });
        }
    }
}
