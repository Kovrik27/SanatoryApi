using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Events : ControllerBase
    {
        readonly SanatoryContext db;
        public Events(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllEvents")]
        public async Task<List<Event>> GetAllEvents()
        {
            return new List<Event>(await db.Events.ToListAsync());
        }

        [HttpPost("AddNewEvent")]
        public async Task<ActionResult> AddNewEvent(Event eventt)
        {
            db.Events.Add(eventt);
            await db.SaveChangesAsync();
            return Ok("Новое мероприятие успешно добавлено!");
        }

        [HttpPut("EditEvent")]
        public async Task<ActionResult> EditeVENT(Event eventt)
        {
            db.Events.Update(eventt);
            await db.SaveChangesAsync();
            return Ok("Данные мероприятия успешно изменены!");
        }

        [HttpDelete("DeleteEvent/{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var eventToDelete = db.Events.FirstOrDefault(s => s.Id == id);
            if (eventToDelete != null)
            {
                db.Events.Remove(eventToDelete);
                await db.SaveChangesAsync();
                return Ok("Мероприятие успешно удалено/завершено!");
            }
            else
            {
                return BadRequest("Мероприятие для удаления/завершения не найдено!");
            }
        }
    }
}
