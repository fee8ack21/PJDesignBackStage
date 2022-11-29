using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblAdministratorGroup
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 管理員組別名稱
    /// </summary>
    public int CAdministratorId { get; set; }

    public int CGroupId { get; set; }
}
