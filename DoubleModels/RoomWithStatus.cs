using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class RoomWithStatus
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Type { get; set; } = null!;

        public decimal Price { get; set; }

        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
    }
}
