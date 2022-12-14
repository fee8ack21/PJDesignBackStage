using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblSettingAfter")]
public partial class TblSettingAfter
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 單元JSON內容
    /// </summary>
    [Column("cContent")]
    public string? CContent { get; set; }

    /// <summary>
    /// 單元ID
    /// </summary>
    [Column("cUnitId")]
    public int CUnitId { get; set; }

    /// <summary>
    /// 編輯人員ID
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
}
