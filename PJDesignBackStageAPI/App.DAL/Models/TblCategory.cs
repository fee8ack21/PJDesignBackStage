using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblCategory
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 標籤/分類名稱
    /// </summary>
    public string CName { get; set; } = null!;

    /// <summary>
    /// 所屬單元ID
    /// </summary>
    public int CUnitId { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? CIsEnabled { get; set; }

    public DateTime CEditDt { get; set; }
}
