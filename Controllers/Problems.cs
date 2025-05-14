using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Problems : ControllerBase
    {
        readonly SanatoryContext db;
        public Problems(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllProblems")]
        public async Task<List<Problem>> GetAllGuests()
        {
            return await db.Problems.ToListAsync();
        }

        [HttpGet("GetProblemsByStaff/{id}")]
        public async Task<List<Problem>> GetProblemsByStaff(int id)
        {
            return await db.Problems.Where(s=>s.StaffId==id).ToListAsync();
        }

        [HttpPost("AddNewProblem")]
        public async Task<ActionResult> AddNewProblem(Problem problem)
        {
            problem.StatusProblem = null;
            db.Problems.Add(problem);
            await db.SaveChangesAsync();
            return Ok("Новая задача успешно добавлена!");
        }

        [HttpPut("EditProblem")]
        public async Task<ActionResult> EditProblem(Problem problem)
        {
                db.Problems.Update(problem);
                await db.SaveChangesAsync();
                return Ok("Данные задачи успешно изменены!");
        }

        [HttpDelete("DeleteProblem/{id}")]
        public async Task<ActionResult> DeleteProblem(int id)
        {
            var problemToDelete = db.Problems.FirstOrDefault(s => s.Id == id);
            if (problemToDelete != null)
            {
                db.Problems.Remove(problemToDelete);
                await db.SaveChangesAsync();
                return Ok("Задача успешно удалена!");
            }
            else
            {
                return BadRequest("Задача для удаления не найдена!");
            }
        }

        [HttpPut("EditStatusProblem")]
        public async Task<ActionResult> EditStatusProblem(Problem problem)
        {
            var problemToeditstatus = db.Problems.FirstOrDefault(s => s.Id == problem.Id);
            if (problemToeditstatus != null)
            {
                problemToeditstatus.StatusProblem.Id = 3;
                return Ok("Задача успешно завершена!");
            }
            else
                return BadRequest("Задача не найдена!");
        }

    }
}
