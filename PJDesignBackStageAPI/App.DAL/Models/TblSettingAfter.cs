using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblSettingAfter
{
    public int CId { get; set; }

    public string? CContent { get; set; }

    public int CUnitId { get; set; }

    public int CEditorId { get; set; }

    public DateTime CCreateDt { get; set; }

    public DateTime CEditDt { get; set; }
}
