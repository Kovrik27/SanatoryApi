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
            var dirtyroom = db.Rooms.Include(r => r.Guests).Include(r => r.Status).FirstOrDefault(r => r.Id == id);
            if (dirtyroom == null)
            {
                return BadRequest("Грязный номер не найден!");
            }
            var guest = dirtyroom.Guests.FirstOrDefault();
            if(guest == null)
            {
                return BadRequest("Номер пустой");
            }
            dirtyroom.Guests.Remove(guest);
            guest.RoomId = 0;
            db.Guests.Update(guest);
            await db.SaveChangesAsync();
              
            var dirtystatus = db.Statuses.FirstOrDefault(s => s.Title == "Грязный");
            dirtyroom.StatusId = dirtystatus.Id;
            await db.SaveChangesAsync();

            var problem = new Problem
            {
                Description = $"Помыть номер {dirtyroom.Number}"
            };
            db.Problems.Add(problem);
            await db.SaveChangesAsync();
            return Ok("Грязный номер");
        }

        //[HttpPost("AddNewProcedureOnGuest")]
        //public async 
    }
}
