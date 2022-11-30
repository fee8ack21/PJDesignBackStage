using App.DAL;
using App.DAL.Models;
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
    public class AdministratorService : IAdministratorService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AdministratorService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<List<GetAdministratorsResponse>>> GetAdministrators()
        {
            var response = new ResponseBase<List<GetAdministratorsResponse>>() { Entries = new List<GetAdministratorsResponse>() };
            try
            {
                response.Entries = await _repositoryWrapper.Administrator.GetAll().Select(x => new GetAdministratorsResponse()
                {
                    Id = x.CId,
                    Account = x.CAccount,
                    Name = x.CName,
                    IsEnabled = x.CIsEnabled,
                    CreateDt = x.CCreateDt
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<List<GetGroupsResponse>>> GetGroups()
        {
            var response = new ResponseBase<List<GetGroupsResponse>>() { Entries = new List<GetGroupsResponse>() };
            try
            {
                response.Entries = await _repositoryWrapper.Group.GetAll().Select(x => new GetGroupsResponse()
                {
                    Id = x.CId,
                    Name = x.CName,
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<List<GetRightsResponse>>> GetRights()
        {
            var response = new ResponseBase<List<GetRightsResponse>>() { Entries = new List<GetRightsResponse>() };
            try
            {
                response.Entries = await _repositoryWrapper.Right.GetAll().Select(x => new GetRightsResponse()
                {
                    Id = x.CId,
                    Name = x.CName,
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<CreateGroupResponse>> CreateGroup(CreateGroupRequest request)
        {
            var response = new ResponseBase<CreateGroupResponse>() { Entries = new CreateGroupResponse() };
            try
            {
                var tblGroup = new TblGroup() { CName = request.Name };
                _repositoryWrapper.Group.Create(tblGroup);
                await _repositoryWrapper.SaveAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdateAdministrator(CreateOrUpdateAdministratorRequest request)
        {
            var response = new ResponseBase<string>();
            try
            {
                if (request.Id == null)
                {
                    // 新增
                }
                else
                {
                    // 修改
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
