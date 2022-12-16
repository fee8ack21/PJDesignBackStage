using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblType1ContentBefore")]
public partial class TblType1ContentBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    [Column("cContent")]
    public string? CContent { get; set; }

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
    public byte? CEditStatus { get; set; }

    [Column("cEditorId")]
    public int CEditorId { get; set; }

    [Column("cReviewerId")]
    public int? CReviewerId { get; set; }

    [Column("cEditDt", TypeName = "datetime")]
    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 單元ID
    /// </summary>
    [Column("cUnitId")]
    public int CUnitId { get; set; }
}
