using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblPortfolioAfter
{
    public int CId { get; set; }

    public string CName { get; set; } = null!;

    public DateTime? CDate { get; set; }

    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 0.停用 1.啟用 2.審核中 3.駁回
    /// </summary>
    public byte CStatus { get; set; }

    public DateTime CEditDt { get; set; }
}
