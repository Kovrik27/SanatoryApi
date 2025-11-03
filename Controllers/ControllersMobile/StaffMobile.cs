using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffMobile : ControllerBase
    {
        readonly SanatoryContext db;
        public StaffMobile(SanatoryContext db)
        {
            {
                this.db = db;
            }
        }

        [HttpPut("DoneProblem/{idProblem}")]
        public async Task<ActionResult> DoneProblem(int idProblem)
        {
            var problem = db.Problems.FirstOrDefault(s => s.Id == idProblem);
            if (problem != null)
            {
                problem.StatusProblemId = 3;
                problem.StaffId = null;
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
