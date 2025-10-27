using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
