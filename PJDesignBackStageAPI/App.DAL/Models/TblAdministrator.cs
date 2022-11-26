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
    /// 權限等級 1:系統管理員 2:一般用戶
    /// </summary>
    public byte CLevel { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }
}
