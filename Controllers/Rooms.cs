using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Rooms : ControllerBase
    {
        readonly SanatoryContext db;
        public Rooms(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllRooms")]
        public async Task<List<Room>> GetAllRooms()
        {
            return new List<Room>(await db.Rooms.ToListAsync());
        }

        [HttpPost("AddNewRoom")]
        public async Task<ActionResult> AddNewRoom(Room room)
        {
            db.Rooms.Add(room);
            await db.SaveChangesAsync();
            return Ok("Новый номер успешно добавлен!");
        }

        [HttpPut("EditRoom")]
        public async Task<ActionResult> EditRoom(Room room)
        {
            db.Rooms.Update(room);
            await db.SaveChangesAsync();
            return Ok("Данные номера успешно изменены!");
        }

        [HttpDelete("DeleteRoom/{id}")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var roomToDelete = db.Rooms.FirstOrDefault(s => s.Id == id);
            if (roomToDelete.Id == id && roomToDelete.Status == "Свободен")
            {
                db.Rooms.Remove(roomToDelete);
                await db.SaveChangesAsync();
                return Ok("Номер успешно снесён!");
            }
            else
            {
                return BadRequest("Произошла ошибка!");
            }
        }
    }
}

