using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// 管理員登入
        /// </summary>
        [HttpPost]
        [Route("Login")]
        public async Task<ResponseBase<AuthLoginResponse>> Login(AuthLoginRequest request)
        {
            return await _service.Login(request); ;
        }
    }
}
