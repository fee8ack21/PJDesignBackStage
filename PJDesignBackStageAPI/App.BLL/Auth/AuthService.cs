using App.Common;
using App.DAL.Models;
using App.DAL.Repositories;
using App.Enum;
using App.Model;
using Microsoft.EntityFrameworkCore;

namespace App.BLL
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AuthService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ResponseBase<AuthLoginResponse>> Login(AuthLoginRequest request)
        {
            var response = new ResponseBase<AuthLoginResponse>() { Entries = new AuthLoginResponse() };

            try
            {
                var administrator = await _repositoryWrapper.Administrator.GetByCondition(x => x.CAccount == request.Account).FirstOrDefaultAsync();

                if (administrator == null)
                {
                    throw new Exception("無此用戶");
                }
                if (administrator.CIsEnabled != true)
                {
                    throw new Exception("帳號停用中");
                }

                var hashedPassword = HashHelper.GetPbkdf2Value(request.Password);
                if (hashedPassword != administrator.CPassword)
                {
                    administrator.CLoginAttemptCount += 1;

                    if (administrator.CLoginAttemptCount < 3)
                    {
                        _repositoryWrapper.Administrator.Update(administrator);
                        await _repositoryWrapper.SaveAsync();

                        throw new Exception("密碼錯誤");
                    }

                    administrator.CIsEnabled = false;
                    _repositoryWrapper.Administrator.Update(administrator);
                    await _repositoryWrapper.SaveAsync();

                    throw new Exception("嘗試登入達三次，帳戶停用");
                }

                var tblAdministratorGroup = await _repositoryWrapper.AdministratorGroup.GetByCondition(x => x.CAdministratorId == administrator.CId).FirstOrDefaultAsync();
                if (tblAdministratorGroup == null)
                {
                    throw new Exception("請求錯誤");
                }

                var payload = JWTHelper.CreatePayload(administrator.CId, administrator.CAccount, tblAdministratorGroup.CGroupId);
                var token = JWTHelper.GetToken(payload);

                response.Entries.Id = administrator.CId;
                response.Entries.Name = administrator.CName;
                response.Entries.Token = token;
                response.Message = "登入成功";
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
