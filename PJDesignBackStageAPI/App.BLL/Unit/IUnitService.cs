using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IUnitService
    {
        Task<ResponseBase<List<GetBackStageUnitsResponse>>> GetBackStageUnits();
        Task<ResponseBase<List<GetBackStageUnitsByGroupIdResponse>>> GetBackStageUnitsByGroupId(JWTPayload payload);
        Task<ResponseBase<List<GetType2UnitsResponse>>> GetType2Units();
        Task<ResponseBase<GetSettingByUnitIdResponse>> GetSettingByUnitId(int id);
        Task<ResponseBase<string>> CreateOrUpdateSetting(CreateOrUpdateSettingRequest request);
    }
}
