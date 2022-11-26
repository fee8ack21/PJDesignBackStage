using App.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly PjdesignContext _context;
        public GenericRepository(PjdesignContext context)
        {
            _context = context;
        }

        public async Task<List<TModel>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TModel>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
