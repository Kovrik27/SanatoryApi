using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemMobile : ControllerBase
    {
        readonly SanatoryContext db;
        public ProblemMobile(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetProblemsByStaff/{id}")]
        public async Task<List<Problem>> GetProblemsByStaff(int id)
        {
            return await db.Problems.Where(s => s.StaffId == id).ToListAsync();
        }

        [HttpPost("AddNewProblem")]
        public async Task<ActionResult> AddNewProblem(Problem problem)
        {
            problem.StatusProblemId = 3;
            db.Problems.Add(problem);
            await db.SaveChangesAsync();
            return Ok("Новая задача успешно добавлена!");
        }
    }
}
