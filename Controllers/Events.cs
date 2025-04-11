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

        private static List<EventOnDay> eventsOnDays = new();


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

        //[HttpPut("EditEventOnDay")]
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

        //[HttpDelete("DeleteEvent/{id}")]
        //public async Task<ActionResult> DeleteEvent(int id)
        //{
        //    var eventToDelete = db.Events.FirstOrDefault(s => s.Id == id);
        //    if (eventToDelete != null)
        //    {
        //        db.Events.Remove(eventToDelete);
        //        await db.SaveChangesAsync();
        //        return Ok("Мероприятие успешно удалено/завершено!");
        //    }
        //    else
        //    {
        //        return BadRequest("Мероприятие для удаления/завершения не найдено!");
        //    }
        //}
    }
}
