using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class EventOnDay
    {
        public List<Event> Events { get; set; } = new List<Event>();
        public DateOnly Day { get; set; }
    }
}
