using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblRight
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 權限名稱
    /// </summary>
    public string CName { get; set; } = null!;
}
