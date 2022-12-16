using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblType1ContentAfter")]
public partial class TblType1ContentAfter
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    [Column("cContent")]
    public string? CContent { get; set; }

    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    [Column("cEditDt", TypeName = "datetime")]
    public DateTime CEditDt { get; set; }

    [Column("cEditorId")]
    public int CEditorId { get; set; }

    [Column("cCreatorId")]
    public int CCreatorId { get; set; }

    /// <summary>
    /// 單元ID
    /// </summary>
    [Column("cUnitId")]
    public int CUnitId { get; set; }
}
