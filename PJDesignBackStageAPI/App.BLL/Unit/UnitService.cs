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
    public class UnitService : IUnitService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public UnitService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetBackStageUnitsResponse>>> GetBackStageUnits()
        {
            var response = new ResponseBase<List<GetBackStageUnitsResponse>>() { Entries = new List<GetBackStageUnitsResponse>() };
            try
            {
                var query = _repositoryWrapper.Unit.GetByCondition(x => x.CStageType == (int)StageType.後台 || x.CStageType == (int)StageType.前後台).Select(x => new GetBackStageUnitsResponse
                {
                    Id = x.CId,
                    Name = x.CName
                });

                response.Entries = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>> GetBackStageUnitsByGroupId(JwtPayload payload)
        {
            var response = new ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>();
            try
            {
                var query = _repositoryWrapper.GroupUnitRight.GetByCondition(x => x.CGroupId == payload.GroupId)
                    .Join(_repositoryWrapper.Unit.GetByCondition(y => y.CStageType == (int)StageType.後台 || y.CStageType == (int)StageType.前後台), x => x.CUnitId, y => y.CId, (x, y) => new GetBackStageUnitsByGroupIdResponse
                    {
                        Id = y.CId,
                        Name = y.CName,
                        Url = y.CBackStageUrl,
                        Parent = y.CParent,
                        TemplateType = y.CTemplateType
                    });

                response.Entries = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<List<GetType2UnitsResponse>>> GetType2Units()
        {
            var response = new ResponseBase<List<GetType2UnitsResponse>>() { Entries = new List<GetType2UnitsResponse>() };
            try
            {
                var query = _repositoryWrapper.Unit.GetByCondition(x => x.CTemplateType == (int)TemplateType.模板二).Select(x => new GetType2UnitsResponse
                {
                    Id = x.CId,
                    Name = x.CName
                });

                response.Entries = await query.ToListAsync();
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
                    response.Entries.Status = tblSettingBefore!.CEditStatus;
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
