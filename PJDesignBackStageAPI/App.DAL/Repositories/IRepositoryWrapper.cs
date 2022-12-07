﻿using App.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
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
        IGroupUnitRightRepository GroupUnitRight { get; }
        IRightRepository Right { get; }
        ISettingBeforeRepository SettingBefore { get; }
        ISettingAfterRepository SettingAfter { get; }
        IContactRepository Contact { get; }
        IQuestionAfterRepository QuestionAfter { get; }
        IQuestionBeforeRepository QuestionBefore { get; }
        ICategoryRepository Category { get; }
        ICategoryMappingBeforeRepository CategoryMappingBefore { get; }
        ICategoryMappingAfterRepository CategoryMappingAfter { get; }

        IDbContextTransaction CreateTransaction();
        void Save();
        Task SaveAsync();
    }
}
