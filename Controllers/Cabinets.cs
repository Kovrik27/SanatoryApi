using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cabinets : ControllerBase
    {
        readonly SanatoryContext db;
        public Cabinets(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllCabinets")]
        public async Task<List<Cabinet>> GetAllCabinets()
        {
            return new List<Cabinet>(await db.Cabinets.ToListAsync());
        }

        [HttpPost("AddNewCabinet")]
        public async Task<ActionResult> AddNewCabinet(Cabinet cabinet)
        {
            db.Cabinets.Add(cabinet);
            await db.SaveChangesAsync();
            return Ok("Новый кабинет успешно добавлен!");
        }

        [HttpPut("EditCabinet")]
        public async Task<ActionResult> EditCabinet(Cabinet cabinet)
        {
            db.Cabinets.Update(cabinet);
            await db.SaveChangesAsync();
            return Ok("Данные кабинета успешно изменены!");
        }
    }
}
