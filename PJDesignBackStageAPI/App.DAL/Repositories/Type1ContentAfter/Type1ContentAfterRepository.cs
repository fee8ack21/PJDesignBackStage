using App.DAL.Contexts;
using App.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class Type1ContentAfterRepository : GenericRepository<TblType1ContentAfter>, IType1ContentAfterRepository
    {
        public Type1ContentAfterRepository(PjdesignContext context) : base(context)
        {
        }
    }
}
