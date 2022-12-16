using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IType1Service
    {
        Task<ResponseBase<GetType1ContentByUnitIdResponse>> GetType1ContentByUnitId(int id);
        Task<ResponseBase<string>> CreateOrUpdateType1Content(CreateOrUpdateType1ContentRequest request, JwtPayload payload);
    }
}
