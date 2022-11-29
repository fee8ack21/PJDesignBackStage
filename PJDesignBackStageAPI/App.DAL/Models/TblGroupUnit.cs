using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblGroupUnit
{
    public int CId { get; set; }

    public int CGroupId { get; set; }

    public int CUnitId { get; set; }

    /// <summary>
    /// 操作權限: 0.CRUD
    /// </summary>
    public byte CRightType { get; set; }
}
