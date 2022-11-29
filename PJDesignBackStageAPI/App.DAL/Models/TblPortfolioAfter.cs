using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblPortfolioAfter
{
    public int CId { get; set; }

    public string CName { get; set; } = null!;

    public DateTime? CDate { get; set; }

    public DateTime CCreateDt { get; set; }
}
