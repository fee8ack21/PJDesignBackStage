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
    public class ReviewService : IReviewService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ReviewService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<ResponseBase<List<GetReviewsResponse>>> GetReviews(JwtPayload payload)
        {
            throw new NotImplementedException();

            // Setting / Home / Footer
            // 問題
            // 作品集
            // Type1
            // Type2

            // OrderBy EditDt

            // List<Task> When All
        }
    }
}
