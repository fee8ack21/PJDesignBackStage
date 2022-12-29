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
using System.Text.Json;
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

                var afterContents = await _repositoryWrapper.Type2ContentAfter.GetByCondition(x => x.CUnitId == id && !beforeAfterIDs.Contains(x.CId)).GroupJoin(
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

        public async Task<ResponseBase<GetType2ContentByIdResponse>> GetType2ContentById(int id, bool isBefore, int unitId)
        {
            var response = new ResponseBase<GetType2ContentByIdResponse>() { Entries = new GetType2ContentByIdResponse() };
            try
            {
                if (isBefore)
                {
                    var content = await _repositoryWrapper.Type2ContentBefore
                       .GetByCondition(x => x.CId == id && x.CUnitId == unitId)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == unitId)
                           .Join(
                               _repositoryWrapper.CategoryMappingBefore.GetAll(),
                               a => a.CId,
                               b => b.CCategoryId,
                               (a, b) => new
                               {
                                   Id = a.CId,
                                   Name = a.CName,
                                   ContentId = b.CContentId,
                               }),
                           x => x.CId,
                           y => y.ContentId,
                           (x, y) => new
                           {
                               x,
                               y
                           }).Join(_repositoryWrapper.Administrator.GetAll(), x => x.x.CEditorId, y => y.CId, (x, y) => new
                           {
                               x,
                               y
                           }).FirstOrDefaultAsync();

                    if (content == null) { throw new Exception("無此內容"); }

                    response.Entries.Id = content.x.x.CId;
                    response.Entries.AfterId = content.x.x.CAfterId;
                    response.Entries.IsBefore = true;
                    response.Entries.Content = content.x.x.CContent ?? "";
                    response.Entries.ThumbnailUrl = content.x.x.CThumbnailUrl ?? "";
                    response.Entries.ImageUrl = content.x.x.CImageUrl ?? "";
                    response.Entries.EditStatus = content.x.x.CEditStatus;
                    response.Entries.EditorId = content.x.x.CEditorId;
                    response.Entries.EditorName = content.y.CName;
                    response.Entries.EditDt = content.x.x.CEditDt;
                    response.Entries.CreateDt = content.x.x.CCreateDt;
                    response.Entries.Categories = content.x.y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList();
                    response.Entries.IsEnabled = content.x.x.CIsEnabled ?? false;
                    response.Entries.Title = content.x.x.CTitle;
                    response.Entries.Notes = content.x.x.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(content.x.x.CNotes) : null;
                }
                else
                {
                    var content = await _repositoryWrapper.Type2ContentAfter
                       .GetByCondition(x => x.CId == id && x.CUnitId == unitId)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == unitId)
                           .Join(
                               _repositoryWrapper.CategoryMappingAfter.GetAll(),
                               a => a.CId, b => b.CCategoryId, (a, b) => new
                               {
                                   Id = a.CId,
                                   Name = a.CName,
                                   ContentId = b.CContentId,
                               }),
                           x => x.CId, y => y.ContentId, (x, y) => new
                           {
                               x,
                               y
                           }).Join(_repositoryWrapper.Administrator.GetAll(), x => x.x.CEditorId, y => y.CId, (x, y) => new
                           {
                               x,
                               y
                           }).FirstOrDefaultAsync();

                    if (content == null) { throw new Exception("無此內容"); }

                    response.Entries.Id = content.x.x.CId;
                    response.Entries.AfterId = null;
                    response.Entries.UnitId = content.x.x.CUnitId;
                    response.Entries.IsBefore = true;
                    response.Entries.ThumbnailUrl = content.x.x.CThumbnailUrl ?? "";
                    response.Entries.ImageUrl = content.x.x.CImageUrl ?? "";
                    response.Entries.Content = content.x.x.CContent ?? "";
                    response.Entries.EditStatus = null;
                    response.Entries.EditorId = content.x.x.CEditorId;
                    response.Entries.EditorName = content.y.CName;
                    response.Entries.EditDt = content.x.x.CEditDt;
                    response.Entries.CreateDt = content.x.x.CCreateDt;
                    response.Entries.Categories = content.x.y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList();
                    response.Entries.IsEnabled = content.x.x.CIsEnabled ?? false;
                    response.Entries.Title = content.x.x.CTitle;
                    response.Entries.Notes = null;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdateType2Content(CreateOrUpdateType2ContentRequest request, JwtPayload payload)
        {
            var response = new ResponseBase<string>();
            try
            {
                using (var transaction = _repositoryWrapper.CreateTransaction())
                {

                    TblType2ContentBefore? tblType2ContentBefore;
                    switch (request.EditStatus)
                    {
                        case (int)EditStatus.審核中:
                            if (request.Id == null)
                            {
                                // 新增before 資料 | after 新增before 資料
                                tblType2ContentBefore = new TblType2ContentBefore()
                                {
                                    CTitle = request.Title,
                                    CUnitId = request.UnitId,
                                    CImageUrl = request.ImageUrl,
                                    CThumbnailUrl = request.ThumbnailUrl,
                                    CContent = HtmlHelper.Sanitize(request.Content),
                                    CIsEnabled = request.IsEnabled,
                                    CEditStatus = request.EditStatus,
                                    CCreateDt = DateHelper.GetNowDate(),
                                    CEditDt = DateHelper.GetNowDate(),
                                    CEditorId = payload.Id,
                                    CAfterId = request.AfterId
                                };

                                _repositoryWrapper.Type2ContentBefore.Create(tblType2ContentBefore);
                                await _repositoryWrapper.SaveAsync();

                                if (request.CategoryIDs != null)
                                {
                                    var tblCategoryMappingBefores = new List<TblCategoryMappingBefore>();
                                    foreach (var categoryId in request.CategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = tblType2ContentBefore.CId };
                                        tblCategoryMappingBefores.Add(temp);
                                    }
                                    _repositoryWrapper.CategoryMappingBefore.CreateRange(tblCategoryMappingBefores);
                                }

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            else
                            {
                                // 駁回後編輯既有before 資料
                                tblType2ContentBefore = await _repositoryWrapper.Type2ContentBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                                if (tblType2ContentBefore == null) { throw new Exception("無此項目"); }

                                tblType2ContentBefore.CTitle = request.Title;
                                tblType2ContentBefore.CContent = HtmlHelper.Sanitize(request.Content);
                                tblType2ContentBefore.CIsEnabled = request.IsEnabled;
                                tblType2ContentBefore.CEditStatus = request.EditStatus;
                                tblType2ContentBefore.CEditDt = DateHelper.GetNowDate();
                                tblType2ContentBefore.CImageUrl = request.ImageUrl;
                                tblType2ContentBefore.CThumbnailUrl = request.ThumbnailUrl;

                                _repositoryWrapper.Type2ContentBefore.Update(tblType2ContentBefore);

                                var tblCategoryMappingBefores = await _repositoryWrapper.Category
                                    .GetByCondition(x => x.CUnitId == request.UnitId)
                                    .Join(
                                        _repositoryWrapper.CategoryMappingBefore.GetByCondition(y => y.CContentId == request.Id),
                                        x => x.CId,
                                        y => y.CCategoryId,
                                        (x, y) => new TblCategoryMappingBefore
                                        {
                                            CId = y.CId,
                                            CCategoryId = y.CCategoryId,
                                            CContentId = y.CContentId
                                        }).ToListAsync();

                                if (request.CategoryIDs == null || request.CategoryIDs.Count() == 0)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(tblCategoryMappingBefores);
                                }
                                else
                                {
                                    var createCategoryIDs = request.CategoryIDs.Except(tblCategoryMappingBefores.Select(x => x.CCategoryId).ToList());
                                    var createCategories = new List<TblCategoryMappingBefore>();
                                    var removeCategories = new List<TblCategoryMappingBefore>();
                                    foreach (var mappingBefore in tblCategoryMappingBefores)
                                    {
                                        if (!request.CategoryIDs.Contains(mappingBefore.CCategoryId))
                                        {
                                            removeCategories.Add(mappingBefore);

                                        }
                                    }
                                    foreach (var categoryId in createCategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = (int)request.Id };
                                        createCategories.Add(temp);
                                    }

                                    _repositoryWrapper.CategoryMappingBefore.CreateRange(createCategories);
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(removeCategories);
                                }

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            break;
                        case (int)EditStatus.駁回:
                            tblType2ContentBefore = await _repositoryWrapper.Type2ContentBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblType2ContentBefore == null) { throw new Exception("無此項目"); }

                            tblType2ContentBefore.CEditStatus = request.EditStatus;
                            tblType2ContentBefore.CEditDt = DateHelper.GetNowDate();
                            tblType2ContentBefore.CReviewerId = payload.Id;

                            var existedNotes = tblType2ContentBefore.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(tblType2ContentBefore.CNotes) : new List<ReviewNote>();

                            request.Note!.Date = DateHelper.GetNowDate();
                            existedNotes!.Add(request.Note!);
                            tblType2ContentBefore.CNotes = JsonSerializer.Serialize(existedNotes);

                            _repositoryWrapper.Type2ContentBefore.Update(tblType2ContentBefore);

                            await _repositoryWrapper.SaveAsync();
                            transaction.Commit();
                            break;
                        case (int)EditStatus.批准:
                            tblType2ContentBefore = await _repositoryWrapper.Type2ContentBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblType2ContentBefore == null) { throw new Exception("無此項目"); }

                            if (tblType2ContentBefore.CAfterId == null)
                            {
                                // 新增after 資料
                                var tblType2ContentAfter = new TblType2ContentAfter()
                                {
                                    CTitle = request.Title,
                                    CContent = HtmlHelper.Sanitize(request.Content),
                                    CEditorId = tblType2ContentBefore.CEditorId,
                                    CEditDt = DateHelper.GetNowDate(),
                                    CCreatorId = tblType2ContentBefore.CEditorId,
                                    CCreateDt = DateHelper.GetNowDate(),
                                    CIsEnabled = tblType2ContentBefore.CIsEnabled,
                                    CImageUrl = tblType2ContentBefore.CImageUrl,
                                    CThumbnailUrl = tblType2ContentBefore.CThumbnailUrl,
                                    CUnitId = tblType2ContentBefore.CUnitId,
                                };

                                _repositoryWrapper.Type2ContentAfter.Create(tblType2ContentAfter);
                                _repositoryWrapper.Type2ContentBefore.Delete(tblType2ContentBefore);
                                await _repositoryWrapper.SaveAsync();

                                if (request.CategoryIDs != null && request.CategoryIDs.Count() > 0)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(_repositoryWrapper.CategoryMappingBefore.GetByCondition(x => request.CategoryIDs.Contains(x.CCategoryId) && x.CContentId == tblType2ContentBefore.CId));

                                    var tblCategoryMappingAfters = new List<TblCategoryMappingAfter>();
                                    foreach (var id in request.CategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingAfter() { CCategoryId = id, CContentId = tblType2ContentAfter.CId };
                                        tblCategoryMappingAfters.Add(temp);
                                    }

                                    _repositoryWrapper.CategoryMappingAfter.CreateRange(tblCategoryMappingAfters);
                                }

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            else
                            {
                                // 修改after 資料
                                var tblType2ContentAfter = await _repositoryWrapper.Type2ContentAfter.GetByCondition(x => x.CId == tblType2ContentBefore.CAfterId).FirstOrDefaultAsync();

                                if (tblType2ContentAfter == null) { throw new Exception("請求錯物"); }

                                tblType2ContentAfter.CTitle = request.Title;
                                tblType2ContentAfter.CContent = HtmlHelper.Sanitize(request.Content);
                                tblType2ContentAfter.CImageUrl = request.ImageUrl;
                                tblType2ContentAfter.CThumbnailUrl = request.ThumbnailUrl;
                                tblType2ContentAfter.CEditorId = tblType2ContentBefore.CEditorId;
                                tblType2ContentAfter.CEditDt = DateHelper.GetNowDate();
                                tblType2ContentAfter.CIsEnabled = request.IsEnabled;

                                _repositoryWrapper.Type2ContentAfter.Update(tblType2ContentAfter);
                                _repositoryWrapper.Type2ContentBefore.Delete(tblType2ContentBefore);
                                await _repositoryWrapper.SaveAsync();

                                // 刪除所有categoryMappingBefore 資料
                                if (request.CategoryIDs != null)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(_repositoryWrapper.CategoryMappingBefore.GetByCondition(x => request.CategoryIDs.Contains(x.CCategoryId) && x.CContentId == tblType2ContentBefore.CId));
                                }

                                var tblCategoryMappingAfters = _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == request.UnitId)
                                    .Join(
                                        _repositoryWrapper.CategoryMappingAfter.GetByCondition(y => y.CContentId == tblType2ContentAfter.CId),
                                        x => x.CId, y => y.CCategoryId, (x, y) => new TblCategoryMappingAfter
                                        {
                                            CId = y.CId,
                                            CCategoryId = y.CCategoryId,
                                            CContentId = y.CContentId
                                        });

                                var createCategoryIDs = request.CategoryIDs == null ? new List<int>() : request.CategoryIDs.Except(tblCategoryMappingAfters.Select(x => x.CCategoryId).ToList());
                                var createCategories = new List<TblCategoryMappingAfter>();
                                var removeCategories = new List<TblCategoryMappingAfter>();
                                foreach (var mappingAfter in tblCategoryMappingAfters)
                                {
                                    if (request.CategoryIDs == null || !request.CategoryIDs.Contains(mappingAfter.CCategoryId))
                                    {
                                        removeCategories.Add(mappingAfter);

                                    }
                                }
                                foreach (var categoryId in createCategoryIDs)
                                {
                                    var temp = new TblCategoryMappingAfter() { CCategoryId = categoryId, CContentId = tblType2ContentBefore.CAfterId };
                                    createCategories.Add(temp);
                                }

                                _repositoryWrapper.CategoryMappingAfter.CreateRange(createCategories);
                                _repositoryWrapper.CategoryMappingAfter.DeleteRange(removeCategories);

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            break;
                    }
                }
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
