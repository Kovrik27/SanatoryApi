using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestMobile : ControllerBase
    {
        readonly SanatoryContext db;
        public GuestMobile(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpPost("AddProcedureOnGuest")]
        public async Task<ActionResult> AddProcedureOnGuest(GuestProcedureDTO guestProcedureDTO)
        {
            var guest = db.Guests.FirstOrDefault(s => s.Id == guestProcedureDTO.GuestId);
            var procedure = db.Procedures.FirstOrDefault(p => p.Id == guestProcedureDTO.ProcedureId);

            guest.Procedures.Add(procedure);
            await db.SaveChangesAsync();
            return Ok("Процедура успешно добавлена");
        }

    }
}
