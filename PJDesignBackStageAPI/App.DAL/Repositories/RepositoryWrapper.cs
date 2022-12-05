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
        private ISettingBeforeRepository? _settingBefore;
        private ISettingAfterRepository? _settingAfter;
        private IContactRepository? _contact;
        private IQuestionAfterRepository? _questionAfter;
        private IQuestionBeforeRepository? _questionBefore;
        private ICategoryRepository? _category;
        private ICategoryMappingBeforeRepository? _categoryMappingBefore;

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

        public ISettingBeforeRepository SettingBefore
        {
            get
            {
                if (_settingBefore == null)
                {
                    _settingBefore = new SettingBeforeRepository(_context);
                }

                return _settingBefore;
            }
        }

        public ISettingAfterRepository SettingAfter
        {
            get
            {
                if (_settingAfter == null)
                {
                    _settingAfter = new SettingAfterRepository(_context);
                }

                return _settingAfter;
            }
        }

        public IContactRepository Contact
        {
            get
            {
                if (_contact == null)
                {
                    _contact = new ContactRepository(_context);
                }

                return _contact;
            }
        }

        public IQuestionBeforeRepository QuestionBefore
        {
            get
            {
                if (_questionBefore == null)
                {
                    _questionBefore = new QuestionBeforeRepository(_context);
                }

                return _questionBefore;
            }
        }

        public IQuestionAfterRepository QuestionAfter
        {
            get
            {
                if (_questionAfter == null)
                {
                    _questionAfter = new QuestionAfterRepository(_context);
                }

                return _questionAfter;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_context);
                }

                return _category;
            }
        }

        public ICategoryMappingBeforeRepository CategoryMappingBefore
        {
            get
            {
                if (_categoryMappingBefore == null)
                {
                    _categoryMappingBefore = new CategoryMappingBeforeRepository(_context);
                }

                return _categoryMappingBefore;
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
