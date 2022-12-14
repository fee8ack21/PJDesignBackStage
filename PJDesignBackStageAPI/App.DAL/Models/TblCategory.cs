using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblCategory")]
public partial class TblCategory
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 標籤/分類名稱
    /// </summary>
    [Column("cName")]
    [StringLength(50)]
    public string CName { get; set; } = null!;

    /// <summary>
    /// 所屬單元ID
    /// </summary>
    [Column("cUnitId")]
    public int CUnitId { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
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
}
