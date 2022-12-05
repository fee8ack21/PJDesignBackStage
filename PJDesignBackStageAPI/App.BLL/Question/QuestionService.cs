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
                    await _repositoryWrapper.SaveAsync();

                    if (request.CategoryIDs != null)
                    {
                        foreach (var categoryId in request.CategoryIDs)
                        {
                            var tblCategoryMappingBefore = new TblCategoryMappingBefore() { CCategoryId = categoryId, CContentId = tblQuestionBefore.CId };
                            _repositoryWrapper.CategoryMappingBefore.Create(tblCategoryMappingBefore);
                        }
                        await _repositoryWrapper.SaveAsync();
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
