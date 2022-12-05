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

        [HttpGet]
        [Route("GetBackStageUnits")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetBackStageUnitsResponse>>> GetBackStageUnits()
        {
            return await _service.GetBackStageUnits();
        }

        [HttpGet]
        [Route("GetBackStageUnitsByGroupId")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>> GetBackStageUnitsByGroupId()
        {
            var payload = (JwtPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.GetBackStageUnitsByGroupId(payload);
        }

        [HttpGet]
        [Route("GetType2Units")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetType2UnitsResponse>>> GetType2Units()
        {
            return await _service.GetType2Units();
        }

        [HttpGet]
        [Route("GetSettingByUnitId")]
        [JwtFilter]
        public async Task<ResponseBase<GetSettingByUnitIdResponse>> GetSettingByUnitId(int id)
        {
            return await _service.GetSettingByUnitId(id);
        }

        [HttpPost]
        [Route("CreateOrUpdateSetting")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateSetting(CreateOrUpdateSettingRequest request)
        {
            var payload = (JwtPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.CreateOrUpdateSetting(request, payload);
        }
    }
}
