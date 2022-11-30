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
    public string CFilename { get; set; } = null!;

    /// <summary>
    /// 圖片檔案路徑
    /// </summary>
    public string CPath { get; set; } = null!;

    /// <summary>
    /// 作品集ID
    /// </summary>
    public int CPortfolioId { get; set; }

    public string CContentHash { get; set; } = null!;
}
