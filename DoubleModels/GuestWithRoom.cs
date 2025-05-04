using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class GuestWithRoom
    {
        public int Id { get; set; }

        public string Surname { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Pasport { get; set; } = null!;

        public string Policy { get; set; } = null!;

        public DateOnly DataArrival { get; set; }

        public DateOnly DataOfDeparture { get; set; }

        public int RoomId { get; set; }

        public virtual Room Room { get; set; } = null!;

        public List<ProcedureDTO> Procedures = new();

    }

    public class ProcedureDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

    }

    public class GuestProcedureDTO
    {
        public int GuestId { get; set; }
        public int ProcedureId { get; set; }
    }
      
}
