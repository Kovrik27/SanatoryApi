using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;
using System.Data;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Daytims : ControllerBase
    {
        readonly SanatoryContext db;
        public Daytims(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllDaytime")]
        public async Task<List<Daytime>> GetAllDaytime()
        {
            return new List<Daytime>(await db.Daytimes.ToListAsync());
        }

        [HttpPost("AddNewDaytime")]
        public async Task<ActionResult> AddNewDaytime(Daytime daytime)
        {
            db.Daytimes.Add(daytime);
            await db.SaveChangesAsync();
            return Ok("Новый день успешно добавлен!");
        }

        [HttpPut("EditDaytime")]
        public async Task<ActionResult> EditDaytime(Daytime daytime)
        {
            db.Daytimes.Update(daytime);
            await db.SaveChangesAsync();
            return Ok("Данные дня успешно изменены!");
        }

        [HttpDelete("DeleteDaytime/{id}")]
        public async Task<ActionResult> DeleteDaytime(int id)
        {
            var daytimeToDelete = db.Daytimes.FirstOrDefault(s => s.Id == id);
            if (daytimeToDelete != null)
            {
                db.Daytimes.Remove(daytimeToDelete);
                await db.SaveChangesAsync();
                return Ok("День успешно удалён!");
            }
            else
            {
                return BadRequest("День для удаления не найден!");
            }
        }
    }
}
