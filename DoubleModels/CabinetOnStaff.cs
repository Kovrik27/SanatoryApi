using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class CabinetOnStaff
    {
        public int StaffId { get; set; }

        public int Id { get; set; }

        public string Lastname { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int JobTitleId { get; set; }

        public string Phone { get; set; } = null!;

        public string Mail { get; set; } = null!;

        public int? CabinetId { get; set; }

        public int? Number {  get; set; } = null!;

        public int? WorkDaysId { get; set; }

        public Cabinet? Cabinet { get; set; }

        public JobTitle JobTitle { get; set; }

        public List<DayDTO2> WorkDays { get; set; } = new();
    

    public class DayDTO2
    {
        public int Id { get; set; }
        public string Day1 { get; set; }
    }
}
}
