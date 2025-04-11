using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Days : ControllerBase
    {
        readonly SanatoryContext db;
        public Days(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllDays")]
        public async Task<List<Day>> GetAllDays()
        {
            return new List<Day>(await db.Days.ToListAsync());
        }
    }
}
