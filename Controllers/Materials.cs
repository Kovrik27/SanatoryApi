//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SanatoryApi.Models;

//namespace SanatoryApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Materials : ControllerBase
//    {
//        readonly SanatoryContext db;
//        public Materials(SanatoryContext db)
//        {
//            this.db = db;
//        }

//        [HttpGet("GetAllMaterials")]
//        public async Task<List<Materials>> GetAllMaterials()
//        {
//            return await db.Problems.ToListaAsync();
//        }
//    }
//}
