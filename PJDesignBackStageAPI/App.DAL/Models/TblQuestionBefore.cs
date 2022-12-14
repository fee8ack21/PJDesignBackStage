using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblQuestionBefore")]
public partial class TblQuestionBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    [Column("cTitle")]
    [StringLength(50)]
    public string CTitle { get; set; } = null!;

    /// <summary>
    /// 問題編輯器內容
    /// </summary>
    [Column("cContent")]
    public string? CContent { get; set; }

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

    /// <summary>
    /// 創建時間
    /// </summary>
    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 最近的編輯時間
    /// </summary>
    [Column("cEditDt", TypeName = "datetime")]
    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 1.審核中 2.駁回 3.批准
    /// </summary>
    [Column("cEditStatus")]
    public byte CEditStatus { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Column("cNotes")]
    public string? CNotes { get; set; }

    /// <summary>
    /// 對應After 表的ID
    /// </summary>
    [Column("cAfterId")]
    public int? CAfterId { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Required]
    [Column("cIsEnabled")]
    public bool? CIsEnabled { get; set; }
}
