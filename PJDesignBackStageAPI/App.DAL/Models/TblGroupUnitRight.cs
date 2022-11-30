using System;
using System.Collections.Generic;

namespace App.DAL.Models;

public partial class TblGroupUnitRight
{
    public int CId { get; set; }

    public int CGroupId { get; set; }

    public int CUnitId { get; set; }

    public int CRightId { get; set; }
}
