using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblPortfolioAfter
{
    public int CId { get; set; }

    public string CTitle { get; set; } = null!;

    /// <summary>
    /// 作品日期
    /// </summary>
    public DateTime? CDate { get; set; }

    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? CIsEnabled { get; set; }

    public DateTime CEditDt { get; set; }

    public int CEditorId { get; set; }

    public int CCreatorId { get; set; }
}
