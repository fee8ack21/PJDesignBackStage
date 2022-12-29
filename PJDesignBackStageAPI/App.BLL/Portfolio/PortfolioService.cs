﻿using App.Common;
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

        public async Task<ResponseBase<GetPortfolioByIdResponse>> GetPortfolioById(int id, bool isBefore)
        {
            var response = new ResponseBase<GetPortfolioByIdResponse>() { Entries = new GetPortfolioByIdResponse() };
            try
            {
                if (isBefore)
                {
                    var portfolio = await _repositoryWrapper.PortfolioBefore
                       .GetByCondition(x => x.CId == id)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == (int)UnitId.作品集)
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
                           })
                       .Join(_repositoryWrapper.Administrator.GetAll(), x => x.x.CEditorId, y => y.CId, (x, y) => new
                       {
                           x,
                           y
                       }).FirstOrDefaultAsync();

                    if (portfolio == null) { throw new Exception("無此問題"); }

                    response.Entries.Id = portfolio.x.x.CId;
                    response.Entries.AfterId = portfolio.x.x.CAfterId;
                    response.Entries.IsBefore = true;
                    response.Entries.Date = portfolio.x.x.CDate;
                    response.Entries.EditStatus = portfolio.x.x.CEditStatus;
                    response.Entries.EditorId = portfolio.x.x.CEditorId;
                    response.Entries.EditorName = portfolio.y.CName;
                    response.Entries.EditDt = portfolio.x.x.CEditDt;
                    response.Entries.CreateDt = portfolio.x.x.CCreateDt;
                    response.Entries.Categories = portfolio.x.y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList();
                    response.Entries.IsEnabled = portfolio.x.x.CIsEnabled ?? false;
                    response.Entries.Title = portfolio.x.x.CTitle;
                    response.Entries.Notes = portfolio.x.x.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(portfolio.x.x.CNotes) : null;
                    response.Entries.Photos = await _repositoryWrapper.PortfolioPhotoBefore.GetByCondition(x => x.CPortfolioId == portfolio.x.x.CId).Select(x => x.CUrl).ToListAsync();
                }
                else
                {
                    var portfolio = await _repositoryWrapper.PortfolioAfter
                       .GetByCondition(x => x.CId == id)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == (int)UnitId.作品集)
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

                    if (portfolio == null) { throw new Exception("無此問題"); }

                    response.Entries.Id = portfolio.x.x.CId;
                    response.Entries.AfterId = null;
                    response.Entries.IsBefore = true;
                    response.Entries.Date = portfolio.x.x.CDate;
                    response.Entries.EditStatus = null;
                    response.Entries.EditorId = portfolio.x.x.CEditorId;
                    response.Entries.EditorName = portfolio.y.CName;
                    response.Entries.EditDt = portfolio.x.x.CEditDt;
                    response.Entries.CreateDt = portfolio.x.x.CCreateDt;
                    response.Entries.Categories = portfolio.x.y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList();
                    response.Entries.IsEnabled = portfolio.x.x.CIsEnabled ?? false;
                    response.Entries.Title = portfolio.x.x.CTitle;
                    response.Entries.Notes = null;
                    response.Entries.Photos = await _repositoryWrapper.PortfolioPhotoAfter.GetByCondition(x => x.CPortfolioId == portfolio.x.x.CId).Select(x => x.CUrl).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdatePortfolio(CreateOrUpdatePortfolioRequest request, JwtPayload payload)
        {
            var response = new ResponseBase<string>();
            try
            {
                using (var transaction = _repositoryWrapper.CreateTransaction())
                {

                    TblPortfolioBefore? tblPortfolioBefore;
                    switch (request.EditStatus)
                    {
                        case (int)EditStatus.審核中:
                            if (request.Id == null)
                            {
                                // 新增before 資料 | after 新增before 資料
                                tblPortfolioBefore = new TblPortfolioBefore()
                                {
                                    CTitle = request.Title,
                                    CDate = request.Date,
                                    CIsEnabled = request.IsEnabled,
                                    CEditStatus = request.EditStatus,
                                    CCreateDt = DateHelper.GetNowDate(),
                                    CEditDt = DateHelper.GetNowDate(),
                                    CEditorId = payload.Id,
                                    CAfterId = request.AfterId
                                };

                                _repositoryWrapper.PortfolioBefore.Create(tblPortfolioBefore);
                                await _repositoryWrapper.SaveAsync();

                                if (request.CategoryIDs != null)
                                {
                                    var tblCategoryMappingBefores = new List<TblCategoryMappingBefore>();
                                    foreach (var categoryId in request.CategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = tblPortfolioBefore.CId };
                                        tblCategoryMappingBefores.Add(temp);
                                    }
                                    _repositoryWrapper.CategoryMappingBefore.CreateRange(tblCategoryMappingBefores);
                                }
                                if (request.Photos != null)
                                {
                                    var tblPortfolioPhotoBefores = new List<TblPortfolioPhotoBefore>();
                                    foreach (var photo in request.Photos)
                                    {
                                        var temp = new TblPortfolioPhotoBefore() { CUrl = photo, CPortfolioId = tblPortfolioBefore.CId };
                                        tblPortfolioPhotoBefores.Add(temp);
                                    }
                                    _repositoryWrapper.PortfolioPhotoBefore.CreateRange(tblPortfolioPhotoBefores);
                                }

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            else
                            {
                                // 駁回後編輯既有before 資料
                                tblPortfolioBefore = await _repositoryWrapper.PortfolioBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                                if (tblPortfolioBefore == null) { throw new Exception("無此項目"); }

                                tblPortfolioBefore.CTitle = request.Title;
                                tblPortfolioBefore.CDate = request.Date;
                                tblPortfolioBefore.CIsEnabled = request.IsEnabled;
                                tblPortfolioBefore.CEditStatus = request.EditStatus;
                                tblPortfolioBefore.CEditDt = DateHelper.GetNowDate();

                                _repositoryWrapper.PortfolioBefore.Update(tblPortfolioBefore);

                                var tblCategoryMappingBefores = await _repositoryWrapper.Category
                                    .GetByCondition(x => x.CUnitId == (int)UnitId.作品集)
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

                                var tblPortfolioPhotoBefores = await _repositoryWrapper.PortfolioPhotoBefore.GetByCondition(x => x.CPortfolioId == tblPortfolioBefore.CId).ToListAsync();
                                if (request.Photos == null || request.Photos.Count() == 0)
                                {
                                    _repositoryWrapper.PortfolioPhotoBefore.DeleteRange(tblPortfolioPhotoBefores);
                                }
                                else
                                {
                                    var createPortfolioPhotoBefores = new List<TblPortfolioPhotoBefore>();
                                    var removePortfolioPhotoBefores = new List<TblPortfolioPhotoBefore>();

                                    foreach (var photoBefore in tblPortfolioPhotoBefores)
                                    {
                                        bool isRemove = true;
                                        foreach (var photo in request.Photos)
                                        {
                                            if (photoBefore.CUrl == photo) { isRemove = false; break; }
                                        }

                                        if (isRemove) { removePortfolioPhotoBefores.Add(photoBefore); }
                                    }
                                    foreach (var photo in request.Photos)
                                    {
                                        if (!tblPortfolioPhotoBefores.Any(x => x.CUrl == photo))
                                        {
                                            createPortfolioPhotoBefores.Add(new TblPortfolioPhotoBefore { CUrl = photo, CPortfolioId = tblPortfolioBefore.CId });
                                        }
                                    }

                                    _repositoryWrapper.PortfolioPhotoBefore.CreateRange(createPortfolioPhotoBefores);
                                    _repositoryWrapper.PortfolioPhotoBefore.DeleteRange(removePortfolioPhotoBefores);
                                }

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            break;
                        case (int)EditStatus.駁回:
                            tblPortfolioBefore = await _repositoryWrapper.PortfolioBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblPortfolioBefore == null) { throw new Exception("無此項目"); }

                            tblPortfolioBefore.CEditStatus = request.EditStatus;
                            tblPortfolioBefore.CEditDt = DateHelper.GetNowDate();
                            tblPortfolioBefore.CReviewerId = payload.Id;

                            var existedNotes = tblPortfolioBefore.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(tblPortfolioBefore.CNotes) : new List<ReviewNote>();

                            request.Note!.Date = DateHelper.GetNowDate();
                            existedNotes!.Add(request.Note!);
                            tblPortfolioBefore.CNotes = JsonSerializer.Serialize(existedNotes);

                            _repositoryWrapper.PortfolioBefore.Update(tblPortfolioBefore);

                            await _repositoryWrapper.SaveAsync();
                            transaction.Commit();
                            break;
                        case (int)EditStatus.批准:
                            tblPortfolioBefore = await _repositoryWrapper.PortfolioBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblPortfolioBefore == null) { throw new Exception("無此項目"); }

                            if (tblPortfolioBefore.CAfterId == null)
                            {
                                // 新增after 資料
                                var tblPortfolioAfter = new TblPortfolioAfter()
                                {
                                    CTitle = request.Title,
                                    CDate = request.Date,
                                    CEditorId = tblPortfolioBefore.CEditorId,
                                    CEditDt = DateHelper.GetNowDate(),
                                    CCreatorId = tblPortfolioBefore.CEditorId,
                                    CCreateDt = DateHelper.GetNowDate(),
                                    CIsEnabled = tblPortfolioBefore.CIsEnabled,
                                };

                                _repositoryWrapper.PortfolioAfter.Create(tblPortfolioAfter);
                                _repositoryWrapper.PortfolioBefore.Delete(tblPortfolioBefore);
                                await _repositoryWrapper.SaveAsync();

                                if (request.CategoryIDs != null && request.CategoryIDs.Count() > 0)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(_repositoryWrapper.CategoryMappingBefore.GetByCondition(x => request.CategoryIDs.Contains(x.CCategoryId) && x.CContentId == tblPortfolioBefore.CId));

                                    var tblCategoryMappingAfters = new List<TblCategoryMappingAfter>();
                                    foreach (var id in request.CategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingAfter() { CCategoryId = id, CContentId = tblPortfolioAfter.CId };
                                        tblCategoryMappingAfters.Add(temp);
                                    }

                                    _repositoryWrapper.CategoryMappingAfter.CreateRange(tblCategoryMappingAfters);
                                }

                                if (request.Photos != null && request.Photos.Count() > 0)
                                {
                                    _repositoryWrapper.PortfolioPhotoBefore.DeleteRange(_repositoryWrapper.PortfolioPhotoBefore.GetByCondition(x => x.CPortfolioId == tblPortfolioBefore.CId));
                                    var tblPortfolioPhotoAfters = new List<TblPortfolioPhotoAfter>();
                                    foreach (var photo in request.Photos)
                                    {
                                        var temp = new TblPortfolioPhotoAfter() { CUrl = photo, CPortfolioId = tblPortfolioAfter.CId };
                                        tblPortfolioPhotoAfters.Add(temp);
                                    }

                                    _repositoryWrapper.PortfolioPhotoAfter.CreateRange(tblPortfolioPhotoAfters);
                                }

                                await _repositoryWrapper.SaveAsync();
                                transaction.Commit();
                            }
                            else
                            {
                                // 修改after 資料
                                var tblPortfolioAfter = await _repositoryWrapper.PortfolioAfter.GetByCondition(x => x.CId == tblPortfolioBefore.CAfterId).FirstOrDefaultAsync();

                                if (tblPortfolioAfter == null) { throw new Exception("請求錯物"); }

                                tblPortfolioAfter.CTitle = request.Title;
                                tblPortfolioAfter.CDate = request.Date;
                                tblPortfolioAfter.CEditorId = tblPortfolioBefore.CEditorId;
                                tblPortfolioAfter.CEditDt = DateHelper.GetNowDate();
                                tblPortfolioAfter.CIsEnabled = request.IsEnabled;

                                _repositoryWrapper.PortfolioAfter.Update(tblPortfolioAfter);
                                _repositoryWrapper.PortfolioBefore.Delete(tblPortfolioBefore);
                                await _repositoryWrapper.SaveAsync();

                                // 刪除所有categoryMappingBefore 資料
                                if (request.CategoryIDs != null)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(_repositoryWrapper.CategoryMappingBefore.GetByCondition(x => request.CategoryIDs.Contains(x.CCategoryId) && x.CContentId == tblPortfolioBefore.CId));
                                }

                                var tblCategoryMappingAfters = _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitId.作品集)
                                    .Join(
                                        _repositoryWrapper.CategoryMappingAfter.GetByCondition(y => y.CContentId == tblPortfolioAfter.CId),
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
                                    var temp = new TblCategoryMappingAfter() { CCategoryId = categoryId, CContentId = tblPortfolioBefore.CAfterId };
                                    createCategories.Add(temp);
                                }

                                _repositoryWrapper.CategoryMappingAfter.CreateRange(createCategories);
                                _repositoryWrapper.CategoryMappingAfter.DeleteRange(removeCategories);


                                _repositoryWrapper.PortfolioPhotoBefore.DeleteRange(_repositoryWrapper.PortfolioPhotoBefore.GetByCondition(x => x.CPortfolioId == tblPortfolioBefore.CId));

                                var tblPortfolioPhotoAfters = _repositoryWrapper.PortfolioPhotoAfter.GetByCondition(x => x.CPortfolioId == tblPortfolioAfter.CId);

                                var removeAfterPhotos = new List<TblPortfolioPhotoAfter>();
                                var createAfterPhotos = new List<TblPortfolioPhotoAfter>();

                                foreach (var photo in tblPortfolioPhotoAfters)
                                {
                                    if (request.Photos == null || !request.Photos.Any(x => x == photo.CUrl))
                                    {
                                        removeAfterPhotos.Add(photo);
                                    }
                                }

                                if (request.Photos != null)
                                {
                                    foreach (var photo in request.Photos)
                                    {
                                        if (!tblPortfolioPhotoAfters.Any(x => x.CUrl == photo))
                                        {
                                            createAfterPhotos.Add(new TblPortfolioPhotoAfter() { CUrl = photo, CPortfolioId = tblPortfolioAfter.CId });
                                        }
                                    }
                                }

                                _repositoryWrapper.PortfolioPhotoAfter.CreateRange(createAfterPhotos);
                                _repositoryWrapper.PortfolioPhotoAfter.DeleteRange(removeAfterPhotos);

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
