using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

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
            return new List<Staff>(await db.Staffs.ToListAsync());
        }

        [HttpPost("AddNewStaff")]
        public async Task<ActionResult> AddNewStaff(Staff staff)
        {
            db.Staffs.Add(staff);
            await db.SaveChangesAsync();
            return Ok("Новый сотрудник успешно добавлен!");
        }

        [HttpPut("EditStaff")]
        public async Task<ActionResult> EditStaff(Staff staff)
        {
            db.Staffs.Update(staff);
            await db.SaveChangesAsync();
            return Ok("Данные работника успешно изменены!");
        }

        [HttpDelete("GoOutStaff/{id}")]
        public async Task<ActionResult> GoOutStaff(int id)
        {
            var staffToDelete = db.Staffs.FirstOrDefault(s => s.Id == id);
            if (staffToDelete.Id == id && staffToDelete.CabinetId == null && staffToDelete.ProblemId == null)
            {
                db.Staffs.Remove(staffToDelete);
                await db.SaveChangesAsync();
                return Ok("Работник успешно уволен!");
            }
            else
            {
                return BadRequest("Произошла ошибка!");
            }
        }
    }
}
