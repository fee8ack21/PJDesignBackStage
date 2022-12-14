using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblUnit")]
public partial class TblUnit
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 單元名稱
    /// </summary>
    [Column("cName")]
    [StringLength(50)]
    public string CName { get; set; } = null!;

    /// <summary>
    /// 後台單元路徑
    /// </summary>
    [Column("cBackStageUrl")]
    [StringLength(200)]
    public string? CBackStageUrl { get; set; }

    /// <summary>
    /// 模板類別: -1.固定單元 0.無 1.模板一 2.模板二
    /// </summary>
    [Column("cTemplateType")]
    public int CTemplateType { get; set; }

    /// <summary>
    /// 前台單元路徑
    /// </summary>
    [Column("cFrontStageUrl")]
    [StringLength(200)]
    public string? CFrontStageUrl { get; set; }

    /// <summary>
    /// 是否另開視窗
    /// </summary>
    [Column("cIsAnotherWindow")]
    public bool CIsAnotherWindow { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Required]
    [Column("cIsEnabled")]
    public bool? CIsEnabled { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 母單元
    /// </summary>
    [Column("cParent")]
    public int? CParent { get; set; }

    /// <summary>
    /// 0.前後台 1.僅前台 2.僅後台
    /// </summary>
    [Column("cStageType")]
    public int CStageType { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Column("cSort")]
    public byte? CSort { get; set; }
}
