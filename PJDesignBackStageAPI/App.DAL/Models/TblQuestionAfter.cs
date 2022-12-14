using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblQuestionAfter")]
public partial class TblQuestionAfter
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
    /// 最近的編輯人員ID
    /// </summary>
    [Column("cEditorId")]
    public int CEditorId { get; set; }

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
    /// 創建人員ID
    /// </summary>
    [Column("cCreatorId")]
    public int CCreatorId { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Required]
    [Column("cIsEnabled")]
    public bool? CIsEnabled { get; set; }
}
