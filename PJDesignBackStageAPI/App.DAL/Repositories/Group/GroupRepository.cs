using App.DAL.Contexts;
using App.DAL.Models;
using App.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class GroupRepository : GenericRepository<TblGroup>, IGroupRepository
    {
        public GroupRepository(PJDesignContext context) : base(context)
        {
        }
    }
}
