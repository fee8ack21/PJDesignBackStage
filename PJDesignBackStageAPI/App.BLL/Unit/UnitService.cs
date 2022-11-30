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
    }
}
