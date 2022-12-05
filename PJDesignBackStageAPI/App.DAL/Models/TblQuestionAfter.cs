using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblQuestionAfter
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    public string CTitle { get; set; } = null!;

    /// <summary>
    /// 問題編輯器內容
    /// </summary>
    public string? CContent { get; set; }

    /// <summary>
    /// 最近的編輯人員ID
    /// </summary>
    public int CEditorId { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 最近的編輯時間
    /// </summary>
    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 創建人員ID
    /// </summary>
    public int CCreatorId { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? CIsEnabled { get; set; }
}
