﻿using App.DAL.Repositories;
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

        public async Task<ResponseBase<List<GetReviewsResponse>>> GetReviews(JwtPayload payload)
        {
            var response = new ResponseBase<List<GetReviewsResponse>>() { Entries = new List<GetReviewsResponse>() };
        
            var settings = await _repositoryWrapper.SettingBefore.GetByCondition(x => x.CEditorId != payload.Id)
            .Join(_repositoryWrapper.Unit.GetAll(), x => x.CUnitId, y => y.CId, (x, y) => new { x, y })
            .Join(_repositoryWrapper.Administrator.GetAll(), x => x.x.CEditorId, y => y.CId, (x, y) => new GetReviewsResponse
            {
                UnitId = x.x.CUnitId,
                UnitName = x.y.CName,
                Title = x.x.CUnitId != (int)UnitId.首頁設定 && x.x.CUnitId != (int)UnitId.Footer設定 ? "Banner設定" : null,
                url = x.y.CBackStageUrl,
                ContentId = x.x.CId,
                EditDt = x.x.CEditDt,
                EditStatus = x.x.CEditStatus,
                EditorId = x.x.CEditorId,
                EditorName = y.CName
            }).ToListAsync();

            var questions = await _repositoryWrapper.QuestionBefore.GetByCondition(x => x.CEditorId != payload.Id)
                .Join(_repositoryWrapper.Administrator.GetAll(), x => x.CEditorId, y => y.CId, (x, y) => new GetReviewsResponse
                {
                    UnitId = (int)UnitId.常見問題,
                    UnitName = "常見問題",
                    url = $"/question/detail?id={x.CId}&status={x.CEditStatus}&isBefore=true",
                    Title = x.CTitle,
                    ContentId = x.CId,
                    EditDt = x.CEditDt,
                    EditStatus = x.CEditStatus,
                    EditorId = x.CEditorId,
                    EditorName = y.CName
                }).ToListAsync();

            var portfolios = await _repositoryWrapper.PortfolioBefore.GetByCondition(x => x.CEditorId != payload.Id)
                .Join(_repositoryWrapper.Administrator.GetAll(), x => x.CEditorId, y => y.CId, (x, y) => new GetReviewsResponse
                {
                    UnitId = (int)UnitId.作品集,
                    UnitName = "作品集",
                    Title = x.CTitle,
                    url = $"/portfolio/detail?id={x.CId}&status={x.CEditStatus}&isBefore=true",
                    ContentId = x.CId,
                    EditDt = x.CEditDt,
                    EditStatus = x.CEditStatus,
                    EditorId = x.CEditorId,
                    EditorName = y.CName
                }).ToListAsync();

            var type1Contents = await _repositoryWrapper.Type1ContentBefore.GetByCondition(x => x.CEditorId != payload.Id)
              .Join(_repositoryWrapper.Unit.GetAll(), x => x.CUnitId, y => y.CId, (x, y) => new { x, y })
              .Join(_repositoryWrapper.Administrator.GetAll(), x => x.x.CEditorId, y => y.CId, (x, y) => new GetReviewsResponse
              {
                  UnitId = x.x.CUnitId,
                  UnitName = x.y.CName,
                  ContentId = x.x.CId,
                  EditDt = x.x.CEditDt,
                  url = x.y.CBackStageUrl,
                  EditStatus = x.x.CEditStatus,
                  EditorId = x.x.CEditorId,
                  EditorName = y.CName
              }).ToListAsync();

            var type2Contents = await _repositoryWrapper.Type2ContentBefore.GetByCondition(x => x.CEditorId != payload.Id)
               .Join(_repositoryWrapper.Unit.GetAll(), x => x.CUnitId, y => y.CId, (x, y) => new { x, y })
               .Join(_repositoryWrapper.Administrator.GetAll(), x => x.x.CEditorId, y => y.CId, (x, y) => new GetReviewsResponse
               {
                   UnitId = x.x.CUnitId,
                   UnitName = x.y.CName,
                   url = $"/type2/detail?id={x.x.CId}&uid={x.x.CUnitId}&status={x.x.CEditStatus}&isBefore=true",
                   Title = x.x.CTitle,
                   ContentId = x.x.CId,
                   EditDt = x.x.CEditDt,
                   EditStatus = x.x.CEditStatus,
                   EditorId = x.x.CEditorId,
                   EditorName = y.CName
               }).ToListAsync();

            response.Entries.AddRange(settings);
            response.Entries.AddRange(questions);
            response.Entries.AddRange(portfolios);
            response.Entries.AddRange(type1Contents);
            response.Entries.AddRange(type2Contents);
            response.Entries = response.Entries.OrderByDescending(x => x.EditDt).ToList();

            return response;
        }
    }
}
