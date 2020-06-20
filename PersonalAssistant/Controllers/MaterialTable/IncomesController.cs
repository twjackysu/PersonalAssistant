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
    public class IncomesController : Controller
    {
        private readonly ILogger<IncomesController> _logger;
        private readonly ApplicationDbContext _context;

        public IncomesController(ILogger<IncomesController> logger, ApplicationDbContext context)
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

            var allForeignkey = new Dictionary<string, Dictionary<int?, string>>();
            allForeignkey.Add("AccountID", _context.AccountInitialization.Where(x => x.OwnerID == userID).ToDictionary(x => x.ID, x => x.Name));

            return Ok(new Models.FrontEndDataObject.MaterialTable
            {
                title = typeof(Income).Name,
                columns = typeof(Income).GetMaterialTableColumns(allForeignkey),
                data = await _context.Income.Where(x => x.OwnerID == userID)
                .Select(x => new {
                    x.ID,
                    x.AccountID,
                    EffectiveDate = x.EffectiveDate.ToString("yyyy-MM-dd"),
                    x.Amount,
                    x.Remarks
                })
                .AsNoTracking().ToArrayAsync()
            });
        }
    }
}
