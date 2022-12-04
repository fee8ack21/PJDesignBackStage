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

    /// <summary>
    /// 0.停用 1.啟用 2.暫存 3.審核中 4.駁回
    /// </summary>
    public byte CStatus { get; set; }

    /// <summary>
    /// 編輯人員ID
    /// </summary>
    public int CEditAdministratorId { get; set; }

    /// <summary>
    /// 審核人員ID
    /// </summary>
    public int CReviewAdministratorId { get; set; }

    public int CAfterId { get; set; }

    public DateTime CEditDt { get; set; }
}
