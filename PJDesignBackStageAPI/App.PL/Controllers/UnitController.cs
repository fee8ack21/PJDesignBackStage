using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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
        /// 取得所有單元ByCondition
        /// </summary>
        [HttpPost]
        [Route("GetUnits")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetUnitsResponse>>> GetUnits(GetUnitsRequest request)
        {
            var payload = (JwtPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.GetUnits(request, payload);
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
        /// 新增或修改單元
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateUnit")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateUnit(CreateOrUpdateUnitRequest request)
        {
            return await _service.CreateOrUpdateUnit(request);
        }

        /// <summary>
        /// 修改前台單元排序
        /// </summary>
        [HttpPost]
        [Route("UpdateUnitsSort")]
        [JwtFilter]
        public async Task<ResponseBase<string>> UpdateUnitsSort(IEnumerable<UpdateUnitsSortRequest> request)
        {
            return await _service.UpdateUnitsSort(request);
        }
    }
}
