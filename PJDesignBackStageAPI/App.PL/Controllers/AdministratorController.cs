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

        [HttpPost]
        [Route("CreateGroup")]
        [JWTFilter]
        public async Task<ResponseBase<CreateGroupResponse>> CreateGroup(CreateGroupRequest request)
        {
            return await _service.CreateGroup(request);
        }

        [HttpGet]
        [Route("GetRights")]
        [JWTFilter]
        public async Task<ResponseBase<List<GetRightsResponse>>> GetRights()
        {
            return await _service.GetRights();
        }
    }
}
