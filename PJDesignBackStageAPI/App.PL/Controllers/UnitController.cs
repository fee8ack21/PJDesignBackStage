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
        [Route("GetUnits")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetUnitsResponse>>> GetUnits()
        {
            var payload = (JWTPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.GetUnits(payload);
        }
    }
}
