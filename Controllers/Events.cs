using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        private static List<EventOnDay> eventsOnDays = new();

        [HttpGet("GetAllEvents")]
        public async Task<List<Event>> GetAllEvents()
        {
            return new List<Event>(await db.Events.ToListAsync());
        }

        [HttpGet("GetAllEventsOnDay")]
        public ActionResult<EventOnDay> GetAllEventsOnDay()
        {
            return Ok(eventsOnDays);
        }

        [HttpPost("AddNewEventOnDay")]
        public async Task<ActionResult> AddNewEventOnDay(DateOnly date, Event newEvent)
        {
            var day = eventsOnDays.FirstOrDefault(d => d.Day == date);
            if (day == null)
            {
                day = new EventOnDay { Day = date };
                eventsOnDays.Add(day);
            }
            newEvent.Id = day.Events.Max(e => e.Id) + 1;
            day.Events.Add(newEvent);
            await db.SaveChangesAsync();
            return Ok("Мероприятие на день успешно добавлено!");
        }

        [HttpPut("EditEventOnDay")]
        //public async ActionResult<Event> EditEventOnDay(DateOnly date, int EventId)
        //{
        //    var day = eventsOnDays.FirstOrDefault(d => d.Day == date);
        //    if (day == null)
        //        return BadRequest("На этот день нет мероприятий, чтобы их изменять");

        //}

        //[HttpDelete("DeleteEventOnDay/{id}")]
        //public async Task<ActionResult> DeleteEventOnDay(DateOnly date, int EventId)
        //{
        //    var day = eventsOnDays.FirstOrDefault(d => d.Day == date);
        //    if (day == null)
        //    return BadRequest("На этот день нет мероприятий, чтобы их удалять, ты чо слепой");
        //    else
        //    {
                
        //    }
        //}

        [HttpPost("AddNewEvent")]
        public async Task<ActionResult> AddNewEvent(Event eventt)
        {
            db.Events.Add(eventt);
            await db.SaveChangesAsync();
            return Ok("Новое мероприятие успешно добавлено!");
        }

        //[HttpPost("AddNewEventOnDay")]
        //public async Task<ActionResult> AddNewEventOnDay()
        //{

        //}

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
