using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblGroupUnitRight")]
public partial class TblGroupUnitRight
{
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    [Column("cGroupId")]
    public int CGroupId { get; set; }

    [Column("cUnitId")]
    public int CUnitId { get; set; }

    [Column("cRightId")]
    public int CRightId { get; set; }
}
