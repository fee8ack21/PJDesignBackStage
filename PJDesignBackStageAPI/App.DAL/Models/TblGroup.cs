using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblGroup
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 管理員組別名稱
    /// </summary>
    public string CName { get; set; } = null!;
}
