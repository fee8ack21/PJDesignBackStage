using App.DAL.DbContexts;
using App.DAL.Models;
using App.DAL.Repositories;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IGenericRepository<TblAdministrator> _repository;

        public AdministratorService(IGenericRepository<TblAdministrator> repository)
        {
            _repository = repository;
        }

        public async Task<ResponseBase<List<TblAdministrator>>> GetAdministrators()
        {
            var response = new ResponseBase<List<TblAdministrator>>();
            try
            {
                response.Entries = await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
    }
}
