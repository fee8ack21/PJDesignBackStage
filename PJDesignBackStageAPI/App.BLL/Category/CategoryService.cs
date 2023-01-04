using App.Common;
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
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CategoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetCategoriesByUnitIdResponse>>> GetCategoriesByUnitId(int id)
        {
            var response = new ResponseBase<List<GetCategoriesByUnitIdResponse>>() { Entries = new List<GetCategoriesByUnitIdResponse>() };

            response.Entries = await _repositoryWrapper.Category
                .GetByCondition(x => x.CUnitId == id)
                .OrderByDescending(x => x.CCreateDt)
                .Select(x => new GetCategoriesByUnitIdResponse
                {
                    Id = x.CId,
                    Name = x.CName,
                    CreateDt = x.CCreateDt,
                    IsEnabled = x.CIsEnabled
                }).ToListAsync();

            return response;
        }

        public async Task<ResponseBase<string>> CreateCategory(CreateCategoryRequest request)
        {
            var response = new ResponseBase<string>();

            var tblCategory = new TblCategory() { CUnitId = request.UnitId, CName = request.Name, CIsEnabled = request.IsEnabled };
            _repositoryWrapper.Category.Create(tblCategory);
            await _repositoryWrapper.SaveAsync();

            return response;
        }

        public async Task<ResponseBase<string>> UpdateCategories(IEnumerable<UpdateCategoriesRequest> requests)
        {
            var response = new ResponseBase<string>();

            foreach (var request in requests)
            {
                var tblCategory = await _repositoryWrapper.Category.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                if (tblCategory == null) { continue; }

                tblCategory.CName = request.Name;
                tblCategory.CIsEnabled = request.IsEnabled;
                tblCategory.CEditDt = DateHelper.GetNowDate();

                _repositoryWrapper.Category.Update(tblCategory);
            }

            await _repositoryWrapper.SaveAsync();

            return response;
        }
    }
}
