using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblPortfolioPhotoBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 圖片檔案名稱
    /// </summary>
    public string CUrl { get; set; } = null!;

    /// <summary>
    /// 作品集ID
    /// </summary>
    public int CPortfolioId { get; set; }
}
