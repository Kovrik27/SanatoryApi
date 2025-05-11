using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
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
        public async ActionResult<Events> GetAllEvents()
        {
            return await db.Events.ToListAsync();
        }


        [HttpGet("GetAllDaysWithEvents")]
        public async ActionResult<DaysWithEvents> GetAllDaysWithEvents()
        {
            var days = await db.Daytimes.Include(s => s.Events).ToListAsync();

            var dev = days.Select(s => new DaysWithEvents
            {
                Id = s.Id,
                Time = s.Time,
                Events = s.Events.Select(s=> new Event { Id = s.Id, Title = s.Title, Date = s.Date, Duration = s.Duration, Place = s.Place })
            });
        }

        //[HttpPost("AddNewEventOnDay")]
        //public async Task<ActionResult> AddNewEventOnDay(DateOnly date, Event newEvent)
        //{
        //    var day = eventsOnDays.FirstOrDefault(d => d.Day == date);
        //    if (day == null)
        //    {
        //        day = new EventOnDay { Day = date };
        //        eventsOnDays.Add(day);
        //    }
        //    newEvent.Id = day.Events.Max(e => e.Id) + 1;
        //    day.Events.Add(newEvent);
        //    await db.SaveChangesAsync();
        //    return Ok("Мероприятие на день успешно добавлено!");
        //}


        [HttpDelete("DeleteEvent/{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var eventToDelete = db.Events.FirstOrDefault(s => s.Id == id);
            if (eventToDelete != null)
            {
                db.Events.Remove(eventToDelete);
                await db.SaveChangesAsync();
                return Ok("Мероприятие успешно завершено!");
            }
            else
            {
                return BadRequest("Мероприятие для завершения не найдено!");
            }
        }
    }
}
