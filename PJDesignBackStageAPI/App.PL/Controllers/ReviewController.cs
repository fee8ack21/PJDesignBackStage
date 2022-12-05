using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得審核列表
        /// </summary>
        [HttpGet]
        [Route("GetReviews")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetReviewsResponse>>> GetReviews()
        {
            return await _service.GetReviews(); ;
        }
    }
}
