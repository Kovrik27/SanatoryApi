using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class EventOnDay
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public virtual List<Event>? Events { get; set; } = new ();
    }
}
