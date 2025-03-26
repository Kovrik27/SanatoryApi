using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Guests : ControllerBase
    {
        readonly SanatoryContext db;
        public Guests(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllGuests")]
        public async Task<List<Guest>> GetAllGuests()
        {
            return new List<Guest>(await db.Guests.ToListAsync());
        }

        [HttpPost("AddNewGuest")]
        public async Task<ActionResult> AddNewGuest(Guest guest)
        {
            db.Guests.Add(guest);
            await db.SaveChangesAsync();
            return Ok("Новый гость успешно добавлен!");
        }

        [HttpPut("EditGuest")]
        public async Task<ActionResult> EditGuest(Guest guest)
        {
            db.Guests.Update(guest);
            await db.SaveChangesAsync();
            return Ok("Данные гостя успешно изменены!");
        }

        [HttpDelete("GoOutGuest/{id}")]
        public async Task<ActionResult> GoOutGuest(int id)
        {
            var guestToDelete = db.Guests.FirstOrDefault(s => s.Id == id);
            if (guestToDelete != null)
            {
                db.Guests.Remove(guestToDelete);
                await db.SaveChangesAsync();
                return Ok("Гость успешно выселен!");
            }
            else
            {
                return BadRequest("Гость для выселения не найден!");
            }
        }
    }
}
