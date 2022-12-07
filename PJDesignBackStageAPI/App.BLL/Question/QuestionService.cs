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
            // Todo: SQL 寫法優化
            var response = new ResponseBase<List<GetQuestionsResponse>>() { Entries = new List<GetQuestionsResponse>() };
            try
            {
                var afterQuestions = await _repositoryWrapper.QuestionAfter.GetAll().ToListAsync();

                var beforeCategories = await _repositoryWrapper.Category.GetByCondition(x => x.CUnitId == (int)UnitID.常見問題).Join(_repositoryWrapper.CategoryMappingBefore.GetAll(), x => x.CId, y => y.CCategoryId, (x, y) => new
                {
                    name = x.CName,
                    id = x.CId,
                    contentId = y.CContentId
                }).ToListAsync();

                var beforeQuestion = await _repositoryWrapper.QuestionBefore
                    .GetAll()
                    .ToListAsync();

                var afterIDsInBefoe = new List<int>();

                // Todo: 取得category mapping data
                foreach (var item in beforeQuestion)
                {
                    var temp = new GetQuestionsResponse()
                    {
                        Id = item.CId,
                        IsBefore = true,
                        Title = item.CTitle,
                        CreateDt = item.CCreateDt,
                        EditDt = item.CEditDt,
                        EditorId = item.CEditorId,
                        EditStatus = item.CEditStatus,
                        IsEnabled = item.CIsEnabled ?? false,
                        Categories = new List<Category>()
                    };

                    foreach (var category in beforeCategories)
                    {
                        if (category.contentId == temp.Id)
                        {
                            temp.Categories.Add(new Category { Id = category.id, Name = category.name });
                        }
                    }

                    if (item.CAfterId != null)
                    {
                        afterIDsInBefoe.Add((int)item.CAfterId);
                    }
                    response.Entries.Add(temp);
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
                // 新增
                if (request.Id == null)
                {
                    var tblQuestionBefore = new TblQuestionBefore()
                    {
                        CTitle = request.Title,
                        CContent = HtmlHelper.Sanitize(request.Content),
                        CIsEnabled = request.IsEnabled,
                        CEditStatus = request.EditStatus,
                        CCreateDt = DateHelper.GetNowDate(),
                        CEditDt = DateHelper.GetNowDate(),
                        CEditorId = payload.Id,
                    };

                    _repositoryWrapper.QuestionBefore.Create(tblQuestionBefore);

                    if (request.CategoryIDs != null)
                    {
                        foreach (var categoryId in request.CategoryIDs)
                        {
                            var tblCategoryMappingBefore = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = tblQuestionBefore.CId };
                            _repositoryWrapper.CategoryMappingBefore.Create(tblCategoryMappingBefore);
                        }
                    }

                    await _repositoryWrapper.SaveAsync();
                }
                else
                {
                    TblQuestionBefore tblQuestionBefore;
                    switch (request.EditStatus)
                    {
                        case (int)EditStatus.審核中:
                            tblQuestionBefore = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblQuestionBefore == null) { throw new Exception("無此項目"); }

                            tblQuestionBefore.CTitle = request.Title;
                            tblQuestionBefore.CContent = HtmlHelper.Sanitize(request.Content);
                            tblQuestionBefore.CIsEnabled = request.IsEnabled;
                            tblQuestionBefore.CEditStatus = request.EditStatus;
                            tblQuestionBefore.CEditDt = DateHelper.GetNowDate();

                            _repositoryWrapper.QuestionBefore.Update(tblQuestionBefore);

                            // 既有ID
                            var tblCategoryMappingBefores = await _repositoryWrapper.CategoryMappingBefore.GetByCondition(x => x.CContentId == request.Id).ToListAsync();
                            IEnumerable<int> createIDs;

                            if (request.CategoryIDs == null || request.CategoryIDs.Count() == 0)
                            {
                                _repositoryWrapper.CategoryMappingBefore.DeleteRange(tblCategoryMappingBefores);
                            }
                            else
                            {
                                createIDs = request.CategoryIDs.Except(tblCategoryMappingBefores.Select(x => x.CCategoryId).ToList());

                                foreach (var mappingBefore in tblCategoryMappingBefores)
                                {
                                    if (request.CategoryIDs.Contains(mappingBefore.CCategoryId))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        _repositoryWrapper.CategoryMappingBefore.Delete(mappingBefore);
                                    }
                                }

                                foreach (var id in createIDs)
                                {
                                    var tblCategoryMappingBefore = new TblCategoryMappingBefore() { CCategoryId = id, CContentId = (int)request.Id };
                                    _repositoryWrapper.CategoryMappingBefore.Create(tblCategoryMappingBefore);
                                }
                            }

                            await _repositoryWrapper.SaveAsync();
                            break;
                        case (int)EditStatus.駁回:
                            tblQuestionBefore = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblQuestionBefore == null) { throw new Exception("無此項目"); }

                            tblQuestionBefore.CEditStatus = request.EditStatus;
                            tblQuestionBefore.CEditDt = DateHelper.GetNowDate();
                            tblQuestionBefore.CReviewerId = payload.Id;

                            var existedNotes = tblQuestionBefore.CNotes != null ? JsonSerializer.Deserialize<List<SettingNote>>(tblQuestionBefore.CNotes) : new List<SettingNote>();
                            existedNotes!.Add(request.Note!);
                            tblQuestionBefore.CNotes = JsonSerializer.Serialize(existedNotes);

                            _repositoryWrapper.QuestionBefore.Update(tblQuestionBefore);
                            await _repositoryWrapper.SaveAsync();

                            break;
                        case (int)EditStatus.批准:
                            tblQuestionBefore = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();

                            if (tblQuestionBefore == null) { throw new Exception("無此項目"); }


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

        public async Task<ResponseBase<GetQuestionByIdResponse>> GetQuestionById(int id, bool isBefore)
        {
            var response = new ResponseBase<GetQuestionByIdResponse>() { Entries = new GetQuestionByIdResponse() };
            try
            {
                GetQuestionByIdResponse? question;

                if (isBefore)
                {
                    question = await _repositoryWrapper.QuestionBefore
                       .GetByCondition(x => x.CId == id)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == (int)UnitID.常見問題)
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
                           (x, y) => new GetQuestionByIdResponse
                           {
                               Id = x.CId,
                               IsBefore = true,
                               Content = x.CContent ?? "",
                               EditStatus = x.CEditStatus,
                               EditorId = x.CEditorId,
                               EditDt = x.CEditDt,
                               CreateDt = x.CCreateDt,
                               Categories = y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList(),
                               IsEnabled = x.CIsEnabled ?? false,
                               Title = x.CTitle,
                               Notes = x.CNotes,
                           }).FirstOrDefaultAsync();
                }
                else
                {
                    question = await _repositoryWrapper.QuestionAfter
                       .GetByCondition(x => x.CId == id)
                       .GroupJoin(
                           _repositoryWrapper.Category
                           .GetByCondition(a => a.CUnitId == (int)UnitID.常見問題)
                           .Join(
                               _repositoryWrapper.CategoryMappingAfter.GetAll(),
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
                           (x, y) => new GetQuestionByIdResponse
                           {
                               Id = x.CId,
                               IsBefore = true,
                               Content = x.CContent ?? "",
                               EditStatus = null,
                               EditorId = x.CEditorId,
                               EditDt = x.CEditDt,
                               CreateDt = x.CCreateDt,
                               Categories = y.Select(a => new Category { Id = a.Id, Name = a.Name }).ToList(),
                               IsEnabled = x.CIsEnabled ?? false,
                               Title = x.CTitle,
                               Notes = null,
                           }).FirstOrDefaultAsync();
                }

                if (question == null) { throw new Exception("無此問題"); }
                response.Entries = question;
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
