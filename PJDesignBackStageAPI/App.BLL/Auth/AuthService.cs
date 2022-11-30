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
                var query = _repositoryWrapper.Administrator
                    .GetByCondition(x => x.CAccount == request.Account)
                    .Join(_repositoryWrapper.AdministratorGroup.GetAll(), x => x.CId, y => y.CAdministratorId, (x, y) => new
                    {
                        Id = x.CId,
                        Name = x.CName,
                        Account = x.CAccount,
                        Password = x.CPassword,
                        GroupId = y.CGroupId
                    });

                var administrator = await query.FirstOrDefaultAsync();

                if (administrator == null)
                {
                    response.StatusCode = StatusCode.Fail;
                    response.Message = "無此用戶!";
                }
                else
                {
                    var hashedPassword = HashHelper.GetPbkdf2Value(request.Password);
                    if (hashedPassword != administrator.Password)
                    {
                        response.StatusCode = StatusCode.Fail;
                        response.Message = "密碼錯誤!";
                    }
                    else
                    {
                        var payload = JWTHelper.CreatePayload(administrator.Id, administrator.Account, administrator.GroupId);
                        var token = JWTHelper.GetToken(payload);

                        response.Entries.Name = administrator.Name;
                        response.Entries.Token = token;
                        response.Message = "登入成功!";
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
    }
}
