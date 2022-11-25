using App.EF;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Administrator.Implement
{
    public class Administrator : ServiceBase, IAdministrator
    {
        public async Task<ResponseBase<List<TblEftest>>> Test()
        {
            var response = new ResponseBase<List<TblEftest>>();

            try
            {
                await using (var db = base.PJDesignContext())
                {
                    response.Entries = db.TblEftests.ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }
    }
}
