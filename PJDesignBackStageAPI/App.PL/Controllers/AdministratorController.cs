using App.BLL;
using App.DAL.Models;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private IAdministratorService _service;

        public AdministratorController(IAdministratorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAdministrators")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetAdministratorsResponse>>> GetAdministrators()
        {
            var payload = HttpContext.Items["jwtPayload"];
            return await _service.GetAdministrators(); ;
        }

        [HttpGet]
        [Route("GetGroups")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetGroupsResponse>>> GetGroups()
        {
            return await _service.GetGroups(); ;
        }

        [HttpGet]
        [Route("GetRights")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetRightsResponse>>> GetRights()
        {
            return await _service.GetRights();
        }

        [HttpPost]
        [Route("CreateOrUpdateAdministrator")]
        [JWTFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateAdministrator(CreateOrUpdateAdministratorRequest request)
        {
            var payload = HttpContext.Items["jwtPayload"] as JWTPayload;
            return await _service.CreateOrUpdateAdministrator(request, payload);
        }

        [HttpGet]
        [Route("GetAdministratorById")]
        [JWTFilter]
        public async Task<ResponseBase<GetAdministratorByIdResponse>> GetAdministratorById(int id)
        {
            return await _service.GetAdministratorById(id);
        }

        [HttpPost]
        [Route("CreateOrUpdateGroup")]
        [JWTFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request)
        {
            return await _service.CreateOrUpdateGroup(request);
        }

        [HttpPost]
        [Route("GetUnitRightsByGroupId")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetUnitRightsByGroupIdResponse>>> GetUnitRightsByGroupId(GetUnitRightsByGroupIdRequest request)
        {
            return await _service.GetUnitRightsByGroupId(request);
        }
    }
}
