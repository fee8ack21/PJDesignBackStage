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
    /// 管理員ID
    /// </summary>
    public int CAdministratorId { get; set; }

    /// <summary>
    /// 管理組別ID
    /// </summary>
    public int CGroupId { get; set; }
}
