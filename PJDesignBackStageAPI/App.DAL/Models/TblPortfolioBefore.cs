using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblPortfolioBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string CName { get; set; } = null!;

    /// <summary>
    /// 作品日期
    /// </summary>
    public DateTime? CDate { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    public string? CNote { get; set; }
}
