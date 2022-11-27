﻿using App.BLL;
using App.Model;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Route("Login")]
        public async Task<ResponseBase<string>> Login(AuthLoginRequest request)
        {
            return await _service.Login(request); ;
        }
    }
}
