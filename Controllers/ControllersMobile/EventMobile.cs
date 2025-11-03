using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventMobile : ControllerBase
    {
        readonly SanatoryContext db;
        public EventMobile(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllDaysWithEvents")]
        public async Task<ActionResult<DaysWithEvents>> GetAllDaysWithEvents()
        {
            var days = await db.Daytimes.Include(s => s.Events).ToListAsync();

            var dev = days.Select(s => new DaysWithEvents
            {
                Id = s.Id,
                Time = s.Time,
                Events = s.Events.Select(s => new Models.Event { Id = s.Id, Title = s.Title, Date = s.Date, Duration = s.Duration, Place = s.Place }).ToList(),
            });

            return Ok(dev);
        }

        [HttpGet("GetEventByDate/{date}")]
        public async Task<ActionResult<List<Event>>> GetEventsByDate(DateTime date)
        {
            var filteredEvents = await db.Events
                .Where(e => e.Days.Any(s => s.Time.Date == date.Date))
                .ToListAsync();

            if (!filteredEvents.Any())
            {
                return NotFound();
            }
            return Ok(filteredEvents);
        }
    }
}
