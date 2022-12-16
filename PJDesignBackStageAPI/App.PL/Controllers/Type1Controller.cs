using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Type1Controller : ControllerBase
    {
        private IType1Service _service;

        public Type1Controller(IType1Service service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得Type1 內容ByUnitId
        /// </summary>
        [HttpGet]
        [Route("GetType1ContentByUnitId")]
        [JwtFilter]
        public async Task<ResponseBase<GetType1ContentByUnitIdResponse>> GetType1ContentByUnitId(int id)
        {
            return await _service.GetType1ContentByUnitId(id);
        }

        /// <summary>
        /// 新增或修改Type1 內容
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateType1Content")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateType1Content(CreateOrUpdateType1ContentRequest request)
        {
            var payload = HttpContext.Items["jwtPayload"] as JwtPayload;
            return await _service.CreateOrUpdateType1Content(request, payload);
        }
    }
}
