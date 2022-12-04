using App.Common;
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
                response.Entries = await _repositoryWrapper.Administrator.GetAll()
                    .Join(_repositoryWrapper.AdministratorGroup.GetAll(), x => x.CId, y => y.CAdministratorId, (x, y) => new
                    {
                        x,
                        y
                    }).Join(_repositoryWrapper.Group.GetAll(), x => x.y.CGroupId, y => y.CId, (x, y) => new GetAdministratorsResponse
                    {
                        Id = x.x.CId,
                        Account = x.x.CAccount,
                        Name = x.x.CName,
                        GroupId = y.CId,
                        GroupName = y.CName,
                        IsEnabled = x.x.CIsEnabled,
                        CreateDt = x.x.CCreateDt
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

        public async Task<ResponseBase<string>> CreateOrUpdateGroup(CreateOrUpdateGroupRequest request)
        {
            var response = new ResponseBase<string>();
            try
            {
                using (var transaction = _repositoryWrapper.CreateTransaction())
                {
                    if (request.Id == null)
                    {
                        if (_repositoryWrapper.Group.GetByCondition(x => x.CName == request.Name).Any())
                        {
                            throw new Exception("此組別名稱已存在");
                        }

                        var tblGroup = new TblGroup() { CName = request.Name };

                        _repositoryWrapper.Group.Create(tblGroup);
                        await _repositoryWrapper.SaveAsync();

                        if (request.UnitRights != null)
                        {
                            var tblGroupUnitRights = new List<TblGroupUnitRight>();

                            foreach (var unitRight in request.UnitRights)
                            {
                                var tblGroupUnitRight = new TblGroupUnitRight() { CGroupId = tblGroup.CId, CRightId = unitRight.RightId, CUnitId = unitRight.UnitId };
                                tblGroupUnitRights.Add(tblGroupUnitRight);
                            }

                            _repositoryWrapper.GroupUnitRight.CreateRange(tblGroupUnitRights);
                            await _repositoryWrapper.SaveAsync();
                        }

                        transaction.Commit();
                    }
                    else
                    {
                        var tblGroup = await _repositoryWrapper.Group.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();
                        if (tblGroup == null)
                        {
                            throw new Exception("無此組別");
                        }

                        if (tblGroup.CName != request.Name)
                        {
                            if (_repositoryWrapper.Group.GetByCondition(x => x.CName == request.Name).Any())
                            {
                                throw new Exception("此組別名稱已存在");
                            }

                            tblGroup.CName = request.Name;
                            _repositoryWrapper.Group.Update(tblGroup);
                        }

                        if (request.UnitRights != null)
                        {
                            var unitIDs = request.UnitRights.Select(x => x.UnitId).ToList();

                            var existedUnitRights = await _repositoryWrapper.GroupUnitRight.GetByCondition(x => x.CGroupId == request.Id).ToListAsync();
                            var createdUnitRights = new List<TblGroupUnitRight>();
                            var removedUnitRights = existedUnitRights.Where(x => !unitIDs.Contains(x.CUnitId)).ToList();
                            var updatedUnitRights = existedUnitRights.Except(removedUnitRights);

                            foreach (var unitRight in request.UnitRights)
                            {
                                if (updatedUnitRights.Any(x => x.CUnitId == unitRight.UnitId))
                                {
                                    updatedUnitRights.Where(x => x.CUnitId == unitRight.UnitId).FirstOrDefault()!.CRightId = unitRight.RightId;
                                }
                                else
                                {
                                    createdUnitRights.Add(new TblGroupUnitRight() { CGroupId = (int)request.Id, CUnitId = unitRight.UnitId, CRightId = unitRight.RightId });
                                }
                            }

                            _repositoryWrapper.GroupUnitRight.DeleteRange(removedUnitRights);
                            _repositoryWrapper.GroupUnitRight.UpdateRange(updatedUnitRights);
                            _repositoryWrapper.GroupUnitRight.CreateRange(createdUnitRights);
                        }

                        await _repositoryWrapper.SaveAsync();
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<string>> CreateOrUpdateAdministrator(CreateOrUpdateAdministratorRequest request, JWTPayload? payload)
        {
            var response = new ResponseBase<string>();
            try
            {
                if (request.Id == null)
                {
                    if (_repositoryWrapper.Administrator.GetByCondition(x => x.CAccount == request.Account.Trim()).Any())
                    {
                        throw new Exception("此帳號已被使用");
                    }

                    using (var transaction = _repositoryWrapper.CreateTransaction())
                    {
                        var tblAdministrator = new TblAdministrator() { CAccount = request.Account, CPassword = HashHelper.GetPbkdf2Value(request.Password), CName = request.Name, CIsEnabled = request.IsEnabled };
                        _repositoryWrapper.Administrator.Create(tblAdministrator);
                        await _repositoryWrapper.SaveAsync();

                        var tblAdministratorGroup = new TblAdministratorGroup() { CAdministratorId = tblAdministrator.CId, CGroupId = request.GroupId };
                        _repositoryWrapper.AdministratorGroup.Create(tblAdministratorGroup);
                        await _repositoryWrapper.SaveAsync();

                        transaction.Commit();
                        return response;
                    }
                }

                var administrator = await _repositoryWrapper.Administrator.GetByCondition(x => x.CId == request.Id).FirstOrDefaultAsync();
                if (administrator == null)
                {
                    throw new Exception("無此帳號");
                }
                if (request.Id == payload?.Id && !request.IsEnabled)
                {
                    throw new Exception("無法停用當前帳號");
                }

                administrator.CName = request.Name;
                administrator.CAccount = request.Account;

                if (administrator.CIsEnabled == false && administrator.CLoginAttemptCount >= 3 && request.IsEnabled)
                {
                    administrator.CLoginAttemptCount = 0;
                }
                
                administrator.CIsEnabled = request.IsEnabled;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    administrator.CPassword = HashHelper.GetPbkdf2Value(request.Password);
                }

                _repositoryWrapper.Administrator.Update(administrator);

                var administratorGroup = await _repositoryWrapper.AdministratorGroup.GetByCondition(x => x.CAdministratorId == request.Id).FirstOrDefaultAsync();
                if (administratorGroup == null)
                {
                    throw new Exception("請求錯誤");
                }

                administratorGroup.CGroupId = request.GroupId;
                _repositoryWrapper.AdministratorGroup.Update(administratorGroup);

                await _repositoryWrapper.SaveAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<GetAdministratorByIdResponse>> GetAdministratorById(int id)
        {
            var response = new ResponseBase<GetAdministratorByIdResponse>() { Entries = new GetAdministratorByIdResponse() };
            try
            {
                response.Entries = await _repositoryWrapper.Administrator.GetByCondition(x => x.CId == id)
                    .Join(_repositoryWrapper.AdministratorGroup.GetByCondition(y => y.CAdministratorId == id), x => x.CId, y => y.CAdministratorId, (x, y) => new GetAdministratorByIdResponse
                    {
                        Id = id,
                        Account = x.CAccount,
                        Name = x.CName,
                        GroupId = y.CGroupId,
                        IsEnabled = x.CIsEnabled ?? true
                    }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCode.Fail;
            }

            return response;
        }

        public async Task<ResponseBase<List<GetUnitRightsByGroupIdResponse>>> GetUnitRightsByGroupId(GetUnitRightsByGroupIdRequest request)
        {
            var response = new ResponseBase<List<GetUnitRightsByGroupIdResponse>>() { Entries = new List<GetUnitRightsByGroupIdResponse>() };
            try
            {
                response.Entries = await _repositoryWrapper.GroupUnitRight.GetByCondition(x => x.CGroupId == request.Id).Select(x => new GetUnitRightsByGroupIdResponse()
                {
                    UnitId = x.CUnitId,
                    RightId = x.CRightId
                }).ToListAsync();
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
