using App.BLL;
using App.EF;
using App.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private IAdministrator _service;

        public AdministratorController(IAdministrator service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResponseBase<List<TblEftest>>> Test()
        {
            var res = await _service.Test();
            return res;
        }
    }
}
