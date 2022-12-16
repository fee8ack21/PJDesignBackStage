using App.DAL.Contexts;
using App.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class Type2ContentAfterRepository : GenericRepository<TblType2ContentAfter>, IType2ContentAfterRepository
    {
        public Type2ContentAfterRepository(PjdesignContext context) : base(context)
        {
        }
    }
}
