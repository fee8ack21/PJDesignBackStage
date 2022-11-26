using App.BLL;
using App.DAL.Models;
using App.Model;
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
        public async Task<ResponseBase<List<TblAdministrator>>> GetAdministrators()
        {
            var res = await _service.GetAdministrators();
            return res;
        }
    }
}
