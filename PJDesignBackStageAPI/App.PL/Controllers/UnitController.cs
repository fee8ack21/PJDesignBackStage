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
        [JWTFilter]
        public async Task<ResponseBase<List<GetBackStageUnitsResponse>>> GetBackStageUnits()
        {
            return await _service.GetBackStageUnits();
        }

        [HttpGet]
        [Route("GetBackStageUnitsByGroupId")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>> GetBackStageUnitsByGroupId()
        {
            var payload = (JWTPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.GetBackStageUnitsByGroupId(payload);
        }
    }
}
