﻿using App.BLL;
using App.Model;
using App.PL.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetContacts")]
        [JwtFilter]
        public async Task<ResponseBase<List<GetContactsResponse>>> GetContacts()
        {
            return await _service.GetContacts();
        }
    }
}
