using App.Common;
using App.DAL.Models;
using App.DAL.Repositories;
using App.Enum;
using App.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.BLL
{
    public class UnitService : IUnitService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public UnitService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetUnitsResponse>>> GetUnits(GetUnitsRequest request, JwtPayload payload)
        {
            var response = new ResponseBase<List<GetUnitsResponse>>() { Entries = new List<GetUnitsResponse>() };
            try
            {
                var predicate = PredicateBuilder.True<TblUnit>();

                if (request.StageType != null)
                {
                    predicate = predicate.And(x => x.CStageType == request.StageType || x.CStageType == (int)StageType.前後台);
                }
                if (request.TemplateType != null)
                {
                    predicate = predicate.And(x => x.CTemplateType == request.TemplateType);
                }

                if (request.GroupId == null)
                {
                    response.Entries = await _repositoryWrapper.Unit
                        .GetByCondition(predicate)
                        .Select(x => new GetUnitsResponse
                        {
                            Id = x.CId,
                            Name = x.CName,
                            BackStageUrl = x.CBackStageUrl,
                            TemplateType = x.CTemplateType,
                            FrontStageUrl = x.CFrontStageUrl,
                            IsAnotherWindow = x.CIsAnotherWindow,
                            IsEnabled = x.CIsEnabled,
                            CreateDt = x.CCreateDt,
                            Parent = x.CParent,
                            StageType = x.CStageType,
                            Sort = x.CSort
                        }).ToListAsync();
                }
                else
                {
                    response.Entries = await _repositoryWrapper.Unit
                        .GetByCondition(predicate)
                        .Join(_repositoryWrapper.GroupUnitRight.GetByCondition(a => a.CGroupId == request.GroupId),
                        x => x.CId,
                        y => y.CUnitId, (x, y) => new GetUnitsResponse
                        {
                            Id = x.CId,
                            Name = x.CName,
                            BackStageUrl = x.CBackStageUrl,
                            TemplateType = x.CTemplateType,
                            FrontStageUrl = x.CFrontStageUrl,
                            IsAnotherWindow = x.CIsAnotherWindow,
                            IsEnabled = x.CIsEnabled,
                            CreateDt = x.CCreateDt,
                            Parent = x.CParent,
                            StageType = x.CStageType,
                            Sort = x.CSort
                        }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<GetSettingByUnitIdResponse>> GetSettingByUnitId(int id)
        {
            var response = new ResponseBase<GetSettingByUnitIdResponse>() { Entries = new GetSettingByUnitIdResponse() { UnitId = id } };
            try
            {
                var tblSettingBefore = await _repositoryWrapper.SettingBefore.GetByCondition(x => x.CUnitId == id).FirstOrDefaultAsync();

                if (tblSettingBefore != null)
                {
                    response.Entries.UnitId = tblSettingBefore.CUnitId;
                    response.Entries.Content = tblSettingBefore?.CContent != null ? JsonSerializer.Deserialize<object?>(tblSettingBefore.CContent) : null;
                    response.Entries.EditStatus = tblSettingBefore!.CEditStatus;
                    response.Entries.EditorId = tblSettingBefore.CEditorId;
                    response.Entries.EditorName = _repositoryWrapper.Administrator.GetByCondition(x => x.CId == tblSettingBefore.CEditorId).FirstOrDefault()?.CName;
                    response.Entries.ReviewerId = tblSettingBefore.CReviewerId;
                    response.Entries.Notes = tblSettingBefore.CNotes != null ? JsonSerializer.Deserialize<object?>(tblSettingBefore.CNotes) : null;
                    response.Entries.CreateDt = tblSettingBefore.CCreateDt;
                    return response;
                }

                var tblSettingAfter = await _repositoryWrapper.SettingAfter.GetByCondition(x => x.CUnitId == id).FirstOrDefaultAsync();

                if (tblSettingAfter != null)
                {
                    response.Entries.UnitId = tblSettingAfter.CUnitId;
                    response.Entries.Content = tblSettingAfter?.CContent != null ? JsonSerializer.Deserialize<object?>(tblSettingAfter.CContent) : null;
                    response.Entries.EditorId = tblSettingAfter!.CEditorId;
                    response.Entries.EditorName = _repositoryWrapper.Administrator.GetByCondition(x => x.CId == tblSettingAfter.CEditorId).FirstOrDefault()?.CName;
                    response.Entries.CreateDt = tblSettingAfter.CCreateDt;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdateSetting(CreateOrUpdateSettingRequest request, JwtPayload payload)
        {
            var response = new ResponseBase<string>();
            try
            {
                var tblUnit = await _repositoryWrapper.Unit.GetByCondition(x => x.CId == request.UnitId).FirstOrDefaultAsync();

                if (tblUnit == null) { throw new Exception("無此單元"); }

                var existedSettingBefore = await _repositoryWrapper.SettingBefore.GetByCondition(x => x.CUnitId == request.UnitId).FirstOrDefaultAsync();

                // 若無before 資料，只允許新增(Status to 審核中)
                if (existedSettingBefore == null)
                {
                    if (request.EditStatus != (int)EditStatus.審核中) { throw new Exception("請求錯誤"); }
                    if (request.Content == null) { throw new Exception("請求錯誤"); }

                    var tblSettingBefore = new TblSettingBefore();
                    tblSettingBefore.CEditStatus = (int)EditStatus.審核中;
                    tblSettingBefore.CEditorId = payload.Id;
                    tblSettingBefore.CUnitId = request.UnitId;
                    tblSettingBefore.CContent = JsonSerializer.Serialize(request.Content);

                    _repositoryWrapper.SettingBefore.Create(tblSettingBefore);
                    await _repositoryWrapper.SaveAsync();
                    return response;
                }

                // 若before 資料為審核狀態，不允許Editor 再進行操作
                if (existedSettingBefore.CEditStatus == (int)EditStatus.審核中)
                {
                    if (existedSettingBefore.CEditorId == payload.Id)
                    {
                        throw new Exception("設定處於審核狀態");
                    }

                    if (request.EditStatus == (int)EditStatus.駁回)
                    {
                        if (request.Note == null) { throw new Exception("請填寫備註欄位"); }

                        existedSettingBefore.CEditStatus = request.EditStatus;
                        existedSettingBefore.CEditDt = DateHelper.GetNowDate();
                        existedSettingBefore.CReviewerId = payload.Id;

                        var existedNotes = existedSettingBefore.CNotes != null ? JsonSerializer.Deserialize<List<SettingNote>>(existedSettingBefore.CNotes) : new List<SettingNote>();
                        existedNotes!.Add(request.Note);
                        existedSettingBefore.CNotes = JsonSerializer.Serialize(existedNotes);

                        _repositoryWrapper.SettingBefore.Update(existedSettingBefore);
                        await _repositoryWrapper.SaveAsync();
                        return response;
                    }

                    if (request.EditStatus == (int)EditStatus.批准)
                    {
                        var tblSettingAfter = await _repositoryWrapper.SettingAfter.GetByCondition(x => x.CUnitId == tblUnit.CId).FirstOrDefaultAsync();

                        if (tblSettingAfter == null)
                        {
                            tblSettingAfter = new TblSettingAfter();
                            tblSettingAfter.CUnitId = existedSettingBefore.CUnitId;
                            tblSettingAfter.CContent = existedSettingBefore.CContent;
                            tblSettingAfter.CEditorId = existedSettingBefore.CEditorId;
                            _repositoryWrapper.SettingAfter.Create(tblSettingAfter);
                        }
                        else
                        {
                            tblSettingAfter.CContent = existedSettingBefore.CContent;
                            tblSettingAfter.CEditorId = existedSettingBefore.CEditorId;
                            tblSettingAfter.CEditDt = DateHelper.GetNowDate();
                            _repositoryWrapper.SettingAfter.Update(tblSettingAfter);
                        }

                        _repositoryWrapper.SettingBefore.Delete(existedSettingBefore);
                        await _repositoryWrapper.SaveAsync();
                        return response;
                    }
                }

                // 若before 資料為駁回狀態，只允許Editor 再進行操作
                if (existedSettingBefore.CEditStatus == (int)EditStatus.駁回)
                {
                    if (existedSettingBefore.CEditorId != payload.Id)
                    {
                        throw new Exception("設定處於駁回狀態");
                    }

                    existedSettingBefore.CEditStatus = (int)EditStatus.審核中;
                    existedSettingBefore.CContent = JsonSerializer.Serialize(request.Content);
                    existedSettingBefore.CEditDt = DateHelper.GetNowDate();
                    _repositoryWrapper.SettingBefore.Update(existedSettingBefore);
                    await _repositoryWrapper.SaveAsync();
                    return response;
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
