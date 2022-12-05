using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IQuestionService
    {
        Task<ResponseBase<List<GetQuestionsResponse>>> GetQuestions();
        Task<ResponseBase<string>> CreateOrUpdateQuestion(CreateOrUpdateQuestionRequest request, JwtPayload payload);
    }
}
