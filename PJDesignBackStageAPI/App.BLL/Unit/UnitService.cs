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

        public async Task<ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>> GetBackStageUnitsByGroupId(JWTPayload payload)
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
            var response = new ResponseBase<GetSettingByUnitIdResponse>() { Entries = new GetSettingByUnitIdResponse() };
            try
            {
                response.Entries.UnitId = id;
                
                var contentString = await _repositoryWrapper.Unit.GetByCondition(x => x.CId == id).Join(_repositoryWrapper.Setting.GetAll(), x => x.CSettingId, y => y.CId, (x, y) => y.CContent).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(contentString))
                {
                    response.Entries.Content = JsonSerializer.Deserialize<object?>(contentString); ;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdateSetting(CreateOrUpdateSettingRequest request)
        {
            var response = new ResponseBase<string>();
            try
            {
                using (var transaction = _repositoryWrapper.CreateTransaction())
                {
                    if (request.Content == null) { throw new Exception("請求錯誤"); }

                    var tblUnit = await _repositoryWrapper.Unit.GetByCondition(x => x.CId == request.UnitId).FirstOrDefaultAsync();

                    if (tblUnit == null) { throw new Exception("無此單元"); }

                    var tblSetting = await _repositoryWrapper.Setting.GetByCondition(x => x.CId == tblUnit.CSettingId).FirstOrDefaultAsync();

                    if (tblSetting != null)
                    {
                        tblSetting.CContent = JsonSerializer.Serialize(request.Content);
                        _repositoryWrapper.Setting.Update(tblSetting);
                        await _repositoryWrapper.SaveAsync();
                        transaction.Commit();

                        return response;
                    }

                    tblSetting = new TblSetting();
                    tblSetting.CContent = JsonSerializer.Serialize(request.Content);

                    _repositoryWrapper.Setting.Create(tblSetting);
                    await _repositoryWrapper.SaveAsync();

                    tblUnit.CSettingId = tblSetting.CId;

                    _repositoryWrapper.Unit.Update(tblUnit);
                    await _repositoryWrapper.SaveAsync();

                    transaction.Commit();
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
