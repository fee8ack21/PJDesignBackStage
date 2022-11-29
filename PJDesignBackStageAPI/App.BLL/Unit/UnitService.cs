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

        public async Task<ResponseBase<List<GetUnitsResponse>>> GetUnits(JWTPayload payload)
        {
            var response = new ResponseBase<List<GetUnitsResponse>>();
            try
            {
                var query = _repositoryWrapper.GroupUnit.GetByCondition(x => x.CGroupId == payload.GroupId)
                    .Join(_repositoryWrapper.Unit.GetByCondition(y => y.CIsBackStage), x => x.CUnitId, y => y.CId, (x, y) => new GetUnitsResponse
                    {
                        Id = y.CId,
                        Name = y.CName,
                        Url = y.CUrl,
                        Parent = y.CParent,
                        IsAnotherWindow = y.CIsAnotherWindow,
                        Type = y.CType
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
