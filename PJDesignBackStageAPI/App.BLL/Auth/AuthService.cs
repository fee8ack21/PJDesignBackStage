using App.Common;
using App.DAL.Models;
using App.DAL.Repositories;
using App.Enum;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<TblAdministrator> _repository;

        public AuthService(IGenericRepository<TblAdministrator> repository)
        {
            _repository = repository;
        }

        public async Task<ResponseBase<string>> Login(AuthLoginRequest request)
        {
            var response = new ResponseBase<string>();

            try
            {
                var tblAdministrator = await _repository.GetFirstOrDefaultByConditionAsync(x => x.CAccount == request.Account);
                if (tblAdministrator == null)
                {
                    response.StatusCode = StatusCode.Fail;
                    response.Message = "無此用戶!";
                }
                else
                {
                    var hashedPassword = HashHelper.GetPbkdf2Value(request.Password);
                    if (hashedPassword != tblAdministrator.CPassword)
                    {
                        response.StatusCode = StatusCode.Fail;
                        response.Message = "密碼錯誤!";
                    }
                    else
                    {
                        var payload = JWTHelper.CreatePayload(tblAdministrator.CId, tblAdministrator.CAccount, tblAdministrator.CLevel);
                        var token = JWTHelper.GetToken(payload);

                        response.Entries = token;
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
