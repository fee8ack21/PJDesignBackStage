using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private IUnitService _service;

        public UnitController(IUnitService service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得所有呈現於後台的單元
        /// </summary>
        [HttpGet]
        [Route("GetBackStageUnits")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetBackStageUnitsResponse>>> GetBackStageUnits()
        {
            return await _service.GetBackStageUnits();
        }

        /// <summary>
        /// 取得所有呈現於後台的單元ByID
        /// </summary>
        [HttpGet]
        [Route("GetBackStageUnitsByGroupId")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>> GetBackStageUnitsByGroupId()
        {
            var payload = (JwtPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.GetBackStageUnitsByGroupId(payload);
        }

        /// <summary>
        /// 取得所有Type2 模板單元
        /// </summary>
        [HttpGet]
        [Route("GetType2Units")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetType2UnitsResponse>>> GetType2Units()
        {
            return await _service.GetType2Units();
        }

        /// <summary>
        /// 取得單元設定ByID
        /// </summary>
        [HttpGet]
        [Route("GetSettingByUnitId")]
        [JwtFilter]
        public async Task<ResponseBase<GetSettingByUnitIdResponse>> GetSettingByUnitId(int id)
        {
            return await _service.GetSettingByUnitId(id);
        }

        /// <summary>
        /// 新增或修改單元設定
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateSetting")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateSetting(CreateOrUpdateSettingRequest request)
        {
            var payload = (JwtPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.CreateOrUpdateSetting(request, payload);
        }

        /// <summary>
        /// 取得所有前台單元
        /// </summary>
        [HttpGet]
        [Route("GetFrontStageUnits")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetFrontStageUnitsResponse>>> GetFrontStageUnits()
        {
            return await _service.GetFrontStageUnits();
        }
    }
}
