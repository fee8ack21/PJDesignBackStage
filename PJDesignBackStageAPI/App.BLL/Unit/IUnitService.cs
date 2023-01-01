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
        Task<ResponseBase<List<GetUnitsResponse>>> GetUnits(GetUnitsRequest request);
        Task<ResponseBase<GetSettingByUnitIdResponse>> GetSettingByUnitId(int id);
        Task<ResponseBase<string>> CreateOrUpdateSetting(CreateOrUpdateSettingRequest request, JwtPayload payload);
        Task<ResponseBase<string>> CreateOrUpdateUnit(CreateOrUpdateUnitRequest request);
        Task<ResponseBase<string>> UpdateUnitsSort(IEnumerable<UpdateUnitsSortRequest> request);
    }
}
