using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblAdministrator")]
public partial class TblAdministrator
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    [Column("cAccount")]
    [StringLength(50)]
    public string CAccount { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    [Column("cPassword")]
    [StringLength(50)]
    public string CPassword { get; set; } = null!;

    /// <summary>
    /// 名稱
    /// </summary>
    [Column("cName")]
    [StringLength(50)]
    public string CName { get; set; } = null!;

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

    /// <summary>
    /// 嘗試登入次數
    /// </summary>
    [Column("cLoginAttemptCount")]
    public int CLoginAttemptCount { get; set; }
}
