using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureMobile : ControllerBase
    {
        readonly SanatoryContext db;
        public ProcedureMobile(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetProceduresByGuest/{id}")]
        public async Task<List<Procedure>> GetProceduresByGuest(int id)
        {
            var procedures = await db.Guests.Where(g => g.Id == id).SelectMany(g => g.Procedures).ToListAsync();
            return procedures;
        }
    }
}
