using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblContact")]
public partial class TblContact
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 訪客名稱
    /// </summary>
    [Column("cName")]
    [StringLength(50)]
    public string CName { get; set; } = null!;

    /// <summary>
    /// 訪客信箱
    /// </summary>
    [Column("cEmail")]
    [StringLength(200)]
    public string CEmail { get; set; } = null!;

    /// <summary>
    /// 訪客電話
    /// </summary>
    [Column("cPhone")]
    [StringLength(200)]
    public string CPhone { get; set; } = null!;

    /// <summary>
    /// 聯絡內容
    /// </summary>
    [Column("cContent")]
    [StringLength(200)]
    public string CContent { get; set; } = null!;

    /// <summary>
    /// 創建時間
    /// </summary>
    [Column("cCreateDt", TypeName = "datetime")]
    public DateTime CCreateDt { get; set; }

    /// <summary>
    /// 自動回覆執行時間
    /// </summary>
    [Column("cAutoReplyDt", TypeName = "datetime")]
    public DateTime? CAutoReplyDt { get; set; }

    /// <summary>
    /// 自動回覆執行狀態 0.未處理 1.已執行 2.未完成
    /// </summary>
    [Column("cAutoReplyStatus")]
    public byte CAutoReplyStatus { get; set; }
}
