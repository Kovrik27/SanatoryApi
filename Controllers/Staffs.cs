 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;
using System.Collections.ObjectModel;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Staffs : ControllerBase
    {
        readonly SanatoryContext db;
        public Staffs(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllStaff")]
        public async Task<List<Staff>> GetAllStaff()
        {
            return new List<Staff>(await db.Staff.ToListAsync());
        }

        [HttpGet("GetStaffWithCabinet")]
        public async Task<ActionResult<List<Staff>>> GetStaffWithCabinet()
        {
            var staffs = await db.Staff.Include(s => s.Cabinet).Include(s => s.JobTitle).Include(s=> s.WorkDays).Where(s=>s.JobTitle.Title.Contains("Врач")).ToListAsync();

            var stc = staffs.Select(s => new CabinetOnStaff
            {
               Id = s.Id,
               Lastname = s.Lastname,
               Name = s.Name,
               Surname = s.Surname,
               JobTitleId = s.JobTitleId,
               Mail = s.Mail,
               Phone = s.Phone,
               WorkDaysId = s.WorkDaysId,
               CabinetId = s.Cabinet?.Id,
               Number = s.Cabinet?.Number,
               WorkDays = new Day { Id = s.WorkDays.Id, Day1 = s.WorkDays.Day1},
               Cabinet = new Cabinet { Id = s.Cabinet.Id, Number = s.Cabinet.Number },
               JobTitle = new JobTitle { Id = s.JobTitle.Id, Title = s.JobTitle.Title }
               
            }).ToList();

            return Ok(stc);
        }

        [HttpGet("GetStaffWithProblem")]
        public async Task<ActionResult<List<Staff>>> GetStaffWithProblem()
        {
            var staffs = await db.Staff.Include(s => s.Problem).Include(s => s.JobTitle).Include(s => s.WorkDays).Where(s => ! s.JobTitle.Title.Contains("Врач")).ToListAsync();

            var stp = staffs.Select(s => new ProblemOnStaff
            {
                Id = s.Id,
                Lastname = s.Lastname,
                Name = s.Name,
                Surname = s.Surname,
                JobTitleId = s.JobTitleId,
                Mail = s.Mail,
                Phone = s.Phone,
                WorkDaysId = s.WorkDaysId,
                ProblemId = s.Problem?.Id,
                Description = s.Problem?.Description,
                Problem = new Problem { Id = s.Problem?.Id, Description = s.Problem?.Description, Place = s.Problem?.Place },
                WorkDays = new Day { Id = s.WorkDays.Id, Day1 = s.WorkDays.Day1 },
                JobTitle = new JobTitle { Id = s.JobTitle.Id, Title = s.JobTitle.Title }
            }).ToList();

            return Ok(stp);
        }


        [HttpPost("AddNewStaff")]
        public async Task<ActionResult> AddNewStaff(Staff staff)
        {
            try
            {
                db.Staff.Add(staff);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }            
            await db.SaveChangesAsync();
            return Ok("Новый сотрудник успешно добавлен!");
        }

        [HttpPut("EditStaff")]
        public async Task<ActionResult> EditStaff(Staff staff)
        {
            db.Staff.Update(staff);
            await db.SaveChangesAsync();
            return Ok("Данные работника успешно изменены!");
        }

        [HttpDelete("GoOutStaff/{id}")]
        public async Task<ActionResult> GoOutStaff(int id)
        {
            var staffToDelete = db.Staff.FirstOrDefault(s => s.Id == id);
            if (staffToDelete != null && staffToDelete.CabinetId == null && staffToDelete.ProblemId == null)
            {
                db.Staff.Remove(staffToDelete);
                await db.SaveChangesAsync();
                return Ok("Работник успешно уволен!");
            }
            else
            {
                return BadRequest("Сотрудник для увольнения не найден!");
            }
        }

        public ObservableCollection<ProblemOnStaff> problemOnStaff = new();

        [HttpPost("AddProblemOnStaff")]
        public async Task<ActionResult> AddProblemOnStaff(ProblemOnStaff ProblemOnStaff)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == ProblemOnStaff.Id);
            var problem = db.Problems.FirstOrDefault(s => s.Id == ProblemOnStaff.ProblemId);
            if(staff != null && problem != null)
            {
                staff.ProblemId = problem.Id;
                await db.SaveChangesAsync();
                return Ok("Задача успешно присвоена сотруднику!");
            }
            else
            {
                return BadRequest("Сотрудник/Задача не найден(а)!");
            }
        }

        public ObservableCollection<CabinetOnStaff> cabinetOnStaff = new ();

        [HttpPost("AddCabinetOnStaff")]
        public async Task<ActionResult> AddCabinetOnStass(CabinetOnStaff CabinetOnStaff)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == CabinetOnStaff.StaffId);
            var cabinet = db.Cabinets.FirstOrDefault(s => s.Id == CabinetOnStaff.CabinetId);
            if (staff != null && cabinet != null)
            {
                staff.CabinetId = cabinet.Id;
                await db.SaveChangesAsync();
                return Ok("Кабинет успешно присвоен сотруднику!");
            }
            else
            {
                return BadRequest("Сотрудник/Кабинет не найден!");
            }
        }

        [HttpPost("DoneCabinet")]
        public async Task<ActionResult> DoneCabinet(CabinetOnStaff CabinetOnStaff)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == CabinetOnStaff.StaffId);
            if(staff != null)
            {
                staff.CabinetId = 0;
                await db.SaveChangesAsync();
                return Ok("Кабинет успешно избавлен от сотрудника");
            }
            else
            {
                return BadRequest("Сотрудник не найден!");
            }
        }
        [HttpPut("DoneProblem/{id}")]
        public async Task<ActionResult> DoneProblem(int id)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == id);
            if (staff != null)
            {
                staff.ProblemId = 1;
                await db.SaveChangesAsync();
                return Ok("Задача успешно выполнена!");
            }
            else
            {
                return BadRequest("Сотрудник не найден!");
            }
        }

    }
}
