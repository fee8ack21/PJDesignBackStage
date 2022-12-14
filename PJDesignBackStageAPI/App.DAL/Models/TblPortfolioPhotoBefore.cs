using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblPortfolioPhotoBefore")]
public partial class TblPortfolioPhotoBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 圖片檔案名稱
    /// </summary>
    [Column("cUrl")]
    [StringLength(200)]
    public string CUrl { get; set; } = null!;

    /// <summary>
    /// 作品集ID
    /// </summary>
    [Column("cPortfolioId")]
    public int CPortfolioId { get; set; }
}
