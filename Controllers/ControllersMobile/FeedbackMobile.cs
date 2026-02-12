using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanatoryApi.DoubleModels;
using SanatoryApi.Models;

namespace SanatoryApi.Controllers.ControllersMobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackMobile : Controller
    {
        readonly SanatoryContext db;
        public FeedbackMobile(SanatoryContext db)
        {
            this.db = db;
        }

        [HttpGet("GetAllFeedbacks")]
        public async Task<List<Feedback>> GetAllFeedbacks()
        {
            return new List<Feedback>(await db.Feedbacks.ToListAsync());
        }

        [HttpPost("AddNewFeedback")]
        public async Task<ActionResult> AddNewFeedback(Feedback feedback)
        {
            db.Feedbacks.Add(feedback);
            await db.SaveChangesAsync();
            return Ok("Данные отзыва успешно изменены!");
        }

        [HttpPut("EditFeedback")]
        public async Task<ActionResult> EditFeedback(Feedback feedback)
        {
            db.Feedbacks.Update(feedback);
            await db.SaveChangesAsync();
            return Ok("Данные отзыва успешно изменены!");
        }



    }
}
