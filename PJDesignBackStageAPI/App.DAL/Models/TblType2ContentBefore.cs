using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblType2ContentBefore")]
public partial class TblType2ContentBefore
{
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    [Column("cTitle")]
    [StringLength(50)]
    public string CTitle { get; set; } = null!;

    [Column("cContent")]
    public string? CContent { get; set; }

    [Column("cEditorId")]
    public int CEditorId { get; set; }

    [Column("cReviewerId")]
    public int? CReviewerId { get; set; }

    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    [Column("cEditDt", TypeName = "datetime")]
    public DateTime CEditDt { get; set; }

    [Column("cEditStatus")]
    public byte CEditStatus { get; set; }

    [Column("cNotes")]
    public string? CNotes { get; set; }

    [Column("cAfterId")]
    public int? CAfterId { get; set; }

    [Required]
    [Column("cIsEnabled")]
    public bool? CIsEnabled { get; set; }

    [Column("cThumbnailUrl")]
    [StringLength(200)]
    public string CThumbnailUrl { get; set; } = null!;

    [Column("cImageUrl")]
    [StringLength(200)]
    public string CImageUrl { get; set; } = null!;

    [Column("cUnitId")]
    public int CUnitId { get; set; }

    /// <summary>
    /// 是否置頂
    /// </summary>
    [Column("cIsFixed")]
    public bool CIsFixed { get; set; }

    /// <summary>
    /// 敘述
    /// </summary>
    [Column("cDescription")]
    [StringLength(300)]
    public string? CDescription { get; set; }
}
