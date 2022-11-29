using App.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public interface IRepositoryWrapper
    {
        IAdministratorRepository Administrator { get; }
        IAdministratorGroupRepository AdministratorGroup { get; }
        IUnitRepository Unit { get; }
        IGroupRepository Group { get; }
        IGroupUnitRepository GroupUnit { get; }

        void Save();
    }
}
