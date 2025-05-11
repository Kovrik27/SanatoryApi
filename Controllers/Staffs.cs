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
            var staffs = await db.Staff.Include(s => s.Cabinet).Include(s => s.JobTitle).Include(s=> s.Days).Where(s=>s.JobTitle.Title.Contains("Врач")).ToListAsync();

            var stc = staffs.Select(s => new CabinetWithStaff
            {
                Id = s.Id,
                Lastname = s.Lastname,
                Name = s.Name,
                Surname = s.Surname,
                JobTitleId = s.JobTitleId,
                Mail = s.Mail,
                Phone = s.Phone,
                CabinetId = s.Cabinet?.Id,
                Number = s.Cabinet?.Number,
                Cabinet = new Cabinet { Id = s.Cabinet.Id, Number = s.Cabinet.Number },
                JobTitle = new JobTitle { Id = s.JobTitle.Id, Title = s.JobTitle.Title },
                WorkDays = s.Days.Select(d => new CabinetWithStaff.DayDTO2
                {
                    Id = d.Id,
                    Day1 = d.Day1,
                }).ToList()
            }).ToList();

            return Ok(stc);
        }

        [HttpGet("GetStaffWithProblem")]
        public async Task<ActionResult<List<Staff>>> GetStaffWithProblem()
        {
            var staffs = await db.Staff.Include(s=>s.Problems).Include(s => s.JobTitle).Include(s => s.Days).Where(s => ! s.JobTitle.Title.Contains("Врач")).ToListAsync();

            var stp = staffs.Select(s => new ProblemWithStaff
            {
                Id = s.Id,
                Lastname = s.Lastname,
                Name = s.Name,
                Surname = s.Surname,
                JobTitleId = s.JobTitleId,
                Mail = s.Mail,
                Phone = s.Phone,
                Problems = s.Problems.Select(s=>new Models.Problem { Id = s.Id, Description = s.Description, Place = s.Place }).ToList(),
                JobTitle = new JobTitle { Id = s.JobTitle.Id, Title = s.JobTitle.Title },
                Days = s.Days.Select(d => new DayDTO
                 {
                     Id = d.Id,
                     Day1 = d.Day1,
                 }).ToList()
            }).ToList();

            return Ok(stp);
        }

        [HttpGet("GetStaffId/{id}")]
        public async Task<ActionResult<Staff>> GetStaffId(int id)
        {
            var staff = await db.Staff.Include(s => s.Problems).Include(s => s.JobTitle).Include(s => s.Days).FirstOrDefaultAsync(s => s.Id == id);

            if (staff == null)
            {
                return NotFound(); 
            }
            var staffid = new ProblemWithStaff
            {
                Id = staff.Id,
                Lastname = staff.Lastname,
                Name = staff.Name,
                Surname = staff.Surname,
                JobTitleId = staff.JobTitleId,
                Mail = staff.Mail,
                Phone = staff.Phone,
                Problems = staff.Problems.Select(s => new Models.Problem { Id = s.Id, Description = s.Description, Place = s.Place }).ToList(),
                JobTitle = new JobTitle { Id = staff.JobTitle.Id, Title = staff.JobTitle.Title },
                Days = staff.Days.Select(d => new DayDTO
                {
                    Id = d.Id,
                    Day1 = d.Day1,
                }).ToList()
            };

            return Ok(staffid); 
        }


        [HttpPost("AddNewStaff")]
        public async Task<ActionResult> AddNewStaff(Staff staff)
        {
            try
            {
                staff.JobTitleId = staff.JobTitle.Id;
                staff.JobTitle = null;
                staff.Days = null;
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
            bool checkProblems = db.Problems.Any(s => s.StaffId == id && s.StatusProblemId != 3);
            if (checkProblems)
            {
                return BadRequest("Пусть доделает свою работу и валит потом");
            }
            var staffToDelete = db.Staff.FirstOrDefault(s => s.Id == id);
            if (staffToDelete != null && staffToDelete.CabinetId == null)
            {
                //db.Staff.Remove(staffToDelete);
                await db.SaveChangesAsync();
                return Ok("Работник успешно уволен!");
            }
            else
            {
                return BadRequest("Сотрудник для увольнения не найден!");
            }
        }


        [HttpPost("AddProblemOnStaff")]
        public async Task<ActionResult> AddProblemOnStaff(ProblemOnStaff ProblemOnStaff)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == ProblemOnStaff.StaffId);
            var problem = db.Problems.FirstOrDefault(s => s.Id == ProblemOnStaff.ProblemId);
            if(staff != null && problem != null)
            {
                problem.StaffId = staff.Id;
                await db.SaveChangesAsync();
                return Ok("Задача успешно присвоена сотруднику!");
            }
            else
            {
                return BadRequest("Сотрудник/Задача не найден(а)!");
            }
        }

        public ObservableCollection<CabinetWithStaff> cabinetOnStaff = new ();

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

        [HttpPut("DoneCabinet/{id}")]
        public async Task<ActionResult> DoneCabinet(int id)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == id);
            if(staff != null)
            {
                staff.CabinetId = 1;
                await db.SaveChangesAsync();
                return Ok("Кабинет успешно избавлен от сотрудника");
            }
            else
            {
                return BadRequest("Сотрудник не найден!");
            }
        }
        [HttpPut("DoneProblem/{idProblem}")]
        public async Task<ActionResult> DoneProblem(int idProblem)
        {
            var problem = db.Problems.FirstOrDefault(s => s.Id == idProblem);
            if (problem != null)
            {
                problem.StatusProblemId = 3;
                await db.SaveChangesAsync();
                return Ok("Задача успешно выполнена!");
            }
            else
            {
                return BadRequest("Сотрудник не найден!");
            }
        }

        [HttpGet("GetAllStatusesProblem")]
        public async Task<ActionResult<List<StatusProblem>>> GetAllStatusesProblem()
        {
            return new List<StatusProblem>(await db.StatusProblems.ToListAsync());
        }

        [HttpGet("GetAllJobTitle")]
        public async Task<ActionResult<List<JobTitle>>> GetAllJobTitle()
        {
            return new List<JobTitle>(await db.JobTitles.ToListAsync());
        }
    }
}
