using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblGroup")]
public partial class TblGroup
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 管理員組別名稱
    /// </summary>
    [Column("cName")]
    [StringLength(50)]
    public string CName { get; set; } = null!;
}
