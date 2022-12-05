using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblUnit
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 單元名稱
    /// </summary>
    public string CName { get; set; } = null!;

    /// <summary>
    /// 後台單元路徑
    /// </summary>
    public string? CBackStageUrl { get; set; }

    /// <summary>
    /// 模板類別: -1.固定單元 0.無 1.模板一 2.模板二
    /// </summary>
    public int CTemplateType { get; set; }

    /// <summary>
    /// 前台單元路徑
    /// </summary>
    public string? CFrontStageUrl { get; set; }

    /// <summary>
    /// 是否另開視窗
    /// </summary>
    public bool CIsAnotherWindow { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? CIsEnabled { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 母單元
    /// </summary>
    public int? CParent { get; set; }

    /// <summary>
    /// 0.前後台 1.僅前台 2.僅後台
    /// </summary>
    public int CStageType { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public byte? CSort { get; set; }
}
