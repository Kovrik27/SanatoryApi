using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return new List<User>(await db.Users.ToListAsync());
        }

        [HttpPost("AddNewUser")]
        public async Task<ActionResult> AddUser(User user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password))
                return BadRequest("Введите данные");
            var check = db.Users.FirstOrDefault(s => s.Login == user.Login);
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
            var check = db.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
            if (check == null)
                return BadRequest("Пользователь не найден");
            else
                return Ok(check);

        }

        [HttpDelete("DeleteUser")]
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
    }
}
