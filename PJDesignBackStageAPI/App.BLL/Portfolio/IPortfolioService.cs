using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public interface IPortfolioService
    {
        Task<ResponseBase<List<GetPortfoliosResponse>>> GetPortfolios();
    }
}
