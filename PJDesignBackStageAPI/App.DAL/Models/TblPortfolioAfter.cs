using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblPortfolioAfter")]
public partial class TblPortfolioAfter
{
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    [Column("cTitle")]
    [StringLength(50)]
    public string CTitle { get; set; } = null!;

    /// <summary>
    /// 作品日期
    /// </summary>
    [Column("cDate", TypeName = "date")]
    public DateTime? CDate { get; set; }

    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Required]
    [Column("cIsEnabled")]
    public bool? CIsEnabled { get; set; }

    [Column("cEditDt", TypeName = "datetime")]
    public DateTime CEditDt { get; set; }

    [Column("cEditorId")]
    public int CEditorId { get; set; }

    [Column("cCreatorId")]
    public int CCreatorId { get; set; }
}
