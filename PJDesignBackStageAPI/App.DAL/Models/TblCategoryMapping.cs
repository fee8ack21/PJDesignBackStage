using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblCategoryMapping
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int CId { get; set; }

    /// <summary>
    /// CodeID
    /// </summary>
    public int CCodeId { get; set; }

    /// <summary>
    /// 單元ID
    /// </summary>
    public int CUnitId { get; set; }

    /// <summary>
    /// 單元內容ID
    /// </summary>
    public int CTemplateId { get; set; }
}
