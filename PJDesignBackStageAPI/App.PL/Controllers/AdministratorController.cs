using App.BLL;
using App.DAL.Models;
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
    public class AdministratorController : ControllerBase
    {
        private IAdministratorService _service;

        public AdministratorController(IAdministratorService service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得所有管理員
        /// </summary>
        [HttpGet]
        [Route("GetAdministrators")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetAdministratorsResponse>>> GetAdministrators()
        {
            var payload = HttpContext.Items["jwtPayload"];
            return await _service.GetAdministrators(); ;
        }

        /// <summary>
        /// 取得所有管理組別
        /// </summary>
        [HttpGet]
        [Route("GetGroups")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetGroupsResponse>>> GetGroups()
        {
            return await _service.GetGroups(); ;
        }

        /// <summary>
        /// 取得所有管理權限選項
        /// </summary>
        [HttpGet]
        [Route("GetRights")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetRightsResponse>>> GetRights()
        {
            return await _service.GetRights();
        }

        /// <summary>
        /// 新增或更改管理員
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateAdministrator")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateAdministrator(CreateOrUpdateAdministratorRequest request)
        {
            var payload = (JwtPayload)HttpContext.Items["jwtPayload"]!;
            return await _service.CreateOrUpdateAdministrator(request, payload);
        }

        /// <summary>
        /// 取得管理員By ID
        /// </summary>
        [HttpGet]
        [Route("GetAdministratorById")]
        [JwtFilter]
        public async Task<ResponseBase<GetAdministratorByIdResponse>> GetAdministratorById(int id)
        {
            return await _service.GetAdministratorById(id);
        }

        /// <summary>
        /// 新增或更改管理組別
        /// </summary>
        [HttpPost]
        [Route("CreateOrUpdateGroup")]
        [JwtFilter]
        public async Task<ResponseBase<string>> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request)
        {
            return await _service.CreateOrUpdateGroup(request);
        }

        /// <summary>
        /// 取得管理組別下各單元權限
        /// </summary>
        [HttpPost]
        [Route("GetUnitRightsByGroupId")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetUnitRightsByGroupIdResponse>>> GetUnitRightsByGroupId(GetUnitRightsByGroupIdRequest request)
        {
            return await _service.GetUnitRightsByGroupId(request);
        }
    }
}
