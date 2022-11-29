using App.DAL;
using App.DAL.Models;
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
    public class AdministratorService : IAdministratorService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AdministratorService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<TblAdministrator>>> GetAdministratorsAsync()
        {
            var response = new ResponseBase<List<TblAdministrator>>();
            try
            {
                response.Entries = await _repositoryWrapper.Administrator.GetAll().ToListAsync();
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
