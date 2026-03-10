namespace SanatoryApi.DoubleModels
{

    public class ReportPeriodDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AccommodationReportDTO
    {
        public ReportPeriodDTO Period { get; set; } = new();
        public AccommodationSummaryDTO Summary { get; set; } = new();
        public List<AccommodationDetailDTO> Details { get; set; } = new();
        public List<RoomOccupancyDTO> RoomStats { get; set; } = new();
        public List<DailyIncomeDTO> DailyIncome { get; set; } = new();
    }

    public class AccommodationSummaryDTO
    {
        public int TotalGuests { get; set; }          
        public int TotalNights { get; set; }            
        public decimal TotalIncome { get; set; }        
        public decimal AverageStayLength { get; set; }  
        public decimal AverageDailyRate { get; set; }   
        public double OccupancyRate { get; set; }      
    }

    public class AccommodationDetailDTO
    {
        public int StayId { get; set; }
        public string GuestName { get; set; } = null!;
        public string Passport { get; set; } = null!;
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Nights { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? PaymentDate { get; set; }
    }

    public class RoomOccupancyDTO
    {
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int TotalDays { get; set; }              
        public int OccupiedDays { get; set; }            
        public double OccupancyPercentage { get; set; }  
        public decimal TotalIncome { get; set; }         
    }


    public class ProcedureReportDTO
    {
        public ReportPeriodDTO Period { get; set; } = new();
        public ProcedureSummaryDTO Summary { get; set; } = new();
        public List<ProcedurePopularityDTO> Popularity { get; set; } = new();
        public List<ProcedureDetailDTO> Details { get; set; } = new();
        public List<DoctorStatsDTO> DoctorStats { get; set; } = new();
        public List<CategoryStatsDTO> CategoryStats { get; set; } = new();
    }

    public class ProcedureSummaryDTO
    {
        public int TotalProcedures { get; set; }        
        public int UniqueGuests { get; set; }            
        public decimal TotalIncome { get; set; }         
        public decimal AveragePerDay { get; set; }       
        public int ProceduresPerGuest { get; set; }      
        public decimal AverageProcedurePrice { get; set; } 
    }

    public class ProcedurePopularityDTO
    {
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int TimesPerformed { get; set; }          
        public int UniqueGuests { get; set; }            
        public decimal TotalIncome { get; set; }         
        public decimal AveragePrice { get; set; }         
        public double PopularityPercent { get; set; }     
    }

    public class ProcedureDetailDTO
    {
        public int GuestProcedureId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string GuestName { get; set; } = null!;
        public int RoomNumber { get; set; }
        public string ProcedureName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalCost { get; set; }
        public string DoctorName { get; set; } = null!;
        public bool IsPaid { get; set; }
    }

    public class DoctorStatsDTO
    {
        public string DoctorName { get; set; } = null!;
        public int ProceduresCount { get; set; }
        public int UniqueGuests { get; set; }
        public decimal TotalIncome { get; set; }
        public List<ProcedurePopularityDTO> TopProcedures { get; set; } = new();
    }

    public class CategoryStatsDTO
    {
        public string Category { get; set; } = null!;
        public int ProceduresCount { get; set; }
        public decimal TotalIncome { get; set; }
        public double Percentage { get; set; }
    }

    public class DailyIncomeDTO
    {
        public DateTime Date { get; set; }
        public decimal AccommodationIncome { get; set; }
        public decimal ProceduresIncome { get; set; }
        public decimal TotalIncome { get; set; }
        public int GuestsCheckedIn { get; set; }
        public int GuestsCheckedOut { get; set; }
        public int ProceduresDone { get; set; }
    }


    public class FinancialReportDTO
    {
        public ReportPeriodDTO Period { get; set; } = new();
        public FinancialSummaryDTO Summary { get; set; } = new();
        public IncomeBreakdownDTO IncomeBreakdown { get; set; } = new();
        public List<MonthlyComparisonDTO> MonthlyComparison { get; set; } = new();
        public List<TopSpenderDTO> TopSpenders { get; set; } = new();
    }

    public class FinancialSummaryDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal AccommodationIncome { get; set; }
        public decimal ProceduresIncome { get; set; }
        public double AccommodationPercent { get; set; }
        public double ProceduresPercent { get; set; }
        public int TotalGuests { get; set; }
        public decimal AverageGuestSpending { get; set; }
    }

    public class IncomeBreakdownDTO
    {
        public List<CategoryIncomeDTO> ByCategory { get; set; } = new();
        public List<RoomIncomeDTO> ByRoom { get; set; } = new();
    }

    public class CategoryIncomeDTO
    {
        public string Category { get; set; } = null!;
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
    }

    public class RoomIncomeDTO
    {
        public int RoomNumber { get; set; }
        public decimal AccommodationIncome { get; set; }
        public decimal ProceduresIncome { get; set; }
        public decimal TotalIncome { get; set; }
    }

    public class MonthlyComparisonDTO
    {
        public string Month { get; set; } = null!;
        public int Year { get; set; }
        public decimal AccommodationIncome { get; set; }
        public decimal ProceduresIncome { get; set; }
        public decimal TotalIncome { get; set; }
    }

    public class TopSpenderDTO
    {
        public string GuestName { get; set; } = null!;
        public string RoomNumber { get; set; } = null!;
        public decimal AccommodationCost { get; set; }
        public decimal ProceduresCost { get; set; }
        public decimal TotalSpent { get; set; }
        public int ProceduresCount { get; set; }
    }
}