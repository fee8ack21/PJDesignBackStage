using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblSettingBefore
{
    public int CId { get; set; }

    public string? CContent { get; set; }

    public int CUnitId { get; set; }

    public int CEditorId { get; set; }

    public int? CReviewerId { get; set; }

    public string? CNotes { get; set; }

    public DateTime CCreateDt { get; set; }

    public DateTime CEditDt { get; set; }

    /// <summary>
    /// 2.審核中 3.駁回 4.批准
    /// </summary>
    public byte CStatus { get; set; }
}
