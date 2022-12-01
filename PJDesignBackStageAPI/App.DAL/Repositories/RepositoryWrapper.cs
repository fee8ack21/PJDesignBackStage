using App.DAL.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private PjdesignContext _context;
        private IAdministratorRepository? _administrator;
        private IAdministratorGroupRepository? _administratorGroup;
        private IUnitRepository? _unit;
        private IGroupRepository? _group;
        private IGroupUnitRightRepository? _groupUnitRight;
        private IRightRepository? _right;

        public RepositoryWrapper(PjdesignContext context)
        {
            _context = context;
        }

        public IAdministratorRepository Administrator
        {
            get
            {
                if (_administrator == null)
                {
                    _administrator = new AdministratorRepository(_context);
                }

                return _administrator;
            }
        }
        public IAdministratorGroupRepository AdministratorGroup
        {
            get
            {
                if (_administratorGroup == null)
                {
                    _administratorGroup = new AdministratorGroupRepository(_context);
                }

                return _administratorGroup;
            }
        }

        public IUnitRepository Unit
        {
            get
            {
                if (_unit == null)
                {
                    _unit = new UnitRepository(_context);
                }

                return _unit;
            }
        }

        public IGroupUnitRightRepository GroupUnitRight
        {
            get
            {
                if (_groupUnitRight == null)
                {
                    _groupUnitRight = new GroupUnitRightRepository(_context);
                }

                return _groupUnitRight;
            }
        }

        public IGroupRepository Group
        {
            get
            {
                if (_group == null)
                {
                    _group = new GroupRepository(_context);
                }

                return _group;
            }
        }

        public IRightRepository Right
        {
            get
            {
                if (_right == null)
                {
                    _right = new RightRepository(_context);
                }

                return _right;
            }
        }

        public IDbContextTransaction CreateTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
