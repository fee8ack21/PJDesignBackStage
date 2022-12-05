using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }
        /// <summary>
        /// 取得問題列表
        /// </summary>
        [HttpGet]
        [Route("GetQuestions")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetQuestionsResponse>>> GetQuestions()
        {
            return await _service.GetQuestions(); ;
        }
    }
}
