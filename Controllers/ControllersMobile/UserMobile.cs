using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMobile : ControllerBase
    {
        readonly SanatoryContext db;

        public UserMobile(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<User>> GetUsers()
        {
            return new List<User>(await db.Users.ToListAsync());
        }

        //[HttpPost("AddNewUser")]
        //public async Task<ActionResult> AddNewUser(User user)
        //{
        //    user.RoleId = 0;

        //    if(user == null)
        //    {
        //        return BadRequest("Пользователь не может быть null");
        //    }

        //    try
        //    {
        //        db.Users.Add(user);
        //        await db.SaveChangesAsync();
        //        return Ok("Новый пользователь успешно добавлен!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Ошибка при добавлении пользователя: {ex.Message}");
        //    }
        //}
        [HttpPost("CheckUser")]
        public async Task<ActionResult<User>> CheckUser(User user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Введите данные");
            }
            var check = await db.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (check == null)
                return BadRequest("Пользователь не найден");
            else
                return Ok(check);

        }

      
    }

       
    
}
