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
        public async Task<ResponseBase<List<GetQuestionsResponse>>> GetQuestions()
        {
            return await _service.GetQuestions(); ;
        }

        /// <summary>
        /// 取得問題ById
        /// </summary>
        [HttpGet]
        [Route("GetQuestionById")]
        public async Task<ResponseBase<GetQuestionByIdResponse>> GetQuestionById(int id, bool isBefore)
        {
            return await _service.GetQuestionById(id, isBefore);
        }

        /// <summary>
        /// 新增或修改問題
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateQuestion")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateQuestion(CreateOrUpdateQuestionRequest request)
        {
            var payload = HttpContext.Items["jwtPayload"] as JwtPayload;
            return await _service.CreateOrUpdateQuestion(request, payload);
        }
    }
}
