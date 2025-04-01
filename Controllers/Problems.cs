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
            return new List<Problem>(await db.Problems.ToListAsync());
        }

        [HttpPost("AddNewProblem")]
        public async Task<ActionResult> AddNewProblem(Problem problem)
        {
            db.Problems.Add(problem);
            await db.SaveChangesAsync();
            return Ok("Новая задача успешно добавлена!");
        }

        [HttpPut("EditProblem")]
        public async Task<ActionResult> EditProblem(int id)
        {
            var problemToEdit = db.Problems.FirstOrDefault(s => s.Id == id);
            if(problemToEdit != null)
            {
                db.Problems.Update(problemToEdit);
                await db.SaveChangesAsync();
                return Ok("Данные задачи успешно изменены!");
            }
           else
            {
                return BadRequest("Ошибка! Задача для изменения не найдена!");
            }
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
        public async Task<ActionResult> EditStatusProblem(int id)
        {
            var problemToeditstatus = db.Problems.FirstOrDefault(s => s.Id == id);
            if(problemToeditstatus != null)
            {
                 problemToeditstatus.
            }
        }
    }
}
