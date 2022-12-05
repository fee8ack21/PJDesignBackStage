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
            var response = new ResponseBase<List<GetQuestionsResponse>>() { Entries = new List<GetQuestionsResponse>() };
            try
            {
                var afterQuestionsTask = _repositoryWrapper.QuestionAfter.GetAll().ToListAsync();
                var beforeQuestionTask = _repositoryWrapper.QuestionBefore.GetAll().ToListAsync();
                var tasks = new List<Task>() { afterQuestionsTask, beforeQuestionTask };

                await Task.WhenAll(tasks);

                var afterQuestions = afterQuestionsTask.Result;
                var beforeQuestions = beforeQuestionTask.Result;

                var afterIDsInBefoe = new List<int>();

                // Todo: 取得category mapping data
                foreach (var item in beforeQuestions)
                {
                    var temp = new GetQuestionsResponse()
                    {
                        Id = item.CId,
                        IsBefore = true,
                        Name = item.CName,
                        CreateDt = item.CCreateDt,
                        EditDt = item.CEditDt,
                        EditorId = item.CEditorId,
                        Content = item.CContent,
                        Status = item.CStatus
                    };

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
    }
}
