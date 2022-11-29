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
        [JWTFilter]
        public async Task<ResponseBase<List<TblAdministrator>>> GetAdministrators()
        {
            var payload = HttpContext.Items["jwtPayload"];
            return await _service.GetAdministratorsAsync(); ;
        }
    }
}
