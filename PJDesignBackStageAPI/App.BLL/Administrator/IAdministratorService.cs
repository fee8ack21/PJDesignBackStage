using App.DAL.Models;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IAdministratorService
    {
        Task<ResponseBase<List<GetAdministratorsResponse>>> GetAdministrators();
        Task<ResponseBase<List<GetGroupsResponse>>> GetGroups();
        Task<ResponseBase<List<GetRightsResponse>>> GetRights();
        Task<ResponseBase<string>> CreateOrUpdateAdministrator(CreateOrUpdateAdministratorRequest request, JWTPayload? payload);
        Task<ResponseBase<GetAdministratorByIdResponse>> GetAdministratorById(int id);
        Task<ResponseBase<string>> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request);
        Task<ResponseBase<List<GetUnitRightsByGroupIdResponse>>> GetUnitRightsByGroupId(GetUnitRightsByGroupIdRequest request);
    }
}
