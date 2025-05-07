using SanatoryApi.Models;

namespace SanatoryApi.DoubleModels
{
    public class ProblemWithStaff
    {    
        public int Id { get; set; }

        public string Lastname { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int JobTitleId { get; set; }

        public string Phone { get; set; } = null!;

        public string Mail { get; set; } = null!;

        public int? ProblemId { get; set; }

        public string? Description { get; set; } = null!;

        public int? Number { get; set; } = null!;

        public List<Problem> Problems { get; set; }

        public JobTitle JobTitle { get; set; }

        public List<DayDTO> Days { get; set; } = new();
    }

    public class DayDTO
    {
        public int Id { get; set; }
        public string Day1 { get; set; }
    }
}
