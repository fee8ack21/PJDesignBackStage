using App.DAL.Repositories;
using App.Enum;
using App.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class ContactService : IContactService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ContactService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetContactsResponse>>> GetContacts()
        {
            var response = new ResponseBase<List<GetContactsResponse>>() { Entries = new List<GetContactsResponse>() };

            try
            {
                response.Entries = await _repositoryWrapper.Contact.GetAll().OrderBy(x => x.CId).Select(x => new GetContactsResponse
                {
                    Id = x.CId,
                    Name = x.CName,
                    Content = x.CContent,
                    Email = x.CEmail,
                    Phone = x.CPhone,
                    CreateDt = x.CCreateDt
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }
    }
}
