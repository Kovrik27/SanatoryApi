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
            var staff = db.Staff.FirstOrDefault(s => s.Id == ProblemOnStaff.StaffId);
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
        [HttpPost("DoneProblem")]
        public async Task<ActionResult> DoneProblem(ProblemOnStaff ProblemOnStaff)
        {
            var staff = db.Staff.FirstOrDefault(s => s.Id == ProblemOnStaff.StaffId);
            if (staff != null)
            {
                staff.ProblemId = 0;
                await db.SaveChangesAsync();
                return Ok("Кабинет успешно избавлен от сотрудника");
            }
            else
            {
                return BadRequest("Сотрудник не найден!");
            }
        }

    }
}
