using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        readonly SanatoryContext db;
        public Users(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<User>> GetUsers()
        {
            return new List<User>(await db.Users.ToListAsync());
        }

        [HttpPost("AddNewUser")]
        public async Task<ActionResult> AddUser(User user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password))
                return BadRequest("Введите данные");
            var check = db.Users.FirstOrDefault(s => s.Login == user.Login && s.Password == user.Password);
            if (check == null)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Ok("Новый пользователь успешно добавлен!");
            }
            else
                return BadRequest("Такой логин уже существует");
        }

        [HttpPost("CheckUser")]
        public async Task<ActionResult<User>> CheckUser(User user)
        {
            if(string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty (user.Password))
            {
               return BadRequest("Введите данные");
            }
            var check = await db.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (check == null)
                return BadRequest("Пользователь не найден");
            else
                return Ok(check);

        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var userTodelete = db.Users.FirstOrDefault(s => s.Id == id);
            if (userTodelete != null)
            {
                db.Users.Remove(userTodelete);
                await db.SaveChangesAsync();
                return Ok("Юзер успешно удалён!");
            }
            else
                return BadRequest("Юзер для удаления не найден!");
        }

        [HttpGet("GetAllRoleUser")]
        public async Task<List<Role>> GetAllRoleUser()
        {
            return new List<Role>(await db.Roles.ToListAsync());
        }


        [HttpPost("AddUserOn")]
        public async Task<ActionResult> AddUserOn(UserOn UserOn)
        {
            var staff = await db.Staff.FirstOrDefault(s=> s.Id == UserOn.StaffId).Include(s => s.JobTitle).Where(s => !s.JobTitle.Title.Contains("Врач"));
            var user = db.Users.FirstOrDefault(s => s.Id == UserOn.UserId);

            if (staff == null)
            {
                var doctor = await db.Staff.Include(s => s.JobTitle).Where(s => s.JobTitle.Title.Contains("Врач")).ToListAsync(); ;

                if (doctor == null)
                {
                    var guest = db.Guests.FirstOrDefault(s => s.Id == UserOn.GuestId);
                    if (guest == null)
                    {
                        return BadRequest("Никого не нашёл:(");
                    }
                    guest.UserId = user.Id;
                    await db.SaveChangesAsync();
                    return Ok("Пользователь успешно назначен гостю!");
                }
                else
                {
                    doctor.UserId = user.Id;
                    await db.SaveChangesAsync();
                    return Ok("Пользователь успешно назначен врачу");
                }
            }
            else
            {
                staff.UserId = user.Id;
                await db.SaveChangesAsync();
                return Ok("Пользователь успешно назначен тех.персоналу");
            }
        }
    }
    
}
