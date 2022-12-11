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
    public class PortfolioService : IPortfolioService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PortfolioService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetPortfoliosResponse>>> GetPortfolios()
        {
            var response = new ResponseBase<List<GetPortfoliosResponse>>() { Entries = new List<GetPortfoliosResponse>() };
            try
            {
                var beforePortfolios = await _repositoryWrapper.PortfolioBefore.GetAll().GroupJoin(
                   _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitId.作品集).Join(
                       _repositoryWrapper.CategoryMappingBefore.GetAll(), a => a.CId, b => b.CCategoryId, (a, b) => new { CategoryId = a.CId, CategoryName = a.CName, ContentId = b.CContentId }),
                   x => x.CId,
                   y => y.ContentId,
                   (x, y) => new GetPortfoliosResponse
                   {
                       Id = x.CId,
                       Title = x.CTitle,
                       AfterId = x.CAfterId,
                       IsBefore = true,
                       IsEnabled = x.CIsEnabled ?? false,
                       EditDt = x.CEditDt,
                       Date = x.CDate,
                       EditStatus = x.CEditStatus,
                       Categories = y.Select(a => new Category { Id = a.CategoryId, Name = a.CategoryName })
                   }).ToListAsync();

                var beforeAfterIDs = beforePortfolios.Where(x => x.AfterId != null).Select(x => x.AfterId) ?? Enumerable.Empty<int?>();

                var afterPortfolios = await _repositoryWrapper.PortfolioAfter.GetByCondition(x => !beforeAfterIDs.Contains(x.CId)).GroupJoin(
                    _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitId.作品集).Join(
                        _repositoryWrapper.CategoryMappingAfter.GetAll(), a => a.CId, b => b.CCategoryId, (a, b) => new { CategoryId = a.CId, CategoryName = a.CName, ContentId = b.CContentId }),
                    x => x.CId,
                    y => y.ContentId,
                    (x, y) => new GetPortfoliosResponse
                    {
                        Id = x.CId,
                        Title = x.CTitle,
                        AfterId = null,
                        Date = x.CDate,
                        IsBefore = false,
                        IsEnabled = x.CIsEnabled ?? false,
                        EditDt = x.CEditDt,
                        EditStatus = null,
                        Categories = y.Select(a => new Category { Id = a.CategoryId, Name = a.CategoryName })
                    }).ToListAsync();

                response.Entries.AddRange(beforePortfolios);
                response.Entries.AddRange(afterPortfolios);
                response.Entries = response.Entries.OrderByDescending(x => x.EditDt).ToList();
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
