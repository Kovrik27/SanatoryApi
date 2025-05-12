using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
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


        [HttpGet("GetRoomWithStatus")]
        public async Task<ActionResult<List<Room>>> GetRoomWithStatus()
        {
            var rooms = await db.Rooms.Include(s=> s.Status).ToListAsync();

            var rms = rooms.Select(s => new RoomWithStatus
            {
                Id = s.Id,
                Number = s.Number,
                Price = s.Price,
                Type = s.Type,
                Status = new Status { Id = s.Status.Id, Title = s.Status.Title },
            }).ToList();

            return Ok(rms);
        }

        [HttpPut("EditRoom")]
        public async Task<ActionResult> EditRoom(Room room)
        {
            var roomput = db.Rooms.FirstOrDefault(s => s.Id == room.Id);
            if(roomput == null)
            {
                return BadRequest("Комната не найдена!");
            }
            roomput.Number = room.Number;
            roomput.Price = room.Price;
            roomput.Type = room.Type;
            roomput.Status = room.Status;
            await db.SaveChangesAsync();
            return Ok("Данные номера успешно изменены!");

        }

        [HttpPut("EditStatusRoom")]
        public async Task<ActionResult> EditRoomStatus(Room room)
        {
            var roomput = db.Rooms.FirstOrDefault(s => s.Id == room.Id);
            if (roomput == null)
            {
                return BadRequest("Комната не найдена!");
            }

            roomput.StatusId = 2;
            await db.SaveChangesAsync();
            return Ok("Номер успешно занят!");

        }


        [HttpGet("GetAllStatusesForRoom")]
        public async Task<List<Status>> GetAllStatusesForRoom()
        {
            return new List<Status>(await db.Statuses.ToListAsync());
        }

        
    }
}

