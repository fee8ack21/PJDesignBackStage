using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioService _service;

        public PortfolioController(IPortfolioService service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得作品集列表
        /// </summary>
        [HttpGet]
        [Route("GetPortfolios")]
        public async Task<ResponseBase<List<GetPortfoliosResponse>>> GetQuestions()
        {
            return await _service.GetPortfolios(); ;
        }

        /// <summary>
        /// 取得作品集ById
        /// </summary>
        [HttpGet]
        [Route("GetPortfolioById")]
        public async Task<ResponseBase<GetPortfolioByIdResponse>> GetPortfolioById(int id, bool isBefore)
        {
            return await _service.GetPortfolioById(id, isBefore);
        }

        /// <summary>
        /// 新增或修改作品集
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdatePortfolio")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdatePortfolio(CreateOrUpdatePortfolioRequest request)
        {
            var payload = HttpContext.Items["jwtPayload"] as JwtPayload;
            return await _service.CreateOrUpdatePortfolio(request, payload);
        }
    }
}
