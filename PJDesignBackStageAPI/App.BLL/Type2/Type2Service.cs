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
    public class Type2Service : IType2Service
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public Type2Service(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetType2ContentsByUnitIdResponse>>> GetType2ContentsByUnitId(int id)
        {
            var response = new ResponseBase<List<GetType2ContentsByUnitIdResponse>>() { Entries = new List<GetType2ContentsByUnitIdResponse>() };
            try
            {
                var beforeContents = await _repositoryWrapper.Type2ContentBefore.GetByCondition(x => x.CUnitId == id).GroupJoin(
                   _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == id).Join(
                       _repositoryWrapper.CategoryMappingBefore.GetAll(), a => a.CId, b => b.CCategoryId, (a, b) => new { CategoryId = a.CId, CategoryName = a.CName, ContentId = b.CContentId }),
                   x => x.CId,
                   y => y.ContentId,
                   (x, y) => new GetType2ContentsByUnitIdResponse
                   {
                       Id = x.CId,
                       Title = x.CTitle,
                       AfterId = x.CAfterId,
                       IsBefore = true,
                       IsEnabled = x.CIsEnabled ?? false,
                       EditDt = x.CEditDt,
                       EditStatus = x.CEditStatus,
                       Categories = y.Select(a => new Category { Id = a.CategoryId, Name = a.CategoryName })
                   }).ToListAsync();

                var beforeAfterIDs = beforeContents.Where(x => x.AfterId != null).Select(x => x.AfterId) ?? Enumerable.Empty<int?>();

                var afterContents = await _repositoryWrapper.Type2ContentBefore.GetByCondition(x => x.CUnitId == id && !beforeAfterIDs.Contains(x.CId)).GroupJoin(
                    _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == id).Join(
                        _repositoryWrapper.CategoryMappingAfter.GetAll(), a => a.CId, b => b.CCategoryId, (a, b) => new { CategoryId = a.CId, CategoryName = a.CName, ContentId = b.CContentId }),
                    x => x.CId,
                    y => y.ContentId,
                    (x, y) => new GetType2ContentsByUnitIdResponse
                    {
                        Id = x.CId,
                        Title = x.CTitle,
                        AfterId = null,
                        IsBefore = false,
                        IsEnabled = x.CIsEnabled ?? false,
                        EditDt = x.CEditDt,
                        EditStatus = null,
                        Categories = y.Select(a => new Category { Id = a.CategoryId, Name = a.CategoryName })
                    }).ToListAsync();

                response.Entries.AddRange(beforeContents);
                response.Entries.AddRange(afterContents);
                response.Entries = response.Entries.OrderByDescending(x => x.EditDt).ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public Task<ResponseBase<string>> CreateOrUpdateType2Content(CreateOrUpdateType2ContentRequest request, JwtPayload payload)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBase<GetType2ContentByIdResponse>> GetType2ContentById(int id, bool isBefore, int unitId)
        {
            throw new NotImplementedException();
        }
    }
}
