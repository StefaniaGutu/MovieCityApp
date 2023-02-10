using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCity.BusinessLogic.Implementation.ReviewImp;
using MovieCity.WebApp.Code.Base;

namespace MovieCity.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : BaseController
    {
        private readonly ReviewService Service;
        public ReviewController(ControllerDependencies dependencies, ReviewService reviewService) : base(dependencies)
        {
            this.Service = reviewService;
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> AllReviews()
        //{
        //    var model = await Service.GetAllReviews();

        //    return View(model);
        //}

        //[HttpPost]
        //[Authorize]
        //public JsonResult CreateReview(Guid movieId, int rating, string reviewText, bool showInFeed, string postText)
        //{
        //    if(reviewText == null)
        //    {
        //        return Json(null);
        //    }

        //    var id = Service.CreateReview(movieId, rating, reviewText, showInFeed, postText);

        //    return Json(id);
        //}

        //[HttpDelete]
        //[Authorize]
        //public async Task<IActionResult> DeleteReview(string id)
        //{
        //    if (await Service.DeleteReview(id))
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    } 
        //}
    }
}
