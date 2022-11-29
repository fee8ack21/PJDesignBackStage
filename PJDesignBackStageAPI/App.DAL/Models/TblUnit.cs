﻿using System;
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
    /// 單元路徑
    /// </summary>
    public string? CUrl { get; set; }

    /// <summary>
    /// 模板類別: -1.固定單元 0.無 1.模板一 2.模板二
    /// </summary>
    public int CType { get; set; }

    /// <summary>
    /// 是否為後台單元
    /// </summary>
    public bool CIsBackStage { get; set; }

    /// <summary>
    /// 是否另開視窗
    /// </summary>
    public bool CIsAnotherWindow { get; set; }

    /// <summary>
    /// 單元半結構化資料
    /// </summary>
    public string? CSettings { get; set; }

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
}
