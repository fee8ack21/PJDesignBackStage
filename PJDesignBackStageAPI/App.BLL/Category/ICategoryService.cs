using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface ICategoryService
    {
        Task<ResponseBase<List<GetCategoriesByUnitIdResponse>>> GetCategoriesByUnitId(int id);
        Task<ResponseBase<string>> CreateCategory(CreateCategoryRequest request);
        Task<ResponseBase<string>> UpdateCategories(IEnumerable<UpdateCategoriesRequest> requests);
    }
}
