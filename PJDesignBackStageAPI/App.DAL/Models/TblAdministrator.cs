using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblAdministrator
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string CAccount { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    public string CPassword { get; set; } = null!;

    /// <summary>
    /// 名稱
    /// </summary>
    public string CName { get; set; } = null!;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? CIsEnabled { get; set; }

    /// <summary>
    /// 嘗試登入次數
    /// </summary>
    public int CLoginAttemptCount { get; set; }
}
