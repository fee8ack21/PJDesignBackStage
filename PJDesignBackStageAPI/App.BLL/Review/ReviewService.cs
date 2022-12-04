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

        public Task<ResponseBase<List<GetReviewsResponse>>> GetReviews()
        {
            throw new NotImplementedException();

            //var response = new ResponseBase<List<GetReviewListResponse>>() { Entries = new List<GetReviewListResponse>() };

            //try
            //{
            //    var tasks = new List<Task>();
            //    var settingBeforeTask = _repositoryWrapper.SettingBefore.GetByCondition(x => x.CStatus == (int)Status.審核中).ToListAsync();
            //    tasks.Add(settingBeforeTask);
            //}
            //catch (Exception ex)
            //{
            //    response.StatusCode = StatusCode.Fail;
            //    response.Message = ex.Message;
            //}

            //return response;
        }
    }
}
