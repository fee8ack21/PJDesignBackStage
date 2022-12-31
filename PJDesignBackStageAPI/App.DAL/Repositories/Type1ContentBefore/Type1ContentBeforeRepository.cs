using App.DAL.Contexts;
using App.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class Type1ContentBeforeRepository : GenericRepository<TblType1ContentBefore>, IType1ContentBeforeRepository
    {
        public Type1ContentBeforeRepository(PJDesignContext context) : base(context)
        {
        }
    }
}
