using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class DaysWithEvents
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public int EventId { get; set; }

        public virtual List<Event> Events { get; set; } = new();
    }
}
