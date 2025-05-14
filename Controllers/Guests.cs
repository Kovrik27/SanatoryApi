using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;
using System.Runtime.InteropServices;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Guests : ControllerBase
    {
        readonly SanatoryContext db;
        public Guests(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllGuests")]
        public async Task<ActionResult<List<Guest>>> GetAllGuests()
        {
            var guests = await db.Guests.Include(s => s.Room).ToListAsync();

            var gur = guests.Select(s => new GuestWithRoom
            {
                Id = s.Id,
                Lastname = s.Lastname,
                Name = s.Name,
                Surname = s.Surname,
                Pasport = s.Pasport,
                Policy = s.Policy,
                DataArrival = s.DataArrival,
                DataOfDeparture = s.DataOfDeparture,
                Room = new Room { Id = s.Room.Id, Number = s.Room.Number, Price = s.Room.Price, StatusId = s.Room.StatusId, Type = s.Room.Type, Status = s.Room.Status },
                Procedures = s.Procedures.Select(p => new ProcedureDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                }).ToList(),
            }).ToList();

            return Ok(gur);
        }

        [HttpGet($"GetProceduresByGuest/{id}")]
        public async Task<List<Procedure>> GetProceduresByGuest(int id)
        {
            var procedures = await db.Guests.Where(g => g.Id == id).SelectMany(g => g.Procedures).ToListAsync();
            return procedures;
        }

        public async Task<ActionResult<Guest>> GetGuestId(int id)
        {
            var guest = await db.Guests.Include(s => s.Procedures).FirstOrDefaultAsync(s => s.UserId == id);

            if (guest == null)
            {
                return NotFound();
            }
            var guestid = new GuestWithRoom
            {
                Id = guest.Id,
                Lastname = guest.Lastname,
                Name = guest.Name,
                Surname = guest.Surname,
                DataArrival = guest.DataArrival,
                DataOfDeparture = guest.DataOfDeparture,
                Pasport = guest.Pasport,
                Policy = guest.Policy,
                RoomId = guest.RoomId,
                Room = guest.Room,
                Procedures = guest.Procedures.Select(s => new ProcedureDTO {Id = s.Id, Date = s.Date, Description = s.Description, Duration = s.Duration, Price = s.Price, Title = s.Title }).ToList(),
            };

            return Ok(guestid);
        }


        [HttpPost("AddNewGuest")]
        public async Task<ActionResult> AddNewGuest(Guest guest)
        {
            guest.Room = null;
            guest.Procedures = null;
            guest.User = null;
            db.Guests.Add(guest);
            await db.SaveChangesAsync();
            return Ok("Новый гость успешно добавлен!");
        }

        [HttpPut("EditGuest")]
        public async Task<ActionResult> EditGuest(Guest guest)
        {
            db.Guests.Update(guest);
            await db.SaveChangesAsync();
            return Ok("Данные гостя успешно изменены!");
        }

        [HttpDelete("GoOutGuest/{id}")]
        public async Task<ActionResult> GoOutGuest(int id)
        {
            var dirtyroom = db.Rooms.Include(r => r.Guests).Include(r => r.Status).FirstOrDefault(r => r.Id == id);
            if (dirtyroom == null)
            {
                return BadRequest("Грязный номер не найден!");
            }
            var guest = dirtyroom.Guests.FirstOrDefault();
            if(guest == null)
            {
                return BadRequest("Номер пустой");
            }
            dirtyroom.Guests.Remove(guest);
            guest.RoomId = 13;
            await db.SaveChangesAsync();
              
            var dirtystatus = db.Statuses.FirstOrDefault(s => s.Title == "Грязный");
            dirtyroom.StatusId = dirtystatus.Id;
            await db.SaveChangesAsync();

            var problem = new Problem
            {
                Description = $"Помыть номер {dirtyroom.Number}"
            };
            db.Problems.Add(problem);
            await db.SaveChangesAsync();
            return Ok("Грязный номер");
        }

        [HttpPost("AddProcedureOnGuest")]
        public async Task<ActionResult> AddProcedureOnGuest(GuestProcedureDTO guestProcedureDTO)
        {
            var guest = db.Guests.FirstOrDefault(s=> s.Id == guestProcedureDTO.GuestId);
            var procedure = db.Procedures.FirstOrDefault(p => p.Id == guestProcedureDTO.ProcedureId);

            guest.Procedures.Add(procedure);
            await db.SaveChangesAsync();
            return Ok("Процедура успешно добавлена");
        }

    }
}
