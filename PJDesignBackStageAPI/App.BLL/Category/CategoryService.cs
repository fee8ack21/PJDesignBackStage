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

            var tblCategories = await _repositoryWrapper.Category.GetByCondition(x => requests.Any(y => y.Id == x.CId)).ToListAsync();

            foreach (var category in tblCategories)
            {
                var temp = requests.Where(x => x.Id == category.CId).FirstOrDefault();

                if (temp == null) { continue; }

                category.CName = temp.Name;
                category.CIsEnabled = temp.IsEnabled;
                category.CEditDt = DateHelper.GetNowDate();
            }

            _repositoryWrapper.Category.UpdateRange(tblCategories);
            await _repositoryWrapper.SaveAsync();

            return response;
        }

        public static List<TblCategoryMappingBefore> GetCreatedCategoryMappingBefores(IEnumerable<int> categoryIDs, int contentId)
        {
            var mappings = new List<TblCategoryMappingBefore>();

            if (categoryIDs == null || categoryIDs.Count() == 0) { return mappings; }

            foreach (var categoryId in categoryIDs)
            {
                var temp = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = contentId };
                mappings.Add(temp);
            }

            return mappings;
        }

        public static List<TblCategoryMappingAfter> GetCreatedCategoryMappingAfters(IEnumerable<int> categoryIDs, int? contentId)
        {
            var mappings = new List<TblCategoryMappingAfter>();

            if (categoryIDs == null || categoryIDs.Count() == 0) { return mappings; }

            foreach (var categoryId in categoryIDs)
            {
                var temp = new TblCategoryMappingAfter() { CCategoryId = categoryId, CContentId = contentId };
                mappings.Add(temp);
            }

            return mappings;
        }

        public static List<TblCategoryMappingBefore> GetRemovedCategoryMappingBefores(List<TblCategoryMappingBefore> sourceBefores, IEnumerable<int>? finalIDs)
        {
            var mappings = new List<TblCategoryMappingBefore>();
            foreach (var mappingBefore in sourceBefores)
            {
                if (!finalIDs.Contains(mappingBefore.CCategoryId))
                {
                    mappings.Add(mappingBefore);
                }
            }

            return mappings;
        }

        public static List<TblCategoryMappingAfter> GetRemovedCategoryMappingAfters(IEnumerable<TblCategoryMappingAfter> sourceAfters, IEnumerable<int>? finalIDs)
        {
            var mappings = new List<TblCategoryMappingAfter>();
            foreach (var mappingAfter in sourceAfters)
            {
                if (finalIDs == null || !finalIDs.Contains(mappingAfter.CCategoryId))
                {
                    mappings.Add(mappingAfter);
                }
            }

            return mappings;
        }
    }
}
