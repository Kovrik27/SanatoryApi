using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Procedures : ControllerBase
    {
        readonly SanatoryContext db;
        public Procedures(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllProcedures")]
        public async Task<List<Procedure>> GetAllProcedures()
        {
            return new List<Procedure>(await db.Procedures.ToListAsync());
        }

        [HttpPost("AddNewProcedure")]
        public async Task<ActionResult> AddNewProcedure(Procedure procedure)
        {
            db.Procedures.Add(procedure);
            await db.SaveChangesAsync();
            return Ok("Новая процедура успешно добавлена!");
        }

        [HttpPut("EditProcedure")]
        public async Task<ActionResult> EditProcedure(Procedure procedure)
        {
            db.Procedures.Update(procedure);
            await db.SaveChangesAsync();
            return Ok("Данные процедуры успешно изменены!");
        }

        [HttpDelete("DeleteProcedure/{id}")]
        public async Task<ActionResult> DeleteProcedure(int id)
        {
            var procedureToDelete = db.Procedures.FirstOrDefault(s => s.Id == id);
            if (procedureToDelete != null)
            {
                db.Procedures.Remove(procedureToDelete);
                await db.SaveChangesAsync();
                return Ok("Процедура успешно удалена!");
            }
            else
            {
                return BadRequest("Процедура для удаления не найдена!");
            }
        }
    }
}
