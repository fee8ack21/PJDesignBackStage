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
    public string CTitle { get; set; } = null!;

    /// <summary>
    /// 作品日期
    /// </summary>
    public DateTime? CDate { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    public string? CNotes { get; set; }

    /// <summary>
    /// 1.審核中 2.駁回 3.批准
    /// </summary>
    public byte CEditStatus { get; set; }

    /// <summary>
    /// 編輯人員ID
    /// </summary>
    public int CEditorId { get; set; }

    /// <summary>
    /// 審核人員ID
    /// </summary>
    public int? CReviewerId { get; set; }

    public int? CAfterId { get; set; }

    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? CIsEnabled { get; set; }
}
