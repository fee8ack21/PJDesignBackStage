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
    public class Type1Service : IType1Service
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public Type1Service(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<GetType1ContentByUnitIdResponse>> GetType1ContentByUnitId(int id)
        {
            var response = new ResponseBase<GetType1ContentByUnitIdResponse>() { Entries = new GetType1ContentByUnitIdResponse() };

            if (id <= 0) { throw new Exception("請求錯誤"); }

            var beforeContent = await _repositoryWrapper.Type1ContentBefore
                .GetByCondition(x => x.CUnitId == id)
                .Join(_repositoryWrapper.Administrator.GetAll(), x => x.CEditorId, y => y.CId, (x, y) => new
                {
                    x,
                    y
                }).FirstOrDefaultAsync();

            if (beforeContent != null)
            {
                response.Entries.Id = beforeContent.x.CId;
                response.Entries.UnitId = beforeContent.x.CUnitId;
                response.Entries.IsBefore = true;
                response.Entries.Content = beforeContent.x.CContent ?? "";
                response.Entries.EditStatus = beforeContent.x.CEditStatus;
                response.Entries.EditorId = beforeContent.x.CEditorId;
                response.Entries.EditorName = beforeContent.y.CName;
                response.Entries.EditDt = beforeContent.x.CEditDt;
                response.Entries.CreateDt = beforeContent.x.CCreateDt;
                response.Entries.Notes = beforeContent.x.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(beforeContent.x.CNotes) : null;
                return response;
            }

            var afterContent = await _repositoryWrapper.Type1ContentAfter
             .GetByCondition(x => x.CUnitId == id)
             .Join(_repositoryWrapper.Administrator.GetAll(), x => x.CEditorId, y => y.CId, (x, y) => new
             {
                 x,
                 y
             }).FirstOrDefaultAsync();

            if (afterContent != null)
            {
                response.Entries.Id = afterContent.x.CId;
                response.Entries.UnitId = afterContent.x.CUnitId;
                response.Entries.IsBefore = false;
                response.Entries.Content = afterContent.x.CContent ?? "";
                response.Entries.EditStatus = null;
                response.Entries.EditorId = afterContent.x.CEditorId;
                response.Entries.EditorName = afterContent.y.CName;
                response.Entries.EditDt = afterContent.x.CEditDt;
                response.Entries.CreateDt = afterContent.x.CCreateDt;
                response.Entries.Notes = null;
                return response;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdateType1Content(CreateOrUpdateType1ContentRequest request, JwtPayload payload)
        {
            var response = new ResponseBase<string>();

            TblType1ContentBefore? tblType1ContentBefore;
            switch (request.EditStatus)
            {
                case (int)EditStatus.審核中:
                    if (!(await _repositoryWrapper.Type1ContentBefore.GetAll().AnyAsync(x => x.CUnitId == request.UnitId)))
                    {
                        // 新增before 資料 | after 新增before 資料
                        tblType1ContentBefore = new TblType1ContentBefore()
                        {
                            CContent = HtmlHelper.Sanitize(request.Content),
                            CEditStatus = request.EditStatus,
                            CCreateDt = DateHelper.GetNowDate(),
                            CEditDt = DateHelper.GetNowDate(),
                            CEditorId = payload.Id,
                            CUnitId = request.UnitId,
                        };

                        _repositoryWrapper.Type1ContentBefore.Create(tblType1ContentBefore);
                        await _repositoryWrapper.SaveAsync();
                    }
                    else
                    {
                        // 駁回後編輯既有before 資料
                        tblType1ContentBefore = await _repositoryWrapper.Type1ContentBefore.GetByCondition(x => x.CUnitId == request.UnitId).FirstOrDefaultAsync();

                        if (tblType1ContentBefore == null) { throw new Exception("無此項目"); }

                        tblType1ContentBefore.CContent = HtmlHelper.Sanitize(request.Content);
                        tblType1ContentBefore.CEditStatus = request.EditStatus;
                        tblType1ContentBefore.CEditDt = DateHelper.GetNowDate();

                        _repositoryWrapper.Type1ContentBefore.Update(tblType1ContentBefore);

                        await _repositoryWrapper.SaveAsync();
                    }
                    break;
                case (int)EditStatus.駁回:
                    tblType1ContentBefore = await _repositoryWrapper.Type1ContentBefore.GetByCondition(x => x.CUnitId == request.UnitId).FirstOrDefaultAsync();

                    if (tblType1ContentBefore == null) { throw new Exception("無此項目"); }

                    tblType1ContentBefore.CEditStatus = request.EditStatus;
                    tblType1ContentBefore.CEditDt = DateHelper.GetNowDate();
                    tblType1ContentBefore.CReviewerId = payload.Id;

                    var existedNotes = tblType1ContentBefore.CNotes != null ? JsonSerializer.Deserialize<List<ReviewNote>>(tblType1ContentBefore.CNotes) : new List<ReviewNote>();

                    request.Note!.Date = DateHelper.GetNowDate();
                    existedNotes!.Add(request.Note!);
                    tblType1ContentBefore.CNotes = JsonSerializer.Serialize(existedNotes);

                    _repositoryWrapper.Type1ContentBefore.Update(tblType1ContentBefore);

                    await _repositoryWrapper.SaveAsync();
                    break;
                case (int)EditStatus.批准:
                    tblType1ContentBefore = await _repositoryWrapper.Type1ContentBefore.GetByCondition(x => x.CUnitId == request.UnitId).FirstOrDefaultAsync();

                    if (tblType1ContentBefore == null) { throw new Exception("無此項目"); }

                    if (!(await _repositoryWrapper.Type1ContentAfter.GetAll().AnyAsync(x => x.CUnitId == request.UnitId)))
                    {
                        // 新增after 資料
                        var tblType1ContentAfter = new TblType1ContentAfter()
                        {
                            CContent = HtmlHelper.Sanitize(request.Content),
                            CEditorId = tblType1ContentBefore.CEditorId,
                            CEditDt = DateHelper.GetNowDate(),
                            CCreatorId = tblType1ContentBefore.CEditorId,
                            CCreateDt = DateHelper.GetNowDate(),
                            CUnitId = request.UnitId,
                        };

                        _repositoryWrapper.Type1ContentAfter.Create(tblType1ContentAfter);
                        _repositoryWrapper.Type1ContentBefore.Delete(tblType1ContentBefore);
                        await _repositoryWrapper.SaveAsync();
                    }
                    else
                    {
                        // 修改after 資料
                        var tblType1ContentAfter = await _repositoryWrapper.Type1ContentAfter.GetByCondition(x => x.CUnitId == tblType1ContentBefore.CUnitId).FirstOrDefaultAsync();

                        if (tblType1ContentAfter == null) { throw new Exception("請求錯誤"); }

                        tblType1ContentAfter.CContent = HtmlHelper.Sanitize(request.Content);
                        tblType1ContentAfter.CEditorId = tblType1ContentBefore.CEditorId;
                        tblType1ContentAfter.CEditDt = DateHelper.GetNowDate();

                        _repositoryWrapper.Type1ContentAfter.Update(tblType1ContentAfter);
                        _repositoryWrapper.Type1ContentBefore.Delete(tblType1ContentBefore);
                        await _repositoryWrapper.SaveAsync();
                    }
                    break;
            }

            return response;
        }
    }
}
