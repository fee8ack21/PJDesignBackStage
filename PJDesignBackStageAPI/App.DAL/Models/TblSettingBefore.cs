using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblSettingBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 單元JSON內容
    /// </summary>
    public string? CContent { get; set; }

    /// <summary>
    /// 單元ID
    /// </summary>
    public int CUnitId { get; set; }

    /// <summary>
    /// 編輯人員ID
    /// </summary>
    public int CEditorId { get; set; }

    /// <summary>
    /// 審核人員ID
    /// </summary>
    public int? CReviewerId { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? CNotes { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 最近的編輯時間
    /// </summary>
    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 2.審核中 3.駁回 4.批准
    /// </summary>
    public byte CStatus { get; set; }
}
