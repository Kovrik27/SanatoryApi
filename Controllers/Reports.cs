//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SanatoryApi.DoubleModels;
//using SanatoryApi.Models;

//namespace SanatoryApi.Controllers
//{
//        [Route("api/[controller]")]
//        [ApiController]
//        public class Reports : ControllerBase
//        {
//            private readonly SanatoryContext _db;

//            public Reports(SanatoryContext db)
//            {
//                _db = db;
//            }
//            [HttpGet("accommodation")]
//        public async Task<ActionResult<AccommodationReportDTO>> GetAccommodationReport(
//            [FromQuery] DateTime? startDate,
//            [FromQuery] DateTime? endDate)
//        {
//            if (!startDate.HasValue)
//                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

//            if (!endDate.HasValue)
//                endDate = startDate.Value.AddMonths(1).AddDays(-1);

//            var stays = await _db.Stays
//                .Include(s => s.Guest)
//                .Include(s => s.Room)
//                .Where(s => s.CheckOutDate >= startDate && s.CheckInDate <= endDate)
//                .ToListAsync();

//            var rooms = await _db.Rooms.ToListAsync();
//            int totalDaysInPeriod = (endDate.Value - startDate.Value).Days + 1;


//            var totalNights = stays.Sum(s => (s.CheckOutDate - s.CheckInDate).Days);
//            var totalIncome = stays.Sum(s => s.TotalCost);

//            var roomStats = rooms.Select(room =>
//            {
//                var roomStays = stays.Where(s => s.RoomId == room.Id).ToList();
//                int occupiedDays = roomStays.Sum(s => (s.CheckOutDate - s.CheckInDate).Days);

//                return new RoomOccupancyDTO
//                {
//                    RoomNumber = room.Number,
//                    RoomType = room.Type,
//                    TotalDays = totalDaysInPeriod,
//                    OccupiedDays = occupiedDays,
//                    OccupancyPercentage = totalDaysInPeriod > 0
//                        ? Math.Round((double)occupiedDays / totalDaysInPeriod * 100, 1)
//                        : 0,
//                    TotalIncome = roomStays.Sum(s => s.TotalCost)
//                };
//            }).ToList();


//            var dailyIncome = new List<DailyIncomeDTO>();
//            for (DateTime date = startDate.Value; date <= endDate.Value; date = date.AddDays(1))
//            {
//                var dayStays = stays.Where(s => s.CheckInDate <= date && s.CheckOutDate >= date);
//                dailyIncome.Add(new DailyIncomeDTO
//                {
//                    Date = date,
//                    AccommodationIncome = dayStays.Sum(s => s.Room.Price),
//                    GuestsCheckedIn = stays.Count(s => s.CheckInDate.Date == date.Date),
//                    GuestsCheckedOut = stays.Count(s => s.CheckOutDate.Date == date.Date)
//                });
//            }

//            var report = new AccommodationReportDTO
//            {
//                Period = new ReportPeriodDTO { StartDate = startDate.Value, EndDate = endDate.Value },
//                Summary = new AccommodationSummaryDTO
//                {
//                    TotalGuests = stays.Select(s => s.GuestId).Distinct().Count(),
//                    TotalNights = totalNights,
//                    TotalIncome = totalIncome,
//                    AverageStayLength = stays.Any() ? Math.Round((decimal)totalNights / stays.Count, 1) : 0,
//                    AverageDailyRate = totalNights > 0 ? Math.Round(totalIncome / totalNights, 2) : 0,
//                    OccupancyRate = Math.Round(roomStats.Average(r => r.OccupancyPercentage), 1)
//                },
//                Details = stays.Select(s => new AccommodationDetailDTO
//                {
//                    StayId = s.Id,
//                    GuestName = $"{s.Guest.Surname} {s.Guest.Name} {s.Guest.Lastname}",
//                    Passport = s.Guest.Pasport,
//                    RoomNumber = s.Room.Number,
//                    RoomType = s.Room.Type,
//                    CheckInDate = s.CheckInDate,
//                    CheckOutDate = s.CheckOutDate,
//                    Nights = (s.CheckOutDate - s.CheckInDate).Days,
//                    PricePerNight = (decimal)s.Room.Price,
//                    TotalCost = s.TotalCost,
//                    PaymentDate = s.PaymentDate
//                }).OrderByDescending(s => s.CheckInDate).ToList(),
//                RoomStats = roomStats,
//                DailyIncome = dailyIncome
//            };

//            return Ok(report);
//        }

//        [HttpGet("procedures")]
//        public async Task<ActionResult<ProcedureReportDTO>> GetProcedureReport(
//            [FromQuery] DateTime? startDate,
//            [FromQuery] DateTime? endDate)
//        {
//            if (!startDate.HasValue)
//                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

//            if (!endDate.HasValue)
//                endDate = DateTime.Now;

//            var guestProcedures = await _db.GuestProcedures
//                .Include(gp => gp.Guest)
//                .ThenInclude(g => g.Room)
//                .Include(gp => gp.Procedure)
//                .Where(gp => gp.ServiceDate >= startDate && gp.ServiceDate <= endDate)
//                .ToListAsync();

//            var totalProcedures = guestProcedures.Sum(gp => gp.Quantity);
//            var totalIncome = guestProcedures.Sum(gp => gp.TotalCost);
//            var uniqueGuests = guestProcedures.Select(gp => gp.GuestId).Distinct().Count();

//            var popularity = guestProcedures
//                .GroupBy(gp => new { gp.ProcedureId, gp.Procedure.Title, gp.Procedure.Category })
//                .Select(g => new ProcedurePopularityDTO
//                {
//                    ProcedureId = g.Key.ProcedureId,
//                    ProcedureName = g.Key.Title,
//                    Category = g.Key.Category ?? "Общее",
//                    TimesPerformed = g.Sum(gp => gp.Quantity),
//                    UniqueGuests = g.Select(gp => gp.GuestId).Distinct().Count(),
//                    TotalIncome = g.Sum(gp => gp.TotalCost),
//                    AveragePrice = Math.Round(g.Average(gp => gp.PriceAtService), 2),
//                    PopularityPercent = totalProcedures > 0
//                        ? Math.Round((double)g.Sum(gp => gp.Quantity) / totalProcedures * 100, 1)
//                        : 0
//                })
//                .OrderByDescending(p => p.TimesPerformed)
//                .ToList();


//            var categoryStats = guestProcedures
//                .GroupBy(gp => gp.Procedure.Category ?? "Общее")
//                .Select(g => new CategoryStatsDTO
//                {
//                    Category = g.Key,
//                    ProceduresCount = g.Sum(gp => gp.Quantity),
//                    TotalIncome = g.Sum(gp => gp.TotalCost),
//                    Percentage = totalIncome > 0
//                        ? Math.Round((double)g.Sum(gp => gp.TotalCost) / (double)totalIncome * 100, 1)
//                        : 0
//                })
//                .OrderByDescending(c => c.TotalIncome)
//                .ToList();


//            var doctorStats = guestProcedures
//                .Where(gp => !string.IsNullOrEmpty(gp.DoctorName))
//                .GroupBy(gp => gp.DoctorName)
//                .Select(g => new DoctorStatsDTO
//                {
//                    DoctorName = g.Key,
//                    ProceduresCount = g.Sum(gp => gp.Quantity),
//                    UniqueGuests = g.Select(gp => gp.GuestId).Distinct().Count(),
//                    TotalIncome = g.Sum(gp => gp.TotalCost),
//                    TopProcedures = g
//                        .GroupBy(gp => gp.Procedure.Title)
//                        .Select(pg => new ProcedurePopularityDTO
//                        {
//                            ProcedureName = pg.Key,
//                            TimesPerformed = pg.Sum(gp => gp.Quantity),
//                            TotalIncome = pg.Sum(gp => gp.TotalCost)
//                        })
//                        .OrderByDescending(p => p.TimesPerformed)
//                        .Take(3)
//                        .ToList()
//                })
//                .OrderByDescending(d => d.TotalIncome)
//                .ToList();

//            var report = new ProcedureReportDTO
//            {
//                Period = new ReportPeriodDTO { StartDate = startDate.Value, EndDate = endDate.Value },
//                Summary = new ProcedureSummaryDTO
//                {
//                    TotalProcedures = totalProcedures,
//                    UniqueGuests = uniqueGuests,
//                    TotalIncome = totalIncome,
//                    AveragePerDay = Math.Round(totalIncome / (endDate.Value - startDate.Value).Days, 2),
//                    ProceduresPerGuest = uniqueGuests > 0 ? (int)Math.Round((double)totalProcedures / uniqueGuests) : 0,
//                    AverageProcedurePrice = totalProcedures > 0 ? Math.Round(totalIncome / totalProcedures, 2) : 0
//                },
//                Popularity = popularity,
//                Details = guestProcedures.Select(gp => new ProcedureDetailDTO
//                {
//                    GuestProcedureId = gp.Id,
//                    ServiceDate = gp.ServiceDate,
//                    GuestName = $"{gp.Guest.Surname} {gp.Guest.Name} {gp.Guest.Lastname}",
//                    RoomNumber = gp.Guest.Room?.Number ?? 0,
//                    ProcedureName = gp.Procedure.Title,
//                    Category = gp.Procedure.Category ?? "Общее",
//                    Quantity = gp.Quantity,
//                    Price = gp.PriceAtService,
//                    TotalCost = gp.TotalCost,
//                    DoctorName = gp.DoctorName ?? "Не указан",
//                    IsPaid = gp.IsPaid
//                }).OrderByDescending(gp => gp.ServiceDate).ToList(),
//                DoctorStats = doctorStats,
//                CategoryStats = categoryStats
//            };

//            return Ok(report);
//        }


//        [HttpGet("financial")]
//        public async Task<ActionResult<FinancialReportDTO>> GetFinancialReport(
//            [FromQuery] DateTime? startDate,
//            [FromQuery] DateTime? endDate)
//        {
//            if (!startDate.HasValue)
//                startDate = new DateTime(DateTime.Now.Year, 1, 1); // Начало года

//            if (!endDate.HasValue)
//                endDate = DateTime.Now;

//            var stays = await _db.Stays
//                .Include(s => s.Guest)
//                .Include(s => s.Room)
//                .Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate)
//                .ToListAsync();

//            var procedures = await _db.GuestProcedures
//                .Include(gp => gp.Guest)
//                .Include(gp => gp.Procedure)
//                .Where(gp => gp.PaymentDate >= startDate && gp.PaymentDate <= endDate && gp.IsPaid)
//                .ToListAsync();

//            var accommodationIncome = stays.Sum(s => s.TotalCost);
//            var proceduresIncome = procedures.Sum(p => p.TotalCost);
//            var totalIncome = accommodationIncome + proceduresIncome;

//            var monthlyComparison = stays
//                .GroupBy(s => new { s.PaymentDate!.Value.Year, s.PaymentDate.Value.Month })
//                .Select(g => new MonthlyComparisonDTO
//                {
//                    Year = g.Key.Year,
//                    Month = System.Globalization.CultureInfo.CurrentCulture
//                        .DateTimeFormat.GetMonthName(g.Key.Month),
//                    AccommodationIncome = g.Sum(s => s.TotalCost),
//                    ProceduresIncome = procedures
//                        .Where(p => p.PaymentDate!.Value.Year == g.Key.Year
//                                 && p.PaymentDate!.Value.Month == g.Key.Month)
//                        .Sum(p => p.TotalCost),
//                    TotalIncome = g.Sum(s => s.TotalCost) + procedures
//                        .Where(p => p.PaymentDate!.Value.Year == g.Key.Year
//                                 && p.PaymentDate!.Value.Month == g.Key.Month)
//                        .Sum(p => p.TotalCost)
//                })
//                .OrderBy(m => m.Year).ThenBy(m => m.Month)
//                .ToList();

//            var allGuestSpendings = stays
//                .GroupBy(s => new { s.GuestId, GuestName = $"{s.Guest.Surname} {s.Guest.Name} {s.Guest.Lastname}" })
//                .Select(g => new TopSpenderDTO
//                {
//                    GuestName = g.Key.GuestName,
//                    RoomNumber = g.First().Guest.Room?.Number.ToString() ?? "Не указан",
//                    AccommodationCost = g.Sum(s => s.TotalCost),
//                    ProceduresCost = procedures.Where(p => p.GuestId == g.Key.GuestId).Sum(p => p.TotalCost),
//                    TotalSpent = g.Sum(s => s.TotalCost) + procedures.Where(p => p.GuestId == g.Key.GuestId).Sum(p => p.TotalCost),
//                    ProceduresCount = procedures.Where(p => p.GuestId == g.Key.GuestId).Sum(p => p.Quantity)
//                })
//                .OrderByDescending(g => g.TotalSpent)
//                .Take(10)
//                .ToList();

//            var report = new FinancialReportDTO
//            {
//                Period = new ReportPeriodDTO { StartDate = startDate.Value, EndDate = endDate.Value },
//                Summary = new FinancialSummaryDTO
//                {
//                    TotalIncome = totalIncome,
//                    AccommodationIncome = accommodationIncome,
//                    ProceduresIncome = proceduresIncome,
//                    AccommodationPercent = totalIncome > 0 ? Math.Round((double)accommodationIncome / (double)totalIncome * 100, 1) : 0,
//                    ProceduresPercent = totalIncome > 0 ? Math.Round((double)proceduresIncome / (double)totalIncome * 100, 1) : 0,
//                    TotalGuests = stays.Select(s => s.GuestId).Union(procedures.Select(p => p.GuestId)).Distinct().Count(),
//                    AverageGuestSpending = totalIncome > 0 ? Math.Round(totalIncome / stays.Select(s => s.GuestId).Union(procedures.Select(p => p.GuestId)).Distinct().Count(), 2) : 0
//                },
//                IncomeBreakdown = new IncomeBreakdownDTO
//                {
//                    ByCategory = procedures
//                        .GroupBy(p => p.Procedure.Category ?? "Общее")
//                        .Select(g => new CategoryIncomeDTO
//                        {
//                            Category = g.Key,
//                            Amount = g.Sum(p => p.TotalCost),
//                            Percentage = totalIncome > 0 ? Math.Round((double)g.Sum(p => p.TotalCost) / (double)totalIncome * 100, 1) : 0
//                        })
//                        .Concat(new[] {
//                            new CategoryIncomeDTO
//                            {
//                                Category = "Проживание",
//                                Amount = accommodationIncome,
//                                Percentage = totalIncome > 0 ? Math.Round((double)accommodationIncome / (double)totalIncome * 100, 1) : 0
//                            }
//                        })
//                        .OrderByDescending(c => c.Amount)
//                        .ToList(),
//                    ByRoom = stays
//                        .GroupBy(s => s.Room.Number)
//                        .Select(g => new RoomIncomeDTO
//                        {
//                            RoomNumber = g.Key,
//                            AccommodationIncome = g.Sum(s => s.TotalCost),
//                            ProceduresIncome = procedures.Where(p => p.Guest.Room?.Number == g.Key).Sum(p => p.TotalCost),
//                            TotalIncome = g.Sum(s => s.TotalCost) + procedures.Where(p => p.Guest.Room?.Number == g.Key).Sum(p => p.TotalCost)
//                        })
//                        .OrderBy(r => r.RoomNumber)
//                        .ToList()
//                },
//                MonthlyComparison = monthlyComparison,
//                TopSpenders = allGuestSpendings
//            };

//            return Ok(report);
//        }


//        [HttpGet("export")]
//        public async Task<IActionResult> ExportReport(
//            [FromQuery] string reportType,
//            [FromQuery] DateTime? startDate,
//            [FromQuery] DateTime? endDate)
//        {

//            var sb = new System.Text.StringBuilder();

//            switch (reportType.ToLower())
//            {
//                case "accommodation":
//                    var accReport = await GetAccommodationReport(startDate, endDate);
//                    var accData = (accReport.Result as OkObjectResult)?.Value as AccommodationReportDTO;

//                    if (accData != null)
//                    {
//                        sb.AppendLine("Дата заезда,Дата выезда,Гость,Номер,Ночей,Сумма");
//                        foreach (var detail in accData.Details)
//                        {
//                            sb.AppendLine($"{detail.CheckInDate:dd.MM.yyyy},{detail.CheckOutDate:dd.MM.yyyy}," +
//                                         $"{detail.GuestName},{detail.RoomNumber},{detail.Nights},{detail.TotalCost}");
//                        }
//                    }
//                    break;

//                case "procedures":
//                    var procReport = await GetProcedureReport(startDate, endDate);
//                    var procData = (procReport.Result as OkObjectResult)?.Value as ProcedureReportDTO;

//                    if (procData != null)
//                    {
//                        sb.AppendLine("Дата,Гость,Номер,Процедура,Врач,Кол-во,Сумма");
//                        foreach (var detail in procData.Details)
//                        {
//                            sb.AppendLine($"{detail.ServiceDate:dd.MM.yyyy},{detail.GuestName}," +
//                                         $"{detail.RoomNumber},{detail.ProcedureName},{detail.DoctorName}," +
//                                         $"{detail.Quantity},{detail.TotalCost}");
//                        }
//                    }
//                    break;
//            }

//            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
//            return File(bytes, "text/csv", $"report_{reportType}_{DateTime.Now:yyyyMMdd}.csv");
//        }
//    }
//}
    

