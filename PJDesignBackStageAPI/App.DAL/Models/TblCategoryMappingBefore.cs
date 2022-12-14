using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblCategoryMappingBefore")]
public partial class TblCategoryMappingBefore
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 分類ID
    /// </summary>
    [Column("cCategoryId")]
    public int CCategoryId { get; set; }

    /// <summary>
    /// 單元內容ID
    /// </summary>
    [Column("cContentId")]
    public int CContentId { get; set; }
}
