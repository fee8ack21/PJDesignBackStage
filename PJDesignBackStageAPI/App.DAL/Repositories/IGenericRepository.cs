using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<List<TModel>> GetAllAsync();
        Task<List<TModel>> GetByConditionAsync(Expression<Func<TModel, bool>> expression);
        Task<TModel?> GetFirstOrDefaultByConditionAsync(Expression<Func<TModel, bool>> expression);
    }
}
