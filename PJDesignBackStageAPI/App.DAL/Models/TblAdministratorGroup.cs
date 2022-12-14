using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Models;

[Table("tblAdministratorGroup")]
public partial class TblAdministratorGroup
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Key]
    [Column("cId")]
    public int CId { get; set; }

    /// <summary>
    /// 管理員ID
    /// </summary>
    [Column("cAdministratorId")]
    public int CAdministratorId { get; set; }

    /// <summary>
    /// 管理組別ID
    /// </summary>
    [Column("cGroupId")]
    public int CGroupId { get; set; }
}
