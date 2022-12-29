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
    public class QuestionService : IQuestionService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public QuestionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetQuestionsResponse>>> GetQuestions()
        {
            var response = new ResponseBase<List<GetQuestionsResponse>>() { Entries = new List<GetQuestionsResponse>() };
            try
            {
                var beforeQuestions = await _repositoryWrapper.QuestionBefore.GetAll().GroupJoin(
                    _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitId.常見問題).Join(
                        _repositoryWrapper.CategoryMappingBefore.GetAll(), a => a.CId, b => b.CCategoryId, (a, b) => new { CategoryId = a.CId, CategoryName = a.CName, ContentId = b.CContentId }),
                    x => x.CId,
                    y => y.ContentId,
                    (x, y) => new GetQuestionsResponse
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

                var beforeAfterIDs = beforeQuestions.Where(x => x.AfterId != null).Select(x => x.AfterId) ?? Enumerable.Empty<int?>();

                var afterQuestions = await _repositoryWrapper.QuestionAfter.GetByCondition(x => !beforeAfterIDs.Contains(x.CId)).GroupJoin(
                    _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitId.常見問題).Join(
                        _repositoryWrapper.CategoryMappingAfter.GetAll(), a => a.CId, b => b.CCategoryId, (a, b) => new { CategoryId = a.CId, CategoryName = a.CName, ContentId = b.CContentId }),
                    x => x.CId,
                    y => y.ContentId,
                    (x, y) => new GetQuestionsResponse
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

                response.Entries.AddRange(beforeQuestions);
                response.Entries.AddRange(afterQuestions);
                response.Entries = response.Entries.OrderByDescending(x => x.EditDt).ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<GetQuestionByIdResponse>> GetQuestionById(int id, bool isBefore)
        {
            var response = new ResponseBase<GetQuestionByIdResponse>() { Entries = new GetQuestionByIdResponse() };
            try
            {
                if (isBefore)
                {
                    var question = await _repositoryWrapper.QuestionBefore
                       .GetByCondition(x => x.CId == id)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == (int)UnitId.常見問題)
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

                    if (question == null) { throw new Exception("無此問題"); }

                    response.Entries.Id = question.x.x.CId;
                    response.Entries.AfterId = question.x.x.CAfterId;
                    response.Entries.IsBefore = true;
                    response.Entries.Content = question.x.x.CContent ?? "";
                    response.Entries.EditStatus = question.x.x.CEditStatus;
                    response.Entries.EditorId = question.x.x.CEditorId;
                    response.Entries.EditorName = question.y.CName;
                    response.Entries.EditDt = question.x.x.CEditDt;
                    response.Entries.CreateDt = question.x.x.CCreateDt;
                    response.Entries.Categories = question.x.y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList();
                    response.Entries.IsEnabled = question.x.x.CIsEnabled ?? false;
                    response.Entries.Title = question.x.x.CTitle;
                    response.Entries.Notes = question.x.x.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(question.x.x.CNotes) : null;
                }
                else
                {
                    var question = await _repositoryWrapper.QuestionAfter
                       .GetByCondition(x => x.CId == id)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == (int)UnitId.常見問題)
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

                    if (question == null) { throw new Exception("無此問題"); }

                    response.Entries.Id = question.x.x.CId;
                    response.Entries.AfterId = null;
                    response.Entries.IsBefore = true;
                    response.Entries.Content = question.x.x.CContent ?? "";
                    response.Entries.EditStatus = null;
                    response.Entries.EditorId = question.x.x.CEditorId;
                    response.Entries.EditorName = question.y.CName;
                    response.Entries.EditDt = question.x.x.CEditDt;
                    response.Entries.CreateDt = question.x.x.CCreateDt;
                    response.Entries.Categories = question.x.y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList();
                    response.Entries.IsEnabled = question.x.x.CIsEnabled ?? false;
                    response.Entries.Title = question.x.x.CTitle;
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

        public async Task<ResponseBase<string>> CreateOrUpdateQuestion(CreateOrUpdateQuestionRequest request, JwtPayload payload)
        {
            var response = new ResponseBase<string>();
            try
            {
                using (var transaction = _repositoryWrapper.CreateTransaction())
                {

                    TblQuestionBefore? tblQuestionBefore;
                    switch (request.EditStatus)
                    {
                        case (int)EditStatus.審核中:
                            if (request.Id == null)
                            {
                                // 新增before 資料 | after 新增before 資料
                                tblQuestionBefore = new TblQuestionBefore()
                                {
                                    CTitle = request.Title,
                                    CContent = HtmlHelper.Sanitize(request.Content),
                                    CIsEnabled = request.IsEnabled,
                                    CEditStatus = request.EditStatus,
                                    CCreateDt = DateHelper.GetNowDate(),
                                    CEditDt = DateHelper.GetNowDate(),
                                    CEditorId = payload.Id,
                                    CAfterId = request.AfterId
                                };

                                _repositoryWrapper.QuestionBefore.Create(tblQuestionBefore);
                                await _repositoryWrapper.SaveAsync();

                                if (request.CategoryIDs != null)
                                {
                                    var tblCategoryMappingBefores = new List<TblCategoryMappingBefore>();
                                    foreach (var categoryId in request.CategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = tblQuestionBefore.CId };
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
                                tblQuestionBefore = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                                if (tblQuestionBefore == null) { throw new Exception("無此項目"); }

                                tblQuestionBefore.CTitle = request.Title;
                                tblQuestionBefore.CContent = HtmlHelper.Sanitize(request.Content);
                                tblQuestionBefore.CIsEnabled = request.IsEnabled;
                                tblQuestionBefore.CEditStatus = request.EditStatus;
                                tblQuestionBefore.CEditDt = DateHelper.GetNowDate();

                                _repositoryWrapper.QuestionBefore.Update(tblQuestionBefore);

                                var tblCategoryMappingBefores = await _repositoryWrapper.Category
                                    .GetByCondition(x => x.CUnitId == (int)UnitId.常見問題)
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
                            tblQuestionBefore = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblQuestionBefore == null) { throw new Exception("無此項目"); }

                            tblQuestionBefore.CEditStatus = request.EditStatus;
                            tblQuestionBefore.CEditDt = DateHelper.GetNowDate();
                            tblQuestionBefore.CReviewerId = payload.Id;

                            var existedNotes = tblQuestionBefore.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(tblQuestionBefore.CNotes) : new List<ReviewNote>();

                            request.Note!.Date = DateHelper.GetNowDate();
                            existedNotes!.Add(request.Note!);
                            tblQuestionBefore.CNotes = JsonSerializer.Serialize(existedNotes);

                            _repositoryWrapper.QuestionBefore.Update(tblQuestionBefore);

                            await _repositoryWrapper.SaveAsync();
                            transaction.Commit();
                            break;
                        case (int)EditStatus.批准:
                            tblQuestionBefore = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblQuestionBefore == null) { throw new Exception("無此項目"); }

                            if (tblQuestionBefore.CAfterId == null)
                            {
                                // 新增after 資料
                                var tblQuestionAfter = new TblQuestionAfter()
                                {
                                    CTitle = request.Title,
                                    CContent = HtmlHelper.Sanitize(request.Content),
                                    CEditorId = tblQuestionBefore.CEditorId,
                                    CEditDt = DateHelper.GetNowDate(),
                                    CCreatorId = tblQuestionBefore.CEditorId,
                                    CCreateDt = DateHelper.GetNowDate(),
                                    CIsEnabled = tblQuestionBefore.CIsEnabled,
                                };

                                _repositoryWrapper.QuestionAfter.Create(tblQuestionAfter);
                                _repositoryWrapper.QuestionBefore.Delete(tblQuestionBefore);
                                await _repositoryWrapper.SaveAsync();

                                if (request.CategoryIDs != null && request.CategoryIDs.Count() > 0)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(_repositoryWrapper.CategoryMappingBefore.GetByCondition(x => request.CategoryIDs.Contains(x.CCategoryId) && x.CContentId == tblQuestionBefore.CId));

                                    var tblCategoryMappingAfters = new List<TblCategoryMappingAfter>();
                                    foreach (var id in request.CategoryIDs)
                                    {
                                        var temp = new TblCategoryMappingAfter() { CCategoryId = id, CContentId = tblQuestionAfter.CId };
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
                                var tblQuestionAfter = await _repositoryWrapper.QuestionAfter.GetByCondition(x => x.CId == tblQuestionBefore.CAfterId).FirstOrDefaultAsync();

                                if (tblQuestionAfter == null) { throw new Exception("請求錯物"); }

                                tblQuestionAfter.CTitle = request.Title;
                                tblQuestionAfter.CContent = HtmlHelper.Sanitize(request.Content);
                                tblQuestionAfter.CEditorId = tblQuestionBefore.CEditorId;
                                tblQuestionAfter.CEditDt = DateHelper.GetNowDate();
                                tblQuestionAfter.CIsEnabled = request.IsEnabled;

                                _repositoryWrapper.QuestionAfter.Update(tblQuestionAfter);
                                _repositoryWrapper.QuestionBefore.Delete(tblQuestionBefore);
                                await _repositoryWrapper.SaveAsync();

                                // 刪除所有categoryMappingBefore 資料
                                if (request.CategoryIDs != null)
                                {
                                    _repositoryWrapper.CategoryMappingBefore.DeleteRange(_repositoryWrapper.CategoryMappingBefore.GetByCondition(x => request.CategoryIDs.Contains(x.CCategoryId) && x.CContentId == tblQuestionBefore.CId));
                                }

                                var tblCategoryMappingAfters = _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitId.常見問題)
                                    .Join(
                                        _repositoryWrapper.CategoryMappingAfter.GetByCondition(y => y.CContentId == tblQuestionAfter.CId),
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
                                    var temp = new TblCategoryMappingAfter() { CCategoryId = categoryId, CContentId = tblQuestionBefore.CAfterId };
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
