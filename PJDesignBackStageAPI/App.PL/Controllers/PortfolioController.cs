using App.BLL;
using App.Model;
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
    }
}
