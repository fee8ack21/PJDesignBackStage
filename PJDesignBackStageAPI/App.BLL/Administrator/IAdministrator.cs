using App.EF;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IAdministrator
    {
        Task<ResponseBase<List<TblEftest>>> Test();
    }
}
