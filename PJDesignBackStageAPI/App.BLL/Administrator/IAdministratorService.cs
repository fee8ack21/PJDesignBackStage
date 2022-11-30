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
        Task<ResponseBase<CreateGroupResponse>> CreateGroup(CreateGroupRequest request);
        Task<ResponseBase<List<GetRightsResponse>>> GetRights();
        Task<ResponseBase<string>> CreateOrUpdateAdministrator(CreateOrUpdateAdministratorRequest request);
    }
}
