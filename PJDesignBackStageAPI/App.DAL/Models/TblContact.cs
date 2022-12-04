using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblContact
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 訪客名稱
    /// </summary>
    public string CName { get; set; } = null!;

    /// <summary>
    /// 訪客信箱
    /// </summary>
    public string CEmail { get; set; } = null!;

    /// <summary>
    /// 訪客電話
    /// </summary>
    public string CPhone { get; set; } = null!;

    /// <summary>
    /// 聯絡內容
    /// </summary>
    public string CContent { get; set; } = null!;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 自動回覆執行時間
    /// </summary>
    public DateTime? CAutoReplyDt { get; set; }

    /// <summary>
    /// 自動回覆執行狀態 0.未處理 1.已執行 2.未完成
    /// </summary>
    public byte CAutoReplyStatus { get; set; }
}
