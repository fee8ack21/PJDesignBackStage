using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Type2Controller : ControllerBase
    {
        private IType2Service _service;

        public Type2Controller(IType2Service service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得Type2 內容列表ByUnitId
        /// </summary>
        [HttpGet]
        [Route("GetType2ContentsByUnitId")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetType2ContentsByUnitIdResponse>>> GetType2ContentsByUnitId(int id)
        {
            return await _service.GetType2ContentsByUnitId(id); ;
        }

        /// <summary>
        /// 取得Type2 內容ById
        /// </summary>
        [HttpGet]
        [Route("GetType2ContentById")]
        [JwtFilter]
        public async Task<ResponseBase<GetType2ContentByIdResponse>> GetType2ContentById(int id, bool isBefore, int unitId)
        {
            return await _service.GetType2ContentById(id, isBefore, unitId);
        }

        /// <summary>
        /// 新增或修改Type2 內容
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateType2Content")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateType2Content(CreateOrUpdateType2ContentRequest request)
        {
            var payload = HttpContext.Items["jwtPayload"] as JwtPayload;
            return await _service.CreateOrUpdateType2Content(request, payload!);
        }
    }
}
