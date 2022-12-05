using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblCategoryMappingAfter
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// 分類ID
    /// </summary>
    public int CCategoryId { get; set; }

    /// <summary>
    /// 單元內容ID
    /// </summary>
    public int? CContentId { get; set; }
}
