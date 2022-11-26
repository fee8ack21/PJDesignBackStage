using App.DAL.DbContexts;
using App.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories.Administrator
{
    public class AdministratorRepository : GenericRepository<TblAdministrator>
    {
        public AdministratorRepository(PjdesignContext context) : base(context)
        {
        }
    }
}
