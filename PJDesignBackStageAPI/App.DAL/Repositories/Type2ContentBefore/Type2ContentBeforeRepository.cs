using App.DAL.Contexts;
using App.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class Type2ContentBeforeRepository : GenericRepository<TblType2ContentBefore>, IType2ContentBeforeRepository
    {
        public Type2ContentBeforeRepository(PJDesignContext context) : base(context)
        {
        }
    }
}
