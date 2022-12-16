using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IType2Service
    {
        Task<ResponseBase<List<GetType2ContentsByUnitIdResponse>>> GetType2ContentsByUnitId(int id);
        Task<ResponseBase<GetType2ContentByIdResponse>> GetType2ContentById(int id, bool isBefore, int unitId);
        Task<ResponseBase<string>> CreateOrUpdateType2Content(CreateOrUpdateType2ContentRequest request, JwtPayload payload);
    }
}
