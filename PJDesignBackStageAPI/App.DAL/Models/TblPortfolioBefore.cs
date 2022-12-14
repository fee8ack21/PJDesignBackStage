using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblPortfolioBefore")]
public partial class TblPortfolioBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Column("cTitle")]
    [StringLength(50)]
    public string CTitle { get; set; } = null!;

    /// <summary>
    /// 作品日期
    /// </summary>
    [Column("cDate", TypeName = "date")]
    public DateTime? CDate { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    [Column("cNotes")]
    public string? CNotes { get; set; }

    /// <summary>
    /// 1.審核中 2.駁回 3.批准
    /// </summary>
    [Column("cEditStatus")]
    public byte CEditStatus { get; set; }

    /// <summary>
    /// 編輯人員ID
    /// </summary>
    [Column("cEditorId")]
    public int CEditorId { get; set; }

    /// <summary>
    /// 審核人員ID
    /// </summary>
    [Column("cReviewerId")]
    public int? CReviewerId { get; set; }

    [Column("cAfterId")]
    public int? CAfterId { get; set; }

    [Column("cEditDt", TypeName = "datetime")]
    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Required]
    [Column("cIsEnabled")]
    public bool? CIsEnabled { get; set; }
}
